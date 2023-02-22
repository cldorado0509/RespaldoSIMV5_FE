var datosAnulacion = {
    id: -1,
    ma: -1,
    fS: 0,
    tS: '',
    fJ: 0,
    tJ: '',
    fAP: 0,
    tAP: '',
    fAT: 0,
    tAT: '',
};


$(document).ready(function () {
    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    });

    $.getJSON($('#app').data('url') + 'Tramites/api/AnulacionDocumentoApi/ObtenerDatosAnulacionDocumento', {
        id: $('#app').data('id'),
        idP: $('#app').data('idp')
    }).done(function (data) {
        $('.my-cloak').removeClass('my-cloak');
        AjustarTamano();

        datosAnulacion = data;

        $('#asistente').accordion();

        $('#motivoAnulacion').dxSelectBox({
            value: datosAnulacion.ma,
            readOnly: (($('#app').data('ro') == 'S') || $('#app').data('t') != "1")
        });

        $('#nombreFuncionarioSolicitud').dxTextBox({
            value: datosAnulacion.nfS,
            width: '100%',
            readOnly: true
        });

        $('#nombreFuncionarioJustificacion').dxTextBox({
            value: datosAnulacion.nfJ,
            width: '100%',
            readOnly: true
        });

        $('#nombreFuncionarioAutorizacion').dxTextBox({
            value: datosAnulacion.nfAP,
            width: '100%',
            readOnly: true
        });

        $('#nombreFuncionarioATU').dxTextBox({
            value: datosAnulacion.nfAT,
            width: '100%',
            readOnly: true
        });

        $('#funcionarioAutorizacion').dxSelectBox({
            value: datosAnulacion.fAP,
            width: '100%',
        });

        $('#funcionarioATU').dxSelectBox({
            value: datosAnulacion.fAT,
            width: '100%',
        });

        $('#observacionesSolicitud').dxTextArea({
            value: datosAnulacion.tS,
            readOnly: (($('#app').data('ro') == 'S') || $('#app').data('t') != "1")
        });

        $('#observacionesJustificacion').dxTextArea({
            value: datosAnulacion.tJ,
            readOnly: (($('#app').data('ro') == 'S') || $('#app').data('t') != "2")
        });

        $('#observacionesAutorizacion').dxTextArea({
            value: datosAnulacion.tAP,
            readOnly: (($('#app').data('ro') == 'S') || $('#app').data('t') != "3")
        });

        $('#observacionesATU').dxTextArea({
            value: datosAnulacion.tAT,
            readOnly: (($('#app').data('ro') == 'S') || $('#app').data('t') != "4")
        });

    }).fail(function (jqxhr, textStatus, error) {
        alert('error cargando datos: ' + textStatus + ", " + error);
    });

    $('#motivoAnulacion').dxSelectBox({
        items: null,
        width: '100%',
        placeholder: 'Seleccionar Motivo de Anulación',
        showClearButton: false,
        readOnly: (($('#app').data('ro') == 'S') || $('#app').data('t') != '1')
    });

    $.getJSON($('#app').data('url') + 'Tramites/api/AnulacionDocumentoApi/ObtenerMotivosAnulacion').done(function (data) {
        $('#motivoAnulacion').dxSelectBox({
            items: data,
            displayExpr: 'S_DESCRIPCION',
            valueExpr: 'ID_MOTIVO_ANULACION',
            placeholder: 'Seleccionar Motivo de Anulación',
            showClearButton: false,
            readOnly: (($('#app').data('ro') == 'S') || $('#app').data('t') != '1'),
        });
    }).fail(function (jqxhr, textStatus, error) {
        alert('error cargando datos: ' + textStatus + ", " + error);
    });

    $.getJSON($('#app').data('url') + 'Tramites/api/AnulacionDocumentoApi/FuncionariosAutorizacion').done(function (data) {
        $('#funcionarioAutorizacion').dxSelectBox({
            items: data,
            displayExpr: 'S_FUNCIONARIO',
            valueExpr: 'CODFUNCIONARIO',
            placeholder: 'Seleccionar Funcionario de Autorización',
            showClearButton: false,
            readOnly: (($('#app').data('ro') == 'S') || $('#app').data('t') != '2'),
        });
    }).fail(function (jqxhr, textStatus, error) {
        alert('error cargando datos: ' + textStatus + ", " + error);
    });

    $.getJSON($('#app').data('url') + 'Tramites/api/AnulacionDocumentoApi/FuncionariosATU').done(function (data) {
        $('#funcionarioATU').dxSelectBox({
            items: data,
            displayExpr: 'S_FUNCIONARIO',
            valueExpr: 'CODFUNCIONARIO',
            placeholder: 'Seleccionar Funcionario de Aprobación ATU',
            showClearButton: false,
            readOnly: (($('#app').data('ro') == 'S') || $('#app').data('t') != '3'),
        });
    }).fail(function (jqxhr, textStatus, error) {
        alert('error cargando datos: ' + textStatus + ", " + error);
    });

    $('#observacionesSolicitud').dxTextArea({
        width: '100%',
        height: '90%'
    });

    $('#observacionesJustificacion').dxTextArea({
        width: '100%',
        height: '90%'
    });

    $('#observacionesAutorizacion').dxTextArea({
        width: '100%',
        height: '90%'
    });

    $('#observacionesATU').dxTextArea({
        width: '100%',
        height: '90%'
    });

    if ($('#app').data('ro') == 'N') {
        $('#almacenar').dxButton({
            icon: '',
            text: 'Almacenar',
            width: '200px',
            type: 'success',
            elementAttr: {
                style: "float: right;"
            },
            onClick: function (params) {
                if (ValidarDatosMinimos()) {
                    $("#loadPanel").dxLoadPanel('instance').show();

                    datosAnulacion.ma = $('#motivoAnulacion').dxSelectBox('instance').option('value');
                    datosAnulacion.tS = $('#observacionesSolicitud').dxTextArea('instance').option('value');

                    if ($('#app').data('t') == "2") {
                        datosAnulacion.tJ = $('#observacionesJustificacion').dxTextArea('instance').option('value');
                        datosAnulacion.fAP = $('#funcionarioAutorizacion').dxSelectBox('instance').option('value');
                    }

                    if ($('#app').data('t') == "3") {
                        datosAnulacion.tAP = $('#observacionesAutorizacion').dxTextArea('instance').option('value');
                        datosAnulacion.fAT = $('#funcionarioATU').dxSelectBox('instance').option('value');
                    }

                    if ($('#app').data('t') == "4") {
                        datosAnulacion.tAT = $('#observacionesATU').dxTextArea('instance').option('value');
                    }

                    $.postJSON(
                        $('#app').data('url') + 'Tramites/api/AnulacionDocumentoApi/AlmacenarDatosAnulacionDocumento', datosAnulacion
                    ).done(function (data) {
                        $("#loadPanel").dxLoadPanel('instance').hide();

                        var respuesta = data;

                        if (respuesta == 'OK') {
                            var mensaje = 'Datos Almacenados Satisfactoriamente.';
                            var result = MostrarNotificacion('alert', null, mensaje);

                            result.done(function () {
                                parent.CerrarPopupDocumento();
                            });
                        } else {
                            var result = MostrarNotificacion('alert', null, 'ERROR: ' + respuesta);

                            result.done(function () {
                                parent.CerrarPopupDocumento();
                            });
                        }
                    }).fail(function (jqxhr, textStatus, error) {
                        $("#loadPanel").dxLoadPanel('instance').hide();
                        MostrarNotificacion('alert', null, 'ERROR DE INVOCACION: ' + textStatus + ", " + error);
                    });
                }
            }
        });
    }
});

function ValidarDatosMinimos() {
    if ($('#app').data('t') == "1")
    {
        if ($('#motivoAnulacion').dxSelectBox('instance').option('value') == null) {
            MostrarNotificacion('alert', null, 'Motivo de Anulación Requerido.');
            return false;
        }

        if ($('#observacionesSolicitud').dxTextArea('instance').option('value') == null || $('#observacionesSolicitud').dxTextArea('instance').option('value').trim() == '') {
            MostrarNotificacion('alert', null, 'Observaciones de la Solicitud Requeridas.');
            return false;
        }
    }

    /*if ($('#app').data('t') == "2")
    {
        if ($('#observacionesJustificacion').dxTextArea('instance').option('value') == null || $('#observacionesJustificacion').dxTextArea('instance').option('value').trim() == '') {
            MostrarNotificacion('alert', null, 'Observaciones de la Justificación Requeridas.');
            return false;
        }

        if ($('#funcionarioAutorizacion').dxSelectBox('instance').option('value') == null) {
            MostrarNotificacion('alert', null, 'Funcionario Autorización Requerido.');
            return false;
        }
    }*/
    return true;
}

$.postJSON = function (url, data) {
    var o = {
        url: url,
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    };
    if (data !== undefined) {
        o.data = JSON.stringify(data);
    }
    return $.ajax(o);
};


function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        return DevExpress.ui.dialog.alert(msg, 'Anulación Documento');
    } else {
        return DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}

function utf8_to_b64(str) {
    return window.btoa(unescape(encodeURIComponent(str)));
}

function b64_to_utf8(str) {
    return decodeURIComponent(escape(window.atob(str)));
}