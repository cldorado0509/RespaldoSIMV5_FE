var myApp = angular.module('SIM', ['dx']);
var tramite = 0;
myApp.controller("ActivarUsuariosController", function ($scope, $http, $location) {
    $scope.comentariosRechazo = '';
    $scope.procesandoVisible = false;

    $scope.txtComentarios = {
        height: '85%',
        width: '100%',
        maxLength: 1024,
        bindingOptions: { value: 'comentariosRechazo' }
    }

    $scope.popComentarios = {
        title: "Comentarios Rechazo Registro",
        fullScreen: false,
    }

    $scope.btnAceptarRechazo = {
        text: 'Aceptar',
        type: 'success',
        onClick: function (params) {
            var txtComentariosInstance = $("#txtComentarios").dxTextArea("instance");
            $scope.comentariosRechazo = txtComentariosInstance.option("value");

            var popComentariosInstance = $("#popComentarios").dxPopup("instance");
            popComentariosInstance.hide();

            $scope.RechazarRegistro($scope.idRolSolicitado);
        }
    }

    $scope.btnCancelarRechazo = {
        text: 'Cancelar',
        type: 'danger',
        onClick: function (params) {
            var popComentariosInstance = $("#popComentarios").dxPopup("instance");
            popComentariosInstance.hide();
        }
    }

    $scope.grdUsuariosActivacionSettings = {
        dataSource: grdUsuariosActivacionDataSource,
        allowColumnResizing: true,
        height: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 0,
        },
        pager: {
            showPageSizeSelector: false,
        },
        filterRow: {
            visible: false,
        },
        groupPanel: {
            visible: false,
        },
        editing: {
            editEnabled: false,
            removeEnabled: false,
            insertEnabled: false

        },
        selection: {
            mode: 'single'
        },
        columns: [
            {
                dataField: "ID_USUARIO",
                dataType: 'number',
                visible: false,
            }, {
                dataField: 'S_LOGIN',
                width: '20%',
                caption: 'Usuario',
                dataType: 'string',
            }, {
                dataField: 'N_DOCUMENTO',
                width: '15%',
                caption: 'Documento',
                dataType: 'string',
            }, {
                dataField: 'S_RSOCIAL',
                width: '45%',
                caption: 'Razón Social',
                dataType: 'string',
            },
            {
                caption: '',
                width: '10%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div />').dxButton(
                        {
                            icon: '',
                            text: 'Activar',
                            type: 'success',
                            onClick: function (params) {
                                $scope.comentariosRechazo = '';
                                var registroActual = options.data;

                                $scope.idRolSolicitado = registroActual.ID_ROL_SOLICITADO;
                                $scope.AceptarRegistro();
                            }
                        }
                        ).appendTo(container);
                }
            },
            {
                caption: '',
                width: '10%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div />').dxButton(
                        {
                            icon: '',
                            text: 'Rechazar',
                            type: 'danger',
                            onClick: function (params) {
                                $scope.comentariosRechazo = '';
                                var registroActual = options.data;
                                $scope.idRolSolicitado = registroActual.ID_ROL_SOLICITADO;

                                var popComentariosInstance = $("#popComentarios").dxPopup("instance");
                                popComentariosInstance.show();
                            }
                        }
                        ).appendTo(container);
                }
            }
        ],
        onSelectionChanged: function (selecteditems) {
            var data = selecteditems.selectedRowsData[0];

            tramite = data.CODTRAMITE;

            $('#grdDocumentosTemporales').dxDataGrid({
                dataSource: grdDocumentosTemporalesDataSource
            });

            $http.get($('#app').data('url') + 'Seguridad/api/UsuarioRolApi/RolesSolicitados?id=' + data.ID_USUARIO).success(function (data, status, headers, config) {
                $('[id^="divRol_"]').css('background-color', '');

                if (data.length > 0) {
                    $('[id^="divRol_"]').each(function (i, e) {
                        var chkRol = $("#chkru_" + e.id.substr(7)).dxCheckBox("instance");

                        if ($.inArray(Number(e.id.substr(7)), data) >= 0) {
                            $('#' + e.id).css('background-color', 'palegreen');
                            chkRol.option("value", true);
                        } else {
                            chkRol.option("value", false);
                        }
                    });
                }
            }).error(function (data, status, headers, config) {
                $scope.cargandoVisible = false;
                MostrarNotificacion('alert', 'error', 'Error cargando roles de usuario');
            });
        }
    }

    $scope.grdDocumentosTemporalesSettings = {
        dataSource: grdDocumentosTemporalesDataSource,
        allowColumnResizing: true,
        height: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        pager: {
            showPageSizeSelector: false,
        },
        filterRow: {
            visible: false,
        },
        groupPanel: {
            visible: false,
        },
        editing: {
            editEnabled: false,
            removeEnabled: false,
            insertEnabled: false

        },
        selection: {
            mode: 'single'
        },
        columns: [
            {
                dataField: "ID_DOCUMENTO",
                dataType: 'number',
                visible: false,
            }, {
                dataField: 'S_DESCRIPCION',
                width: '80%',
                caption: 'Archivo',
                dataType: 'string',
            }, 
            {
                caption: '',
                width: '20%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div />').dxButton(
                        {
                            icon: '',
                            text: 'Ver',
                            type: 'success',
                            onClick: function (params) {
                                registroActual = options.data;
                                VerDocumento(registroActual.ID_DOCUMENTO);
                            }
                        }
                        ).appendTo(container);
                }
            }
        ],
    }

    $scope.AceptarRegistro = function () {
        $scope.cargandoVisible = true;

        $scope.procesandoVisible = true;
        $http.get($('#app').data('url') + 'Seguridad/api/UsuarioRolApi/ActivarUsuario?idRolSolicitado=' + $scope.idRolSolicitado).success(function (data, status, headers, config) {
            $scope.cargandoVisible = false;
            if (data.tipoRespuesta == 'OK')
            {
                $scope.procesandoVisible = false;
                MostrarNotificacion('alert', 'OK', 'Usuario Habilitando Satisfactoriamente.');

                $('#grdUsuariosActivacion').dxDataGrid({
                    dataSource: grdUsuariosActivacionDataSource
                });
            } else {
                MostrarNotificacion('alert', 'error', 'Error Habilitando Usuario. ' + data.detalleRespuesta);
            }
        }).error(function (data, status, headers, config) {
            $scope.procesandoVisible = false;
            MostrarNotificacion('alert', 'error', 'Error Habilitando Usuario');
        });
    }

    $scope.RechazarRegistro = function () {
        $scope.procesandoVisible = true;

        $http.post($('#app').data('url') + 'Seguridad/api/UsuarioRolApi/RechazarUsuario', { idRolSolicitado: $scope.idRolSolicitado, comentarios: $scope.comentariosRechazo }).success(function (data, status, headers, config) {
            $scope.procesandoVisible = false;

            if (data.tipoRespuesta == 'OK') {
                MostrarNotificacion('alert', 'OK', 'Usuario Rechazado Satisfactoriamente.');

                $('#grdUsuariosActivacion').dxDataGrid({
                    dataSource: grdUsuariosActivacionDataSource
                });
            } else {
                MostrarNotificacion('alert', 'error', 'Error Rechazando Usuario. ' + data.detalleRespuesta);
            }
        }).error(function (data, status, headers, config) {
            $scope.procesandoVisible = false;
        });
    }
});

var grdUsuariosActivacionDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ID_ROL_SOLICITADO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (loadOptions.skip ? loadOptions.skip : 0);
        var take = (loadOptions.take ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'Seguridad/api/AccountApi/UsuariosActivacion', {
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    }
});

var grdDocumentosTemporalesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'Seguridad/api/AccountApi/DocumentosTemporales', {
            tramite: tramite
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    }
});

function VerDocumento(idDocumento) {
    window.open($('#app').data('url') + 'Tramites/Documento/ConsultarDocumentoTemporal?idDocumento=' + idDocumento);
}

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Activación de Usuarios');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}
