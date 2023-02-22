var historicoCargado = false;
var posTab = 0;
var filteredState = false;
var idProgramacion = null;
var soloSeleccionados = false;
var tramitesSeleccionados = null;
var cargandoSeleccionados = false;
var tipoProgramacionActual = 1; // 1 CM, 2 Zona
var idProgramacionActual = null;

var itemSeleccionadoCM = false;
var CMSeleccionado = {};

var itemSeleccionadoTramo = false;
var TramoSeleccionado = {};

var opciones = [{ 'text': 'CM', 'value': 1 }, { 'text': 'Zona', 'value': 2 }];
var opcionesSeleccion = [{ 'text': 'Seleccionados', 'value': 1 }, { 'text': 'Nuevas Tareas', 'value': 2 }];

$(document).ready(function () {
    CMSeleccionado.ID = 0;
    CMSeleccionado.CM = '';
    CMSeleccionado.NOMBRE = '';
    CMSeleccionado.instalacionSel = 0;
    CMSeleccionado.terceroSel = 0;

    TramoSeleccionado.ID = 0;
    TramoSeleccionado.NOMBRE = '';

    $(window).trigger('resize');

    setTimeout(function () { $('#grdNuevasTareas').css('display', 'none'); }, 1000);

    $.getJSON($('#app').data('url') + 'Tramites/api/ProgramadorActuacionesApi/ConsultaZonas', {
    }).done(function (data) {
        $('#cboZona').dxSelectBox('instance').option('dataSource', data.datos);
    }).fail(function (jqxhr, textStatus, error) {
        alert('error cargando datos: ' + textStatus + ", " + error);
    });

    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
    });

    $('#tituloSeleccionados').click(function () {
        $('#tituloSeleccionados').css("font-weight", "bold");
        $('#tituloNuevos').css("font-weight", "normal");

        $('#grdTareasSeleccionadas').css('display', 'block');
        $('#grdNuevasTareas').css('display', 'none');
    });

    $('#tituloNuevos').click(function () {
        $('#tituloNuevos').css("font-weight", "bold");
        $('#tituloSeleccionados').css("font-weight", "normal");

        $('#grdNuevasTareas').css('display', 'block');
        $('#grdTareasSeleccionadas').css('display', 'none');
    });

    $('#btnSeleccionar').dxButton(
    {
        icon: '',
        text: 'Seleccionar ->',
        width: '100%',
        type: 'success',
        onClick: function (params) {
            var grdTramites = $('#grdTramitesReparto').dxDataGrid('instance');
            var rows = grdTramites.getSelectedRowsData();

            if (rows.length > 0) {
                var i;
                for (i = 0; i < rows.length; i++) {
                    if (BuscarTramiteSeleccionado(rows[i].CODTRAMITE))
                    {
                        tramitesSeleccionadosDataSource.store().insert({ CM: rows[i].CM, CODTRAMITE: rows[i].CODTRAMITE, CODTAREA: rows[i].CODTAREA, S_ASUNTO: rows[i].S_ASUNTO });
                    }
                }

                $('#grdTareasSeleccionadas').dxDataGrid('instance').option('dataSource', tramitesSeleccionadosDataSource);
            } else {
                MostrarNotificacion('notify', 'error', 'No hay trámites seleccionados.');
            }
        }
    });

    $('#btnProgramarTareas').dxButton(
    {
        icon: '',
        text: 'Programar',
        width: '100%',
        type: 'success',
        onClick: function (params) {
            var anoSel = $('#cboAno').dxSelectBox('option', 'value');
            var mesSel = $('#cboMes').dxSelectBox('option', 'value');

            if (anoSel < new Date().getFullYear())
            {
                MostrarNotificacion('notify', 'error', 'Año Inválido.');
                return;
            }

            if (anoSel == new Date().getFullYear() && (mesSel - 1) < new Date().getMonth())
            {
                MostrarNotificacion('notify', 'error', 'Fecha Inválida.');
                return;
            }

            var grdTramitesSel = $('#grdTareasSeleccionadas').dxDataGrid('instance');
            var rowsTramitesSel = grdTramitesSel.option('dataSource').store()._array;

            var grdTareasNuevasSel = $('#grdNuevasTareas').dxDataGrid('instance');
            var rowsTareasNuevasSel = grdTareasNuevasSel.getSelectedRowsData();
            
            var tipoProgramacion = $('#radTipoProgramacion').dxRadioGroup('option', 'value').value;

            if (tipoProgramacion == 1 && (rowsTramitesSel == null || rowsTramitesSel.length == 0) && (rowsTareasNuevasSel == null || rowsTareasNuevasSel.length == 0)) {
                MostrarNotificacion('notify', 'error', 'No se han Seleccionado Trámites. No se han Seleccionado Nuevas Tareas.');
                return;
            }

            if (tipoProgramacion == 2 && (rowsTramitesSel == null || rowsTramitesSel.length == 0)) {
                MostrarNotificacion('notify', 'error', 'No se han Seleccionado Trámites.');
                return;
            }

            var tramitesSelProgramacion = [];
            for (i = 0; i < rowsTramitesSel.length; i++) {
                tramitesSelProgramacion.push({ CodTramite: rowsTramitesSel[i].CODTRAMITE, CodTarea: rowsTramitesSel[i].CODTAREA, Asunto: rowsTramitesSel[i].S_ASUNTO });
            }

            var nuevasTareasSelProgramacion = [];
            for (i = 0; i < rowsTareasNuevasSel.length; i++) {
                nuevasTareasSelProgramacion.push(rowsTareasNuevasSel[i].ID);
            }

            $.postJSON(
                $('#app').data('url') + 'Tramites/api/ProgramadorActuacionesApi/ProgramarTramites', {
                    Id: idProgramacionActual,
                    TipoProgramacion: tipoProgramacionActual,
                    CM: (tipoProgramacionActual == 1 ? $('#cboCM').dxSelectBox('option', 'value') : -1),
                    Tramo: (tipoProgramacionActual == 1 ? $('#cboTramo').dxSelectBox('option', 'value') : -1),
                    Zona: (tipoProgramacionActual == 2 ? $('#cboZona').dxSelectBox('option', 'value') : -1),
                    Ano: $('#cboAno').dxSelectBox('option', 'value'),
                    Mes: $('#cboMes').dxSelectBox('option', 'value'),
                    Tramites: tramitesSelProgramacion,
                    NuevasTareas: (tipoProgramacionActual == 1 ? nuevasTareasSelProgramacion : null)
                }
            ).done(function (data) {
                MostrarNotificacion('notify', 'error', 'Programación Realizada Satisfactoriamente.');

                LimpiarDatos();
            });
        }
    });

    $("#popAvanzaTareaTramite").dxPopup({
        title: "Avanza Tarea",
        fullScreen: true,
    });

    $("#popDetalleTramite").dxPopup({
        title: "Detalle Trámite",
        fullScreen: false,
    });
    
    var tabsData = [
            { text: 'REGISTRO', pos: 0 },
            { text: 'PROGRAMACIONES', pos: 1 },
            { text: 'HISTORICO', pos: 2 },
    ];

    $("#tabOpciones").dxTabs({
        dataSource: tabsData,
        onItemClick: (function (itemData) {
            switch (itemData.itemIndex)
            {
                case 0:
                    $('#tab01').css('display', 'block');
                    $('#tab02').css('display', 'none');
                    $('#tab03').css('display', 'none');

                    break;
                case 1:
                    $('#tab02').css('display', 'block');
                    $('#tab01').css('display', 'none');
                    $('#tab03').css('display', 'none');

                    $(window).trigger('resize');
                    $('#grdProgramaciones').dxDataGrid('instance').option('dataSource', grdProgramacionesDataSource);

                    break;
                case 2:
                    $('#tab03').css('display', 'block');
                    $('#tab02').css('display', 'none');
                    $('#tab01').css('display', 'none');

                    break;
            }

            posTab = itemData.itemIndex;
        }),
        selectedIndex: 0,
    });

    var anoActual = new Date().getFullYear();
    var anos = [];
    var meses = [{ ID: 1, NOMBRE: "Enero" }, { ID: 2, NOMBRE: "Febrero" }, { ID: 3, NOMBRE: "Marzo" }, { ID: 4, NOMBRE: "Abril" }, { ID: 5, NOMBRE: "Mayo" }, { ID: 6, NOMBRE: "Junio" }, { ID: 7, NOMBRE: "Julio" }, { ID: 8, NOMBRE: "Agosto" }, { ID: 9, NOMBRE: "Septiembre" }, { ID: 10, NOMBRE: "Octubre" }, { ID: 11, NOMBRE: "Noviembre" }, { ID: 12, NOMBRE: "Diciembre" }];

    for (i = 0; i < 10; i++) {
        anos.push({ ID: anoActual++ });
    }

    $("#cboAno").dxSelectBox({
        dataSource: new DevExpress.data.ArrayStore({
            data: anos,
            key: "ID"
        }),
        width: '100%',
        displayExpr: "ID",
        valueExpr: "ID",
        value: new Date().getFullYear()
    });

    $("#cboMes").dxSelectBox({
        dataSource: new DevExpress.data.ArrayStore({
            data: meses,
            key: "ID"
        }),
        width: '100%',
        displayExpr: "NOMBRE",
        valueExpr: "ID",
        value: new Date().getMonth() + 1
    });

    $('#radTipoProgramacion').dxRadioGroup({
        items: opciones,
        value: opciones[0],
        layout: 'horizontal',
        onValueChanged: function (params) {
            switch (params.value.value) {
                case 1: // CM
                    if (tipoProgramacionActual == 2) {
                        $('#divCM').css('display', 'block');
                        $('#divZona').css('display', 'none');
                        tipoProgramacionActual = 1;

                        $('#tituloSeleccionados').css("font-weight", "bold");
                        $('#tituloNuevos').css("font-weight", "normal");
                        $('#tituloNuevos').css('display', 'block');

                        $('#grdTareasSeleccionadas').css('display', 'block');
                        $('#grdNuevasTareas').css('display', 'none');

                        cboTramoDataSource.store().clear();
                        cboTramoDataSource.load();
                        $('#cboTramo').dxSelectBox('option', 'value', null);

                        cboCMDataSource.store().clear();
                        cboCMDataSource.load();
                        $('#cboCM').dxSelectBox('option', 'value', null);

                        $('#grdTramitesReparto').dxDataGrid('instance').option('dataSource', null);

                        //tramitesSeleccionadosDataSource.store().clear();
                        tramitesSeleccionadosDataSource = new DevExpress.data.DataSource({ store: [], key: 'CODTRAMITE' });
                        $('#grdTareasSeleccionadas').dxDataGrid('instance').option('dataSource', null);
                    }
                    break;
                case 2: // Zona
                    if (tipoProgramacionActual == 1) {
                        $('#divZona').css('display', 'block');
                        $('#divCM').css('display', 'none');
                        tipoProgramacionActual = 2;

                        $('#tituloSeleccionados').css("font-weight", "bold");
                        $('#tituloNuevos').css("font-weight", "normal");
                        $('#tituloNuevos').css('display', 'none');

                        $('#grdTareasSeleccionadas').css('display', 'block');
                        $('#grdNuevasTareas').css('display', 'none');

                        $('#grdTramitesReparto').dxDataGrid('instance').option('dataSource', grdTramitesRepartoDataSource);

                        //tramitesSeleccionadosDataSource.store().clear();
                        tramitesSeleccionadosDataSource = new DevExpress.data.DataSource({ store: [], key: 'CODTRAMITE' });
                        $('#grdTareasSeleccionadas').dxDataGrid('instance').option('dataSource', null);
                    }
                    break;
            }
        },
    });

    $("#radTipoSeleccion").dxRadioGroup({
        items: opcionesSeleccion,
        value: opcionesSeleccion[0],
        layout: "horizontal",
        onValueChanged: function (params) {
            switch (params.value.value) {
                case 1: // Seleccionados
                    $('#tituloSeleccionados').css("font-weight", "bold");
                    $('#tituloNuevos').css("font-weight", "normal");

                    $('#grdTareasSeleccionadas').css('display', 'block');
                    $('#grdNuevasTareas').css('display', 'none');
                    break;
                case 2: // Nuevas Tareas
                    $('#tituloNuevos').css("font-weight", "bold");
                    $('#tituloSeleccionados').css("font-weight", "normal");

                    $('#grdNuevasTareas').css('display', 'block');
                    $('#grdTareasSeleccionadas').css('display', 'none');
                    break;
            }
        },
    });

    $("#cboCM").dxSelectBox({
        dataSource: cboCMDataSource,
        valueExpr: 'ID_POPUP',
        displayExpr: 'NOMBRE_POPUP',
        placeholder: '[Seleccionar CM]',
        onOpened: function () {
            var gridInstance = $('#grdCM').dxDataGrid('instance');

            gridInstance.clearSelection();

            $('#cboCM').dxSelectBox('instance').close();
            var popup = $('#popCM').dxPopup('instance');
            popup.show();
        }
    });

    $("#popCM").dxPopup({
        showTitle: true,
        deferRendering: false,
        title: 'Seleccionar CM',
        onHidden: function () {
            if (itemSeleccionadoCM) {
                cboCMDataSource.store().clear();
                cboCMDataSource.store().insert({ ID_POPUP: CMSeleccionado.ID, NOMBRE_POPUP: CMSeleccionado.NOMBRE });
                cboCMDataSource.load();

                $('#cboCM').dxSelectBox('option', 'value', CMSeleccionado.ID);

                cboTramoDataSource.store().clear();
                cboTramoDataSource.load();

                $('#cboTramo').dxSelectBox('option', 'value', null);

                $('#grdTramitesReparto').dxDataGrid('instance').option('dataSource', grdTramitesRepartoDataSource);
            }
        },
    });

    $('#grdCM').dxDataGrid({
        dataSource: grdCMDataSource,
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
                dataField: 'N_DOCUMENTO',
                width: '20%',
                caption: 'DOCUMENTO',
                visible: true,
                dataType: 'number'
            },
            {
                dataField: 'S_RSOCIAL',
                width: '30%',
                caption: 'RAZON SOCIAL',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'INSTALACION',
                width: '30%',
                caption: 'INSTALACION',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'CM',
                width: '20%',
                caption: 'CM',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'ID_POPUP',
                width: '2%',
                caption: 'ID_POPUP',
                visible: false,
                dataType: 'number'
            },
            {
                dataField: 'NOMBRE_POPUP',
                width: '2%',
                caption: 'NOMBRE_POPUP',
                visible: false,
                dataType: 'string'
            },
        ],
        onSelectionChanged: function (selecteditems) {
            if (selecteditems != null && selecteditems.selectedRowsData.length > 0) {
                var data = selecteditems.selectedRowsData[0];
                itemSeleccionadoCM = true;
                CMSeleccionado.ID = data.ID_POPUP;
                CMSeleccionado.CM = data.CM;
                CMSeleccionado.NOMBRE = data.NOMBRE_POPUP;
                CMSeleccionado.instalacionSel = data.ID_INSTALACION;
                CMSeleccionado.terceroSel = data.ID_TERCERO;

                var popup = $('#popCM').dxPopup('instance');
                popup.hide();
            }
        },
        onContentReady: function (e) {
            $(window).trigger('resize');
        }
    });

    $("#cboTramo").dxSelectBox({
        dataSource: cboTramoDataSource,
        valueExpr: 'ID_POPUP',
        displayExpr: 'NOMBRE_POPUP',
        placeholder: '[Seleccionar Tramo]',
        onOpened: function () {
            var gridInstance = $('#grdTramo').dxDataGrid('instance');

            gridInstance.clearSelection();

            gridInstance.option('dataSource', grdTramoDataSource);

            $('#cboTramo').dxSelectBox('instance').close();
            var popup = $('#popTramo').dxPopup('instance');
            popup.show();
        }
    });

    $("#popTramo").dxPopup({
        showTitle: true,
        deferRendering: false,
        title: 'Seleccionar Tramo',
        onHidden: function () {
            if (itemSeleccionadoTramo) {
                cboTramoDataSource.store().clear();
                cboTramoDataSource.store().insert({ ID_POPUP: TramoSeleccionado.ID, NOMBRE_POPUP: TramoSeleccionado.NOMBRE });
                cboTramoDataSource.load();

                $('#cboTramo').dxSelectBox('option', 'value', TramoSeleccionado.ID);
            }

            itemSeleccionadoTramo = false;
        },
    });

    $('#grdTramo').dxDataGrid({
        dataSource: null,
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
                dataField: 'TIPOSOLICITUD',
                width: '13%',
                caption: 'TIPO SOLICITUD',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'NUMERO',
                width: '10%',
                caption: 'SOLICITUD',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'MUNICIPIO',
                width: '15%',
                caption: 'MUNICIPIO',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'CONEXO',
                width: '13%',
                caption: 'CONEXO',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'TRAMO',
                width: '49%',
                caption: 'TRAMO',
                visible: true,
                dataType: 'string'
            },
            {
                dataField: 'ID_POPUP',
                width: '5%',
                caption: 'CM',
                visible: false,
                dataType: 'string'
            },
            {
                dataField: 'NOMBRE_POPUP',
                width: '5%',
                caption: 'NOMBRE_POPUP',
                visible: false,
                dataType: 'string'
            },
        ],
        onSelectionChanged: function (selecteditems) {
            if (selecteditems != null && selecteditems.selectedRowsData.length > 0) {
                var data = selecteditems.selectedRowsData[0];
                itemSeleccionadoTramo = true;
                TramoSeleccionado.ID = data.ID_POPUP;
                TramoSeleccionado.NOMBRE = data.NOMBRE_POPUP;
                TramoSeleccionado.instalacionSel = data.ID_INSTALACION;
                TramoSeleccionado.terceroSel = data.ID_TERCERO;

                var popup = $('#popTramo').dxPopup('instance');
                popup.hide();
            }
        },
        onContentReady: function (e) {
            $(window).trigger('resize');
        }
    });

    $('#cboZona').dxSelectBox({
        dataSource: null,
        displayExpr: 'NOMBRE',
        valueExpr: 'ID_ZONA',
        placeholder: '[Seleccionar Zona]',
        width: '50%',
        layout: 'horizontal'
    });

    $("#grdTramitesReparto").dxDataGrid({
        dataSource: null,
        keyExpr: 'CODTRAMITE',
        allowColumnResizing: true,
        height: '100%',
        width: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            enabled: false,
            pageSize: 10,
        },
        pager: {
            showPageSizeSelector: true,
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
            mode: 'multiple',
            selectAllMode: "allPages",
            showCheckBoxesMode: "always",
            deferred: true
        },
        columns: [
            {
                dataField: 'CM',
                width: '80px',
                caption: 'CM',
                dataType: 'string',
                visible: true,
            }, {
                dataField: "CODTRAMITE",
                width: '100px',
                caption: 'TRAMITE',
                dataType: 'number',
                visible: true,
            }, {
                dataField: 'S_ASUNTO',
                caption: 'ASUNTO',
                dataType: 'string',
            }
        ],
        onRowClick: function (e) {
            var component = e.component;

            function initialClick() {
                component.clickCount = 1;
                component.clickKey = e.key;
                component.clickDate = new Date();
            }

            function doubleClick() {
                component.clickCount = 0;
                component.clickKey = 0;
                component.clickDate = null;

                if (BuscarTramiteSeleccionado(e.data.CODTRAMITE)) {
                    tramitesSeleccionadosDataSource.store().insert({ CM: e.data.CM, CODTRAMITE: e.data.CODTRAMITE, CODTAREA: e.data.CODTAREA, S_ASUNTO: e.data.S_ASUNTO });
                    tramitesSeleccionadosDataSource.load();
                } else {
                    MostrarNotificacion('notify', 'error', 'El trámite ya se encuentra seleccionado.')
                }

                $('#grdTareasSeleccionadas').dxDataGrid({
                    dataSource: tramitesSeleccionadosDataSource
                });
            }

            if ((!component.clickCount) || (component.clickCount != 1) || (component.clickKey != e.key)) {
                initialClick();
            }
            else if (component.clickKey == e.key) {
                if (((new Date()) - component.clickDate) <= 500)
                    doubleClick();
                else
                    initialClick();
            }
        }
    });

    $("#grdTareasSeleccionadas").dxDataGrid({
        dataSource: null,
        keyExpr: 'CODTRAMITE',
        allowColumnResizing: true,
        height: '100%',
        width: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            enabled: false
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
            mode: 'single',
        },
        columns: [
            {
                dataField: 'CM',
                width: '80px',
                caption: 'CM',
                dataType: 'string',
                visible: true,
            }, {
                dataField: "CODTRAMITE",
                width: '100px',
                caption: 'TRAMITE',
                dataType: 'number',
                visible: true,
            }, {
                dataField: "CODTAREA",
                caption: 'TAREA',
                dataType: 'number',
                visible: false,
            }, {
                dataField: 'S_ASUNTO',
                caption: 'ASUNTO',
                dataType: 'string',
            }
        ],
        onRowClick: function (e) {
            var component = e.component;

            function initialClick() {
                component.clickCount = 1;
                component.clickKey = e.key;
                component.clickDate = new Date();
            }

            function doubleClick() {
                component.clickCount = 0;
                component.clickKey = 0;
                component.clickDate = null;

                tramitesSeleccionadosDataSource.store().remove(e.key);
                tramitesSeleccionadosDataSource.load();

                $('#grdTareasSeleccionadas').dxDataGrid({
                    dataSource: tramitesSeleccionadosDataSource
                });
            }

            if ((!component.clickCount) || (component.clickCount != 1) || (component.clickKey != e.key)) {
                initialClick();
            }
            else if (component.clickKey == e.key) {
                if (((new Date()) - component.clickDate) <= 500)
                    doubleClick();
                else
                    initialClick();
            }
        }
    });

    $("#grdNuevasTareas").dxDataGrid({
        dataSource: grdNuevasTareasDataSource,
        keyExpr: 'ID',
        allowColumnResizing: true,
        height: '100%',
        width: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            enabled: false
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
            mode: 'multiple',
            showCheckBoxesMode: 'always'
        },
        columns: [
            {
                dataField: 'ID',
                caption: 'ID',
                dataType: 'number',
                visible: false,
            }, {
                dataField: "NOMBRE",
                width: '100%',
                caption: 'ASUNTO',
                dataType: 'string',
                visible: true,
            }, {
                dataField: 'SELECCIONADO',
                caption: 'SELECCIONADO',
                dataType: 'boolean',
                visible: false,
            }
        ],
        onRowClick: function (e) {
            var component = e.component;

            function initialClick() {
                component.clickCount = 1;
                component.clickKey = e.key;
                component.clickDate = new Date();
            }

            function doubleClick() {
                component.clickCount = 0;
                component.clickKey = 0;
                component.clickDate = null;

                tramitesSeleccionadosDataSource.store().remove(e.key);
                tramitesSeleccionadosDataSource.load();

                $('#grdTareasSeleccionadas').dxDataGrid({
                    dataSource: tramitesSeleccionadosDataSource
                });
            }

            if ((!component.clickCount) || (component.clickCount != 1) || (component.clickKey != e.key)) {
                initialClick();
            }
            else if (component.clickKey == e.key) {
                if (((new Date()) - component.clickDate) <= 500)
                    doubleClick();
                else
                    initialClick();
            }
        }
    });

    $("#grdProgramaciones").dxDataGrid({
        dataSource: null,
        keyExpr: 'ID_PROGRAMACION',
        allowColumnResizing: true,
        height: '100%',
        width: '100%',
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
                dataField: 'ID_PROGRAMACION',
                caption: 'PROGRAMACION',
                dataType: 'number',
                visible: false,
            }, {
                dataField: 'TIPO_PROGRAMACION',
                caption: 'TIPO',
                dataType: 'number',
                visible: false,
            }, {
                dataField: 'TIPO',
                width: '6%',
                caption: 'TIPO',
                dataType: 'string',
                visible: true,
            }, {
                dataField: 'CMZONA',
                width: '9%',
                caption: 'CM/ZONA',
                dataType: 'string',
                visible: true,
            }, {
                dataField: 'S_TRAMITES',
                width: '40%',
                caption: 'TRAMITES',
                dataType: 'string',
                visible: true,
            }, {
                dataField: "FECHA_PROGRAMACION",
                width: '10%',
                caption: 'FECHA PROG',
                dataType: 'date',
                visible: true,
            }, {
                dataField: "FECHA_EJECUCION",
                width: '10%',
                caption: 'FECHA EJEC.',
                dataType: 'string',
                visible: true,
            }, {
                caption: '',
                width: '10%',
                alignment: 'center',
                cellTemplate: function (container, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: '',
                            text: 'Editar',
                            type: 'default',
                            onClick: function (params) {
                                idProgramacionActual = cellInfo.data.ID_PROGRAMACION;
                                CargarProgramacion();
                            }
                        }
                        ).appendTo(container);
                }
            }, {
                caption: '',
                width: '10%',
                alignment: 'center',
                cellTemplate: function (container, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: '',
                            text: 'Eliminar',
                            type: 'danger',
                            onClick: function (params) {
                                var result = DevExpress.ui.dialog.confirm("Está Seguro de Eliminar la Programación Seleccionada?", "Confirmación");
                                result.done(function (dialogResult) {
                                    if (dialogResult) {
                                        idProgramacionActual = cellInfo.data.ID_PROGRAMACION;

                                        $.getJSON($('#app').data('url') + 'Tramites/api/ProgramadorActuacionesApi/EliminarProgramacion?Id=' + idProgramacionActual, {
                                        }).done(function (data) {
                                            DevExpress.ui.dialog.alert('Programación Eliminada Satisfactoriamente', 'Programación Eliminada');

                                            $('#grdProgramaciones').dxDataGrid('instance').option('dataSource', grdProgramacionesDataSource);
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Error Eliminando Programación', 'Error');
                                        });
                                    }
                                });
                            }
                        }
                        ).appendTo(container);
                }
            }, {
                caption: '',
                width: '10%',
                alignment: 'center',
                cellTemplate: function (container, cellInfo) {
                    $('<div />').dxButton(
                        {
                            icon: '',
                            text: 'Ejecutar',
                            type: 'success',
                            onClick: function (params) {
                                
                            }
                        }
                        ).appendTo(container);
                }
            }
        ],
    });

    AjustarTamano();
});

function BuscarTramiteSeleccionado(codTramite) {
    var tramitesSeleccionados = tramitesSeleccionadosDataSource.items();

    for (i = 0; i < tramitesSeleccionados.length ; i++) {
        if (tramitesSeleccionados[i]['CODTRAMITE'] == codTramite) {
            return false;
        }
    }

    return true;
}

var tramitesSeleccionadosDataSource = new DevExpress.data.DataSource({ store: [], key: 'CODTRAMITE' });
var cboCMDataSource = new DevExpress.data.DataSource([]);

grdCMDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_RSOCIAL","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'Tramites/api/ProgramadorActuacionesApi/ConsultaCM', {
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
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

function LimpiarDatos() {
    idProgramacionActual = null;

    $("#cboAno").dxSelectBox('option', 'value', new Date().getFullYear());
    $("#cboMes").dxSelectBox('option', 'value', new Date().getMonth() + 1);

    var grdTareasNuevasSel = $('#grdNuevasTareas').dxDataGrid('instance');
    grdTareasNuevasSel.selectRows([]);

    tramitesSeleccionadosDataSource.store().clear();
    $('#grdTareasSeleccionadas').dxDataGrid('instance').option('dataSource', null);
}

function CargarProgramacion() {
    var loadPanel = $("#loadPanel").dxLoadPanel('instance');
    loadPanel.show();
    $.getJSON($('#app').data('url') + 'Tramites/api/ProgramadorActuacionesApi/ConsultaProgramacion', {
        id: idProgramacionActual
    }).done(function (data) {
        var tab = $('#tabOpciones').dxTabs('instance');
        tab.option('selectedIndex', 0);

        $('#tab01').css('display', 'block');
        $('#tab02').css('display', 'none');
        $('#tab03').css('display', 'none');

        // Tipo
        $('#radTipoProgramacion').dxRadioGroup('option', 'value', opciones[data.programacion.TIPO - 1]);

        $('#radTipoSeleccion').dxRadioGroup('option', 'value', opcionesSeleccion[0]);

        // Fecha de Ejecucion
        $('#cboAno').dxSelectBox('option', 'value', data.programacion.ANO);
        $('#cboMes').dxSelectBox('option', 'value', data.programacion.MES);

        if (data.programacion.TIPO == 1) // CM
        {
            // CM


            // Tramo

        } else { // Zona
            // Zona
            $('#cboZona').dxSelectBox('option', 'value', data.programacion.ID_ZONA);
        }

        // Trámites Seleccionados
        tramitesSeleccionadosDataSource.store().clear();

        var tramites = data.tramites;

        if (tramites.length > 0) {
            var i;
            for (i = 0; i < tramites.length; i++) {
                tramitesSeleccionadosDataSource.store().insert({ CM: tramites[i].CM, CODTRAMITE: tramites[i].CODTRAMITE, CODTAREA: tramites[i].CODTAREA, S_ASUNTO: tramites[i].S_ASUNTO });
            }

            $('#grdTareasSeleccionadas').dxDataGrid('instance').option('dataSource', tramitesSeleccionadosDataSource);
        }

        // Nuevas Tareas
        var nuevosAsuntos = data.nuevosAsuntos;

        if (nuevosAsuntos.length > 0) {
            var grdTareasNuevasSel = $('#grdNuevasTareas').dxDataGrid('instance');
            grdTareasNuevasSel.selectRows([]);

            var nuevosAsuntosSeleccionados = [];

            var i;
            for (i = 0; i < nuevosAsuntos.length; i++) {
                nuevosAsuntosSeleccionados.push(nuevosAsuntos[i].ID_ASUNTO);
            }

            grdTareasNuevasSel.selectRows(nuevosAsuntosSeleccionados);
        }

        loadPanel.hide();
    }).fail(function (jqxhr, textStatus, error) {
        alert('error cargando datos: ' + textStatus + ", " + error);
    });
}

var cboTramoDataSource = new DevExpress.data.DataSource([]);

grdTramoDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"TRAMO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'Tramites/api/ProgramadorActuacionesApi/ConsultaTramo', {
            cm: (CMSeleccionado == null ? null : CMSeleccionado.ID),
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false
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


var grdTramitesRepartoDataSource = new DevExpress.data.CustomStore({
    key: "CODTRAMITE",
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"CODTRAMITE","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (loadOptions.skip ? loadOptions.skip : 0);
        var take = (loadOptions.take ? loadOptions.take : 0);

        $.getJSON($('#app').data('url') + 'Tramites/api/ProgramadorActuacionesApi/TareasReparto', {
            idProgramacion: idProgramacion,
            cm: (itemSeleccionadoCM ? CMSeleccionado.CM : null),
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
            itemSeleccionadoCM = false;
        });
        return d.promise();
    }
});

var grdNuevasTareasDataSource = new DevExpress.data.CustomStore({
    key: "ID",
    load: function (loadOptions) {
        var d = $.Deferred();

        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"NOMBRE","desc":false}]';

        $.getJSON($('#app').data('url') + 'Tramites/api/ProgramadorActuacionesApi/ConsultaAsuntosTipo', {
            idProgramacion: idProgramacion,
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    }
});

grdProgramacionesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ANO","desc":false}, {"selector":"MES","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'Tramites/api/ProgramadorActuacionesApi/ConsultaProgramaciones', {
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false
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

$.postJSON = function (url, data) {
    var o = {
        url: url,
        type: "POST",
        dataType: "json",
        contentType: 'application/json; charset=utf-8'
    };
    if (data !== undefined) {
        o.data = JSON.stringify(data);
    }
    return $.ajax(o);
};


function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Evaluación PMES');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}
