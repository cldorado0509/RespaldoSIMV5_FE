$(function () {
    DevExpress.ui.setTemplateEngine("underscore");

    var grdInformesDataSource = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
            var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector": "RADICADO","desc":false}]';
            var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
            var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
            var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
            var d = $.Deferred(),
                args = {
                    filter: '',
                    sort: '',
                    group: '',
                    skip: 0,
                    take: 12,
                    searchValue: '',
                    searchExpr: '',
                    comparation: '',
                    tipoData: 'f',
                    noFilterNoRecords: true
                };
            $.ajax({
                url: '@Url.Content("~")facturacion/api/FacturApi/GetInformesPend',
                type: 'GET',
                dataType: 'json',
                data: args,
                success: function (result) {
                    d.resolve(result.datos, { totalCount: result.numRegistros });
                },
                error: function () {
                    d.reject("Error cargando datos");
                }
            });
            return d.promise();
        }
    });

    var grdInforPendDataSource = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();
            var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
            var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector": "RADICADO","desc":false}]';
            var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
            var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
            var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
            $.getJSON('@Url.Content("~")facturacion/api/FacturApi/GetInformesPend',
                {
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
                });
            return d.promise();
        }
    });

    var gridDataSource = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector": "RADICADO","desc":false}]';
            var filterOptions = loadOptions.filter ? JSON.stringify(loadOptions.filter) : "";
            var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
            var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
            var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
            var searchValue = loadOptions.searchValue ? JSON.stringify(loadOptions.searchValue) : "";
            var searchExp = loadOptions.searchValue ? JSON.stringify(loadOptions.searchValue) : "";
            var comparation = loadOptions.comparation ? JSON.stringify(loadOptions.comparation) : "";
            var noFilterNoRecords = false;
            var d = $.Deferred();
            $.getJSON('http://localhost/SIMV5/Facturacion/api/FacturApi/InformesPend',
                {
                    skip: skip,
                    take: take,
                    sort: sortOptions,
                    //filter: filterOptions,
                    //group: groupOptions,
                    //searchValue: searchValue,
                    //searchExp: searchExp,
                    //comparation: comparation,
                    noFilterNoRecords: noFilterNoRecords
                }).done(function (result) {
                    d.resolve(result.datos,
                        {
                            totalCount: result.numRegistros
                        });
                }).error(function (jqXHR, textStatus, errorThrown) {
                    alert("error " + textStatus);
                    //alert("incoming Text " + jqXHR.responseText);
                });
            return d.promise();
        } 
    });

    function isNotEmpty(value) {
        return value !== undefined && value !== null && value !== "";
    }

    $("#gridContainer").dxDataGrid({
        dataSource: {
            store: gridDataSource
        },
        height: '480px',
        filterRow: {
            visible: true
        }
    });

    //$("#gridContainer").dxDataGrid({
    //    dataSource: {
    //        store: gridDataSource
    //    },
    //    loadPanel: { enabled: true, text: 'Cargando Datos...' },
    //    noDataText: "Sin datos para mostrar",
    //    showBorders: true,
    //    paging: {
    //        pageSize: 10
    //    },
    //    pager: {
    //        showPageSizeSelector: true,
    //        allowedPageSizes: [5, 10, 20, 50]
    //    },
        columns: [
            {
                dataField: "CODTRAMITE",
                width: '10%',
                caption: 'Codigo del Trámite',
                dataType: 'number',
            }, {
                dataField: 'CODDOCUMENTO',
                width: '5%',
                caption: 'Documento',
                dataType: 'number',
            }, {
                dataField: 'RADICADO',
                width: '10%',
                caption: 'Radicado',
                dataType: 'string',
            }, {
                dataField: 'FECHA RADICADO',
                width: '10%',
                caption: 'Fecha del Radicado',
                alignment: 'right',
                dataType: 'date'
            }, {
                dataField: 'ASUNTO',
                width: '15%',
                caption: 'Asunto del documento',
                alignment: 'right',
                dataType: 'string',
            }, {
                dataField: 'FACTURA ASIGNADA',
                width: '15%',
                caption: 'Factura asignada',
                dataType: 'string',
            }],
    //    filterRow: {
    //        visible: true
    //    }
    //});

        //function CargaInformes() {
        //    var InfTemplate = $("<div id='InformesTemplate'></div>");
        //    var InformesGrid = $('<div>').attr('id', 'InformesGrid').appendTo(InfTemplate);
        //    InformesGrid.dxDataGrid({
        //        dataSource: {
        //            store: grdInformesDataSource
        //        },
        //        allowColumnResizing: true,
        //        loadPanel: { text: 'Cargando Datos...' },
        //        noDataText: "Sin datos para mostrar",
        //        paging: {
        //            pageSize: 10
        //        },
        //        pager: {
        //            showPageSizeSelector: true,
        //            allowedPageSizes: [5, 10, 20, 50]
        //        },
        //        filterRow: {
        //            visible: true
        //        },
        //        groupPanel: {
        //            visible: true,
        //            emptyPanelText: 'Arrastre una columna para agrupar'
        //        },
        //        selection: {
        //            mode: 'simple'
        //        },
        //        editing: { useIcons: true },
        //        onEditorPreparing: true,
        //        columns: [
        //            {
        //                dataField: "CODTRAMITE",
        //                width: '10%',
        //                caption: 'Codigo del Trámite',
        //                dataType: 'number',
        //            }, {
        //                dataField: 'CODDOCUMENTO',
        //                width: '5%',
        //                caption: 'Documento',
        //                dataType: 'number',
        //            }, {
        //                dataField: 'RADICADO',
        //                width: '10%',
        //                caption: 'Radicado',
        //                dataType: 'string',
        //            }, {
        //                dataField: 'FECHA RADICADO',
        //                width: '10%',
        //                caption: 'Fecha del Radicado',
        //                alignment: 'right',
        //                dataType: 'date'
        //            }, {
        //                dataField: 'ASUNTO',
        //                width: '15%',
        //                caption: 'Asunto del documento',
        //                alignment: 'right',
        //                dataType: 'string',
        //            }, {
        //                dataField: 'FACTURA ASIGANADA',
        //                width: '15%',
        //                caption: 'Factura asignada',
        //                dataType: 'string',
        //            }
        //        ]
        //    });
        //    return InfTemplate;
        //}

        //function CargaResoluciones() {
        //    var ResolucionTemplate = $("<div id='ResolTemplate'></div>");
        //    var ResolGrid = $('<div>').attr('id', 'ResolGrid').appendTo(ResolucionTemplate);
        //    ResolGrid.dxDataGrid({
        //        dataSource: grdInformesDataSource,
        //        paging: {
        //            pageSize: 10
        //        },
        //        pager: {
        //            showPageSizeSelector: true,
        //            allowedPageSizes: [5, 10, 20],
        //            showInfo: true
        //        },
        //        columnAutoWidth: true
        //    });
        //    return ResolucionTemplate;
        //}
});
