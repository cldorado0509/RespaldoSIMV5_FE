$(document).ready(function ()
{
    var CodTramite = -1;
    var CodDocumento = -1;
    var TipoTramite = -1;
    var Agnos = generarArrayDeAnos();

    function generarArrayDeAnos() {
        var max = new Date().getFullYear()
        var min = max - 4
        var years = []

        for (var i = max; i >= min; i--) {
            years.push(i)
        }
        return years
    }

    $("#sebTipoUnidad").dxTabs({
        dataSource: [{ id: 0, text: 'Informes Técnicos' }, { id: 1, text: 'Resoluciones' }],
        selectedIndex: 0,
        onContentReady: function (e) {
            $("#PanelInfTecnico").show();
            $("#PanelResoluciones").hide();
            SelTipo = 0;
        },
        onItemClick(e) {
            switch (e.itemData.id) {
                case 0:
                    $("#PanelInfTecnico").show();
                    $("#PanelResoluciones").hide();
                    break;
                case 1:
                    $("#PanelInfTecnico").hide();
                    $("#PanelResoluciones").show();
                    break;
            }
        }
    }).dxTabs('instance');

    $("#grdInformes").dxDataGrid({
        dataSource: grdInfDataSource,
        allowColumnResizing: true,
        caption: 'Informes Ténicos pendientes de facturación',
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
        'export': {
            enabled: true,
            fileName: 'Informes_Facturas'
        },
        remoteOperations: true,
        hoverStateEnabled: true,
        columns: [
            { dataField: "CODTRAMITE", width: '10%', caption: 'Codigo del Trámite', dataType: 'number' },
            { dataField: 'CODDOCUMENTO', width: '5%', caption: 'Documento', dataType: 'number' },
            { dataField: 'RADICADO', width: '10%', caption: 'Radicado', dataType: 'string' },
            { dataField: 'FECHA_RADICADO', width: '10%', caption: 'Fecha del Radicado', dataType: 'string' },
            { dataField: 'ASUNTO', width: '30%', caption: 'Asunto del documento', dataType: 'string' },
            { dataField: 'TECNICO', width: '20%', caption: 'Tecnico encargado', dataType: 'string' },
            {
                caption: 'Ver el documento',
                width: '5%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'doc',
                        hint: 'Visualizar Documento',
                        onClick: function (e) {
                            window.open($("#SIM").data("url") + 'Correspondencia/Correspondencia/LeeDoc?CodTramite=' + options.row.data.CODTRAMITE + '&CodDocumento=' + options.row.data.CODDOCUMENTO, "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
                        }
                    }).appendTo(container);
                }
            },
            {
                caption: 'Calcular',
                width: '5%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'rowfield',
                        hint: 'Calcular el valor del seguimiento',
                        onClick: function (e) {
                            TipoTra.option("value", null);
                            CantProf.option("value", 0);
                            CantVisitas.option("value", 0);
                            CantHorInf.option("value", 0);
                            DuracionVisita.option("value", 0);
                            $("#NombreTercero").text("");
                            //NomTercero.option("value", "");
                            DocTercero.option("value", "");
                            PopCalcular.show();
                            CodTramite = options.row.data.CODTRAMITE;
                            CodDocumento = options.row.data.CODDOCUMENTO;
                            if (options.row.data.CODTRAMITE > 0 && options.row.data.CODDOCUMENTO > -1) {
                                $("#DocumentoInforme").attr("src", $('#SIM').data('url') + 'Correspondencia/Correspondencia/LeeDoc?CodTramite=' + options.row.data.CODTRAMITE + '&CodDocumento=' + options.row.data.CODDOCUMENTO);
                            } else {
                                DevExpress.ui.dialog.alert("El registro no posee un codigo de trámite o código de documento");
                            }
                        }
                    }).appendTo(container);
                }
            }, {
                caption: 'Detalle',
                width: '5%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'event',
                        hint: 'Ver detalle del trámite',
                        onClick: function (e) {
                            CodTramite = options.row.data.CODTRAMITE;
                            popupDetalles.show();
                        }
                    }).appendTo(container);
                }
            }
        ]
    });

    $("#grdResol").dxDataGrid({
        dataSource: grdResDataSource,
        allowColumnResizing: true,
        caption: 'Resoluciones pendientes de facturación',
        loadPanel: { enabled: true, text: 'Cargando Datos...', position: { my: 'center', at: 'center', of: "#TabPanel" } },
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
        'export': {
            enabled: true,
            fileName: 'Informes_Facturas'
        },
        remoteOperations: true,
        hoverStateEnabled: true,
        columns: [
            { dataField: "CODTRAMITE", width: '10%', caption: 'Codigo del Trámite', dataType: 'number' },
            { dataField: 'CODDOCUMENTO', width: '5%', caption: 'Documento', dataType: 'number' },
            { dataField: 'RESOLUCION', width: '10%', caption: 'Resolución', dataType: 'string' },
            { dataField: 'FECHA_RESOLUCION', width: '10%', caption: 'Fecha Resolución', dataType: 'string' },
            { dataField: 'TIPO_RESOLUCION', width: '25%', caption: 'Tipo de Resolución', dataType: 'string' },
            { dataField: 'ABOGADO', width: '20%', caption: 'Abogado Resolución', dataType: 'string' },
            { dataField: 'CM_RESOLUCION', width: '5%', caption: 'CM', dataType: 'string' },
            {
                caption: 'Ver doc.',
                width: '5%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'doc',
                        hint: 'Visualizar Documento',
                        onClick: function (e) {
                            window.open($("#SIM").data("url") + 'Correspondencia/Correspondencia/LeeDoc?CodTramite=' + options.row.data.CODTRAMITE + '&CodDocumento=' + options.row.data.CODDOCUMENTO, "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
                        }
                    }).appendTo(container);
                }
            }, {
                caption: 'Calcular Seguimiento',
                width: '5%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'rowfield',
                        hint: 'Calcular el valor del seguimiento',
                        onClick: function (e) {
                            TipoTra.option("value", null);
                            CantProf.option("value", 0);
                            CantVisitas.option("value", 0);
                            CantHorInf.option("value", 0);
                            DuracionVisita.option("value", 0);
                            $("#NombreTercero").text("");
                            //NomTercero.option("value", "");
                            DocTercero.option("value", "");
                            PopCalcular.show();
                            CodTramite = options.row.data.CODTRAMITE;
                            CodDocumento = options.row.data.CODDOCUMENTO;
                            if (options.row.data.CODTRAMITE > 0 && options.row.data.CODDOCUMENTO > -1) {
                                $("#DocumentoInforme").attr("src", $('#SIM').data('url') + 'Correspondencia/Correspondencia/LeeDoc?CodTramite=' + options.row.data.CODTRAMITE + '&CodDocumento=' + options.row.data.CODDOCUMENTO);
                            } else {
                                DevExpress.ui.dialog.alert("El registro no posee un codigo de trámite o código de documento");
                            }
                        }
                    }).appendTo(container);
                }
            }, {
                caption: 'Detalle',
                width: '5%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'event',
                        hint: 'Ver detalle del trámite',
                        onClick: function (e) {
                            CodTramite = options.row.data.CODTRAMITE;
                            popupDetalles.show();
                        }
                    }).appendTo(container);
                }
            }
        ]
    });


    var CantProf = $("#CantProf").dxNumberBox({
        value: 0
    }).dxNumberBox("instance");

    var CantVisitas = $("#CantVisitas").dxNumberBox({
        value: 0
    }).dxNumberBox("instance");

    var CantHorInf = $("#CantHorInf").dxNumberBox({
        value: 0
    }).dxNumberBox("instance");

    var DuracionVisita = $("#DuracionVisita").dxNumberBox({
        value: 0
    }).dxNumberBox("instance");

    var CM = $("#CM").dxTextBox({
        value: ""
    }).dxTextBox("instance");

    var cmbAno = $("#cmbAno").dxSelectBox({
        items: Agnos,
        value: Agnos[0]
    }).dxSelectBox("instance");

    var CantItem = $("#CantItem").dxNumberBox({
        value: 1
    }).dxNumberBox("instance");

    var CantNormas = $("#CantNormas").dxNumberBox({
        value: 0,
        visible: false
    }).dxNumberBox("instance");

    var CantLineas = $("#CantLineas").dxNumberBox({
        value: 0,
        visible: false
    }).dxNumberBox("instance");

    var popupDetalles = $("#popDetalles").dxPopup({
        height: 600,
        width: 1100,
        title: 'Detalle del trámite',
        visible: false,
        contentTemplate: function (container) {
            $("<iframe>").attr("src", $('#SIM').data('url') + 'Utilidades/DetalleTramite?popup=true&CodTramite=' + CodTramite ).attr("width", "100%").attr("height", "100%").attr("frameborder", "0").attr("scrolling", "0").appendTo(container);
        }
    }).dxPopup("instance");

    var DocTercero = $("#DocTercero").dxTextBox({
        value: "",
        showClearButton: true,
        placeholder: 'ingrese el documento',
        valueChangeEvent: 'focusout',
        onValueChanged: function (e) {
            var nombre = "";
            if (e.value > 0) {
                var _Ruta = $("#SIM").data("url") + "Facturacion/api/FacturApi/NombreTercero";
                $.getJSON(_Ruta, { Documento: e.value })
                    .done(function (data) {
                        if (data != "") {
                            nombre = data;
                            $("#NombreTercero").text(nombre);
                        }
                    }).fail(function (jqxhr, textStatus, error) {
                        $("#NombreTercero").text("");
                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Asociar documento');
                    });
            } else {
                $("#NombreTercero").text("");
            }
        }
    }).dxTextBox("instance");

    var TipoTra = $("#sbTipoTramite").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "CODIGO_TRAMITE",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Facturacion/Seguimiento/TiposTramite");
                }
            })
        }),
        valueExpr: 'CODIGO_TRAMITE',
        displayExpr: 'NOMBRE',
        placeholder: 'Seleccione tipo de trámite',
        showClearButton: true,
        onSelectionChanged: function (e) {
            var item = e.component.option('selectedItem');
            TipoTramite = item.CODIGO_TRAMITE;
            if (TipoTramite == 26) {
                CantNormas.option("visible", true);
                CantLineas.option("visible", true);
                $("#lblCatNor").css("visibility", "visible");
                $("#lblCatLin").css("visibility", "visible");
            } else {
                CantNormas.option("visible", false);
                CantLineas.option("visible", false);
                $("#lblCatNor").css("visibility", "hidden");
                $("#lblCatLin").css("visibility", "hidden");
            }
            var URL = $("#SIM").data("url") + "Facturacion/Seguimiento/ParametrosSeguimiento?Tramite=" + TipoTramite;
            $.getJSON(URL, function (result) {
                CantProf.option("value", result.NumeroProfesionales);
                CantVisitas.option("value", result.NumeroVisitas);
                CantHorInf.option("value", result.HorasInforme);
                DuracionVisita.option("value", result.DuracionVisita);
                $("#lblItems").html(result.Unidad);
                CantNormas.option("value", 0);
                CantLineas.option("value", 0);
                $("#lblValorFactura").text("");
                $.getJSON($("#SIM").data("url") + 'Facturacion/Pendientes/ObtenerCMInforme?CodTramite=' + CodTramite + '&CodDocumento=' + CodDocumento, function (result) {
                    alert(result);
                    CM.option("value", result);
                });
            });

        }
    }).dxSelectBox("instance");

    var PopCalcular = $("#popCalcular").dxPopup({
        title: 'Calcular Seguimiento',
        fullScreen: true,
        toolbarItems: [{
            widget: 'dxButton',
            toolbar: 'bottom',
            location: 'center',
            options: {
                text: 'Calcular Seguimento',
                elementAttr: { class: 'dx-popup-content-bottom' },
                type: 'default',
                onClick: function (e) {
                    var sigue = true;
                    var Documento = DocTercero.option("value");
                    var NomTerc = $("#NombreTercero").text();
                    if (NomTerc != "") {
                        if (TipoTramite > 0 && Documento > 0) {
                            if (TipoTramite == 26) {
                                var CantNor = CantNormas.option("value");
                                var CantLin = CantLineas.option("value");
                                if (CantNor == 0 || CantLin == 0) {
                                    sigue = false;
                                    DevExpress.ui.dialog.alert('Debe ingresar la cantidad de Normas y Líneas', 'Calcular Seguimento');
                                }
                            }
                            var cm = CM.option("value");
                            cm = cm == null ? "" : cm;
                            var datosSeg = {
                                TipoTramite: TipoTramite,
                                DuracionVisita: DuracionVisita.option("value"),
                                HorasInforme: CantHorInf.option("value"),
                                NumeroVisitas: CantVisitas.option("value"),
                                TramitesSINA: 1,
                                NumeroProfesionales: CantProf.option("value"),
                                CM: cm,
                                Observaciones: "",
                                Items: CantItem.option("value"), 
                                Reliquidacion: 0,
                                NIT: Documento,
                                Tercero: NomTerc,
                                CodTramite: CodTramite,
                                CodDocumento: CodDocumento,
                                ConSoportes: 0,
                                CantNormas: CantNor,
                                CantLineas: CantLin,
                                Agno: cmbAno.option("value")
                            };
                        } else {
                            sigue = false;
                            DevExpress.ui.dialog.alert('Faltan datos para el calculo del seguimiento', 'Calcular Seguimento');
                        }
                    } else {
                        sigue = false;
                        DevExpress.ui.dialog.alert('Falta validar el nombre del tercero para el calculo del seguimiento', 'Calcular Seguimento');
                    }
                    if (sigue) {
                        var URL = $("#SIM").data("url") + 'Facturacion/Seguimiento/CalcularSeguimiento';
                        $.ajax({
                            type: "POST",
                            dataType: 'json',
                            url: URL,
                            data: JSON.stringify(datosSeg),
                            contentType: "application/json",
                            beforeSend: function () { },
                            success: function (data) {
                                $("#lblValorFactura").text("Valor a Facturar : " + data.TotalPagar);
                                dataPdf = 'data:application/pdf;base64,' + data.Soporte;
                                $("#DocumentoInforme").attr("src", dataPdf);
                            },
                            error: function (xhr, textStatus, errorThrown) {
                                DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Calcular Seguimento');
                            }
                        });
                    }
                }
            }
        }]

    }).dxPopup("instance");
});

var grdInfDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"CODTRAMITE, CODDOCUMENTO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + 'Facturacion/api/FacturApi/InformesTecnico', {
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

var grdResDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"CODTRAMITE, CODDOCUMENTO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + 'Facturacion/api/FacturApi/Resoluciones', {
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
