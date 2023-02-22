$("#popDetalle").dxPopup({
    title: "Detalle Trámite",
    fullScreen: false,
});

$(document).ready(function () {

    $("#grdListaMemos").dxDataGrid({
        dataSource: MemorandosDataSource,
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
            {
                dataField: "CODTRAMITE",
                width: '10%',
                caption: 'Codigo del Trámite',
                dataType: 'number',
                visible: false,
                allowEditing: false,
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.CODTRAMITE != null && cellInfo.data.CODTRAMITE != '') {
                        var enlaces = '<a href="#" onclick="abrirDetalleTramite(' + cellInfo.data.CODTRAMITE + ');">' + cellInfo.data.CODTRAMITE + '</a>'
                    }
                    cellElement.html(enlaces);
                }
            },
            { dataField: 'CODDOCUMENTO', dataType: 'number', visible: false },
            { dataField: 'RADICADO', with: '15%', caption: 'Radicado', dataType: 'string' },
            { dataField: 'FECHA', width: '15%', caption: 'Fecha Radicado', dataType: 'date', format: 'MMM dd yyyy' },
            { dataField: 'AGNO', width: '10%', caption: 'Año Radicado', dataType: 'number'},
            { dataField: 'ASUNTO', width: '25%', caption: 'Asunto', dataType: 'string' },
            { dataField: 'DE', width: '15%', caption: 'Remitente', dataType: 'string' },
            { dataField: 'PARA', width: '20%', caption: 'Destinatario', dataType: 'string' },
            { dataField: "ID_DOCUMENTO", dataType: 'number', visible: false },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'doc',
                        hint: 'Ver el documento',
                        onClick: function (e) {
                            window.open($('#SIM').data('url') + "Utilidades/LeeDoc?IdDocumento=" + options.data.ID_DOCUMENTO, "Documento " + options.data.ID_DOCUMENTO, "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
                        }
                    }).appendTo(container);
                }
            }
        ]
    });
});


var MemorandosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"FECHADIGITALIZACION","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'GestionDocumental/Api/MemorandosApi/ObtieneMemorandos', {
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
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

function abrirDetalleTramite(id) {
    //var tramiteInstance = $("#popDetalleTramite").dxPopup("instance");
    $("#popDetalle").dxPopup("title", 'Detalle Trámite - ' + id);
    $("#popDetalle").dxPopup("show");
    //tramiteInstance.option('title', 'Detalle Trámite - ' + id);
    //tramiteInstance.show();

    $('#frmDetalleTramite').attr('src', null);
    $('#frmDetalleTramite').attr('src', 'https://webservices.metropol.gov.co/SIM/Tramites/WebForms/DetalleT.aspx?idt=' + id);
}