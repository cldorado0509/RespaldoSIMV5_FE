var tabsDataNuevo;
var tabsData;

var SIMApp = angular.module('SIM', ['dx']);

//Globalize.culture("es");
//Globalize.locale(navigator.language || navigator.browserLanguage);

SIMApp.controller("TerceroController", function ($scope, $http, $rootScope) {
    $scope.cargandoVisible = true;
    $scope.tipoTramite = null;
    $scope.idUsuarioSeleccionado = null;

    $('.my-cloak').removeClass('my-cloak');

    $rootScope.$on("UpdateContactosTercero", function () {
        $('#grdTerceroContactos').dxDataGrid({
            dataSource: grdTerceroContactosDataSource
        });
    });

    $scope.idTercero = $('#app').data('idtercero');
    $scope.idInstalacionSeleccionada = null;

    tabsDataNuevo = [
        { text: 'Información General', pos: 0 }
    ];

    if ($('#app').data('tipo') == 'N') {
        tabsData = [
            { text: 'Información General', pos: 0 },
            { text: 'Establecimientos / Instalaciones', pos: 2 },
            { text: 'Usuarios', pos: 3 },
        ];
    } else {
        tabsData = [
            { text: 'Información General', pos: 0 },
            { text: 'Contactos', pos: 1 },
            { text: 'Establecimientos / Instalaciones', pos: 2 },
            { text: 'Usuarios', pos: 3 },
        ];
    }

    $scope.selectedTab = 0;
    $scope.instalacionesCargadas = false;
    $scope.usuariosCargados = false;
    $scope.contactosCargados = false;
    $scope.tiposIdentificacion = {};
    $scope.datosTercero = {};
    $scope.datosTercero.NATURAL = {};
    $scope.datosTercero.JURIDICA = {};
    $scope.nombres = {};
    if ($('#app').data('tipo') == 'N' && $('#app').data('n') != '') {
        $scope.nombres['NOMBRES'] = $('#app').data('n');
        $scope.nombres['APELLIDOS'] = $('#app').data('a');
        $scope.datosTercero.S_CORREO = $('#app').data('e');
    } else {
        $scope.nombres['NOMBRES'] = '';
        $scope.nombres['APELLIDOS'] = '';
    }
    if ($('#app').data('tipo') == 'J' && $('#app').data('n') != '') {
        if (!isNaN($('#app').data('a')) && (''+$('#app').data('a')).trim() != '')
        {
            $scope.datosTercero.N_DOCUMENTON = $('#app').data('a');
            ObtenerDV($scope.datosTercero);
        }
        $scope.datosTercero.S_RSOCIAL = $('#app').data('n');
        $scope.datosTercero.ID_TIPODOCUMENTO = 2;
        $scope.datosTercero.S_CORREO = $('#app').data('e');
    }
    $scope.almacenandoVisible = false;

    $scope.TipoDocumentoSettings = {
        dataSource: tiposIdentificacionDataSource,
        placeholder: "",
        displayExpr: "S_ABREVIATURA",
        valueExpr: "ID_TIPODOCUMENTO",
        bindingOptions: { value: 'datosTercero.ID_TIPODOCUMENTO' }
    };

    $scope.GeneroSettings = {
        dataSource: generosData,
        placeholder: "[Genero]",
        displayExpr: "NOMBRE",
        valueExpr: "ID",
        bindingOptions: { value: 'datosTercero.NATURAL.S_GENERO' }
    };

    $scope.NaturalezaSettings = {
        dataSource: naturalezaData,
        placeholder: "[Naturaleza]",
        displayExpr: "NOMBRE",
        valueExpr: "ID",
        bindingOptions: { value: 'datosTercero.JURIDICA.S_NATURALEZA' }
    };

    $scope.ActividadEconomicaSettings = {
        dataSource: actividadEconomicaDataSource,
        placeholder: "[Seleccionar Actividad Económica]",
        title: "Actividad Económica",
        displayExpr: "S_NOMBRE_LOOKUP",
        valueExpr: "ID_ACTIVIDADECONOMICA",
        cancelButtonText: "Cancelar",
        pageLoadingText: "Cargando...",
        refreshingText: "Refrescando...",
        searchPlaceholder: "Buscar",
        noDataText: "Sin Datos",
        bindingOptions: { value: 'datosTercero.ID_ACTIVIDADECONOMICA' }
    };

    $scope.btnAlmacenarRolesSettings = {
        text: 'Almacenar',
        type: 'success',
        width: '30%',
        onClick: function (params) {
            $('#btnAlmacenarRoles').dxButton({
                disabled: true
            });

            $scope.rolesUsuario = [];
            $('span[id^="ruId_"]').each(function (i, e) {
                $scope.rolesUsuario.push({ SEL: $('#' + e.textContent).dxCheckBox('instance').option('value'), ID_ROL: parseInt(e.id.substr(5)) });
            });

            $http.post($('#app').data('url') + 'Seguridad/api/UsuarioRolApi/AsignarRolesUsuario?idUsuario=' + $scope.idUsuarioSeleccionado, JSON.stringify($scope.rolesUsuario)).success(function (data, status, headers, config) {
            //$http.post($('#app').data('url') + 'Seguridad/api/UsuarioRolApi/AsignarRolesUsuario', data).success(function (data, status, headers, config) {
                $('#btnAlmacenarRoles').dxButton({
                    disabled: false
                });

                MostrarNotificacion('notify', (data.tipoRespuesta === 'OK' ? 'success' : 'error'), data.detalleRespuesta);
            }).error(function (data, status, headers, config) {
                $('#btnAlmacenarRoles').dxButton({
                    disabled: false
                });

                MostrarNotificacion('notify', (data.tipoRespuesta === 'OK' ? 'success' : 'error'), data.detalleRespuesta);
            });
        }
    };

    $scope.btnAlmacenarSettings = {
        text: 'Almacenar',
        type: 'success',
        onClick: function (params) {
            var result = params.validationGroup.validate();

            if (!result.isValid) return;

            $scope.almacenandoVisible = true;

            $scope.datosTercero.N_DOCUMENTO = $scope.datosTercero.N_DOCUMENTON;

            if ($('#app').data('tipo') == 'N') {
                var nombresTercero = $scope.nombres['NOMBRES'].split(' ');
                var apellidosTercero = $scope.nombres['APELLIDOS'].split(' ');
                $scope.datosTercero.NATURAL.S_NOMBRE1 = nombresTercero[0];
                $scope.datosTercero.NATURAL.S_NOMBRE2 = (nombresTercero[1] === null ? '' : nombresTercero[1]);
                $scope.datosTercero.NATURAL.S_APELLIDO1 = apellidosTercero[0];;
                $scope.datosTercero.NATURAL.S_APELLIDO2 = (apellidosTercero[1] === null ? '' : apellidosTercero[1]);
            }

            $('#btnAlmacenar').dxButton({
                disabled: true
            });
            $('#btnRegresar').dxButton({
                disabled: true
            });

            $http.post($('#app').data('url') + 'General/api/TerceroApi/Tercero', JSON.stringify($scope.datosTercero)).success(function (data, status, headers, config) {
                $scope.almacenandoVisible = false;
                MostrarNotificacion('alert', (data.resp === 'OK' ? 'success' : 'error'), data.mensaje);

                if (data.resp === 'OK') {
                    $scope.datosTercero = data.datos;

                    if ($scope.idTercero == 0) {
                        $('#tabOpciones').dxTabs({
                            dataSource: tabsData
                        });

                        $scope.idTercero = $scope.datosTercero.ID_TERCERO;
                    }

                    //if ($scope.idTercero == 0) {

                    //    location.reload();
                    //}

                    if ($('#app').data('tipo') == 'N') {
                        if ($scope.datosTercero.NATURAL.D_NACIMIENTO != null)
                            $scope.datosTercero.NATURAL.D_NACIMIENTO = new Date($scope.datosTercero.NATURAL.D_NACIMIENTO.substring(0, 4), parseInt($scope.datosTercero.NATURAL.D_NACIMIENTO.substring(5, 7)) - 1, $scope.datosTercero.NATURAL.D_NACIMIENTO.substring(8, 10));
                        $scope.nombres['NOMBRES'] = $scope.datosTercero.NATURAL.S_NOMBRE1 + ($scope.datosTercero.NATURAL.S_NOMBRE2 === null ? '' : ' ' + $scope.datosTercero.NATURAL.S_NOMBRE2);
                        $scope.nombres['APELLIDOS'] = $scope.datosTercero.NATURAL.S_APELLIDO1 + ($scope.datosTercero.NATURAL.S_APELLIDO2 === null ? '' : ' ' + $scope.datosTercero.NATURAL.S_APELLIDO2);
                    } else {
                        if ($scope.datosTercero.JURIDICA.D_CONSTITUCION != null)
                            $scope.datosTercero.JURIDICA.D_CONSTITUCION = new Date($scope.datosTercero.JURIDICA.D_CONSTITUCION.substring(0, 4), parseInt($scope.datosTercero.JURIDICA.D_CONSTITUCION.substring(5, 7)) - 1, $scope.datosTercero.JURIDICA.D_CONSTITUCION.substring(8, 10));
                    }

                    $scope.mensajeValidacion = '';
                    $scope.ValidarAE($scope.datosTercero);
                }

                $('#btnAlmacenar').dxButton({
                    disabled: false
                });

                if ($('#app').data('retorno') == 'S') {
                    $('#btnRegresar').dxButton({
                        disabled: false
                    });
                }
            }).error(function (data, status, headers, config) {
                $scope.almacenandoVisible = false;
                MostrarNotificacion('alert', (data.resp === 'OK' ? 'success' : 'error'), data.mensaje);
                $('#btnAlmacenar').dxButton({
                    disabled: false
                });
                if ($('#app').data('retorno') == 'S') {
                    $('#btnRegresar').dxButton({
                        disabled: false
                    });
                }
            });
        }
    };

    $scope.btnCancelarSettings = {
        text: 'Cancelar',
        type: 'danger',
        onClick: function (params) {
            window.location = $('#app').data('url');
        }
    };

    $scope.btnRegresarSettings = {
        text: 'Regresar',
        type: 'success',
        onClick: function (params) {
            $http.get($('#app').data('url') + 'General/api/TerceroApi/TerceroBasico?id=' + $scope.datosTercero.ID_TERCERO).success(function (data, status, headers, config) {
                window.close();
                opener.SeleccionTercero(data);
            }).error(function (data, status, headers, config) {
                window.close();
                opener.SeleccionTercero(null);
            });
        }
    };

    $scope.textBoxSettings = {
        bindingOptions: { value: 'datosTercero.S_RSOCIAL' },
        maxLength: 20,
        placeholder: 'Ejemplo de PlaceHolder'
    };

    $scope.dxTabsOptions = {
        dataSource: ($scope.idTercero == 0 ? tabsDataNuevo : tabsData),
        onItemClick: (function (itemData) {
            $scope.setTab(itemData.itemIndex, ($scope.idTercero == 0 ? tabsDataNuevo : tabsData)[itemData.itemIndex].pos);
        }),
        selectedIndex: 0
    };

    if ($scope.idTercero > 0) {
        $.getJSON($('#app').data('url') + 'General/api/TerceroApi/Tercero', {
            id: $scope.idTercero,
            tipoTercero: $('#app').data('tipo')
        }).done(function (data) {
            $scope.datosTercero = data;

            if (data != null)
            {
                if ($('#app').data('tipo') == 'N') {
                    if ($scope.datosTercero.NATURAL.D_NACIMIENTO != null)
                        $scope.datosTercero.NATURAL.D_NACIMIENTO = new Date($scope.datosTercero.NATURAL.D_NACIMIENTO.substring(0, 4), parseInt($scope.datosTercero.NATURAL.D_NACIMIENTO.substring(5, 7)) - 1, $scope.datosTercero.NATURAL.D_NACIMIENTO.substring(8, 10));
                    $scope.nombres['NOMBRES'] = $scope.datosTercero.NATURAL.S_NOMBRE1 + ($scope.datosTercero.NATURAL.S_NOMBRE2 === null ? '' : ' ' + $scope.datosTercero.NATURAL.S_NOMBRE2);
                    $scope.nombres['APELLIDOS'] = $scope.datosTercero.NATURAL.S_APELLIDO1 + ($scope.datosTercero.NATURAL.S_APELLIDO2 === null ? '' : ' ' + $scope.datosTercero.NATURAL.S_APELLIDO2);
                } else {
                    if ($scope.datosTercero.JURIDICA.D_CONSTITUCION != null)
                        $scope.datosTercero.JURIDICA.D_CONSTITUCION = new Date($scope.datosTercero.JURIDICA.D_CONSTITUCION.substring(0, 4), parseInt($scope.datosTercero.JURIDICA.D_CONSTITUCION.substring(5, 7)) - 1, $scope.datosTercero.JURIDICA.D_CONSTITUCION.substring(8, 10));
                }
                $scope.ValidarAE($scope.datosTercero);

                $scope.$apply();
            } else {
                window.location = $('#app').data('url');
            }
        }).fail(function (jqxhr, textStatus, error) {
            alert(error);
        });
    }

    $scope.ValidarAE = function (datos) {
        if (datos.VERSION_AE == undefined) {
            if (datos.ID_ACTIVIDADECONOMICA == null) {
                $scope.mensajeValidacion = 'Por favor asignar Actividad Económica.';
            }
        } else {
            if (datos.VERSION_AE == null) {
                $scope.mensajeValidacion = 'Por favor asignar Actividad Económica.';
            } else if (datos.VERSION_AE != '4') {
                $scope.mensajeValidacion = 'Actividad Económica Obsoleta, por favor actualizar.';
            } else {
                $scope.mensajeValidacion = '';
            }
        }
    }

    $scope.setTab = function (tabIndex, pos) {
        $scope.selectedTab = pos;
        if (tabsData[tabIndex].text === 'Contactos' && !$scope.contactosCargados) {
            $('#grdTerceroContactos').dxDataGrid({
                dataSource: grdTerceroContactosDataSource
            });
            $scope.contactosCargados = true;
        }
        if (tabsData[tabIndex].text === 'Establecimientos / Instalaciones' && !$scope.instalacionesCargadas) {
            $('#grdTerceroInstalaciones').dxDataGrid({
                dataSource: grdTerceroInstalacionesDataSource
            });
            $scope.instalacionesCargadas = true;
        }
        if (tabsData[tabIndex].text === 'Usuarios' && !$scope.usuariosCargados) {
            $('#grdTerceroUsuarios').dxDataGrid({
                dataSource: grdTerceroUsuariosDataSource
            });
            $scope.usuariosCargados = true;
        }
    };

    $scope.isActive = function (tabIndex) {
        if ($scope.selectedTab === tabIndex)
            return true;
        else
            return false;
    };

    $scope.CalcularDV = function () {
        ObtenerDV($scope.datosTercero);
    };
    
    var grdTerceroInstalacionesDataSource = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();

            var filterOptions = "";
            var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ID_INSTALACION","desc":false}]';
            var groupOptions = "";

            var skip = loadOptions.skip;
            var take = loadOptions.take;
            $.getJSON($('#app').data('url') + 'General/api/TerceroApi/TerceroInstalaciones', {
                idTercero: $scope.datosTercero.ID_TERCERO,
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

    $scope.grdTerceroInstalacionesSettings = {
        //dataSource: grdTerceroInstalacionesDataSource,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: false,
            allowedPageSizes: [5, 10, 20, 50]
        },
        filterRow: {
            visible: false,
        },
        groupPanel: {
            visible: false,
        },
        editing: {
            editMode: 'row',
            editEnabled: true,
            removeEnabled: false,
            insertEnabled: true
        },
        onInitNewRow: function (rowInfo) {
            //alert($('#app').data('idtercero'));
            rowInfo.cancel = true;
            var dataGrid = $('#grdTerceroInstalaciones').dxDataGrid('instance');
            dataGrid.cancelEditData();
            window.location = $('#app').data('url') + 'General/Instalacion/Instalacion?idTercero=' + $scope.datosTercero.ID_TERCERO + '&T=' + $scope.idTercero; //$('#app').data('idtercero');
        },
        onEditingStart: function (rowInfo) {
            window.location = $('#app').data('url') + 'General/Instalacion/Instalacion/' + rowInfo.data.ID_INSTALACION + '?T=' + $scope.idTercero; //$('#app').data('idtercero');
            rowInfo.cancel = true;
        },
        selection: {
            mode: 'single'
        },
        columns: [
            {
                dataField: 'S_NOMBRE',
                width: '25%',
                caption: 'Nombre',
                dataType: 'string',
            }, {
                dataField: 'S_TIPOINSTALACION',
                width: '25%',
                caption: 'Tipo',
                dataType: 'string',
            }, {
                dataField: 'S_ACTIVIDAD_ECONOMICA',
                width: '25%',
                caption: 'Actividad Económica',
                dataType: 'string',
            }, {
                dataField: 'S_ESTADO',
                width: '10%',
                caption: 'Estado',
                dataType: 'string',
            }/*, {
                caption: '',
                width: '15%',
                alignment: 'center',
                cssClass: 'botonColumna',
                cellTemplate: function (container, options) {
                    $('<div />').dxButton(
                        {
                            icon: '',
                            text: 'Nuevo Trámite',
                            type: 'normal',
                            onClick: function (params) {
                                $scope.idInstalacionSeleccionada = options.data.ID_INSTALACION;
                                $('#cboTipoTramite').dxSelectBox('instance').option('value', null);
                                $scope.tipoTramite = null;

                                var popup = $('#popTipoTramite').dxPopup('instance');
                                popup.show();
                            }
                        }
                        ).appendTo(container);
                }
            },*/
        ],
    };

    $scope.popTipoTramiteSettings = {
        showTitle: true,
        deferRendering: false,
        title: 'Seleccionar Tipo de Trámite',
        height: 180,
    }

    /*$.get($('#app').data('url') + 'Tramites/api/TramitesApi/TipoTramite', function (data) {
        for (var i = 0; i < data.datos.length; i++) {
            cboTipoTramiteDataSource.store().insert(data.datos[i]);
        }
        cboTipoTramiteDataSource.load();
        $scope.cargandoVisible = false;
        $scope.$apply();
    }, "json");*/

    $scope.tipoTramiteSelectBoxSettings = {
        dataSource: cboTipoTramiteDataSource,
        valueExpr: 'ID_TRAMITE',
        displayExpr: 'TRAMITE',
        placeholder: '[Seleccionar Tipo Trámite]',
        bindingOptions: { value: 'tipoTramite' },
    };

    var grdTerceroContactosDataSource = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();

            var filterOptions = "";
            var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ID_CONTACTO","desc":false}]';
            var groupOptions = "";

            var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
            var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
            $.getJSON($('#app').data('url') + 'General/api/TerceroApi/TerceroContactos', {
                idTercero: $scope.idTercero,
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
        },
        /*insert: function (values) {
            //alert('insert');
            //alert(JSON.stringify(values));
            //return dbImpl._sendRequest('POST', values);
            values.ID_JURIDICO = $scope.datosTercero.ID_TERCERO;
            //values.ID_TIPODOCUMENTO = 
            $http.post($('#app').data('url') + 'General/api/TerceroApi/TerceroContacto', JSON.stringify(values)).success(function (data, status, headers, config) {
                MostrarNotificacion('alert', (data.resp === 'OK' ? 'success' : 'error'), data.mensaje);
            });
        },
        update: function (key, values) {
            alert('update');
            alert(JSON.stringify(key));
            alert(JSON.stringify(values));
            //var params = $.extend({}, values);
            //params[dbImpl.key] = key;
            //return dbImpl._sendRequest('PUT', params);
        },
        remove: function (key) {
            alert('remove');
            alert(JSON.stringify(key));
            //return dbImpl._sendRequest('DELETE', key);
        }*/
    });


    var grdTerceroUsuariosDataSource = new DevExpress.data.CustomStore({
        load: function (loadOptions) {
            var d = $.Deferred();

            var filterOptions = "";
            var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ID_USUARIO","desc":false}]';
            var groupOptions = "";

            var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
            var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
            $.getJSON($('#app').data('url') + 'General/api/TerceroApi/TerceroUsuarios', {
                idTercero: $scope.datosTercero.ID_TERCERO,
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

    $scope.popTerceroUsuariosSettings = {
        showTitle: true,
        title: 'Seleccionar Usuario',
        deferRendering: false,
        onHidden: function () {
            var dataGrid = $('#grdTerceroUsuarios').dxDataGrid('instance');
            dataGrid.cancelEditData();

            $scope.almacenandoVisible = true;
            if ($scope.itemSeleccionado) {
                /*cboDocumentoAsociadoDataSource.store().clear();
                cboDocumentoAsociadoDataSource.store().insert({ ID_POPUP: $scope.ID_POPUP, NOMBRE_POPUP: $scope.NOMBRE_POPUP });
                cboDocumentoAsociadoDataSource.load();

                $scope.radicador.documentoAsociado = $scope.ID_POPUP;*/
                $http.post($('#app').data('url') + 'General/api/TerceroApi/UsuarioTercero', { ID_TERCERO: $scope.datosTercero.ID_TERCERO, ID_USUARIO: $scope.ID_POPUP }).success(function (data, status, headers, config) {
                    $scope.almacenandoVisible = false;
                    if (data.resp === 'OK') {
                        $('#grdTerceroUsuarios').dxDataGrid({
                            dataSource: grdTerceroUsuariosDataSource
                        });
                    } else {
                        MostrarNotificacion('alert', 'error', data.mensaje);
                    }
                }).error(function (data, status, headers, config) {
                    $scope.almacenandoVisible = false;
                    MostrarNotificacion('alert', (data.resp === 'OK' ? 'success' : 'error'), data.mensaje);
                });
            }
            $scope.itemSeleccionado = false;
            $scope.almacenandoVisible = false;
        },
    }

    $scope.grdTerceroUsuariosSettings = {
        //dataSource: grdTerceroUsuariosDataSource,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: false,
            allowedPageSizes: [5, 10, 20, 50]
        },
        filterRow: {
            visible: false,
        },
        groupPanel: {
            visible: false,
        },
        editing: {
            editMode: 'row',
            editEnabled: false,
            removeEnabled: false,//true,
            insertEnabled: true
        },
        onInitNewRow: function (rowInfo) {
            rowInfo.cancel = true;

            var popup = $('#popTerceroUsuarios').dxPopup('instance');
            popup.show();
        },
        onEditingStart: function (rowInfo) {
            //rowInfo.data.ID_TERCERO
            //if (rowInfo.key) {
            //    rowInfo.cancel = true;
            //}
        },
        selection: {
            mode: 'single'
        },
        columns: [
            {
                dataField: 'S_LOGIN',
                width: '25%',
                caption: 'Usuario',
                dataType: 'string',
            }, {
                dataField: 'S_NOMBRES',
                width: '30%',
                caption: 'Nombres',
                dataType: 'string',
            }, {
                dataField: 'S_APELLIDOS',
                width: '30%',
                caption: 'Apellidos',
                dataType: 'string',
            },
            {
                caption: '',
                width: '15%',
                alignment: 'center',
                cssClass: 'botonColumna',
                cellTemplate: function (container, options) {
                    $('<div />').dxButton(
                        {
                            icon: '',
                            text: 'Permisos',
                            type: 'normal',
                            onClick: function (params) {
                                $scope.idUsuarioSeleccionado = options.data.ID_USUARIO;
                                registroActual = options.data;
                                $scope.AsignarRoles(registroActual);
                            }
                        }
                        ).appendTo(container);
                }
            },
        ],
    };

    $scope.AsignarRoles = function (registro) {
        $scope.cargandoVisible = true;

        $http.get($('#app').data('url') + 'Seguridad/api/UsuarioRolApi/RolesSolicitados?id=' + registro.ID_USUARIO).success(function (data, status, headers, config) {
            $('[id^="divRol_"]').css('background-color', '');

            if (data.length > 0) {
                $('[id^="divRol_"]').each(function (i, e) {
                    if ($.inArray(Number(e.id.substr(7)), data) >= 0) {
                        $('#' + e.id).css('background-color', 'yellow');
                    }
                });
            }
        }).error(function (data, status, headers, config) {
            $scope.cargandoVisible = false;
            MostrarNotificacion('alert', 'error', 'Error cargando roles de usuario');
        });

        $http.get($('#app').data('url') + 'Seguridad/api/UsuarioRolApi/RolesUsuarioExterno?id=' + registro.ID_USUARIO).success(function (data, status, headers, config) {
            $scope.cargandoVisible = false;

            $('span[id^="ruId_"]').each(function (i, e) {
                if ($.inArray(Number(e.id.substr(5)), data) >= 0) {
                    $('#' + e.textContent).dxCheckBox('instance').option('value', true);
                } else {
                    $('#' + e.textContent).dxCheckBox('instance').option('value', false);
                }
            });

            var popup = $('#popRolesUsuario').dxPopup('instance');
            popup.show();
        }).error(function (data, status, headers, config) {
            $scope.cargandoVisible = false;
            MostrarNotificacion('alert', 'error', 'Error cargando roles de usuario');
        });
    }

    $scope.grdUsuariosSettings = {
        dataSource: usuariosDataSource,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 8
        },
        pager: {
            showPageSizeSelector: false
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
                dataField: 'S_LOGIN',
                width: '25%',
                caption: 'Usuario',
                dataType: 'string',
            }, {
                dataField: 'S_NOMBRES',
                width: '30%',
                caption: 'Nombres',
                dataType: 'string',
            }, {
                dataField: 'S_APELLIDOS',
                width: '30%',
                caption: 'Apellidos',
                dataType: 'string',
            },
            /*{
                caption: '',
                width: '15%',
                alignment: 'center',
                cssClass: 'botonColumna',
                cellTemplate: function (container, options) {
                    $('<div />').dxButton(
                        {
                            icon: '',
                            text: 'Rechazar',
                            type: 'danger',
                            onClick: function (params) {
                                //registroActual = options.data;
                                //$scope.AsignarRoles(registroActual);
                            }
                        }
                        ).appendTo(container);
                }
            },*/
        ],
        onSelectionChanged: function (selecteditems) {
            var data = selecteditems.selectedRowsData[0];
            $scope.itemSeleccionado = true;
            $scope.ID_POPUP = data.ID_USUARIO;

            var popup = $('#popTerceroUsuarios').dxPopup('instance');
            popup.hide();
        }
    };

    $scope.grdTercerosSettings = {
        dataSource: grdTercerosDataSource,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20, 50]
        },
        filterRow: {
            visible: true,
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
                dataField: 'S_TIPO_DOCUMENTO',
                width: '7%',
                caption: 'Tipo',
                dataType: 'string',
            }, {
                dataField: 'N_DOCUMENTON',
                width: '10%',
                caption: 'Identificación',
                alignment: 'right',
                dataType: 'number',
            }, {
                dataField: 'N_DIGITOVER',
                width: '4%',
                caption: 'DV',
                alignment: 'right',
                dataType: 'number',
            }, {
                dataField: 'S_RSOCIAL',
                width: '35%',
                caption: 'Razón Social',
                dataType: 'string',
            }
        ],
    }

    $scope.popRolesUsuarioSettings = {
        showTitle: true,
        title: 'Asignación de Roles',
        deferRendering: false,
        onContentReady: function (e) {
            
        },
        onHidden: function () {
            
        },
    }

    $scope.cancelarTTSettings = {
        text: 'Cancelar',
        type: 'danger',
        width: '100%',
        onClick: function (params) {
            var popup = $('#popTipoTramite').dxPopup('instance');
            popup.hide();
        }
    };

    $scope.aceptarTTSettings = {
        text: 'Aceptar',
        type: 'success',
        width: '100%',
        onClick: function (params) {
            $scope.tipoTramite = $('#cboTipoTramite').dxSelectBox('instance').option('value');
            if ($scope.tipoTramite != null)
            {
                var popup = $('#popTipoTramite').dxPopup('instance');
                popup.hide();

                if (inIframe())
                {
                    window.parent.IniciarTramite($scope.tipoTramite, $scope.idInstalacionSeleccionada, $scope.idTercero);
                }
                else
                {
                    window.location = $('#app').data('url') + 'Tramites/NuevoTramite?idTercero=' + $scope.idTercero + '&idInstalacion=' + $scope.idInstalacionSeleccionada + '&idTramite=' + $scope.tipoTramite;
                }
            }
        }
    };
});

function inIframe() {
    try {
        return window.self !== window.top;
    } catch (e) {
        return true;
    }
}

SIMApp.controller("ContactosTerceroController", function ($scope, $location, $http, $rootScope) {
    $scope.datosContactoTercero = {};
    $scope.datosContactoTercero.TERCERO = {};
    $scope.datosContactoTercero.ID_JURIDICO = $scope.idTercero;
    $scope.nombres = {};
    $scope.nombres['NOMBRES'] = '';
    $scope.nombres['APELLIDOS'] = '';
    $scope.cargandoContactoVisible = false;

    $scope.nuevoTercero = false;
    $scope.cambioContacto = false;
    $scope.visiblePopup = false;

    $scope.btnAceptarSettings = {
        text: 'Aceptar',
        type: 'success',
        //width: '100%',
        onClick: function (params) {
            //var result = params.validationGroup.validate();
            var result = $('#contactosGroupValidator').dxValidationGroup('instance').validate();

            if (!result.isValid) return;

            var nombresTercero = $scope.nombres['NOMBRES'].split(' ');
            var apellidosTercero = $scope.nombres['APELLIDOS'].split(' ');
            $scope.datosContactoTercero.TERCERO.S_NOMBRE1 = nombresTercero[0];
            $scope.datosContactoTercero.TERCERO.S_NOMBRE2 = (nombresTercero[1] === null ? '' : nombresTercero[1]);
            $scope.datosContactoTercero.TERCERO.S_APELLIDO1 = apellidosTercero[0];;
            $scope.datosContactoTercero.TERCERO.S_APELLIDO2 = (apellidosTercero[1] === null ? '' : apellidosTercero[1]);

            //$('#btnAceptar').dxButton({
            //    disabled: true
            //});
            params.component.option('disabled', true);

            $http.post($('#app').data('url') + 'General/api/TerceroApi/ContactoTercero', JSON.stringify($scope.datosContactoTercero)).success(function (data, status, headers, config) {
                if (data.resp === 'OK') {
                    $scope.cambioContacto = true;
                    $scope.visiblePopup = false;
                    //var popup = $('#popContactosTercero').dxPopup('instance');
                    //popup.hide();
                } else {
                    MostrarNotificacion('alert', 'error', data.mensaje);
                }
                params.component.option('disabled', false);
            }).error(function (data, status, headers, config) {
                MostrarNotificacion('alert', (data.resp === 'OK' ? 'success' : 'error'), data.mensaje);
                params.component.option('disabled', false);
            });
        }
    };

    $scope.btnCancelarSettings = {
        text: 'Cancelar',
        type: 'danger',
        //width: '100%',
        onClick: function (params) {
            var popup = $('#popContactosTercero').dxPopup('instance');
            popup.hide();
        }
    };

    $scope.popContactosTerceroSettings = {
        showTitle: true,
        title: 'Contacto',
        bindingOptions: {
            visible: 'visiblePopup'
        },
        buttons: [
            {
                toolbar: 'bottom', location: 'after', widget: 'button', options: $scope.btnAceptarSettings
            },
            {
                toolbar: 'bottom', location: 'after', widget: 'button', options: $scope.btnCancelarSettings
            }
        ],
        //deferRendering: false,
        onHidden: function () {
            var dataGrid = $('#grdTerceroContactos').dxDataGrid('instance');
            dataGrid.cancelEditData();

            if ($scope.cambioContacto) {
                /*$('#grdTerceroContactos').dxDataGrid({
                    dataSource: grdTerceroContactosDataSource
                });*/
                $rootScope.$emit("UpdateContactosTercero", {});
            }

            $scope.cambioContacto = false;
        },
    }

    $scope.ActividadEconomicaSettings = {
        dataSource: actividadEconomicaDataSource,
        placeholder: "[Seleccionar Actividad Económica]",
        title: "Actividad Económica",
        displayExpr: "S_NOMBRE_LOOKUP",
        valueExpr: "ID_ACTIVIDADECONOMICA",
        bindingOptions: { value: 'datosContactoTercero.TERCERO.ID_ACTIVIDADECONOMICA' }
    };

    $scope.TipoDocumentoSettings = {
        dataSource: tiposIdentificacionNaturalDataSource,
        placeholder: "",
        displayExpr: "S_ABREVIATURA",
        valueExpr: "ID_TIPODOCUMENTO",
        bindingOptions: { value: 'datosContactoTercero.TERCERO.ID_TIPODOCUMENTO', readOnly: 'datosContactoTercero.ID_CONTACTO != 0' }
    };

    $scope.ProfesionSettings = {
        dataSource: profesionesDataSource,
        placeholder: "[Seleccionar Profesión]",
        displayExpr: "S_NOMBRE_LOOKUP",
        valueExpr: "ID_PROFESION",
        bindingOptions: { value: 'datosContactoTercero.TERCERO.ID_PROFESION' }
    };

    $scope.IdentificacionSettings = {
        onFocusOut: function (params) {
            if ($scope.nuevoTercero) {
                $scope.cargandoContactoVisible = true;
                $.getJSON($('#app').data('url') + 'General/api/TerceroApi/ContactoTerceroIdentificacion?idTercero=' + $scope.idTercero + '&tipoDocumento=' + $scope.datosContactoTercero.TERCERO.ID_TIPODOCUMENTO + '&identificacion=' + $scope.datosContactoTercero.TERCERO.N_DOCUMENTON).done(function (data) {
                    if (data != null) {
                        $scope.datosContactoTercero = data;

                        if ($scope.datosContactoTercero.TERCERO.D_NACIMIENTO != null)
                            $scope.datosContactoTercero.TERCERO.D_NACIMIENTO = new Date($scope.datosContactoTercero.TERCERO.D_NACIMIENTO.substring(0, 4), parseInt($scope.datosContactoTercero.TERCERO.D_NACIMIENTO.substring(5, 7)) - 1, $scope.datosContactoTercero.TERCERO.D_NACIMIENTO.substring(8, 10));
                        $scope.nombres['NOMBRES'] = $scope.datosContactoTercero.TERCERO.S_NOMBRE1 + ($scope.datosContactoTercero.TERCERO.S_NOMBRE2 === null ? '' : ' ' + $scope.datosContactoTercero.TERCERO.S_NOMBRE2);
                        $scope.nombres['APELLIDOS'] = $scope.datosContactoTercero.TERCERO.S_APELLIDO1 + ($scope.datosContactoTercero.TERCERO.S_APELLIDO2 === null ? '' : ' ' + $scope.datosContactoTercero.TERCERO.S_APELLIDO2);

                        $scope.terceroOK = true;
                    } else {
                        $scope.datosContactoTercero = { ID_CONTACTO: 0, ID_JURIDICO: $scope.idTercero, TERCERO: { ID_TIPODOCUMENTO: $scope.datosContactoTercero.TERCERO.ID_TIPODOCUMENTO, N_DOCUMENTON: $scope.datosContactoTercero.TERCERO.N_DOCUMENTON, N_DIGITOVER: $scope.datosContactoTercero.TERCERO.N_DIGITOVER } };
                        $scope.nombres = {};
                        $scope.nombres['NOMBRES'] = '';
                        $scope.nombres['APELLIDOS'] = '';

                        //$('#personalGroupValidator').dxValidationGroup('instance').reset();
                    }
                    $scope.nuevoTercero = false;
                    $scope.cargandoContactoVisible = false;

                    var popupLoading = $('#popLoadingContacto').dxLoadPanel('instance');
                    popupLoading.hide();
                }).fail(function (jqxhr, textStatus, error) {
                    $scope.cargandoContactoVisible = false;
                    alert(error);
                });
            }
        },
        onChange: function (params) {
            $scope.terceroOK = false;
            $scope.nuevoTercero = true;
            //$scope.datosContactoTercero = {};
            //$scope.datosContactoTercero.TERCERO = {};
            //$scope.nombres = {};
            //$scope.nombres['NOMBRES'] = '';
            //$scope.nombres['APELLIDOS'] = '';
            $scope.CalcularDV();
        },
        bindingOptions: { value: 'datosContactoTercero.TERCERO.N_DOCUMENTON', min: 0, max: 9, readOnly: 'datosContactoTercero.ID_CONTACTO != 0' }
    };

    $scope.CalcularDV = function () {
        ObtenerDV($scope.datosContactoTercero.TERCERO);
    };

    $scope.GeneroSettings = {
        dataSource: generosData,
        placeholder: "[Genero]",
        displayExpr: "NOMBRE",
        valueExpr: "ID",
        bindingOptions: { value: 'datosContactoTercero.TERCERO.S_GENERO' }
    };

    $scope.TipoContactoSettings = {
        dataSource: tiposCotactoData,
        placeholder: "[Tipo Contacto]",
        displayExpr: "NOMBRE",
        valueExpr: "ID",
        bindingOptions: { value: 'datosContactoTercero.TIPO' }
    };

    $scope.grdTerceroContactosSettings = {
        //dataSource: grdTerceroContactosDataSource,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: false
        },
        filterRow: {
            visible: false,
        },
        groupPanel: {
            visible: false,
        },
        editing: {
            editMode: 'row',
            editEnabled: true,
            removeEnabled: false,
            insertEnabled: true
        },
        onInitNewRow: function (rowInfo) {
            rowInfo.cancel = true;

            $scope.cambioContacto = false;

            $scope.datosContactoTercero = {};
            $scope.datosContactoTercero.TERCERO = {};
            $scope.nombres = {};
            $scope.nombres['NOMBRES'] = '';
            $scope.nombres['APELLIDOS'] = '';

            $scope.datosContactoTercero.ID_CONTACTO = 0;
            //alert($scope.idTercero);
            $scope.datosContactoTercero.ID_JURIDICO = $scope.idTercero;
            $scope.visiblePopup = true;

            var contactosGroupValidator = $('#contactosGroupValidator').dxValidationGroup('instance');
            if (contactosGroupValidator != undefined)
                contactosGroupValidator.reset();
        },
        onEditingStart: function (rowInfo) {
            rowInfo.cancel = true;
            $scope.cargandoContactoVisible = true;
            $.getJSON($('#app').data('url') + 'General/api/TerceroApi/ContactoTercero?idContacto=' + rowInfo.data.ID_CONTACTO).done(function (data) {
                $scope.cambioContacto = false;
                if (data != null) {
                    $scope.datosContactoTercero = data;

                    if ($scope.datosContactoTercero.TERCERO.D_NACIMIENTO != null)
                        $scope.datosContactoTercero.TERCERO.D_NACIMIENTO = new Date($scope.datosContactoTercero.TERCERO.D_NACIMIENTO.substring(0, 4), parseInt($scope.datosContactoTercero.TERCERO.D_NACIMIENTO.substring(5, 7)) - 1, $scope.datosContactoTercero.TERCERO.D_NACIMIENTO.substring(8, 10));
                    $scope.nombres['NOMBRES'] = $scope.datosContactoTercero.TERCERO.S_NOMBRE1 + ($scope.datosContactoTercero.TERCERO.S_NOMBRE2 === null ? '' : ' ' + $scope.datosContactoTercero.TERCERO.S_NOMBRE2);
                    $scope.nombres['APELLIDOS'] = $scope.datosContactoTercero.TERCERO.S_APELLIDO1 + ($scope.datosContactoTercero.TERCERO.S_APELLIDO2 === null ? '' : ' ' + $scope.datosContactoTercero.TERCERO.S_APELLIDO2);

                    $scope.cargandoContactoVisible = false;
                    $scope.terceroOK = true;
                } else {
                    $scope.cargandoContactoVisible = false;
                    MostrarNotificacion('alert', 'error', 'Datos No Encontrados');
                }
                $scope.nuevoTercero = false;
                var popup = $('#popContactosTercero').dxPopup('instance');
                popup.show();
            }).fail(function (jqxhr, textStatus, error) {
                $scope.cargandoContactoVisible = false;
                alert(error);
            });
        },
        //onRowRemoving: function (rowInfo) {  
        //}
        //onEditingStart: function (rowInfo) {
        //    alert('EditStart');
        //rowInfo.data.ID_TERCERO
        //if (rowInfo.key) {
        //    rowInfo.cancel = true;
        //}
        //},
        //onRowUpdating: function (rowInfo) {
        //    alert('Editando');
        //},
        //onRowInserting: function (rowInfo) {
        //    alert('onRowInserting');
        //},
        selection: {
            mode: 'single'
        },
        columns: [
            {
                dataField: 'S_TIPO_DOCUMENTO',
                width: '10%',
                caption: 'Tipo Doc',
                dataType: 'string',
                /*"lookup":{
                    "dataSource":[
                       { "name":"CC" },
                       { "name":"CE" },
                       { "name":"TI" }
                    ],
                    "valueExpr":"name",
                    "displayExpr":"name"
                },*/
            }, {
                dataField: 'N_DOCUMENTON',
                width: '12%',
                caption: 'Identificación',
                alignment: 'right',
                dataType: 'number',
                /*editCellTemplate: function (cellElement, cellInfo) {
                    $("<div />").dxTextBox({
                        value: cellInfo.value,
                        onValueChanged: function(e) {
                            cellInfo.setValue(e.value);
                        }
                    }).appendTo(cellElement);
                    $("<div />").dxButton({
                        icon: 'search',
                        onClick: function (e) {
                            $('#popTerceros').dxPopup('instance').show();
                            var dataGrid = $('#grdTerceros').dxDataGrid('instance');

                            dataGrid.columnOption('N_DOCUMENTON', 'filterValue', cellInfo.value);
                            dataGrid.refresh();
                        }
                    }).appendTo(cellElement);
                }*/
            }, {
                dataField: 'N_DIGITOVER',
                width: '5%',
                caption: 'DV',
                alignment: 'right',
                dataType: 'number',
            }, {
                dataField: 'S_NOMBRES',
                width: '20%',
                caption: 'Nombres',
                dataType: 'string',
                /*editCellTemplate: function (cellElement, cellInfo) {
                    $("<div />").dxTextBox({
                        value: cellInfo.value,
                        onValueChanged: function(e) {
                            cellInfo.setValue(e.value);
                        }
                    }).appendTo(cellElement);
                    $("<div />").dxButton({
                        icon: 'search',
                        onClick: function (e) {
                            $('#popTerceros').dxPopup('instance').show();
                            var dataGrid = $('#grdTerceros').dxDataGrid('instance');

                            dataGrid.columnOption('S_RSOCIAL', 'filterValue', cellInfo.value);
                            dataGrid.refresh();
                        }
                    }).appendTo(cellElement);
                },*/
            }, {
                dataField: 'S_APELLIDOS',
                width: '20%',
                caption: 'Apellidos',
                dataType: 'string',
                /*editCellTemplate: function (cellElement, cellInfo) {
                    $("<div />").dxTextBox({
                        value: cellInfo.value,
                        onValueChanged: function(e) {
                            cellInfo.setValue(e.value);
                        }
                    }).appendTo(cellElement);
                    $("<div />").dxButton({
                        icon: 'search',
                        onClick: function (e) {
                            $('#popTerceros').dxPopup('instance').show();
                            var dataGrid = $('#grdTerceros').dxDataGrid('instance');

                            dataGrid.columnOption('S_APELLIDOS', 'filterValue', cellInfo.value);
                            dataGrid.refresh();
                        }
                    }).appendTo(cellElement);
                },*/
            }, {
                dataField: 'TIPO',
                width: '5%',
                caption: 'Tipo',
                dataType: 'string',
                visible: true,
            }, {
                dataField: 'S_CORREO',
                width: '25%',
                caption: 'Correo Electrónico',
                dataType: 'string',
            },
            {
                caption: '',
                width: '15%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div />').dxButton(
                        {
                            icon: '',
                            text: 'Deshabilitar',
                            type: 'danger',
                            onClick: function (params) {
                                var result = DevExpress.ui.dialog.confirm("Está Seguro(a) de Deshabilitar el Contacto Seleccionado?", "Confirmación");
                                result.done(function (dialogResult) {
                                    if (dialogResult) {
                                        registroActual = options.data;
                                        DeshabilitarContacto(registroActual);
                                    }
                                });
                            }
                        }
                        ).appendTo(container);
                }
            },
        ],
    };

    function DeshabilitarContacto(registro) {
        $.getJSON($('#app').data('url') + 'General/api/TerceroApi/ContactoTerceroDeshabilitar?idContacto=' + registro.ID_CONTACTO
        ).done(function (data) {
            /*$('#grdTerceroContactos').dxDataGrid({
                dataSource: grdTerceroContactosDataSource
            });*/
            $rootScope.$emit("UpdateContactosTercero", {});
            MostrarNotificacion('notify', (data.resp === 'OK' ? 'success' : 'error'), (data.resp === 'OK' ? 'Contacto Deshabilitado' : 'Error Deshabilitando Contacto'));
        }).fail(function (jqxhr, textStatus, error) {
            alert('falla: ' + textStatus + ", " + error);
        });
    }
});


var actividadEconomicaDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {

        var d = $.Deferred();
        
        var searchValueOptions = loadOptions.searchValue;
        var searchExprOptions = loadOptions.searchExpr;

        var skip = loadOptions.skip;
        var take = loadOptions.take;

        $.getJSON($('#app').data('url') + 'General/api/ActividadEconomicaApi/ActividadesEconomicas', {
            filter: '',
            sort: '[{"selector":"S_NOMBRE_LOOKUP","desc":false}]',
            group: '',
            skip: skip,
            take: take,
            searchValue: (searchValueOptions === undefined || searchValueOptions === null ? '' : searchValueOptions),
            searchExpr: (searchExprOptions === undefined || searchExprOptions === null ? '' : searchExprOptions),
            comparation: 'contains',
            tipoData: 'l',
            noFilterNoRecords: true
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('falla2a: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
    byKey: function (key, extra) {
        /*return key.toString();*/
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'General/api/ActividadEconomicaApi/ActividadesEconomicas', {
            filter: '',
            sort: '[{"selector":"S_NOMBRE_LOOKUP","desc":false}]',
            group: '',
            skip: 0,
            take: 1,
            searchValue: key,
            searchExpr: 'ID_ACTIVIDADECONOMICA',
            comparation: '=',
            tipoData: 'l',
            noFilterNoRecords: true
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('falla1: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
});

var cboTipoTramiteDataSource = new DevExpress.data.DataSource([]);

var generosData = [
    { ID: 'M', NOMBRE: 'Masculino' },
    { ID: 'F', NOMBRE: 'Femenino' },
];

var tiposCotactoData = [
    { ID: 'R', NOMBRE: 'Representante Legal' },
];

var naturalezaData = [
    { ID: 'V', NOMBRE: 'Privada' },
    { ID: 'U', NOMBRE: 'Pública' },
    { ID: 'M', NOMBRE: 'Mixta' },
];

var grdTercerosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_RSOCIAL","desc":false}]';

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'General/api/TerceroApi/Terceros', {
            filter: filterOptions,
            sort: sortOptions,
            group: '',
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'r',
            noFilterNoRecords: true
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('falla: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
    byKey: function (key, extra) {
        /*return key.toString();*/
        var d = $.Deferred();
        $.getJSON($('#app').data('url') + 'General/api/TerceroApi/Terceros', {

            filter: '',
            sort: '[{"selector":"S_NOMBRE_LOOKUP","desc":false}]',
            group: '',
            skip: 0,
            take: 1,
            searchValue: key == '0' ? '' : key,
            searchExpr: 'ID_TERCERO',
            comparation: '=',
            tipoData: 'l',
            noFilterNoRecords: true
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('falla: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
});

var usuariosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_APELLIDOS","desc":false}]';
        var groupOptions = "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'General/api/UsuarioApi/Usuarios', {
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
    },
});

var profesionesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var searchValueOptions = loadOptions.searchValue;
        var searchExprOptions = loadOptions.searchExpr;

        var skip = loadOptions.skip;
        var take = loadOptions.take;

        $.getJSON($('#app').data('url') + 'General/api/ProfesionApi/Profesiones', {
            filter: '',
            sort: '[{"selector":"S_NOMBRE_LOOKUP","desc":false}]',
            group: '',
            skip: skip,
            take: take,
            searchValue: (searchValueOptions === undefined || searchValueOptions === null ? '' : searchValueOptions),
            searchExpr: (searchExprOptions === undefined || searchExprOptions === null ? '' : searchExprOptions),
            comparation: 'contains',
            tipoData: 'l',
            noFilterNoRecords: true
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('falla2p: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
    byKey: function (key, extra) {
        /*return key.toString();*/
        var d = $.Deferred();

        $.getJSON($('#app').data('url') + 'General/api/ProfesionApi/Profesiones', {
            filter: '',
            sort: '[{"selector":"S_NOMBRE_LOOKUP","desc":false}]',
            group: '',
            skip: 0,
            take: 1,
            searchValue: key,
            searchExpr: 'ID_PROFESION',
            comparation: '=',
            tipoData: 'l',
            noFilterNoRecords: false
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('falla1: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
});

function ObtenerDV(tercero) {
    nume = parseInt(tercero.N_DOCUMENTON);

    if (nume >= 1)
        ceros = "00000000000000";
    if (nume >= 10)
        ceros = "0000000000000";
    if (nume >= 100)
        ceros = "000000000000";
    if (nume >= 1000)
        ceros = "00000000000";
    if (nume >= 10000)
        ceros = "0000000000";
    if (nume >= 100000)
        ceros = "000000000";
    if (nume >= 1000000)
        ceros = "00000000";
    if (nume >= 10000000)
        ceros = "0000000";
    if (nume >= 100000000)
        ceros = "000000";
    if (nume >= 1000000000)
        ceros = "00000";
    if (nume >= 10000000000)
        ceros = "0000";
    if (nume >= 100000000000)
        ceros = "000";
    if (nume >= 1000000000000)
        ceros = "00";
    if (nume >= 10000000000000)
        ceros = "0";
    if (nume >= 100000000000000)
        ceros = "";

    li_peso = new Array();
    li_peso[0] = 71;
    li_peso[1] = 67;
    li_peso[2] = 59;
    li_peso[3] = 53;
    li_peso[4] = 47;
    li_peso[5] = 43;
    li_peso[6] = 41;
    li_peso[7] = 37; //8
    li_peso[8] = 29; //3
    li_peso[9] = 23; //0
    li_peso[10] = 19; //1
    li_peso[11] = 17; //2
    li_peso[12] = 13; //0
    li_peso[13] = 7; //9
    li_peso[14] = 3; //9

    ls_str_nit = ceros + tercero.N_DOCUMENTON;
    li_suma = 0;
    for (i = 0; i < 15; i++) {
        li_suma += ls_str_nit.substring(i, i + 1) * li_peso[i];
    }
    digito_chequeo = li_suma % 11;
    if (digito_chequeo >= 2)
        digito_chequeo = 11 - digito_chequeo;

    tercero.N_DIGITOVER = (digito_chequeo);
}

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Tercero');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }

}