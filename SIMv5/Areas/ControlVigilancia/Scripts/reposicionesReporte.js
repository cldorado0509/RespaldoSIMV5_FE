var IdReposicion = -1;
var IdDetReposicion = 0;
var idDocActo = 0;
var idAsunto = 0;
var tipomedida = 0;
var idControl = 0;
var idDocumento = 0;
var IdRegistro = -1;
var idTramite = 0;


$(document).ready(function () {

    $("#GidListado").dxDataGrid({
        dataSource: ReposicionesDataSource,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        export: {
            enabled: true,
            allowExportSelectedData: true,
        },
        paging: {
            pageSize: 5
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10]
        },
        selection: {
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: false,
        columns: [
            { dataField: 'ID', width: '5%', caption: 'Id' },
            { dataField: 'IDDETAIL', width: '5%', caption: 'Id', visible: false },
            { dataField: 'CM', width: '5%', caption: 'CM' },
            { dataField: 'PROYECTO', width: '20%', caption: 'Nombre del Proyecto - (Instalación)' },
            { dataField: 'COORDENADAX', width: '10%', caption: 'Latitud', visible: false },
            { dataField: 'MUNICIPIO', width: '10%', caption: 'Municipio', visible: false },
            { dataField: 'COORDENADAY', width: '10%', caption: 'Longitud', visible: false },
            { dataField: 'NUMERO_ACTO', width: '10%', caption: 'Radicado Acto Adm.', visible: false },
            { dataField: 'FECHA_ACTO', width: '10%', caption: 'Fecha Acto Adm.', dataType:'date', visible: false },
            { dataField: 'ANIO_ACTO', width: '10%', caption: 'Año Acto Adm.', visible: false },
            { dataField: 'ASUNTO', width: '20%', caption: 'Asunto - (Permiso)' },
            {dataField: 'ASUNTO', width: '20%', caption: 'Acto Radicado Acto Administrativo', visible: true, customizeText: function (cellInfo) {

                let dato = cellInfo.value.split(':')[1];
                if (dato) {
                    return dato.split('-')[0];
                } else {
                    return '';
                }
            }
            },
            {
                dataField: 'ASUNTO', width: '20%', caption: 'F.Acto Administrativo', visible: true, customizeText: function (cellInfo) {
                    let dato = cellInfo.value.split(':')[2];
                    if (dato) {
                        return dato.split('-')[0] + '-' + dato.split('-')[1] + '-'+ dato.split('-')[2];
                    } else {
                        return '';
                    }
                }
            },
            {
                dataField: 'ASUNTO', width: '20%', caption: 'Año Act Admin', visible: true, customizeText: function (cellInfo) {
                    let dato = cellInfo.value.split(':')[2];
                    if (dato) {
                        return dato.split('-')[2];
                    } else {
                        return '';
                    }
                }
            },
            { dataField: 'CODIGO_TRAMITE', width: '10%', caption: 'Cod Tarea Informe Técnico' },
            { dataField: 'OBSERVACIONES', width: '35%', caption: 'Observaciones', dataType: 'string' },
            { dataField: 'TALA_AUTORIZADO', caption: 'Tala Autorizada', dataType: 'number', visible: false },
            { dataField: 'TALA_EJECUTADA', caption: 'Tala Ejecutada', dataType: 'number', visible: false },
            { dataField: 'DAP_MEN_10_AUTORIZADO', caption: 'Dap<10 Autorizado', dataType: 'number', visible: false },
            { dataField: 'DAP_MEN_10_EJECUTADA', caption: 'Dap<10 Ejecutado', dataType: 'number', visible: false },
            { dataField: 'VOLUMEN_AUTORIZADO', caption: 'Volumen Autorizado Tala',visible: false },
            { dataField: 'VOLUMEN_EJECUTADO', caption: 'Volumen Ejecutado Tala', visible: false },
            { dataField: 'TRASPLANTE_AUTORIZADO', caption: 'Transplante Autorizado', visible: false },
            { dataField: 'TRASPLANTE_EJECUTADO', caption: 'Transplante Ejecutado', visible: false },
            { dataField: 'PODA_AUTORIZADO', caption: 'Poda Autorizada', visible: false },
            { dataField: 'PODA_EJECUTADA', caption: 'Pada Ejecutada', visible: false },
            { dataField: 'CONSERVACION_AUTORIZADO', caption: 'Conservación Autorizada', visible: false },
            { dataField: 'CONSERVACION_EJECUTADA', caption: 'Conservación Ejecutada', visible: false },
            { dataField: 'REPOSICION_AUTORIZADO', caption: 'Reposición Autorizada', visible: false },
            { dataField: 'REPOSICION_EJECUTADA', caption: 'Reposición Ejecutada', visible: false },
            { dataField: 'TIPOMEDIDAADICIONAL', caption: 'Tipo Medida Adicional', visible: false },
            { dataField: 'MEDIDA_ADICIONAL_EJECUTADA', caption: 'Medida Adicional Ejecutada', visible: false },
            { dataField: 'AUTORIZADO', caption: 'Medida Adicional Autorizada', visible: false },
            { dataField: 'OBSERVACIONES', caption: 'Observaciones', visible: false },
            { dataField: 'FECHAVISITA',  caption: 'Fecha Visita', dataType: 'date', visible: true },
            { dataField: 'FECHA_RADICADO_VISITA', caption: 'Fecha Radicado Informe Tec', dataType: 'date', visible: false },
            { dataField: 'RADICADOVISITA', caption: 'Radicado Inf Téc', visible: false },
            { dataField: 'TECNICO', caption: 'Técnico', visible: false },
            {
                dataField: 'ENTIDAD_PUBLICA', caption: 'Tipo de Entidad', visible: false, customizeText: function (cellInfo) {
                    let dato = cellInfo.value;
                    if (dato === '1') {
                        return 'Pública';
                    } else {
                        return 'Privada';
                    }
                } },
        ],
        onExporting: function (e) {
            e.component.beginUpdate();
            e.component.columnOption('MUNICIPIO', 'visible', true);
            e.component.columnOption('COORDENADAX', 'visible', true);
            e.component.columnOption('COORDENADAY', 'visible', true);
            e.component.columnOption('MUNICIPIO', 'visible', true);
            e.component.columnOption('NUMERO_ACTO', 'visible', true);
            e.component.columnOption('FECHA_ACTO', 'visible', false);
            e.component.columnOption('ANIO_ACTO', 'visible', false);
            e.component.columnOption('TALA_AUTORIZADO', 'visible', true);
            e.component.columnOption('TALA_EJECUTADA', 'visible', true);
            e.component.columnOption('DAP_MEN_10_AUTORIZADO', 'visible', true);
            e.component.columnOption('DAP_MEN_10_EJECUTADA', 'visible', true);
            e.component.columnOption('VOLUMEN_AUTORIZADO', 'visible', true);
            e.component.columnOption('VOLUMEN_EJECUTADO', 'visible', true);
            e.component.columnOption('TRASPLANTE_AUTORIZADO', 'visible', true);
            e.component.columnOption('TRASPLANTE_EJECUTADO', 'visible', true);
            e.component.columnOption('PODA_AUTORIZADO', 'visible', true);
            e.component.columnOption('PODA_EJECUTADA', 'visible', true);
            e.component.columnOption('CONSERVACION_AUTORIZADO', 'visible', true);
            e.component.columnOption('CONSERVACION_EJECUTADA', 'visible', true);
            e.component.columnOption('REPOSICION_AUTORIZADO', 'visible', true);
            e.component.columnOption('REPOSICION_EJECUTADA', 'visible', true);
            e.component.columnOption('TIPOMEDIDAADICIONAL', 'visible', true);
            e.component.columnOption('MEDIDA_ADICIONAL_EJECUTADA', 'visible', true);
            e.component.columnOption('AUTORIZADO', 'visible', true);
            e.component.columnOption('OBSERVACIONES', 'visible', true);
            e.component.columnOption('FECHAVISITA', 'visible', true);
            e.component.columnOption('FECHA_RADICADO_VISITA', 'visible', true);
            e.component.columnOption('RADICADOVISITA', 'visible', true);
            e.component.columnOption('TECNICO', 'visible', true);
            e.component.columnOption('ENTIDAD_PUBLICA', 'visible', true);
        },
        onExported: function (e) {
            e.component.columnOption('MUNICIPIO', 'visible', false);
            e.component.columnOption('COORDENADAX', 'visible', false);
            e.component.columnOption('COORDENADAY', 'visible', false);
            e.component.columnOption('NUMERO_ACTO', 'visible', false);
            e.component.columnOption('FECHA_ACTO', 'visible', false);
            e.component.columnOption('ANIO_ACTO', 'visible', false);
            e.component.columnOption('TALA_AUTORIZADO', 'visible', false);
            e.component.columnOption('TALA_EJECUTADA', 'visible', false);
            e.component.columnOption('DAP_MEN_10_AUTORIZADO', 'visible', false);
            e.component.columnOption('DAP_MEN_10_EJECUTADA', 'visible', false);
            e.component.columnOption('VOLUMEN_AUTORIZADO', 'visible', false);
            e.component.columnOption('VOLUMEN_EJECUTADO', 'visible', false);
            e.component.columnOption('TRASPLANTE_AUTORIZADO', 'visible', false);
            e.component.columnOption('TRASPLANTE_EJECUTADO', 'visible', false);
            e.component.columnOption('PODA_AUTORIZADO', 'visible', false);
            e.component.columnOption('PODA_EJECUTADA', 'visible', false);
            e.component.columnOption('CONSERVACION_AUTORIZADO', 'visible', false);
            e.component.columnOption('CONSERVACION_EJECUTADA', 'visible', false);
            e.component.columnOption('REPOSICION_AUTORIZADO', 'visible', false);
            e.component.columnOption('REPOSICION_EJECUTADA', 'visible', false);
            e.component.columnOption('TIPOMEDIDAADICIONAL', 'visible', false);
            e.component.columnOption('MEDIDA_ADICIONAL_EJECUTADA', 'visible', false);
            e.component.columnOption('AUTORIZADO', 'visible', false);
            e.component.columnOption('OBSERVACIONES', 'visible', false);
            e.component.columnOption('FECHAVISITA', 'visible', false);
            e.component.columnOption('FECHA_RADICADO_VISITA', 'visible', false);
            e.component.columnOption('RADICADOVISITA', 'visible', false);
            e.component.columnOption('TECNICO', 'visible',  false);
            e.component.columnOption('ENTIDAD_PUBLICA', 'visible', false);
            e.component.endUpdate();
        }
    });
});


//Data Stores
var ReposicionesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"IDDETAIL","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'ControlVigilancia/api/ReposicionesApi/GetReposicionesReporte', {
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
            CodFuncionario: CodigoFuncionario
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

