﻿var IdProceso = -1;
$(document).ready(function () {
    var Propuesta = null;
    var popupProp = null;
    var popupOpcPpta = null;
    var popupOpcEco = null;
    var TipoDoc = 0;
    var DatoDoc = null;

    $("#cbProcesos").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "IdProceso",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Contratos/api/PropuestaApi/ListaProcesos");
                }
            })
        }),
        displayExpr: "Nombre",
        searchEnabled: true,
        onValueChanged: function (data) {
            if (data.value != null) {
                $("#lblObjeto").html("<p style='tex-align: justify'>" + data.value.Objeto + "<p><br /><b>Proceso " + data.value.Sellado + " es de sobre sellado</b>");
                var GridPropuestas = $("#GridPropuestas").dxDataGrid("instance");
                GridPropuestas.option("visible", true);
                IdProceso = data.value.IdProceso;
                GridPropuestas.refresh();
                $("#btnDownProp").dxButton("instance").option("visible", true);
                $("#btnDownEco").dxButton("instance").option("visible", true);
            } 
        }
    });

    $("#GridPropuestas").dxDataGrid({
        dataSource: PropuestasDataSource,
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
            mode: "single"
        },
        export: {
            enabled: true,
            fileName: 'Lista de Propuestas Presentadas'
        },
        onExporting: function (e) {
            e.component.beginUpdate();
            e.component.columnOption("RADICADO", "visible", true);
        },
        onExported: function (e) {
            e.component.columnOption("RADICADO", "visible", false);
            e.component.endUpdate();
        },
        hoverStateEnabled: true,
        remoteOperations: true,
        visible: false,
        columns: [
            { dataField: 'D_REGISTRO', width: '15%', caption: 'Registro', alignment: 'left', dataType: 'date', format: 'MMM dd yyyy HH:mm:ss' },
            { dataField: 'PROPONENTE', width: '15%', caption: 'Proponente', dataType: 'string' },
            { dataField: 'DIRECCION', width: '15%', caption: 'Dirección proponente', dataType: 'string' },
            { dataField: 'TELEFONO', width: '10%', caption: 'Teléfono Proponente', dataType: 'string' },
            { dataField: 'EMAIL', width: '15%', caption: 'Correo electrónico Proponente', dataType: 'string' },
            { dataField: 'REPONSABLE', width: '20%', caption: 'Responsable Proponente', dataType: 'string' },
            { dataField: 'CODFUNCIONARIO', dataType: 'number', visible: false, allowSearch: false, allowExporting: false },
            { dataField: 'RADICADO', dataType: 'string', width: '20%', visible: false, allowSearch: false, allowExporting: true  },
            {
                dataField: "ABIERTO", width: '5%', caption: 'Abierto', dataType: 'string'
            },
            {
                width: 40,
                allowExporting: false,
                alignment: 'center',
                caption: 'Detalles',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: '../Content/Images/VerDetalle.png',
                        height: 30,
                        hint: 'Ver detalles del proceso',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Contratos/api/PropuestaApi/ObtenerPropuesta";
                            $.getJSON(_Ruta, { IdPropuesta: options.data.IDPROPUESTA })
                                .done(function (data) {
                                    if (data != null) {
                                        showPropuesta(data);
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');
                                });
                        }
                    }).appendTo(container);
                }
            },
            {
                width: 40,
                allowExporting: false,
                alignment: 'center',
                caption: 'Propuesta',
                cellTemplate: function (container, options) {
                    if (options.data.SOBRESELLADO == "1") {
                        if (options.data.CODFUNCIONARIO == CodigoFuncionario) {
                            if (options.data.ABIERTO == "NO") {
                                $("<div/>").dxButton({
                                    icon: '../Content/Images/Radicar_16.png',
                                    height: 30,
                                    hint: 'Subir y abrir la propuesta',
                                    onClick: function (e) {
                                        var result = DevExpress.ui.dialog.confirm('Desea abrir el documento de la prouesta de ' + options.data.PROPONENTE + '?', 'Confirmación');
                                        result.done(function (dialogResult) {
                                            if (dialogResult) {
                                                var _Ruta = $('#SIM').data('url') + "Contratos/api/PropuestaApi/AbrePropuesta";
                                                loadPanel.show();
                                                $.getJSON(_Ruta, { IdPropuesta: options.data.IDPROPUESTA })
                                                    .done(function (data) {
                                                        loadPanel.hide();
                                                        if (data.SubirCorrecto != "Error") {
                                                            DevExpress.ui.dialog.alert(data.Mensaje, 'Subir Documento');
                                                            $('#GridPropuestas').dxDataGrid({ dataSource: PropuestasDataSource });
                                                        } else {
                                                            DevExpress.ui.dialog.alert('Ocurrió un error : ' + data.Mensaje, 'Subir Documento');
                                                        }
                                                    }).fail(function (jqxhr, textStatus, error) {
                                                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Subir Documento');
                                                    });
                                            }
                                        });
                                    }
                                }).appendTo(container);;
                            } else if (options.data.ABIERTO == "SI") {
                                $("<div/>").dxButton({
                                    icon: '../Content/Images/Ver_Doc.png',
                                    height: 30,
                                    hint: 'Ver el documento de la propuesta',
                                    onClick: function (e) {
                                        Propuesta = options.data;
                                        popup = $("#popDocumento").dxPopup("instance");
                                        popup.option("contentTemplate", popupOpcPpta.contentTemplate.bind(Propuesta));
                                        $('#popDocumento').css({ 'visibility': 'visible' });
                                        $("#popDocumento").fadeTo("slow", 1);
                                        popup.show();
                                    }
                                }).appendTo(container);
                            }
                        }
                    } else if (options.data.SOBRESELLADO == "0") {
                        $("<div/>").dxButton({
                            icon: '../Content/Images/Ver_Doc.png',
                            height: 30,
                            hint: 'Ver el documento de la propuesta',
                            onClick: function (e) {
                                popup = $("#popDocumento").dxPopup("instance");
                                popup.option("contentTemplate", popupOpcPpta.contentTemplate.bind(options.data.IDPROPUESTA));
                                $('#popDocumento').css({ 'visibility': 'visible' });
                                $("#popDocumento").fadeTo("slow", 1);
                                popup.show();
                            }
                        }).appendTo(container);
                    }
                }
            },
            {
                width: 40,
                allowExporting: false,
                alignment: 'center',
                caption: 'económica',
                cellTemplate: function (container, options) {
                    if (options.data.TIENEECO == "SI") {
                        if (options.data.SOBRESELLADO == "1") {
                            if (options.data.CODFUNCIONARIO == CodigoFuncionario) {
                                if (options.data.ABIERTOECO == "NO") {
                                    $("<div/>").dxButton({
                                        icon: '../Content/Images/Radicar_16.png',
                                        height: 30,
                                        hint: 'Subir y abrir la propuesta',
                                        onClick: function (e) {
                                            var result = DevExpress.ui.dialog.confirm('Desea abrir el documento de la propuesta económica de ' + options.data.PROPONENTE + '?', 'Confirmación');
                                            result.done(function (dialogResult) {
                                                if (dialogResult) {
                                                    loadPanel.show();
                                                    var _Ruta = $('#SIM').data('url') + "Contratos/api/PropuestaApi/AbrePropEco";
                                                    $.getJSON(_Ruta, { IdPropuesta: options.data.IDPROPUESTA })
                                                        .done(function (data) {
                                                            loadPanel.hide();
                                                            if (data.SubirCorrecto != "Error") {
                                                                DevExpress.ui.dialog.alert(data.Mensaje, 'Subir Documento');
                                                                $('#GridPropuestas').dxDataGrid({ dataSource: PropuestasDataSource });
                                                            } else {
                                                                DevExpress.ui.dialog.alert('Ocurrió un error : ' + data.Mensaje, 'Subir Documento');
                                                            }
                                                        }).fail(function (jqxhr, textStatus, error) {
                                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Subir Documento');
                                                        });
                                                }
                                            });
                                        }
                                    }).appendTo(container);;
                                } else if (options.data.ABIERTOECO == "SI") {
                                    $("<div/>").dxButton({
                                        icon: '../Content/Images/Ver_Doc.png',
                                        height: 30,
                                        hint: 'Ver el documento de la propuesta económica',
                                        onClick: function (e) {
                                            Propuesta = options.data;
                                            popup = $("#popDocumento").dxPopup("instance");
                                            popup.option("contentTemplate", popupOpcEco.contentTemplate.bind(Propuesta));
                                            $('#popDocumento').css({ 'visibility': 'visible' });
                                            $("#popDocumento").fadeTo("slow", 1);
                                            popup.show();
                                        }
                                    }).appendTo(container);
                                }
                            } else {
                                if (options.data.ABIERTOECO == "SI") {
                                    $("<div/>").dxButton({
                                        icon: '../Content/Images/Ver_Doc.png',
                                        height: 30,
                                        hint: 'Ver el documento de la propuesta económica',
                                        onClick: function (e) {
                                            Propuesta = options.data;
                                            popup = $("#popDocumento").dxPopup("instance");
                                            popup.option("contentTemplate", popupOpcEco.contentTemplate.bind(Propuesta));
                                            $('#popDocumento').css({ 'visibility': 'visible' });
                                            $("#popDocumento").fadeTo("slow", 1);
                                            popup.show();
                                        }
                                    }).appendTo(container);
                                }
                            }
                        }
                    }
                }
            }
        ]
        //,
        //onContentReady: function (e) {
        //    e.component.selectRowsByIndexes(0);
        //}

    });

    var showPropuesta = function (data) {
        Propuesta = data;
        if (popupProp) {
            popupProp.option("contentTemplate", popupOptions.contentTemplate.bind(this));
        } else {
            popupProp = $("#popupPpta").dxPopup(popupOptions).dxPopup("instance");
        }
        popupProp.show();
    };

    var popupOpcPpta = {
        fullScreen: true,
        title: 'Documento de la propuesta radicado',
        closeOnOutsideClick: true,
        visible: false,
        contentTemplate: function (container) {
            var _ruta = $('#SIM').data('url') + 'Utilidades/Documento?url=' + $('#SIM').data('url') + "Contratos/Propuesta/LeeDoc?Propuesta=" + Propuesta.IDPROPUESTA;
            $("<iframe>").attr("src", _ruta).attr("width", "100%").attr("height", "100%").appendTo(container);
        }
    };

    var popupOpcEco = {
        fullScreen: true,
        title: 'Documento de la propuesta económica radicada',
        closeOnOutsideClick: true,
        visible: false,
        contentTemplate: function (container) {
            var _ruta = $('#SIM').data('url') + 'Utilidades/Documento?url=' + $('#SIM').data('url') + "Contratos/Propuesta/LeeDocEco?Propuesta=" + Propuesta.IDPROPUESTA;
            $("<iframe>").attr("src", _ruta).attr("width", "100%").attr("height", "100%").appendTo(container);
        }
    };


    $("#popDocumento").dxPopup({
        fullScreen: true,
        showTitle: true,
        title: "Documento de la propuesta radicado",
        dragEnabled: false,
        closeOnOutsideClick: true
    });

    var loadPanel = $("#LoadingPanel").dxLoadPanel({
        shadingColor: "rgba(0,0,0,0.4)",
        visible: false,
        showIndicator: true,
        showPane: true,
        shading: true,
        closeOnOutsideClick: false
    }).dxLoadPanel("instance");

    $("#btnDownProp").dxButton({
        stylingMode: "contained",
        text: " Descargar Propuesta",
        type: "success",
        width: 200,
        height: 30,
        visible: false,
        icon: '../Content/Images/descargar.png',
        onClick: function () {
            var Data = $("#GridPropuestas").dxDataGrid("instance").getSelectedRowsData();
            if (Data.length == 0) {
                DevExpress.ui.dialog.alert('No ha seleccionado una propuesta!', 'Propuestas');
            } else {
                if (Data[0].CODFUNCIONARIO == CodigoFuncionario) {
                    if (Data[0].ABIERTO == "SI") {
                        window.location.href = "Propuesta/DescargaPropuesta?Propuesta=" + Data[0].IDPROPUESTA;
                    } else {
                        DevExpress.ui.dialog.alert('La propuesta aún no ha sido abierta!', 'Propuestas');
                    }
                } else {
                    DevExpress.ui.dialog.alert('Usted no es el administrador de este proceso!', 'Propuestas');
                }
            }
        }
    });

    $("#btnDownEco").dxButton({
        stylingMode: "contained",
        text: "Descargar Propuesta Económica",
        type: "success",
        width: 300,
        height: 30,
        visible: false,
        icon: '../Content/Images/descargar.png',
        onClick: function () {
            var Data = $("#GridPropuestas").dxDataGrid("instance").getSelectedRowsData();
            if (Data.length == 0) {
                DevExpress.ui.dialog.alert('No ha seleccionado una propuesta!', 'Propuestas');
            } else {
                if (Data[0].TIENEECO == "SI") {
                    if (Data[0].CODFUNCIONARIO == CodigoFuncionario) {
                        if (Data[0].ABIERTOECO == "SI") {
                            window.location.href = "Propuesta/DescargaEconomica?Propuesta=" + Data[0].IDPROPUESTA;
                        } else {
                            DevExpress.ui.dialog.alert('La propuesta económica aún no ha sido abiera!', 'Propuestas');
                        }
                    } else {
                        DevExpress.ui.dialog.alert('Usted no es el administrador de este proceso!', 'Propuestas');
                    }
                } else {
                    DevExpress.ui.dialog.alert('Esta propuesta no contiene propuesta económica!', 'Propuestas');
                }
            }
        }
    });

    var popupOptions = {
        width: 700,
        height: "auto",
        hoverStateEnabled: true,
        title: "Propuesta para el proceso",
        visible: false,
        closeOnOutsideClick: true,
        contentTemplate: function () {
            return $("<div />").append(
                $("<p>Nombre del proceso : <span><b>" + Propuesta.Proceso + "</b></span></p>"),
                $("<p>Funcionario responsable : <span><b>" + Propuesta.Funcionario + "</b></span></p>"),
                $("<p>Nombre del proponente : <span><b>" + Propuesta.Proponente + "</b></span></p>"),
                $("<p>Documento del proponente : <span><b>" + Propuesta.Documento + "</b></span></p>"),
                $("<p>Dirección del proponente : <span><b>" + Propuesta.Direccion + "</b></span></p>"),
                $("<p>Teléfono del proponente : <span><b>" + Propuesta.Telefono + "</b></span></p>"),
                $("<p>Correo Eletrónico del proponente : <span><b>" + Propuesta.Correo + "</b></span></p>"),
                $("<p>Responsable por parte del proponente : <span><b>" + Propuesta.Responsable + "</b></span></p>"),
                $("<p>Correo electrónico del responsable : <span><b>" + Propuesta.CorreoResp + "</b></span></p>"),
                $("<p>El proceso es de sobre sellado : <span><b>" + Propuesta.SobreSellado + "</b></span></p>"),
                $("<p>Fecha de propuesta : <span><b>" + Propuesta.Fecha + "</b></span></p>"),
                $("<p>Estado del documento de la propuesta : <span><b>" + Propuesta.ArchivoPropuesta + "</b></span></p>"),
                $("<p>Radicado asignado al recibirlo : <span><b>" + Propuesta.Radicado + "</b></span></p>"),
                $("<p>Código de trámite asignado al recibirlo : <span><b>" + Propuesta.Tramite + "</b></span></p>")
            );
        }
    };

});

var PropuestasDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"D_REGISTRO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'Contratos/api/PropuestaApi/Propuestas', {
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
            Proceso: IdProceso
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});