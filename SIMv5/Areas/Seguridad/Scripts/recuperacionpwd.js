var myApp = angular.module('SIM', ['dx']);

myApp.controller("recuperacionController", function ($scope, $location, $http) {
    $('.my-cloak').removeClass('my-cloak');
    $scope.procesandoVisible = false;
    $scope.recuperacionProcesada = false;

    $scope.model = {};

    $scope.RecuperacionProcesada = function ()
    {
        return $scope.recuperacionProcesada;
    }

    $scope.btnRecuperarSettings = {
        text: 'Recuperar',
        type: 'success',
        width:'100%',
        onClick: function (params) {
            $scope.recuperacionProcesada = false;
            var result = params.validationGroup.validate();

            if (!result.isValid) return;

            $scope.procesandoVisible = true;

            $http.post($('#app').data('url') + 'Seguridad/api/AccountApi/RecoverPassword', JSON.stringify($scope.model)).success(function (data, status, headers, config) {
                $scope.procesandoVisible = false;

                if (data.tipoRespuesta === 'OK') {
                    $scope.recuperacionProcesada = true;
                } else {
                    MostrarNotificacion('notify', 'error', data.detalleRespuesta);
                }
            }).error(function (data, status, headers, config) {
                MostrarNotificacion('notify', (data.tipoRespuesta === 'OK' ? 'success' : 'error'), data.detalleRespuesta);
                $scope.procesandoVisible = false;
            });
        }
    }

    $scope.btnCancelarSettings = {
        text: 'Cancelar',
        type: 'danger',
        width:'100%',
        onClick: function (params) {
            window.location = $('#app').data('url') + "Seguridad/Account/Login";
        }
    }

    $scope.validarConfirmacionPassword = function () {
        return $scope.model.Password;
    }
});

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Recuperación de Contraseña');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }

}