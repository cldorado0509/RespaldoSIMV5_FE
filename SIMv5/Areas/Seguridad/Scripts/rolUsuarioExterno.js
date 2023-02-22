var myApp = angular.module('SIM', ['dx']);

myApp.controller("rolUsuarioExternoController", function ($scope, $location, $http) {
    $('.my-cloak').removeClass('my-cloak');
    $scope.procesandoVisible = false;

    $scope.btnAlmacenarSettings = {
        text: 'Almacenar',
        type: 'success',
        width:'100%',
        onClick: function (params) {
            $scope.procesandoVisible = true;

            $('#btnAlmacenar').dxButton({
                disabled: true
            });

            $scope.rolesUsuario = [];
            $('span[id^="ruId_"]').each(function (i, e) {
                $scope.rolesUsuario.push({ SEL : $('#' + e.textContent).dxCheckBox('instance').option('value'), ID_ROL : parseInt(e.id.substr(5)) });
            });

            $http.post($('#app').data('url') + 'Seguridad/api/UsuarioRolApi/AsignarRoles', JSON.stringify($scope.rolesUsuario)).success(function (data, status, headers, config) {
                $scope.procesandoVisible = false;

                $('#btnAlmacenar').dxButton({
                    disabled: false
                });

                MostrarNotificacion('notify', (data.tipoRespuesta === 'OK' ? 'success' : 'error'), data.detalleRespuesta);

                if (data.tipoRespuesta === 'OK')
                {
                    window.location = $('#app').data('url') + "Seguridad/Account/LoginUpdate";
                }
            }).error(function (data, status, headers, config) {
                $('#btnAlmacenar').dxButton({
                    disabled: false
                });

                MostrarNotificacion('notify', (data.tipoRespuesta === 'OK' ? 'success' : 'error'), data.detalleRespuesta);
                $scope.procesandoVisible = false;
            });
        }
    }
});

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Roles Usuario');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }

}