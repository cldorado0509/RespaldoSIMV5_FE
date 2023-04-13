$(document).ready(function () {
    var TipoTramite = -1;
    var Calculado = false;
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


    $("#grdListaSoprtesPago").dxDataGrid({
        dataSource: SoportesPagoDataSource,
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
            { dataField: 'ID_CALCULO', width: '5%', caption: 'Identificador', alignment: 'center' },
            { dataField: 'TIPO', width: '25%', caption: 'Tipo de Trámite', dataType: 'string' },
            { dataField: 'NIT', width: '10%', caption: 'Documento tercero', dataType: 'string' },
            { dataField: 'TERCERO', width: '35%', caption: 'Tercero', dataType: 'string' },
            { dataField: 'FECHA', width: '12%', caption: 'Fecha', dataType: 'date', format: 'MMM dd yyyy HH:mm' },
            { dataField: 'CONSECUTIVO', width: '8%', caption: 'Soporte Pago', dataType: 'string' },
            {
                width: '5%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'fields',
                        hint: 'Ver parámetros del cálculo',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/ValorTramiteApi/ParametrosCalculo";
                            $.getJSON(_Ruta, { IdCalculo: options.data.ID_CALCULO })
                                .done(function (data) {
                                    if (data != null) {
                                        if (data.IdCalculo == options.data.ID_CALCULO) {
                                            showParametros(data);
                                        } else {
                                            DevExpress.ui.dialog.alert('Ocurrió un error No se encontraron datos para el cálculo del valor del trámite', 'Parámetros Cálculo');
                                        }
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Parámetros Cálculo');
                                });
                        }
                    }).appendTo(container);
                }
            },
            {
                width: '5%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'doc',
                        hint: 'Ver soporte de pago',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/ValorTramiteApi/ExisteSoporte";
                            $.getJSON(_Ruta, { IdCalculo: options.data.ID_CALCULO })
                                .done(function (data) {
                                    if (data.resp == "Error") {
                                        DevExpress.ui.dialog.alert(data.mensaje, 'Soporte Cálculo');
                                    } else {
                                        popSoporte.show();
                                        $("#DocSoporte").attr("src", $('#SIM').data('url') + 'AtencionUsuarios/ValorTramite/ObtieneSoporte?IdCalculo=' + options.row.data.ID_CALCULO);
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Soporte Cálculo');
                                });
                        }
                    }).appendTo(container);
                }
            }
        ]
    });

    var popupParam = null;

    var showParametros = function (data) {
        Parametro = data;
        if (popupParam) {
            popupParam.option("contentTemplate", popupOptions.contentTemplate.bind(this));
        } else {
            popupParam = $("#PopupParametros").dxPopup(popupOptions).dxPopup("instance");
        }
        popupParam.show();
    };

    var popupOptions = {
        width: 700,
        height: "auto",
        hoverStateEnabled: true,
        title: "Parámetros del cálculo del valor del trámite",
        visible: false,
        closeOnOutsideClick: true,
        contentTemplate: function () {
            const formatter = new Intl.NumberFormat('sp-CO', {
                style: 'currency', currency: 'COP',
                minimumFractionDigits: 0,
                maximumFractionDigits: 0
            });
            if (Parametro.Calculado == true) {
                return $("<div />").append(
                    $("<div class='row col-md-12'><p style='text-align: center'><b>CÁLCULO DEL VALOR DE LA EVALUACIÓN</b></p></div><br />"),
                    $("<div class='row'><div class='col-md-8'><p><b>ITEM</b></p></div><div class='col-md-4'><p><b>VALOR</b></p></div></div>"),
                    $("<div class='row'><div class='col-md-8'><p>GASTOS POR SUELDOS Y HONORARIOS (A):</p></div><div class='col-md-4'><p>" + formatter.format(Parametro.Sueldos) + "</p></div>"),
                    $("<div class='row'><div class='col-md-8'><p>GASTOS DE VIAJE (B):</p></div><div class='col-md-4'><p>" + formatter.format(Parametro.Viajes) + "</p></div>"),
                    $("<div class='row'><div class='col-md-8'><p>GASTOS ANÁLISIS DE LABORATORIO Y OTROS TRABAJOS TÉCNICOS (C):</p></div><div class='col-md-4'><p>" + formatter.format(Parametro.Otros) + "</p></div>"),
                    $("<div class='row'><div class='col-md-8'><p>GASTOS DE ADMINISTRACIÓN 25% (D):</p></div><div class='col-md-4'><p>" + formatter.format(Parametro.Admin) + "</p></div><br />"),
                    $("<div class='row'><div class='col-md-8'><b><p>COSTO TOTAL DE LA TARIFA :</p></div><div class='col-md-4'><p>" + formatter.format(Parametro.Costo) + "</p></b></div>"),
                    $("<div class='row'><div class='col-md-8'><p>DETERMINACIÓN DE LOS TOPES DE LAS TARIFAS (To):</p></div><div class='col-md-4'><p>" + formatter.format(Parametro.Topes) + "</p></div>"),
                    $("<div class='row'><div class='col-md-8'><p><b>VALOR A CANCELAR POR TRÁMITE:</p></div><div class='col-md-4'><p>" + formatter.format(Parametro.Valor) + "</p></b></div>"),
                    $("<div class='row'><div class='col-md-8'><p><b>VALOR A CANCELAR POR PUBLICACIÓN:</p></div><div class='col-md-4'><p>" + formatter.format(Parametro.Publicacion) + "</p></b></div>")
                );
            } else {
                return $("<div />").append(
                    $("<div class='row col-md-12'><p style='text-align: center'><b>CÁLCULO DEL VALOR DE LA EVALUACIÓN</b></p></div><br />"),
                    $("<div class='row'><div class='col-md-12'><p>" + Parametro.Mensaje + "</p></div>")             
                );
            }
        }
    };

    var popSoporte = $("#popSoporte").dxPopup({
        title: 'Soporte Calculo Tramite',
        width: 700,
        height: 800,
        closeOnOutsideClick: true
    }).dxPopup("instance");

    var DocTercero = $("#DocTercero").dxTextBox({
        value: "",
        showClearButton: true,
        placeholder: 'ingrese el documento',
        valueChangeEvent: 'focusout',
        onValueChanged: function (e) {
            var nombre = "";
            if (e.value > 0) {
                var _Ruta = $("#SIM").data("url") + "AtencionUsuarios/api/ValorTramiteApi/NombreTercero";
                $.getJSON(_Ruta, { Documento: e.value })
                    .done(function (data) {
                        if (data != "") {
                            if (data.includes(";")) {
                                const ArrResp = data.split(";");
                                nombre = ArrResp[0];
                                $("#NombreTercero").text(nombre);
                                DevExpress.ui.dialog.alert(ArrResp[1], 'Tercero');
                            } else { 
                                nombre = data;
                                $("#NombreTercero").text(nombre);
                            }
                        }
                    }).fail(function (jqxhr, textStatus, error) {
                        $("#NombreTercero").text("");
                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Tercero');
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
                    return $.getJSON($("#SIM").data("url") + "AtencionUsuarios/api/ValorTramiteApi/TiposTramiteEvaluacion");
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
            Calculado = false;
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
            var URL = $("#SIM").data("url") + "AtencionUsuarios/api/ValorTramiteApi/ParametrosEvaluacion?IdTramite=" + TipoTramite;
            $.getJSON(URL, function (result) {
                CantProf.option("value", result.NumeroProfesionales);
                CantVisitas.option("value", result.NumeroVisitas);
                CantHorInf.option("value", result.HorasInforme);
                DuracionVisita.option("value", result.DuracionVisita);
                $("#lblItems").html(result.Unidad);
                CantNormas.option("value", 0);
                CantLineas.option("value", 0);
                $("#lblValorFactura").text("");
            });

        }
    }).dxSelectBox("instance");

    var CantProf = $("#CantProf").dxNumberBox({
        value: 0,
        onValueChanged() {
            Calculado = false;
        }
    }).dxNumberBox("instance");

    var CantVisitas = $("#CantVisitas").dxNumberBox({
        value: 0,
        onValueChanged() {
            Calculado = false;
        }
    }).dxNumberBox("instance");

    var CantHorInf = $("#CantHorInf").dxNumberBox({
        value: 0,
        onValueChanged() {
            Calculado = false;
        }
    }).dxNumberBox("instance");

    var DuracionVisita = $("#DuracionVisita").dxNumberBox({
        value: 0,
        onValueChanged() {
            Calculado = false;
        }
    }).dxNumberBox("instance");

    var CM = $("#CM").dxTextBox({
        value: ""
    }).dxTextBox("instance");

    var cmbAno = $("#cmbAno").dxSelectBox({
        items: Agnos,
        value: Agnos[0]
    }).dxSelectBox("instance");

    var CantItem = $("#CantItem").dxNumberBox({
        value: 1,
        onValueChanged(data) {
            var TipoTra = TipoTra.option("value");
            Calculado = false;
            if (TipoTra == 26) {
                var Normas = CantNormas.option("value");
                var Lineas = CantLineas.option("value");
                var horasinfE = (2 * (data.value + (5 * (Normas))));
                var duracionVE = (2 * data.value) + (2 * Normas);
                if (Lineas > 2) {
                    horasinfE += (Lineas - 2);
                    duracionVE += (Lineas - 2);
                }
                CantHorInf.option("value", Math.ceil(horasinfE));
                DuracionVisita.option("value", Math.ceil(duracionVE));
            } 
        }
    }).dxNumberBox("instance");

    var CantNormas = $("#CantNormas").dxNumberBox({
        value: 0,
        visible: false,
        onValueChanged() {
           Calculado = false;
        }
    }).dxNumberBox("instance");

    var CantLineas = $("#CantLineas").dxNumberBox({
        value: 0,
        visible: false,
        onValueChanged() {
            Calculado = false;
        }
    }).dxNumberBox("instance");

    var ValProy = $("#ValProy").dxNumberBox({
        format: '$ #,##0.##',
        value: 0,
        onValueChanged() {
            Calculado = false;
        }
    }).dxNumberBox("instance");

    var ValPublica = $("#ValPublica").dxNumberBox({
        format: '$ #,##0.##',
        value: 0
    }).dxNumberBox("instance");

    var CantTram = $("#CantTram").dxNumberBox({
        value: 1,
        min: 1,
        max: 10,
        onValueChanged() {
            Calculado = false;
        }
    }).dxNumberBox("instance");

    var CheckSoportes = $("#chkSoportes").dxCheckBox({
        value: undefined,
        onValueChanged() {
            Calculado = false;
        }
    }).dxCheckBox("instance");

    var Descrip = $("#txtDescripcion").dxTextArea({
        value: '',
        height: 90,
        autoResizeEnabled: true
    }).dxTextArea('instance');

    $("#btnNuevoCalculo").dxButton({
        text: "Nuevo Cálculo Valor Trámite",
        icon: "money",
        hint: 'Generar nuevo cálculo del valor de un trámite',
        type: 'default',
        onClick: function () {
            TipoTra.option("value", null);
            CantProf.option("value", 0);
            CantVisitas.option("value", 0);
            CantHorInf.option("value", 0);
            DuracionVisita.option("value", 0);
            $("#NombreTercero").text("");
            ValProy.option("value", 0);
            ValPublica.option("value", 0);
            CantTram.option("value", 1);
            CheckSoportes.option("value", false);
            DocTercero.option("value", "");
            Descrip.option("value", "");
            Calculado = false;
            PopCalcular.show();
        }
    });

    var PopCalcular = $("#popCalcular").dxPopup({
        title: 'Calcular Valor del Trámite – Evaluación',
        fullScreen: true,
        toolbarItems: [{
            widget: 'dxButton',
            toolbar: 'bottom',
            location: 'center',
            options: {
                text: 'Calcular Valor Trámite',
                elementAttr: { class: 'dx-popup-content-bottom' },
                type: 'default',
                onClick: function (e) {
                    var sigue = true;
                    Calculado = false;
                    var Documento = DocTercero.option("value");
                    if (Documento == "") {
                        sigue = false;
                        DevExpress.ui.dialog.alert('Debe ingresar el documento del tercero', 'Calcular Valor Trámite');
                    }
                    var NomTerc = $("#NombreTercero").text();
                    if (NomTerc == "") {
                        sigue = false;
                        DevExpress.ui.dialog.alert('Falta validar el nombre del tercero para el calculo del Trámite', 'Calcular Valor Trámite');
                    }
                    if (TipoTramite > 0) {
                        if (TipoTramite == 26) {
                            var CantNor = CantNormas.option("value");
                            var CantLin = CantLineas.option("value");
                            if (CantNor == 0 || CantLin == 0) {
                                sigue = false;
                                DevExpress.ui.dialog.alert('Debe ingresar la cantidad de Normas y Líneas', 'Calcular Valor Trámite');
                            }
                        }
                    } else {
                        sigue = false;
                        DevExpress.ui.dialog.alert('Debe seleccionar el tipo de Trámite', 'Calcular Valor Trámite');
                    }
                    if (CantProf.option("value") == "") {
                        sigue = false;
                        DevExpress.ui.dialog.alert('Debe ingresar la cantidad de profesionales técnicos que participaran en el trámite', 'Calcular Valor Trámite');
                    }
                    if (ValProy.option("value") == "") {
                        sigue = false;
                        DevExpress.ui.dialog.alert('Debe ingresar el valor del proyecto', 'Calcular Valor Trámite');
                    }
                    if (DuracionVisita.option("value") == "") {
                        sigue = false;
                        DevExpress.ui.dialog.alert('Debe ingresar el valor de duración de visita', 'Calcular Valor Trámite');
                    }
                    if (CantVisitas.option("value") == "") {
                        sigue = false;
                        DevExpress.ui.dialog.alert('Debe ingresar el valor de cantidad de visitas', 'Calcular Valor Trámite');
                    }
                    if (CantHorInf.option("value") == "") {
                        sigue = false;
                        DevExpress.ui.dialog.alert('Debe ingresar el valor de cantidad de horas por informe', 'Calcular Valor Trámite');
                    }
                    if (CantTram.option("value") == "") {
                        sigue = false;
                        DevExpress.ui.dialog.alert('Debe ingresar la cantidad de trámites SINA', 'Calcular Valor Trámite');
                    }
                    var cm = CM.option("value");
                    cm = cm == null ? "" : cm;
                    var chkSop = CheckSoportes.option("value") ? 1 : 0;

                    if (sigue) {
                        var datosEvaluacion = {
                            TipoTramite: TipoTramite,
                            DuracionVisita: DuracionVisita.option("value"),
                            HorasInforme: CantHorInf.option("value"),
                            NumeroVisitas: CantVisitas.option("value"),
                            TramitesSINA: CantTram.option("value"),
                            NumeroProfesionales: CantProf.option("value"),
                            CM: cm,
                            Observaciones: Descrip.option("value"),
                            Items: CantItem.option("value"),
                            Reliquidacion: 0,
                            NIT: Documento,
                            Tercero: NomTerc,
                            ConSoportes: chkSop,
                            CantNormas: CantNor,
                            CantLineas: CantLin,
                            Agno: cmbAno.option("value"),
                            ValorProyecto: ValProy.option("value"),
                            ValorPublicacion: ValPublica.option("value")
                        };

                        var URL = $("#SIM").data("url") + 'AtencionUsuarios/api/ValorTramiteApi/CalcularValorTramite';
                        $.ajax({
                            type: "POST",
                            dataType: 'json',
                            url: URL,
                            data: JSON.stringify(datosEvaluacion),
                            contentType: "application/json",
                            beforeSend: function () { },
                            success: function (data) {
                                if (data != null) {
                                    showParametros(data);
                                    Calculado = true;
                                }
                            },
                            error: function (xhr, textStatus, errorThrown) {
                                DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Calcular Valor Trámite');
                            }
                        });
                    }
                }
            }
        }, {
            widget: 'dxButton',
            toolbar: 'bottom',
            location: 'center',
            options: {
                text: 'Generar Soporte Pago',
                elementAttr: { class: 'dx-popup-content-bottom' },
                type: 'default',
                onClick: function (e) {
                    if (!Calculado) {
                        DevExpress.ui.dialog.alert('Ocurrió un problema : Aún no se ha realizado el cálculo del valor del trámite!', 'Calcular Valor Trámite');
                    } else {
                        var sigue = true;
                        Calculado = false;
                        var Documento = DocTercero.option("value");
                        if (Documento == "") {
                            sigue = false;
                            DevExpress.ui.dialog.alert('Debe ingresar el documento del tercero', 'Calcular Valor Trámite');
                        }
                        var NomTerc = $("#NombreTercero").text();
                        if (NomTerc == "") {
                            sigue = false;
                            DevExpress.ui.dialog.alert('Falta validar el nombre del tercero para el calculo del Trámite', 'Calcular Valor Trámite');
                        }
                        if (TipoTramite > 0) {
                            if (TipoTramite == 26) {
                                var CantNor = CantNormas.option("value");
                                var CantLin = CantLineas.option("value");
                                if (CantNor == 0 || CantLin == 0) {
                                    sigue = false;
                                    DevExpress.ui.dialog.alert('Debe ingresar la cantidad de Normas y Líneas', 'Calcular Valor Trámite');
                                }
                            }
                        } else {
                            sigue = false;
                            DevExpress.ui.dialog.alert('Debe seleccionar el tipo de Trámite', 'Calcular Valor Trámite');
                        }
                        if (CantProf.option("value") == "") {
                            sigue = false;
                            DevExpress.ui.dialog.alert('Debe ingresar la cantidad de profesionales técnicos que participaran en el trámite', 'Calcular Valor Trámite');
                        }
                        if (ValProy.option("value") == "") {
                            sigue = false;
                            DevExpress.ui.dialog.alert('Debe ingresar el valor del proyecto', 'Calcular Valor Trámite');
                        }
                        if (DuracionVisita.option("value") == "") {
                            sigue = false;
                            DevExpress.ui.dialog.alert('Debe ingresar el valor de duración de visita', 'Calcular Valor Trámite');
                        }
                        if (CantVisitas.option("value") == "") {
                            sigue = false;
                            DevExpress.ui.dialog.alert('Debe ingresar el valor de cantidad de visitas', 'Calcular Valor Trámite');
                        }
                        if (CantHorInf.option("value") == "") {
                            sigue = false;
                            DevExpress.ui.dialog.alert('Debe ingresar el valor de cantidad de horas por informe', 'Calcular Valor Trámite');
                        }
                        if (CantTram.option("value") == "") {
                            sigue = false;
                            DevExpress.ui.dialog.alert('Debe ingresar la cantidad de trámites SINA', 'Calcular Valor Trámite');
                        }
                        var cm = CM.option("value");
                        cm = cm == null ? "" : cm;
                        var chkSop = CheckSoportes.option("value") ? 1 : 0;

                        if (sigue) {
                            var datosEvaluacion = {
                                TipoTramite: TipoTramite,
                                DuracionVisita: DuracionVisita.option("value"),
                                HorasInforme: CantHorInf.option("value"),
                                NumeroVisitas: CantVisitas.option("value"),
                                TramitesSINA: CantTram.option("value"),
                                NumeroProfesionales: CantProf.option("value"),
                                CM: cm,
                                Observaciones: Descrip.option("value"),
                                Items: CantItem.option("value"),
                                Reliquidacion: 0,
                                NIT: Documento,
                                Tercero: NomTerc,
                                ConSoportes: chkSop,
                                CantNormas: CantNor,
                                CantLineas: CantLin,
                                Agno: cmbAno.option("value"),
                                ValorProyecto: ValProy.option("value"),
                                ValorPublicacion: ValPublica.option("value")
                            };

                            var URL = $("#SIM").data("url") + 'AtencionUsuarios/api/ValorTramiteApi/ImprimeCalculoTramite';
                            $.ajax({
                                type: "POST",
                                dataType: 'json',
                                url: URL,
                                data: JSON.stringify(datosEvaluacion),
                                contentType: "application/json",
                                beforeSend: function () { },
                                success: function (data) {
                                    if (data != null) {
                                        if (data.Mensaje != "") {
                                            showParametros(data);
                                        } else {
                                            PopCalcular.hide();
                                            $('#grdListaSoprtesPago').dxDataGrid("instance").getDataSource().load();

                                            if (data.IdCalculo > 0) {
                                                popSoporte.show();
                                                $("#DocSoporte").attr("src", $('#SIM').data('url') + 'AtencionUsuarios/ValorTramite/ObtieneSoporte?IdCalculo=' + data.IdCalculo);
                                            }
                                        }
                                    }
                                },
                                error: function (xhr, textStatus, errorThrown) {
                                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Calcular Seguimento');
                                }
                            });
                        }
                    }
                }
            }
        }]

    }).dxPopup("instance");

});

var SoportesPagoDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ID_CALCULO","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'AtencionUsuarios/api/ValorTramiteApi/ObtienSoportes', {
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
