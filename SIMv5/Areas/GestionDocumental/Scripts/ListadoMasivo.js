$(document).ready(function () {

    $("#grdListaMasivos").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "ID",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "GestionDocumental/api/MasivosApi/ListadoMasivos?CodFunc=" + CodFunc);
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
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20, 50]
        },
        selection: {
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'ID', width: '5%', caption: 'Identificador', alignment: 'center' },
            { dataField: 'TEMA', width: '20%', caption: 'Tema del proceso de Radicación', dataType: 'string' },
            { dataField: 'D_FECHA', width: '15%', caption: 'Fecha del Proceso', dataType: 'date', format: 'MMM dd yyyy HH:mm' },
            { dataField: 'CANTIDAD_FILAS', width: '20%', caption: 'Documentos a generar', dataType: 'string' },
            { dataField: 'ESTADO', width: '10%', caption: 'Estado', dataType: 'string' },
            { dataField: 'MENSAJE', dataType: 'string', visible: false }
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        type: 'success',
                        hint: 'Editar proceso masivo COD',
                        onClick: function (e) {

                        }
                    }).appendTo(container);
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'check',
                        type: 'success',
                        hint: 'Firmar la plantilla',
                        onClick: function (e) {

                        }
                    }).appendTo(container);
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'doc',
                        type: 'success',
                        hint: 'Previsualizar muestra del documento',
                        onClick: function (e) {

                        }
                    }).appendTo(container);
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'alignjustify',
                        type: 'success',
                        hint: 'Ver la plantilla del proceso',
                        onClick: function (e) {

                        }
                    }).appendTo(container);
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'tips',
                        type: 'success',
                        hint: 'Ver motivo de rechazo firma',
                        onClick: function (e) {

                        }
                    }).appendTo(container);
                }
            }
        ]
    });
});