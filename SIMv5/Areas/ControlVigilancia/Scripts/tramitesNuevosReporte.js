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
            { dataField: 'ID', width: '5%', caption: 'Id', visible: false },
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
            { dataField: 'ANIO_ACTO', width: '10%', caption: 'Año Radicado', visible: false },
            { dataField: 'CM', width: '5%', caption: 'CM' },
            { dataField: 'PROYECTO', width: '20%', caption: 'Nombre del Proyecto - (Instalación)' },
            { dataField: 'NUMERO_ACTO', width: '10%', caption: 'Radicado Resolución', visible: false },
            { dataField: 'FECHA_ACTO', width: '10%', caption: 'Fecha Radicado', dataType:'date', visible: false },
            {dataField: 'ASUNTO', width: '20%', caption: 'Acto Administrativo', visible: true, customizeText: function (cellInfo) {

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
            { dataField: 'ASUNTO', width: '20%', caption: 'Asunto - (Permiso)' },
            { dataField: 'OBSERVACIONES', width: '35%', caption: 'Observaciones', dataType: 'string' },
            { dataField: 'CANTIDAD_DESTOCONADO', width: '10%', caption: 'Cantidad Destoconado', visible: false },
            { dataField: 'CANTIDAD_LEVANTAMIENTO_PISO', width: '10%', caption: 'Cantidad Levantamiento Piso', visible: false },
            { dataField: 'CANTIDAD_MANTENIMIENTO', width: '10%', caption: 'Cantidad Mantenimiento', visible: false },
            { dataField: 'CODIGO_ACTOADMINISTRATIVO', width: '10%', caption: 'Código Acto Administrativo', visible: false },
            { dataField: 'CODIGO_SOLICITUD', width: '10%', caption: 'Código Solicitud', visible: false },
            { dataField: 'CONSERVACION_AUTORIZADO', width: '10%', caption: 'Conservación Autorizada', visible: false },
            { dataField: 'CONSERVACION_EJECUTADA', width: '10%', caption: 'Canservación Ejecutada', visible: false },
            { dataField: 'CONSERVACION_SOLICITADO', width: '10%', caption: 'Conservación Solicitada', visible: false },
            { dataField: 'COORDENADAX', width: '10%', caption: 'Coordenada X', visible: false },
            { dataField: 'COORDENADAY', width: '10%', caption: 'Coordenada Y', visible: false },
            { dataField: 'DAP_MEN_10_AUTORIZADO', width: '10%', caption: 'Dap Men 10 Autorizado', visible: false },
            { dataField: 'DAP_MEN_10_EJECUTADA', width: '10%', caption: 'Dap Men 10 Ejecutado', visible: false },
            { dataField: 'DAP_MEN_10_SOLICITADO', width: '10%', caption: 'Dap Menor 10 Solicitado', visible: false },
            { dataField: 'INVERSION_MEDIDA_ADICIONAL_DESTOCONADO', width: '10%', caption: 'Inversión Medida Adicional Destoconado', visible: false },
            { dataField: 'INVERSION_MEDIDA_ADICIONAL_LEVANTAMIENTO_PISO', width: '10%', caption: 'Inversión Medida Adicional Levantamiento Piso', visible: false },
            { dataField: 'INVERSION_MEDIDA_ADICIONAL_MANTENIMIENTO', width: '10%', caption: 'Inversión Medida Adicional Mantenimiento', visible: false },
            { dataField: 'INVERSION_MEDIDA_ADICIONAL_SIEMBRA', width: '10%', caption: 'Inversión Medida Adicional Siembra', visible: false },
            { dataField: 'INVERSION_MEDIDAS_ADICIONALES', width: '10%', caption: 'Inversión Medidas Adicionales', visible: false },
            { dataField: 'INVERSION_REPOSICION_MINIMA', width: '10%', caption: 'Inversión Reposición Mínima', visible: false },
            { dataField: 'MEDIDA_ADICIONAL_ASIGNADA', width: '10%', caption: 'Medida Adicional Asignada', visible: false },
            { dataField: 'MEDIDA_ADICIONAL_EJECUTADA', width: '10%', caption: 'Medida Adicional Ejecutada', visible: false },
            { dataField: 'NOMBREPROYECTO', width: '10%', caption: 'Nombre Proyecto', visible: false },
            { dataField: 'NRO_LENIOS_AUTORIZADOS', width: '10%', caption: 'Nro Leños Autorizados', visible: false },
            { dataField: 'NRO_LENIOS_SOLICITADOS', width: '10%', caption: 'Nro Leños Solicitados', visible: false },
            { dataField: 'OBSERVACIONVISITA', width: '10%', caption: 'Observaciones Visita', visible: false },
            { dataField: 'PAGO_FONDO_VERDE_METROPOLITANO', width: '10%', caption: 'Pago Fondo Verde Metropolitano', visible: false },
            { dataField: 'PODA_AUTORIZADO', width: '10%', caption: 'Poda Autorizada', visible: false },
            { dataField: 'PODA_EJECUTADA', width: '10%', caption: 'Poda Ejecutada', visible: false },
            { dataField: 'PODA_SOLICITADO', width: '10%', caption: 'Poda Solicitada', visible: false },
            { dataField: 'REPOSICION_AUTORIZADO', width: '10%', caption: 'Reposición Autorizada ', visible: false },
            { dataField: 'REPOSICION_EJECUTADA', width: '10%', caption: 'Reposición Ejecutada', visible: false },
            { dataField: 'REPOSICION_MINIMA_OBLIGATORIA', width: '10%', caption: 'Reposición Mínima Obligatoria', visible: false },
            { dataField: 'REPOSICION_PROPUESTA', width: '10%', caption: 'Reposición Propuesta', visible: false },
            { dataField: 'TALA_AUTORIZADO', width: '10%', caption: 'Tala Autorizada', visible: false },
            { dataField: 'TALA_EJECUTADA', width: '10%', caption: 'Tala Ejecutada', visible: false },
            { dataField: 'TALA_SOLICITADA', width: '10%', caption: 'Tala Solicitada', visible: false },
            { dataField: 'TIPO_DOCUMENTO', width: '10%', caption: 'Tipo Documento', visible: false },
            { dataField: 'TIPOMEDIDAADICIONAL', width: '10%', caption: 'Tipo de Medida Adicional', visible: false },
            { dataField: 'TRASPLANTE_AUTORIZADO', width: '10%', caption: 'Transplante Autorizado', visible: false },
            { dataField: 'TRASPLANTE_EJECUTADO', width: '10%', caption: 'Transplante Ejecutado', visible: false },
            { dataField: 'TRASPLANTE_SOLICITADO', width: '10%', caption: 'Transplante Solicitado', visible: false },
            { dataField: 'VALORACION_INVENTARIO_FORESTAL', width: '10%', caption: 'Valoración Inventario Forestal', visible: false },
            { dataField: 'VALORACION_TALA', width: '10%', caption: 'Valoración Tala', visible: false },
            { dataField: 'VOLUMEN_AUTORIZADO', width: '10%', caption: 'Volumen Autorizado', visible: false },
            { dataField: 'VOLUMEN_EJECUTADO', width: '10%', caption: 'Volumen Ejecutado', visible: false },
            { dataField: 'DIRECCION', width: '10%', caption: 'Dirección', visible: false },
            { dataField: 'CODIGO_TRAMITE', width: '10%', caption: 'Cod Tarea Informe Técnico' },
            { dataField: 'RADICADOVISITA', caption: 'Radicado Inf Téc', visible: false },
            { dataField: 'FECHAVISITA', caption: 'Fecha Visita', dataType: 'date', visible: true },
            { dataField: 'FECHA_RADICADO_VISITA', caption: 'Fecha Radicado Informe Tec', dataType: 'date', visible: false },
          
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
            e.component.columnOption('ID', 'visible', false);
            e.component.columnOption('ANIO_ACTO', 'visible', false);
            e.component.columnOption('CM', 'visible', true);
            e.component.columnOption('PROYECTO', 'visible', true);
            e.component.columnOption('ASUNTO', 'visible', true);
            e.component.columnOption('OBSERVACIONES', 'visible', true);
            e.component.columnOption('CANTIDAD_DESTOCONADO', 'visible', true);
            e.component.columnOption('CANTIDAD_LEVANTAMIENTO_PISO', 'visible', true);
            e.component.columnOption('CANTIDAD_MANTENIMIENTO', 'visible', true);
            e.component.columnOption('CODIGO_ACTOADMINISTRATIVO', 'visible', true);
            e.component.columnOption('CONSERVACION_AUTORIZADO', 'visible', true);
            e.component.columnOption('CONSERVACION_EJECUTADA', 'visible', true);
            e.component.columnOption('CONSERVACION_SOLICITADO', 'visible', true);
            e.component.columnOption('COORDENADAX', 'visible', true);
            e.component.columnOption('COORDENADAY', 'visible', true);
            e.component.columnOption('DAP_MEN_10_AUTORIZADO', 'visible', true);
            e.component.columnOption('DAP_MEN_10_EJECUTADA', 'visible', true);
            e.component.columnOption('DAP_MEN_10_SOLICITADO', 'visible', true);
            e.component.columnOption('INVERSION_MEDIDA_ADICIONAL_DESTOCONADO', 'visible', true);
            e.component.columnOption('INVERSION_MEDIDA_ADICIONAL_LEVANTAMIENTO_PISO', 'visible', true);
            e.component.columnOption('INVERSION_MEDIDA_ADICIONAL_MANTENIMIENTO', 'visible', true);
            e.component.columnOption('INVERSION_MEDIDA_ADICIONAL_SIEMBRA', 'visible', true);
            e.component.columnOption('INVERSION_MEDIDAS_ADICIONALES', 'visible', true);
            e.component.columnOption('INVERSION_REPOSICION_MINIMA', 'visible', true);
            e.component.columnOption('MEDIDA_ADICIONAL_ASIGNADA', 'visible', true);
            e.component.columnOption('MEDIDA_ADICIONAL_EJECUTADA', 'visible', true);
            e.component.columnOption('NOMBREPROYECTO', 'visible', true);
            e.component.columnOption('NRO_LENIOS_AUTORIZADOS', 'visible', true);
            e.component.columnOption('NRO_LENIOS_SOLICITADOS', 'visible', true);
            e.component.columnOption('OBSERVACIONVISITA', 'visible', true);
            e.component.columnOption('PAGO_FONDO_VERDE_METROPOLITANO', 'visible', true);
            e.component.columnOption('PODA_AUTORIZADO', 'visible', true);
            e.component.columnOption('PODA_EJECUTADA', 'visible', true);
            e.component.columnOption('PODA_SOLICITADO', 'visible', true);
            e.component.columnOption('REPOSICION_AUTORIZADO', 'visible', true);
            e.component.columnOption('REPOSICION_EJECUTADA', 'visible', true);
            e.component.columnOption('REPOSICION_MINIMA_OBLIGATORIA', 'visible', true);
            e.component.columnOption('REPOSICION_PROPUESTA', 'visible', true);
            e.component.columnOption('TALA_AUTORIZADO', 'visible', true);
            e.component.columnOption('TALA_EJECUTADA', 'visible', true);
            e.component.columnOption('TALA_SOLICITADA', 'visible', true);
            e.component.columnOption('TIPOMEDIDAADICIONAL', 'visible', true);
            e.component.columnOption('TRASPLANTE_AUTORIZADO', 'visible', true);
            e.component.columnOption('TRASPLANTE_EJECUTADO', 'visible', true);
            e.component.columnOption('TRASPLANTE_SOLICITADO', 'visible', true);
            e.component.columnOption('VALORACION_INVENTARIO_FORESTAL', 'visible', true);
            e.component.columnOption('VALORACION_TALA', 'visible', true);
            e.component.columnOption('VOLUMEN_AUTORIZADO', 'visible', true);
            e.component.columnOption('VOLUMEN_EJECUTADO', 'visible', true);
            e.component.columnOption('DIRECCION', 'visible', true);
            e.component.columnOption('CODIGO_TRAMITE', 'visible', true);
            e.component.columnOption('RADICADOVISITA', 'visible', true);
            e.component.columnOption('FECHAVISITA', 'visible', true);
            e.component.columnOption('TECNICO', 'visible', true);
            e.component.columnOption('ENTIDAD_PUBLICA', 'visible', true);
        },
        onExported: function (e) {
            e.component.columnOption('ID', 'visible', true);
            e.component.columnOption('ANIO_ACTO', 'visible', false);
            e.component.columnOption('CM', 'visible', false);
            e.component.columnOption('PROYECTO', 'visible', false);
            e.component.columnOption('ASUNTO', 'visible', false);
            e.component.columnOption('OBSERVACIONES', 'visible', false);
            e.component.columnOption('CANTIDAD_DESTOCONADO', 'visible', false);
            e.component.columnOption('CANTIDAD_LEVANTAMIENTO_PISO', 'visible', false);
            e.component.columnOption('CANTIDAD_MANTENIMIENTO', 'visible', false);
            e.component.columnOption('CODIGO_ACTOADMINISTRATIVO', 'visible', false);
            e.component.columnOption('CONSERVACION_AUTORIZADO', 'visible', false);
            e.component.columnOption('CONSERVACION_EJECUTADA', 'visible', false);
            e.component.columnOption('CONSERVACION_SOLICITADO', 'visible', false);
            e.component.columnOption('COORDENADAX', 'visible', false);
            e.component.columnOption('COORDENADAY', 'visible', false);
            e.component.columnOption('DAP_MEN_10_AUTORIZADO', 'visible', false);
            e.component.columnOption('DAP_MEN_10_EJECUTADA', 'visible', false);
            e.component.columnOption('DAP_MEN_10_SOLICITADO', 'visible', false);
            e.component.columnOption('INVERSION_MEDIDA_ADICIONAL_DESTOCONADO', 'visible', false);
            e.component.columnOption('INVERSION_MEDIDA_ADICIONAL_LEVANTAMIENTO_PISO', 'visible', false);
            e.component.columnOption('INVERSION_MEDIDA_ADICIONAL_MANTENIMIENTO', 'visible', false);
            e.component.columnOption('INVERSION_MEDIDA_ADICIONAL_SIEMBRA', 'visible', false);
            e.component.columnOption('INVERSION_MEDIDAS_ADICIONALES', 'visible', false);
            e.component.columnOption('INVERSION_REPOSICION_MINIMA', 'visible', false);
            e.component.columnOption('MEDIDA_ADICIONAL_ASIGNADA', 'visible', false);
            e.component.columnOption('MEDIDA_ADICIONAL_EJECUTADA', 'visible', false);
            e.component.columnOption('NOMBREPROYECTO', 'visible', false);
            e.component.columnOption('NRO_LENIOS_AUTORIZADOS', 'visible', false);
            e.component.columnOption('NRO_LENIOS_SOLICITADOS', 'visible', false);
            e.component.columnOption('OBSERVACIONVISITA', 'visible', false);
            e.component.columnOption('PAGO_FONDO_VERDE_METROPOLITANO', 'visible', false);
            e.component.columnOption('PODA_AUTORIZADO', 'visible', false);
            e.component.columnOption('PODA_EJECUTADA', 'visible', false);
            e.component.columnOption('PODA_SOLICITADO', 'visible', false);
            e.component.columnOption('REPOSICION_AUTORIZADO', 'visible', false);
            e.component.columnOption('REPOSICION_EJECUTADA', 'visible', false);
            e.component.columnOption('REPOSICION_MINIMA_OBLIGATORIA', 'visible', false);
            e.component.columnOption('REPOSICION_PROPUESTA', 'visible', false);
            e.component.columnOption('TALA_AUTORIZADO', 'visible', false);
            e.component.columnOption('TALA_EJECUTADA', 'visible', false);
            e.component.columnOption('TALA_SOLICITADA', 'visible', false);
            e.component.columnOption('TIPOMEDIDAADICIONAL', 'visible', false);
            e.component.columnOption('TRASPLANTE_AUTORIZADO', 'visible', false);
            e.component.columnOption('TRASPLANTE_EJECUTADO', 'visible', false);
            e.component.columnOption('TRASPLANTE_SOLICITADO', 'visible', false);
            e.component.columnOption('VALORACION_INVENTARIO_FORESTAL', 'visible', false);
            e.component.columnOption('VALORACION_TALA', 'visible', false);
            e.component.columnOption('VOLUMEN_AUTORIZADO', 'visible', false);
            e.component.columnOption('VOLUMEN_EJECUTADO', 'visible', false);
            e.component.columnOption('DIRECCION', 'visible', false);
            e.component.columnOption('CODIGO_TRAMITE', 'visible', false);
            e.component.columnOption('RADICADOVISITA', 'visible', false);
            e.component.columnOption('FECHAVISITA', 'visible', false);
            e.component.columnOption('TECNICO', 'visible', false);
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
        $.getJSON($('#SIM').data('url') + 'ControlVigilancia/api/TramitesNuevosApi/GetReposicionesReporte', {
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

