var SIMApp = angular.module('SIM', ['dx']);

//Globalize.culture("es");
//Globalize.locale(navigator.language || navigator.browserLanguage);

SIMApp.controller("ValorAutoController", function ($scope, $location, $http) {
    $('.my-cloak').removeClass('my-cloak');

    $scope.datosValorAutoParams = {};
    $scope.datosValorAutoResult = {};
    $scope.calculoVisible = false;
    $scope.calculandoVisible = false;
    $scope.validacionVisible = false;
    $scope.unidadVisible = false;
    $scope.idTercero = 0;
    $scope.unidad = '';
    $scope.idCalculo = 0;

    /*$("#myModal").dialog({
        autoOpen: false,
        modal: true,
        height: 768,
        width: 1024,
        title: 'Titulo',
        buttons: [
            {
                text: "Salir",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ],
        open: function (ev, ui) {
            $('#myIframe').attr('src', $('#app').data('url') + 'AtencionUsuarios/api/ValorAutoApi/PrintCalculoTramite/' + $scope.idCalculo);
        }
    });*/

    $scope.OcultarCalculo = function (e) {
        $scope.calculoVisible = false;
    };

    $scope.ObservacionesSettings = {
        width: '100%',
        bindingOptions: { value: 'datosValorAutoParams.S_OBSERVACIONES' }
    };

    $scope.TipoTramiteSettings = {
        dataSource: tiposTramiteDataSource,
        placeholder: "[Seleccionar Tipo de Trámite]",
        searchEnabled: true,
        displayExpr: "NOMBRE",
        valueExpr: "CODIGO_TRAMITE",
        onValueChanged: function (e) {
            $('#txtUnidad').dxValidator({
                validationRules: [],
            });
            $scope.OcultarCalculo();
            $scope.datosValorAutoParams.N_NUMPROF = TipoTramiteFromArray(e.value).TECNICOS;
            $scope.unidad = TipoTramiteFromArray(e.value).UNIDAD;
            if ($scope.unidad != '' && $scope.unidad != null && $scope.unidad != undefined) {
                $scope.unidadVisible = true;
                $('#txtUnidad').dxValidator({
                    validationRules: [{
                        type: 'required',
                        message: 'Dato Requerido'
                    }],
                });
            } else {
                $scope.unidadVisible = false;
            }
        },
        bindingOptions: { value: 'datosValorAutoParams.CODIGO_TRAMITE' }
    };

    $scope.IdentificacionSettings = {
        onFocusOut: function (params) {
            if (!isNaN($scope.datosValorAutoParams.N_DOCUMENTO)) {
                $scope.validacionVisible = true;
                $.getJSON($('#app').data('url') + 'General/api/TerceroApi/TerceroValidacion', {
                    identificacion: $scope.datosValorAutoParams.N_DOCUMENTO
                }).done(function (data) {
                    if (data != null && data > 0) {
                        $scope.idTercero = data;
                        $scope.validacionVisible = false;
                        $scope.$apply();
                    } else {
                        $scope.validacionVisible = false;
                        $scope.$apply();
                        MostrarNotificacion('alert', 'error', 'La identificación ingresada no se encuentra registrada en la base de datos de Terceros. Por favor crear el tercero.');
                    }
                }).fail(function (jqxhr, textStatus, error) {
                    $scope.validacionVisible = false;
                    alert(error);
                });
            }
        },
        onChange: function (params) {
            $scope.idTercero == 0;
        },
        bindingOptions: { value: 'datosValorAutoParams.N_DOCUMENTO', min: 0, max: 9 }
    };

    $scope.NumProfSettings = {
        onInput: $scope.OcultarCalculo,
        bindingOptions: { value: 'datosValorAutoParams.N_NUMPROF', min: 0, max: 9 }
    };

    $scope.ValorProySettings = {
        onInput: $scope.OcultarCalculo,
        bindingOptions: { value: 'datosValorAutoParams.N_VALORPROY', min: 0, max: 9 }
    };

    $scope.CMSettings = {
        onInput: $scope.OcultarCalculo,
        bindingOptions: { value: 'datosValorAutoParams.N_CM', min: 0, max: 9 }
    };

    $scope.UnidadSettings = {
        onInput: $scope.OcultarCalculo,
        bindingOptions: { value: 'datosValorAutoParams.N_UNIDAD', min: 0, max: 9 }
    };

    $scope.btnCalcularSettings = {
        text: 'Calcular',
        type: 'success',
        width: '100%',
        onClick: function (params) {
            var result = params.validationGroup.validate();

            if (!result.isValid) return;

            if ($scope.idTercero == 0)
            {
                MostrarNotificacion('alert', 'error', 'La identificación ingresada no se encuentra registrada en la base de datos de Terceros. Por favor crear el tercero antes de realizar el cálculo.');
                return;
            }

            $scope.calculoVisible = false;
            $scope.calculandoVisible = true;

            $('#btnCalcular').dxButton({
                disabled: true
            });

            $http.post($('#app').data('url') + 'AtencionUsuarios/api/ValorAutoApi/CalcularValorAuto', JSON.stringify($scope.datosValorAutoParams)).success(function (data, status, headers, config) {
                $scope.calculandoVisible = false;

                //alert(JSON.stringify(data));

                if (data.tipoRespuesta === 'OK') {
                    $scope.datosValorAutoResult = data.valoresCalculo;
                    $scope.idCalculo = data.valoresCalculo.ID_CALCULO_E;
                    $scope.calculoVisible = true;
                } else {
                    MostrarNotificacion('alert', 'error', data.detalleRespuesta);
                }

                $('#btnCalcular').dxButton({
                    disabled: false
                });
            }).error(function (data, status, headers, config) {
                $scope.calculandoVisible = false;
                MostrarNotificacion('alert', 'error', data);
                $('#btnCalcular').dxButton({
                    disabled: false
                });
            });
        }
    };

    $scope.btnImprimirSettings = {
        text: 'Imprimir',
        type: 'success',
        width: '100%',
        onClick: function (params) {
            //$('#myModal').dialog('open');
            popValorAuto.SetContentUrl($('#app').data('url') + 'AtencionUsuarios/api/ValorAutoApi/PrintCalculoTramite/' + $scope.idCalculo);
            popValorAuto.Show();
        }
    };
});

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Tercero');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}

function TipoTramiteFromArray(ID) {
    return $.grep(tiposTramiteDataSource, function (n, i) {
        return (n.CODIGO_TRAMITE == ID);
    })[0];
}