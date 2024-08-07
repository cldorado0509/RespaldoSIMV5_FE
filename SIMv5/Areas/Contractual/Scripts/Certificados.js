var Tercero = -1;
$(document).ready(function () {

    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    });

    $("#txtDocumento").dxTextBox({
        placeholder: "Ingrese el documento o NIT",
        value: "",
        onChange: function (e) {
            if (e.component.option("value")) {
                $("#loadPanel").dxLoadPanel("instance").show();
                var _text = e.component.option("value");
                var _Ruta = $('#SIM').data('url') + "Contractual/api/CertificadosApi/ValidaTercero";
                $.getJSON(_Ruta, { doc: _text }).done(function (data) {
                    if (!data.existe) {
                        $("#loadPanel").dxLoadPanel("instance").hide();
                        $("#lblTercero").text(_text + " no existe en la base de datos de terceros, favor ingrese uno diferente!");
                        e.component.option("value", "");
                    } else {
                        $("#lblTercero").text(data.nombre);
                        Tercero = data.idTercero
                        $("#grdListaContratos").dxDataGrid("instance").option("visible", true);
                        $("#grdListaContratos").dxDataGrid("instance").refresh();
                        $("#loadPanel").dxLoadPanel('instance').hide();
                    }
                }).fail(function (jqxhr, textStatus, error) {
                    alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
                });
                $("#loadPanel").dxLoadPanel("instance").hide();
            }
        }
    }).dxValidator({
        validationGroup: "CertificadoGroup",
        validationRules: [{
            type: 'required',
            message: 'El documento del tercero es requerido',
        }]
    });;

    $("#chkActividades").dxCheckBox({
        value: false
    });

    $("#txtMailTercero").dxTextBox({
        placeholder: "Ingrese el correo electrónico para el envío",
        value: ""
    }).dxValidator({
        validationGroup: "CertificadoGroup",
        validationRules: [{
            type: 'required',
            message: 'El correo electrónico es requerido',
        }, {
            type: 'email',
            message: 'El correo electrónico no es válido',
        }]
    });

    $("#grdListaContratos").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "ID_CONTRATO",
                loadMode: "raw",
                load: function () {
                    if (typeof Tercero === 'undefined') Tercero = -1;
                    return $.getJSON($("#SIM").data("url") + "Contractual/api/CertificadosApi/ListaContratos", { IdTercero: Tercero });
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
        visible: false,
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'ID_CONTRATO', caption: 'Identificador', alignment: 'center', dataType: 'number', visible: false },
            { dataField: 'TIPOCONTRATO', width: '30%', caption: 'Tipo de Contrato', dataType: 'string' },
            { dataField: 'FECHA', width: '13%', caption: 'Fecha', dataType: 'date', format: 'dd MMM yyyy' },
            { dataField: 'ANIO', width: '5%', caption: 'Año', dataType: 'number' },
            { dataField: 'NUMERO', width: '5%', caption: 'Numero', dataType: 'number' },
            { dataField: 'VALOR', width: '12%', caption: 'Valor', dataType: 'number', format: { type: 'currency', precision: 0 } },
            { dataField: 'PLAZO', width: '35%', caption: 'Plazo', dataType: 'string' }
        ],
        onContentReady(e) {
            const totalCount = e.component.totalCount();
            if (totalCount > 0) {
                $("#btnGenerar").dxButton("instance").option("visible", true);
                $("#btnPrevisualiza").dxButton("instance").option("visible", true);
            }
            else {
                $("#btnGenerar").dxButton("instance").option("visible", false);
                $("#btnPrevisualiza").dxButton("instance").option("visible", false);
            }

        }
    });

    $("#btnPrevisualiza").dxButton({
        icon: 'export',
        type: 'success',
        text: 'Previsualiza Cetificado',
        visible: false,
        onClick: function (e) {
            $("#loadPanel").dxLoadPanel('instance').show();
            var ConAct = $("#chkActividades").dxCheckBox("instance").option("value");
            var _Ruta = $('#SIM').data('url') + "Contractual/api/CertificadosApi/PrevisualizaCetificado";
            var params = { IdTer: Tercero, Mail: "", Actividades: ConAct };
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (!data.isSucceded) DevExpress.ui.dialog.alert('Ocurrió un error ' + data.Message, 'Generar Certificado');
                    else {
                        var pdfWindow = window.open("");
                        pdfWindow.document.write("'<html><head><title>Certificado Contractual</title></head><body height='100%' width='100%'><iframe width='100%' height='100%' src='data:application/pdf;base64," + data.Certificado + "'></iframe></body></html>");
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Generar Certificado');
                }
            });
            $("#loadPanel").dxLoadPanel('instance').hide();
        }
    });

    $("#btnGenerar").dxButton({
        icon: 'export',
        type: 'success',
        text: 'Generar Cetificado',
        visible: false,
        onClick: function (e) {
            $("#loadPanel").dxLoadPanel('instance').show();
            DevExpress.validationEngine.validateGroup("CertificadoGroup");
            var correo = $("#txtMailTercero").dxTextBox("instance").option("value");
            var cedula = $("#txtDocumento").dxTextBox("instance").option("value");
            if (correo != "") {
                var ConAct = $("#chkActividades").dxCheckBox("instance").option("value");
                var _Ruta = $('#SIM').data('url') + "Contractual/api/CertificadosApi/GeneraCetificado";
                var params = { IdTer: Tercero, Mail: correo, Actividades: ConAct };
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: _Ruta,
                    data: JSON.stringify(params),
                    contentType: "application/json",
                    beforeSend: function () { },
                    success: function (data) {
                        if (!data.isSucceded) DevExpress.ui.dialog.alert('Ocurrió un error ' + data.Message, 'Generar Certificado');
                        else {
                            var pdfWindow = window.open("");
                            pdfWindow.document.write("'<html><head><title>Certificado Contractual</title></head><body height='100%' width='100%'><iframe width='100%' height='100%' src='data:application/pdf;base64," + data.Certificado + "'></iframe></body></html>");
                            DevExpress.validationEngine.resetGroup('CertificadoGroup');
                            Tercero = -1;
                            $("#txtDocumento").dxTextBox("instance").option("value", "");
                            $("#lblTercero").text("");
                            $("#chkActividades").dxCheckBox("instance").option("value", false);
                            $("#txtMailTercero").dxTextBox("instance").option("value","");
                            $("#grdListaContratos").dxDataGrid("instance").option("visible", false);
                            $("#grdListaContratos").dxDataGrid("instance").refresh();
                            $("#loadPanel").dxLoadPanel('instance').hide();
                            DevExpress.ui.dialog.alert('El certificado para el documento ' + cedula + ' se generó correctamente y cuando sea firmado se enviará al correo ' + correo);
                        }
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Generar Certificado');
                    }
                });
            } else {
                DevExpress.ui.dialog.alert('Debe proporcionar un correo electrónico par enviar en certificado!!', 'Guardar Datos');
            }
            $("#loadPanel").dxLoadPanel('instance').hide();
        }
    });
});