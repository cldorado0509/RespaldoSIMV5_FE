var myApp = angular.module('SIM', ['dx']);

myApp.controller("registroController", function ($scope, $location, $http) {
    $('.my-cloak').removeClass('my-cloak');
    $scope.procesandoVisible = false;
    $scope.usuarioRegistrado = false;

    $scope.model = {};
    $scope.model.TipoPersonaUsuario = 'J';
    $scope.model.Roles = '';
    $scope.model.RolesNombres = '';
    $scope.model.Type = 0;

    $scope.MostrarDatosAdicionales = function(tipo)
    {
        if (tipo == 3) {
            return (($scope.model.Type == 1 || $scope.model.Type == 2) && !$scope.usuarioRegistrado)
        } else {
            return ($scope.model.Type == tipo);
        }
    }

    $scope.fupArchivos = {
        selectButtonText: 'Seleccionar Documento',
        labelText: 'Arrastrar Aquí',
        multiple: true,
        showFileList: true,
        uploadMode: 'instantly',
        accept: 'application/pdf',
        uploadedMessage: 'Archivo Cargado',
        uploadFailedMessage: 'Error Cargando Archivo',
        uploadUrl: $('#app').data('url') + 'Seguridad/api/AccountApi/CargarArchivo',
        onValueChanged: function (e) {
            var url = e.component.option("uploadUrl");
            url = updateQueryStringParameter(url, "id",$scope.model.Nit);
            e.component.option("uploadUrl", url);
        }
        //onValueChanged: subirArchivo
    }

    $scope.tipoUsuarioSettings = {
        displayExpr: 'text',
        valueExpr: 'value',
        dataSource: [{ text: 'Persona', value: 'N' }, { text: 'Empresa', value: 'J' }],
        layout: 'horizontal',
        bindingOptions: { value: 'model.TipoPersonaUsuario' }
    };

    $scope.EsUsuarioRegistrado = function()
    {
        return $scope.usuarioRegistrado;
    }

    /*$scope.firstNameSettings = {
        displayExpr: 'text',
        valueExpr: 'value',
        layout: 'horizontal',
        bindingOptions: { value: 'model.FirstName' }
    };*/

    $scope.subirArchivo = function () {
        var data = new FormData();
        var files = $('.dx-fileuploader-input').get(0).files;

        data.append('id', $scope.model.LastName);
        for (i = 0; i < files.length; i++) {
            data.append("file" + i, files[i]);
        }

        $http.post($('#app').data('url') + 'Seguridad/api/AccountApi/CargarArchivo', data, { transformRequest: angular.identity, headers: { 'Content-Type': undefined } }).success(function (data, status, headers, config) {
            alert(data);

        }).error(function (data, status, headers, config) {
            MostrarNotificacion('alert', (data.resp === 'OK' ? 'success' : 'error'), data.mensaje);
        });
    }

    $scope.MostrarPreguntaAdministrador = function() {
        var si = function () {
            return true;
        };
        var no = function () {
            return false;
        };
        var customDialog = DevExpress.ui.dialog.custom({
            title: "Registro Usuario",
            message: "El tercero que registra aun no tiene un usuario administrador. Desea realizar el registro como administrador del tercero ? (Si no se registra como administrador del tercero, debe esperar a que éste exista para registrarse con un perfil diferente)",
            buttons: [
                { text: "Sí", onClick: si },
                { text: "No", onClick: no },
            ]
        });

        customDialog.show().done(function (dialogResult) {
            if (dialogResult) {
                $('#archivos').dxFileUploader('instance').reset();

                $http.get($('#app').data('url') + 'Seguridad/api/AccountApi/Reset?id=' + $scope.model.Nit).success(function (data, status, headers, config) {
                    
                }).error(function (data, status, headers, config) {
                    DevExpress.ui.dialog.alert('Error Inicializando Archivos', 'Error');
                });

                $scope.model.Password = $scope.model.ConfirmPassword = null;
                $scope.model.Type = 1;
            }
            else {
                $scope.model.Type = 0;
            }
            $scope.$apply();
        });
    };

    $scope.btnFinalizarRegistroSettings = {
        text: 'Finalizar Registro',
        type: 'success',
        width: '100%',
        onClick: function (params) {
            var result = $('#datosComplementariosGroupValidator').dxValidationGroup('instance').validate();

            if (!result.isValid) return;

            $scope.procesandoVisible = true;
            
            $scope.model.Roles = $scope.RolesSeleccionados(0);
            $scope.model.RolesNombres = $scope.RolesSeleccionados(1);
            if ($scope.model.Type != 2) {
                var archivosSeleccionados = $('#archivos').dxFileUploader('instance').option('value');

                if (archivosSeleccionados == null || archivosSeleccionados.length == 0) {
                    $scope.procesandoVisible = false;
                    MostrarNotificacion('notify', 'error', 'Debe adjuntar por lo menos un archivo para continuar con el registro.');
                    return;
                }
            }

            $http.post($('#app').data('url') + 'Seguridad/api/AccountApi/Register', JSON.stringify($scope.model)).success(function (data, status, headers, config) {
                $scope.procesandoVisible = false;

                switch (data.codigoRespuesta) {
                    case -203: // Error de proceso en el servidor
                    case -202: // Error Enviando Correo de Validación
                    case -201: // Error Generando Correo de Validación
                    case -200: // Error Registrando Usuario
                    case -101: // Error de datos de Configuración
                    case -100: // Error de Configuración
                    case -1: // El Usuario ya se encuentra registrado
                    case -2: // Datos Inconsistentes
                    case -3: //El Usuario que intenta registrar ya tiene tercero asociado (diferente al digitado en el registro)
                        MostrarNotificacion('notify', 'error', data.detalleRespuesta);
                        break;
                    case 1: // Usuario Existe y está relacionado al tercero al cual se registra. Sin Administrador
                    case 3: // Usuario Existe y no está relacionado al tercero al cual se registra. Sin Administrador
                    case 5: // El Usuario será creado. Sin Administrador
                        $scope.MostrarPreguntaAdministrador();
                        break;
                    case 2: // Usuario Existe y está relacionado al tercero al cual se registra. Con Administrador
                    case 4: // Usuario Existe y no está relacionado al tercero al cual se registra. Con Administrador
                    case 6: // El Usuario será creado. Con Administrador
                        MostrarNotificacion('alert', '', 'Para finalizar el registro, debe selecionar el(los) rol(es) que desea que se habiliten para el usuario registrado.');
                        $scope.model.Type = 2;
                        break;
                    case 0:
                    default:
                        if (data.tipoRespuesta === 'OK') {
                            $scope.model.Type = 0;
                            $scope.usuarioRegistrado = true;

                            $scope.model = {};
                            $scope.model.TipoPersonaUsuario = 'N';
                        } else {
                            MostrarNotificacion('notify', 'error', data.detalleRespuesta);
                        }
                        break;
                }
            }).error(function (data, status, headers, config) {
                MostrarNotificacion('notify', (data.tipoRespuesta === 'OK' ? 'success' : 'error'), data.detalleRespuesta);
                $scope.procesandoVisible = false;
            });
        }
    }

    $scope.btnRegistroSettings = {
        text: 'Validar Registro',
        type: 'success',
        width:'100%',
        onClick: function (params) {
            $scope.usuarioRegistrado = false;
            var result = $('#datosBasicosGroupValidator').dxValidationGroup('instance').validate();

            if (!result.isValid) return;

            if ($scope.model.TipoPersonaUsuario == 'J' && isNaN($scope.model.Nit))
            {
                alert('El NIT debe ser numérico');
                return;
            }

            $scope.model.Type = 0;

            $scope.procesandoVisible = true;

            $http.post($('#app').data('url') + 'Seguridad/api/AccountApi/Register', JSON.stringify($scope.model)).success(function (data, status, headers, config) {
                $scope.procesandoVisible = false;

                switch (data.codigoRespuesta)
                {
                    case -203: // Error de proceso en el servidor
                    case -202: // Error Enviando Correo de Validación
                    case -201: // Error Generando Correo de Validación
                    case -200: // Error Registrando Usuario
                    case -101: // Error de datos de Configuración
                    case -100: // Error de Configuración
                    case -150: // Error de Procesamiento
                    case -1: // El Usuario ya se encuentra registrado
                    case -2: // Datos Inconsistentes
                    case -3: //El Usuario que intenta registrar ya tiene tercero asociado (diferente al digitado en el registro)
                        MostrarNotificacion('notify', 'error', data.detalleRespuesta);
                        break;
                    case 1: // Usuario Existe y está relacionado al tercero al cual se registra. Sin Administrador
                    case 3: // Usuario Existe y no está relacionado al tercero al cual se registra. Sin Administrador
                    case 5: // El Usuario será creado. Sin Administrador
                        $scope.MostrarPreguntaAdministrador();
                        break;
                    case 2: // Usuario Existe y está relacionado al tercero al cual se registra. Con Administrador
                    case 4: // Usuario Existe y no está relacionado al tercero al cual se registra. Con Administrador
                    case 6: // El Usuario será creado. Con Administrador
                        MostrarNotificacion('alert', '', 'Para finalizar el registro, debe selecionar el(los) rol(es) que desea que se habiliten para el usuario registrado.');
                        $scope.model.Type = 2;
                        break;
                    case 0:
                    default:
                        if (data.tipoRespuesta === 'OK') {
                            $scope.usuarioRegistrado = true;

                            $scope.model = {};
                            $scope.model.TipoPersonaUsuario = 'N';
                        } else {
                            MostrarNotificacion('notify', 'error', data.detalleRespuesta);
                        }
                        break;
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

    $scope.RolesSeleccionados = function (tipo) {
        var roles = '';

        if (tipo == 0) {
            $('span[id^="ruId_"]').each(function (i, e) {
                if ($('#' + e.textContent).dxCheckBox('instance').option('value')) {
                    if (roles == '')
                        roles = e.id.substr(5);
                    else
                        roles += ',' + e.id.substr(5);
                }
            });
        } else {
            $('span[id^="ruId_"]').each(function (i, e) {
                if ($('#' + e.textContent).dxCheckBox('instance').option('value')) {
                    if (roles == '')
                        roles = $('#' + e.textContent).dxCheckBox('instance').option('text');
                    else
                        roles += ', ' + $('#' + e.textContent).dxCheckBox('instance').option('text');
                }
            });
        }

        return roles;
    }
});

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Registro');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}

function updateQueryStringParameter(uri, key, value) {
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = uri.indexOf('?') !== -1 ? "&" : "?";
    if (uri.match(re)) {
        return uri.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
        return uri + separator + key + "=" + value;
    }
}