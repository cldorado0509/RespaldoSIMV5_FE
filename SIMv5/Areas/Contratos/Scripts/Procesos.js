﻿$(document).ready(function () {
    var IdRegistro = -1;
    var Proceso = null;

    $("#GidListado").dxDataGrid({
        dataSource: ProcesosDataSource,
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
            { dataField: 'ID_PROCESO', width: '5%', caption: 'Código', alignment: 'center' },
            { dataField: 'MODALIDAD', width: '15%', caption: 'Modalidad', dataType: 'string' },
            { dataField: 'S_NOMBRE', width: '30%', caption: 'Nombre del Proceso', dataType: 'syring' },
            { dataField: 'FUNCIONARIO', width: '30%', caption: 'Funcionario Responsable', dataType: 'string' },
            { dataField: 'B_SOBRE_SELLADO', width: '7%', caption: 'Sobre Sellado', dataType: 'string' },
            { dataField: 'D_CIERREPROPUESTAS', width: '13%', caption: 'Fecha cierre propuestas', dataType: 'date', format: 'MMM dd yyyy HH:mm' },
            { dataFiled: "CODFUNCIONARIO", dataType: 'number', visible: false, allowSearch: false },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                        $('<div/>').dxButton({
                            icon: '../Content/Images/VerDetalle.png',
                            height: 20,
                            hint: 'Ver detalles del proceso',
                            onClick: function (e) {
                                var _Ruta = $('#SIM').data('url') + "Contratos/api/PropuestaApi/ObtenerProceso";
                                $.getJSON(_Ruta, { IdProceso: options.data.ID_PROCESO })
                                    .done(function (data) {
                                        if (data != null) {
                                            var OptSellado = data.SobreSellado == "1" ? "SI" : "NO";
                                            data.PptaEconomica = data.PptaEconomica == "1" ? "SI" : "NO";
                                            data.SobreSellado = OptSellado;
                                            data.Subidas = data.FechaSIM != "" ? data.Subidas : "0";
                                            var DatoMod = data.Modalidad.split(";");
                                            data.Modalidad = DatoMod[1];
                                            showProceso(data);                                            
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
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.data.CODFUNCIONARIO == CodigoFuncionario) {
                        $('<div/>').dxButton({
                            icon: '../Content/Images/edit.png',
                            height: 20,
                            hint: 'Editar datos del proceso',
                            onClick: function (e) {
                                var _Ruta = $('#SIM').data('url') + "Contratos/api/PropuestaApi/ObtenerProceso";
                                $.getJSON(_Ruta,
                                    {
                                        IdProceso: options.data.ID_PROCESO
                                    }).done(function (data) {
                                        if (data != null) {
                                            IdRegistro = parseInt(data.IdProceso);
                                            txtNombre.option("value", data.Nombre);
                                            txtObjeto.option("value", data.Objeto);
                                            var FecInicia = data.FechaInicio != null ? new Date(data.FechaInicio) : "";
                                            dpFechaInicia.option("value", FecInicia);
                                            var FecCierre = data.FechaCierre != "" ? new Date(data.FechaCierre) : "";
                                            dpFechaCierre.option("min", FecInicia);
                                            dpFechaCierre.option("value", FecCierre);
                                            var OptSellado = data.SobreSellado == "1" ? true : false;
                                            var OptPptaEco = data.PptaEconomica == "1" ? true : false;
                                            chkSellado.option("value", OptSellado);
                                            chkPptaEco.option("value", OptPptaEco);
                                            var DatoMod = data.Modalidad.split(";");
                                            var DataModalidad = { "IdModalidad": parseInt(DatoMod[0]), "Modalidad": DatoMod[1] };
                                            cbModalidad.option("value", DataModalidad);
                                            var FecApertura = data.FechaApertura != "" ? new Date(data.FechaApertura) : new Date();
                                            dpFechaApertura.option("value", FecApertura);
                                            if (OptSellado) dpFechaApertura.option("disabled", false);
                                            else dpFechaApertura.option("disabled", true);
                                            var FecAprEco = data.FechaApeEco != "" ? new Date(data.FechaApeEco) : new Date();
                                            dpFechaAprEco.option("value", FecAprEco);
                                            if (OptPptaEco) dpFechaAprEco.option("disabled", false);
                                            else dpFechaAprEco.option("disabled", true);
                                            $("#lblPropuestas").html(data.Propuestas);
                                            popup.show();
                                        }
                                    }).fail(function (jqxhr, textStatus, error) {
                                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');
                                    });
                            }
                        }).appendTo(container);
                    }
                }
            },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.data.CODFUNCIONARIO == CodigoFuncionario) {
                        $('<div/>').dxButton({
                            icon: '../Content/Images/Delete.png',
                            height: 20,
                            hint: 'Eliminar el proceso',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Desea eliminar el proceso ' + options.data.S_NOMBRE + '?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {
                                        var _Ruta = $('#SIM').data('url') + "Contratos/api/PropuestaApi/EliminaProceso";
                                        $.getJSON(_Ruta,
                                            {
                                                objData: options.data.ID_PROCESO
                                            }).done(function (data) {
                                                if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar proceso');
                                                else {
                                                    $('#GidListado').dxDataGrid({ dataSource: ProcesosDataSource });
                                                }
                                            }).fail(function (jqxhr, textStatus, error) {
                                                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');
                                            });
                                    }
                                });
                            }
                        }).appendTo(container);
                    }
                }
            },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.data.CODFUNCIONARIO == CodigoFuncionario) {
                        $('<div/>').dxButton({
                            icon: '../Content/Images/Enviar_Doc.png',
                            height: 20,
                            hint: 'Editar datos del proceso',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Desea registrar en Gestión Documental del SIM todos los documentos abiertos de este proceso?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {
                                        loadPanel.show();
                                        var _Ruta = $('#SIM').data('url') + "Contratos/api/PropuestaApi/RegistraPropuestas";
                                        $.getJSON(_Ruta, { IdProceso: options.data.ID_PROCESO })
                                            .done(function (data) {
                                                loadPanel.hide();
                                                if (data.RegistroCorrecto) {
                                                    DevExpress.ui.dialog.alert(data.Mensaje, 'Subir Propuestas');
                                                    $('#GidListado').dxDataGrid({ dataSource: ProcesosDataSource });
                                                } else {
                                                    DevExpress.ui.dialog.alert('Ocurrió un error : ' + data.Mensaje, 'Subir Propuestas');
                                                }
                                            }).fail(function (jqxhr, textStatus, error) {
                                                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Subir Propuestas');
                                            });
                                    }
                                });
                            }
                        }).appendTo(container);
                    }
                }
            }
        ]
    });

    var popupDet = null;

    var showProceso = function (data) {
        Proceso = data;
        if (popupDet) {
            popupDet.option("contentTemplate", popupOptions.contentTemplate.bind(this));
        } else {
            popupDet = $("#PopupDetalleProceso").dxPopup(popupOptions).dxPopup("instance");
        }
        popupDet.show();
    };

    var txtNombre = $("#txtNombre").dxTextBox({
        placeholder: "Ingrese el nombre del proceso aqui...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El nombre del proceso es obligatorio"
        }]
        }).dxTextBox("instance");

    var txtObjeto = $("#txtObjeto").dxTextArea({
        value: "",
        height: 90
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El objeto del proceso es obligatorio"
        }]
        }).dxTextArea("instance");  

    var dpFechaInicia = $("#dpFechaInicia").dxDateBox({
        type: "datetime",
        value: new Date(),
        showAnalogClock: false,
        onOpened: function (args) {
            let position = args.component._popup.option("position");
            position.my = "center";
            position.at = "left";
            args.component._popup.option("position", position);
        }
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La fecha de inicio de recepción de propuestas es obligatorio"
        }]
    }).dxDateBox("instance");

    var dpFechaCierre = $("#dpFechaCierre").dxDateBox({
        type: "datetime",
        value: new Date(),
        showAnalogClock: false,
        onOpened: function (args) {
            let position = args.component._popup.option("position");
            position.my = "center";
            position.at = "left";
            args.component._popup.option("position", position);
        }
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La fecha de cierre de recepción de propuestas es obligatorio"
        }]
        }).dxDateBox("instance");

    var chkSellado = $("#chkSobreSellado").dxCheckBox({
        value: true,
        width: 80,
        text: "Si",
        onValueChanged: function (data) {
            if (data.value) {
                chkPptaEco.option("disabled", false);
                dpFechaApertura.option("disabled", false);
            } else {
                chkPptaEco.option("disabled", true);
                chkPptaEco.option("value", false);
                dpFechaApertura.option("disabled", true);
            }
        }
    }).dxCheckBox("instance");

    var chkPptaEco = $("#chkPropuestaEco").dxCheckBox({
        value: false,
        width: 80,
        text: "Si",
        onValueChanged: function (data) {
            if (data.value) {
                dpFechaAprEco.option("disabled", false);
            } else {
                dpFechaAprEco.option("disabled", true);
            }
        }
    }).dxCheckBox("instance");

    var cbModalidad = $("#cbModalidad").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "IdModalidad",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Contratos/api/PropuestaApi/Modalidad");
                }
            })
        }),
        displayExpr: "Modalidad",
        searchEnabled: true
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La modalidad del proceso es obligatorio"
        }]
        }).dxSelectBox("instance");

    var dpFechaApertura = $("#dpFechaApertura").dxDateBox({
        type: "datetime",
        value: new Date(),
        showAnalogClock: false,
        onOpened: function (args) {
            let position = args.component._popup.option("position");
            position.my = "center";
            position.at = "left";
            args.component._popup.option("position", position);
        }
    }).dxDateBox("instance");

    var dpFechaAprEco = $("#dpFechaAprEco").dxDateBox({
        type: "datetime",
        value: new Date(),
        showAnalogClock: false,
        onOpened: function (args) {
            let position = args.component._popup.option("position");
            position.my = "center";
            position.at = "left";
            args.component._popup.option("position", position);
        }
    }).dxDateBox("instance");

    $("#btnGuarda").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var nombre = txtNombre.option("value");
            var objeto = txtObjeto.option("value");
            var fechainicio = dpFechaInicia.option("value");
            var fechacierre = dpFechaCierre.option("value");
            if (fechacierre < fechainicio) {
                DevExpress.ui.dialog.alert('Advertencia : El rango de fechas para recepcion de propuestas esta mal establedcido!', 'Guardar Datos');
            } else {
                var modalidad = cbModalidad.option("value").IdModalidad;
                var sobresellado = chkSellado.option("value");
                var propeco = chkPptaEco.option("value");
                var fechaapertura = dpFechaApertura.option("value");
                var fechaaperteco = dpFechaAprEco.option("value");
                var propuestas = $("#lblPropuestas").val() != "" ? $("#lblPropuestas").val() : "0";
                sobresellado = sobresellado == true ? "1" : "0";
                propeco = propeco == true ? "1" : "0";
                var params = { IdProceso: IdRegistro, Nombre: nombre, Objeto: objeto, FechaInicio: fechainicio, FechaCierre: fechacierre, Modalidad: modalidad, SobreSellado: sobresellado, FechaApertura: fechaapertura, FechaApeEco: fechaaperteco, Funcionario: CodigoFuncionario, Propuestas: propuestas, PptaEconomica: propeco };
                var _Ruta = $('#SIM').data('url') + "Contratos/api/PropuestaApi/GuardaProceso";
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: _Ruta,
                    data: JSON.stringify(params),
                    contentType: "application/json",
                    beforeSend: function () { },
                    success: function (data) {
                        if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Guardar Datos');
                        else {
                            DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');
                            $('#GidListado').dxDataGrid({ dataSource: ProcesosDataSource });
                            $("#PopupNuevoProceso").dxPopup("instance").hide();
                        }
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                    }
                });
            }
        }
    });

    var popup = $("#PopupNuevoProceso").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Proceso para recepción de propuestas"
    }).dxPopup("instance");

    var popupOptions = {
        width: 700,
        height: "auto",
        hoverStateEnabled: true,
        title: "Proceso para recepción de propuestas",
        visible: false,
        closeOnOutsideClick: true,
        contentTemplate: function () {
            return $("<div />").append(
                $("<p>Funcionario responsable : <span><b>" + Proceso.Funcionario + "</b></span></p>"),
                $("<p>Nombre del proceso : <span><b>" + Proceso.Nombre + "</b></span></p>"),
                $("<p>Objeto del proceso : <span><b>" + Proceso.Objeto + "</b></span></p>"),
                $("<p>Fecha de inicio de propuestas : <span><b>" + Proceso.FechaInicio + "</b></span></p>"),
                $("<p>Fecha de cierre de propuestas : <span><b>" + Proceso.FechaCierre + "</b></span></p>"),
                $("<p>Modalidad del proceso : <span><b>" + Proceso.Modalidad + "</b></span></p>"),
                $("<p>Sobre sellado : <span><b>" + Proceso.SobreSellado + "</b></span></p>"),
                $("<p>Fecha de apertura sobres : <span><b>" + Proceso.FechaApertura + "</b></span></p>"),
                $("<p>Propuesta económica independiente : <span><b>" + Proceso.PptaEconomica + "</b></span></p>"),
                $("<p>Fecha apertura propuesta económica : <span><b>" + Proceso.FechaApeEco + "</b></span></p>"),
                $("<p>Cantidad de propuestas presentadas : <span><b>" + Proceso.Propuestas + "</b></span></p>"),
                $("<p>Cantidad de propuestas pasadas al SIM : <span><b>" + Proceso.Subidas + "</b></span></p>"),
                $("<p>Fecha paso al SIM : <span><b>" + Proceso.FechaSIM + "</b></span></p>")
            );
        }
    };

    $("#btnNuevo").dxButton({
        stylingMode: "contained",
        text: "Nuevo Proceso",
        type: "success",
        width: 200,
        height: 30,
        icon: '../Content/Images/new.png',
        onClick: function () {
            IdRegistro = -1;
            txtNombre.reset();
            txtObjeto.reset();
            dpFechaInicia.option("value", new Date());
            dpFechaCierre.option("value", new Date());
            cbModalidad.option("value", 0);
            chkSellado.option("value", true);
            dpFechaApertura.option("disabled", false);
            dpFechaApertura.option("value", new Date());
            chkPptaEco.option("value", false);
            chkPptaEco.option("disabled", false);
            dpFechaAprEco.option("value", new Date());
            dpFechaAprEco.option("disabled", true);
            $("#lblPropuestas").text("0");
            popup.show();
        }
    });

    var loadPanel = $("#LoadingPanel").dxLoadPanel({
        shadingColor: "rgba(0,0,0,0.4)",
        visible: false,
        showIndicator: true,
        showPane: true,
        shading: true,
        closeOnOutsideClick: false
    }).dxLoadPanel("instance");
});

var ProcesosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"D_REGISTRO","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'Contratos/api/PropuestaApi/Procesos', {
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
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});