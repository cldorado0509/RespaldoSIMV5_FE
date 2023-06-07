var _OpcEstado = 1;
var _OpcTipo = 1;

$(document).ready(function () {
    var tabPanel = $("#TabPanel").dxTabPanel({
        height: 600,
        dataSource: [{
            "title": "Mis tareas", "template": MisTareasInit
        }],
        selectedIndex: 0,
        loop: false,
        animationEnabled: true,
        swipeEnabled: true,
    }).dxTabPanel("instance");

    var CodTramite = 0;
    var CodTarea = 0;
    var Orden = 0;
    var Copia = 0;
    var Bloqueada = false;

    function MisTareasInit() {
        var MisTarTemplate = $("<div id='MisTareasTemplate'></div>");
        var MisTarGrid = $('<div>').attr('id', 'MisTarGrid').appendTo(MisTarTemplate);
        var BotonesTareas = $('<div>').attr('id', 'BotonesTareas').appendTo(MisTarTemplate);
        var BotonDetalle = $('<div>').attr('id', 'DetalleTarea').appendTo(BotonesTareas);
        var BotonAvanza = $('<div>').attr('id', 'AvanzaTarea').appendTo(BotonesTareas);
        var BotonDevuelve = $('<div>').attr('id', 'DevuelveTarea').appendTo(BotonesTareas);
        $('<div>').attr('id', 'PopupDetalle').appendTo(MisTarTemplate);
        $('<div>').attr('id', 'PopupDev').appendTo(MisTarTemplate);
        $('<div>').attr('id', 'PopupDesbloquea').appendTo(MisTarTemplate);
        var popupOpciones = null;

        var store = DevExpress.data.AspNet.createStore({
            loadUrl: $("#SIM").data("url") + "Tramites/api/MisTareasApi/MotivosDevolucion",
        });

        var listMotivos = new DevExpress.data.DataSource({
            store: store,
            paginate: false,
            pageSize: 1,
            sort: "Motivo",
        });

        MisTarGrid.dxDataGrid({
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
                                    .append($("<img>", { "src": '../Content/Images/leido-devuelto.png', "width": '16px', "height": '16px' }))
                                    .appendTo(container);
                            } else {
                                $("<div>")
                                    .append($("<img>", { "src": '../Content/Images/sin-leer-devuelto.png', "width": '16px', "height": '16px' }))
                                    .appendTo(container);
                            }
                        }
                        else if (options.row.data.RECIBIDA == '1') {
                            $("<div>")
                                .append($("<img>", { "src": '../Content/Images/leido.png', "width": '16px', "height": '16px' }))
                                .appendTo(container);
                        } else {
                            $("<div>")
                                .append($("<img>", { "src": '../Content/Images/sin-leer.png', "width": '16px', "height": '16px' }))
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
                { dataField: 'ORDEN', dataType: 'number', visible: false, allowSearch: false }
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

        BotonDetalle.dxButton({
            stylingMode: "contained",
            text: "Ver detalles de la actividad",
            type: "default",
            width: 400,
            icon: '../Content/Images/detalle.svg',
            onClick: function () {
                if (CodTramite > 0) {
                    popup = $("#PopupDetalle").dxPopup(popupOpciones).dxPopup("instance");
                    $('#PopupDetalle').css({ 'visibility': 'visible' });
                    $("#PopupDetalle").fadeTo("slow", 1);
                    popup.show();
                } else {
                    DevExpress.ui.notify("No ha seleccionado una tarea para ver su detalle");
                }
            }
        });

        BotonAvanza.dxButton({
            stylingMode: "contained",
            text: "Avanzar la actividad",
            type: "default",
            width: 400,
            icon: '../Content/Images/avanzar-01.svg',
            onClick: function () {
                if (CodTramite > 0) {
                    if (Copia == 0) {
                        if (!Bloqueada) {
                            var avanzarInstance = $("#popAvanzaTareaTramite").dxPopup("instance");
                            avanzarInstance.show();
                            var date = new Date;
                            var _ruta = $('#SIM').data('url') + "Tramites/Tramites/AvanzaTareaTramite?codTramites=" + CodTramite + "&tipo=0&origen=AVANZAR&multiTramites=0&c=" + date.getMilliseconds();
                            $('#frmAvanzaTareaTramite').attr('src', _ruta);
                        } else {
                            popupOpcDesb = {
                                width: 500,
                                height: 300,
                                contentTemplate: function () {
                                    return $("<div>").append(
                                        $("<p>Observaciones <span></p>"),
                                        $("<br />"),
                                        $("<div>").attr("id", "TextDesbloqueo").dxTextArea({
                                            placeholder: "Ingrese un comentario ...",
                                            height: 80
                                        }),
                                        $("<br />"),
                                        $("<div>").attr({ "id": "BtnDesbloqueo", "style": "text-align:center" }).append($("<div>").dxButton({
                                            stylingMode: "text",
                                            text: "Desbloquear",
                                            type: "default",
                                            width: 120,
                                            onClick: function () {
                                                var _comentario = $("#TextDesbloqueo").dxTextArea('instance').option("value");
                                                var params = { CodTramite: CodTramite, CodFuncionario: CodigoFuncionario, Comentario: _comentario };
                                                var _Ruta = $('#SIM').data('url') + "Tramites/api/MisTareasApi/DesbloqueaTramite";
                                                $.getJSON(_Ruta, params, function (result, status) {
                                                    if (status === "success") {
                                                        $("#PopupDesbloquea").dxPopup("instance").hide();
                                                        BotonAvanza.dxButton("instance").option("text", "Avanzar la actividad");
                                                        Bloqueada = false;
                                                    } else {
                                                        DevExpress.ui.notify("No fue posible desbloquear el trámite " + CodTramite);
                                                        $("#PopupDesbloquea").dxPopup("instance").hide();
                                                    }
                                                });
                                            }
                                        })));                                },
                                showTitle: true,
                                title: "Desbloquear Trámite",
                                visible: false,
                                dragEnabled: false,
                                closeOnOutsideClick: true
                            };
                            popupDesb = $("#PopupDesbloquea").dxPopup(popupOpcDesb).dxPopup("instance");
                            $('#PopupDesbloquea').css({ 'visibility': 'visible' });
                            $("#PopupDesbloquea").fadeTo("slow", 1);
                            popupDesb.show();
                        }
                    } else {
                        var result = DevExpress.ui.dialog.confirm('¿Después de revisar la información de la actividad desea cerrarla?', 'Confirmación');
                        result.done(function (dialogResult) {
                            if (dialogResult) {
                                var _Ruta = $('#SIM').data('url') + "Tramites/api/MisTareasApi/TerminaCopia?CodTramite=" + CodTramite + "&CodTarea=" + CodTarea + "&Orden=" + Orden;
                                $.getJSON(_Ruta, function (result, status) {
                                    if (status === "success") {
                                        $('#MisTarGrid').dxDataGrid({ dataSource: misTareasDataSource });
                                    }
                                });
                            }
                        });
                    }
                } else {
                    DevExpress.ui.notify("No ha seleccionado una tarea para avanzar");
                }
            }
        });

        BotonDevuelve.dxButton({
            stylingMode: "contained",
            text: "Devolver la actividad",
            type: "default",
            width: 400,
            icon: '../Content/Images/devolver-01.svg',
            onClick: function () {
                if (CodTramite > 0 && CodTarea > 0) {
                    var _FuncRecibe = "";
                    var _Ruta = $('#SIM').data('url') + "Tramites/api/MisTareasApi/FuncionarioAnterior?CodTramite=" + CodTramite + "&CodTarea=" + CodTarea + "&Orden=" + Orden;
                    $.getJSON(_Ruta, function (result, status) {
                        if (status === "success") {
                            _FuncRecibe = result;
                            if (!_FuncRecibe.startsWith("No se encontr") && !_FuncRecibe.startsWith("Error :")) {
                                popupOpcDev = {
                                    width: 700,
                                    height: 500,
                                    contentTemplate: function () {
                                        return $("<div>").append(
                                            $("<p>Esta seguro de devolver la actividad al funcionario </p>"),
                                            $("<br />"),
                                            $("<p><span><b>" + _FuncRecibe + "</b></span></p>"),
                                            $("<br />"),
                                            $("<p><span>Motivos de devolución</span></p>"),
                                            $("<div>").attr("id", "ListaDevol").dxList({
                                                dataSource: listMotivos,
                                                height: 200,
                                                allowItemDeleting: false,
                                                showSelectionControls: true,
                                                selectionMode: "multiple",
                                                itemTemplate: function (itemData, itemIndex, itemElement) {
                                                    itemElement.append('<p>' + itemData.Motivo + '</p>');
                                                },
                                            }),
                                            $("<br />"),
                                            $("<div>").attr("id", "TextDevol").dxTextArea({
                                                placeholder: "Ingrese un comentario para el funcionario...",
                                                height: 65
                                            }),
                                            $("<br />"),
                                            $("<div>").attr({ "id": "BtnDevol", "style": "text-align:center" }).append($("<div>").dxButton({
                                                stylingMode: "text",
                                                text: "Aceptar",
                                                type: "default",
                                                width: 120,
                                                onClick: function () {
                                                    var selectedItems = $("#ListaDevol").dxList("instance").option("selectedItems");
                                                    var seleccionados = "";
                                                    for (var i = 0; i <= selectedItems.length - 1; i++) {
                                                        var seleccionado = selectedItems[i];
                                                        seleccionados += seleccionado["IdMotivo"] + ";" + seleccionado["Motivo"] + ",";
                                                    }
                                                    var _comentario = $("#TextDevol").dxTextArea('instance').option("value");
                                                    var params = { CodTramite: CodTramite, CodTarea: CodTarea, Orden: Orden, Funcionario: CodigoFuncionario, Comentario: _comentario, Motivos: seleccionados };
                                                    var _Ruta = $('#SIM').data('url') + "Tramites/api/MisTareasApi/DevolverTarea";
                                                    $.getJSON(_Ruta, params, function (result, status) {
                                                        if (status === "success") {
                                                            if (!result.startsWith("Error:")) {
                                                                $('#MisTarGrid').dxDataGrid({ dataSource: misTareasDataSource });
                                                                if (result != "") {
                                                                    DevExpress.ui.notify(result);
                                                                } else {
                                                                    $("#PopupDev").dxPopup("instance").hide();
                                                                }
                                                            } else {
                                                                DevExpress.ui.notify(result);
                                                            }
                                                        } else {
                                                            DevExpress.ui.notify("Motivos seleccionados para el tramite " + CodTramite + " " + seleccionados);
                                                        }
                                                    });
                                                }
                                            })));
                                    },
                                    showTitle: true,
                                    title: "Devolución de la actividad",
                                    visible: false,
                                    dragEnabled: false,
                                    closeOnOutsideClick: true
                                };
                                popup = $("#PopupDev").dxPopup(popupOpcDev).dxPopup("instance");
                                $('#PopupDev').css({ 'visibility': 'visible' });
                                $("#PopupDev").fadeTo("slow", 1);
                                popup.show();
                            } else {
                                DevExpress.ui.notify(_FuncRecibe);
                            }
                        } else alert(status);
                    });
                } else {
                    DevExpress.ui.notify("No ha seleccionado una tarea para devolver");
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
                            _OpcEstado = e.value;
                            if (_OpcEstado != 1) {
                                BotonDevuelve.dxButton("instance").option("visible", false);
                                BotonAvanza.dxButton("instance").option("visible", false);
                                $("#MisTarGrid").dxDataGrid('columnOption', 'FINTAREA', 'visible', true); 
                            } else {
                                if (_OpcTipo == 1) {
                                    BotonDevuelve.dxButton("instance").option("visible", true);
                                    BotonAvanza.dxButton("instance").option("visible", true);
                                    $("#MisTarGrid").dxDataGrid('columnOption', 'FINTAREA', 'visible', false); 
                                }
                            }
                            $("#MisTarGrid").dxDataGrid("instance").refresh();
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
                            _OpcTipo = e.value;
                            if (_OpcTipo == 2) {
                                BotonDevuelve.dxButton("instance").option("visible", false);
                                if (_OpcEstado == 1) {
                                    BotonAvanza.dxButton("instance").option("visible", true);
                                } else {
                                    BotonAvanza.dxButton("instance").option("visible", false);
                                }
                            } else {
                                if (_OpcEstado == 1) {
                                    BotonAvanza.dxButton("instance").option("visible", true);
                                    BotonDevuelve.dxButton("instance").option("visible", true);
                                } else {
                                    BotonAvanza.dxButton("instance").option("visible", false);
                                    BotonDevuelve.dxButton("instance").option("visible", false);
                                }
                            }
                            $("#MisTarGrid").dxDataGrid("instance").refresh();
                        }
                    }
                });
                toolbar.option('items', items);
            }
        }

        $("#popAvanzaTareaTramite").dxPopup({
            title: "Avanza Tarea",
            fullScreen: true,
            onHidden: function (e) {
                $('#MisTarGrid').dxDataGrid({ dataSource: misTareasDataSource });
            }
        });
        return MisTarTemplate;
    }

    function TareasDepInit() { }
});

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
            tipo : _OpcTipo
        }).done(function (data) {
           d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
           alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});