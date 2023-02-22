var idDGA;
var SIMApp = angular.module('SIM', ['dx']);

//Globalize.culture("es");
//Globalize.locale(navigator.language || navigator.browserLanguage);

SIMApp.controller("DGAController", function ($scope, $location, $http) {
    $('.my-cloak').removeClass('my-cloak');

    $scope.TercerosCargados = false;
    $scope.selectedTab = 0;
    $scope.ano = 0;
    $scope.permisosAmbientales = [];
    $scope.cargandoVisible = false;
    $scope.idDGA = $('#app').data('iddga');
    idDGA = $('#app').data('iddga');

    $.getJSON($('#app').data('url') + 'General/api/DGAApi/DGA/' + $('#app').data('iddga')).done(function (data) {
        $scope.datosDGA = data;
        $('span[id^="paId_"]').each(function (i, e) {
            if ($.inArray(Number(e.id.substr(5)), $scope.datosDGA.PERMISO_AMBIENTAL) >= 0)
            {
                $('#' + e.textContent).dxCheckBox('instance').option('value', true);
            } else {
                $('#' + e.textContent).dxCheckBox('instance').option('value', false);
            }
        });
        $scope.datosDGA.PERMISO_AMBIENTAL = [];

        $scope.cargandoVisible = false;
        //$scope.$apply();
    }).fail(function (jqxhr, textStatus, error) {
        $scope.cargandoVisible = false;
        alert(error);
    });

    $scope.dxTabsOptions = {
        dataSource: ($('#app').data('iddga') == 0 ? tabsDataNuevo : tabsData),
        onItemClick: (function (itemData) {
            $scope.setTab(($('#app').data('iddga') == 0 ? tabsDataNuevo : tabsData)[itemData.itemIndex].pos);
        }),
        //bindingOptions: { selectedIndex: 'selectedTab' }
        selectedIndex: 0
    };

    $scope.AnoSettings = {
        dataSource: anosDataSource,
        placeholder: "",
        displayExpr: "N_ANO",
        valueExpr: "N_ANO",
        bindingOptions: { value: 'datosDGA.N_ANO', visible: 'datosDGA.ID_DGA == 0' }
    };

    $scope.setTab = function (tabIndex) {
        $scope.selectedTab = tabIndex;

        if (tabsData[tabIndex].text === 'Personal DGA' && !$scope.TercerosCargados) {
            $('#grdTercerosDGA').dxDataGrid({
                dataSource: grdTercerosDGADataSource
            });
            $scope.TercerosCargados = true;
        }
    };

    $scope.isActive = function (tabIndex) {
        if ($scope.selectedTab === tabIndex)
            return true;
        else
            return false;
    };

    $scope.subirArchivo = function () {
        var data = new FormData();
        //var file = $('#imagenOrganigrama').dxFileUploader('instance').option('value');
        var files = $('.dx-fileuploader-input').get(0).files;

        //data.append('id', $('#app').data('iddga'));
        data.append('id', idDGA);
        //data.append('file', file);
        for (i = 0; i < files.length; i++) {
            data.append("file" + i, files[i]);
        }

        /*data.id = $('#app').data('iddga');

        // Add the uploaded image content to the form data collection
        if (files.length > 0) {
            data.append("UploadedImage", files[0]);
        }*/

        // Make Ajax request with the contentType = false, and procesDate = false
        /*var ajaxRequest = $.ajax({
            type: "POST",
            url: "/api/fileupload/uploadfile",
            contentType: false,
            processData: false,
            data: data
        });

        ajaxRequest.done(function (xhr, textStatus) {
            // Do other operation
        });*/

        $scope.cargandoVisible = true;

        $http.post($('#app').data('url') + 'General/api/DGAApi/CargarArchivo', data, { transformRequest: angular.identity, headers: { 'Content-Type': undefined } }).success(function (data, status, headers, config) {
            //$('#imgOrganigrama').attr('src', 'data:image/png;base64,' + JSON.parse(data));
            $('#imgOrganigrama').attr('src', 'data:image/png;base64,' + data);

            $scope.datosDGA.S_ORGANIGRAMA = 'OK';
            $scope.cargandoVisible = false;
        }).error(function (data, status, headers, config) {
            $scope.cargandoVisible = false;
            $scope.datosDGA.S_ORGANIGRAMA = '';
            MostrarNotificacion('alert', (data.resp === 'OK' ? 'success' : 'error'), data.mensaje);
        });

        /*
        //var files = $("#inputFile").get(0).files;
        //var file = $('#imagenOrganigrama').dxFileUploader('instance').option('value');
        var files = $('.dx-fileuploader-input').get(0).files;
        var data = new FormData();
        //data.append("file", file);
        for (i = 0; i < files.length; i++) {
            data.append("file" + i, files[i]);
        }

        $.ajax({
            type: 'POST',
            url: $('#app').data('url') + 'General/api/DGAApi/CargarArchivo2',
            contentType: false,
            processData: false,
            data: data,
            success: function (result) {
                if (result) {
                    alert('Archivos subidos correctamente');
                    $("#inputFile").val('');
                }
            }
        });*/
    };

    $scope.btnSubirArchivoSettings = {
        text: 'Cargar Archivo',
        type: 'success',
        onClick: function (params) {
            $("#file").click();
        }
    };

    $scope.btnAlmacenarSettings = {
        text: 'Almacenar',
        type: 'success',
        onClick: function (params) {
            /*var result = params.validationGroup.validate();

            if (!result.isValid) return;*/

            var result = $('#empresaGroupValidator').dxValidationGroup('instance').validate();

            if (!result.isValid) return;

            $('#btnAlmacenar').dxButton({
                disabled: true
            });

            $scope.datosDGA.D_ANO = new Date($scope.datosDGA.N_ANO, 1, 1);

            $scope.permisosAmbientales = [];
            $('span[id^="paId_"]').each(function (i, e) {
                if ($('#' + e.textContent).dxCheckBox('instance').option('value'))
                {
                    $scope.permisosAmbientales.push(parseInt(e.id.substr(5)));
                }
            });

            var params = {
                permisosAmbientales: ($scope.permisosAmbientales.length > 0 ? $scope.permisosAmbientales.join(',') : ',')
            };

            var config = {
                params: params
            };

            $scope.cargandoVisible = true;
            $http.post($('#app').data('url') + 'General/api/DGAApi/DGA', JSON.stringify($scope.datosDGA), config).success(function (data, status, headers, config) {
                $scope.cargandoVisible = false;
                MostrarNotificacion('alert', (data.resp === 'OK' ? 'success' : 'error'), data.mensaje);

                if (data.resp === 'OK')
                {

                    $(this).data('iddga', data.datos.ID_DGA);
                    idDGA = data.datos.ID_DGA;
                    $scope.selectedTab = 0;

                    $('#tabOpciones').dxTabs({
                        dataSource: tabsData,
                        onItemClick: (function (itemData) {
                            $scope.setTab(tabsData[itemData.itemIndex].pos);
                        }),
                        selectedIndex: 0
                    });

                    $scope.datosDGA = data.datos;
                    //$scope.idDGA = data.datos.ID_DGA;

                    $('span[id^="paId_"]').each(function (i, e) {
                        if ($.inArray(Number(e.id.substr(5)), $scope.datosDGA.PERMISO_AMBIENTAL) >= 0) {
                            $('#' + e.textContent).dxCheckBox('instance').option('value', true);
                        } else {
                            $('#' + e.textContent).dxCheckBox('instance').option('value', false);
                        }
                    });
                    $scope.datosDGA.PERMISO_AMBIENTAL = [];
                }

                $('#btnAlmacenar').dxButton({
                    disabled: false
                });
            }).error(function (data, status, headers, config) {
                $scope.cargandoVisible = false;
                MostrarNotificacion('alert', (data.resp === 'OK' ? 'success' : 'error'), data.mensaje);
                $('#btnAlmacenar').dxButton({
                    disabled: false
                });
            });
        }
    };

    $scope.btnCancelarSettings = {
        text: 'Volver', //text: 'Cancelar',
        type: 'danger',
        onClick: function (params) {
            window.location = $('#app').data('url') + 'General/DGA';
        }
    };
});

SIMApp.controller("PersonalDGAController", function ($scope, $location, $http) {
    $scope.datosTerceroDGA = {};
    $scope.datosTerceroDGA.TERCERO = {};
    //$scope.datosTerceroDGA.ID_DGA = $('#app').data('iddga');
    $scope.datosTerceroDGA.ID_DGA = idDGA;
    $scope.nombres = {};
    $scope.nombres['NOMBRES'] = '';
    $scope.nombres['APELLIDOS'] = '';
    $scope.cargandoPersonalVisible = false;

    $scope.nuevoTercero = false;
    $scope.cambioPersonal = false;
    $scope.visiblePopup = false;

    $scope.btnAceptarSettings = {
        text: 'Aceptar',
        type: 'success',
        visible: ($('#app').data('ro') == '1' ? false : true),
        //width: '100%',
        onClick: function (params) {
            //var result = params.validationGroup.validate();
            var result = $('#personalGroupValidator').dxValidationGroup('instance').validate();

            if (!result.isValid) return;

            $scope.datosTerceroDGA.ID_DGA = idDGA; //$('#app').data('iddga');
            var nombresTercero = $scope.nombres['NOMBRES'].split(' ');
            var apellidosTercero = $scope.nombres['APELLIDOS'].split(' ');
            $scope.datosTerceroDGA.TERCERO.S_NOMBRE1 = nombresTercero[0];
            $scope.datosTerceroDGA.TERCERO.S_NOMBRE2 = (nombresTercero[1] === null ? '' : nombresTercero[1]);
            $scope.datosTerceroDGA.TERCERO.S_APELLIDO1 = apellidosTercero[0];;
            $scope.datosTerceroDGA.TERCERO.S_APELLIDO2 = (apellidosTercero[1] === null ? '' : apellidosTercero[1]);

            //$('#btnAceptar').dxButton({
            //    disabled: true
            //});
            params.component.option('disabled', true);

            $http.post($('#app').data('url') + 'General/api/DGAApi/TerceroDGA', JSON.stringify($scope.datosTerceroDGA)).success(function (data, status, headers, config) {
                if (data.resp === 'OK') {
                    $scope.cambioPersonal = true;
                    $scope.visiblePopup = false;

                    //var popup = $('#popPersonalDGA').dxPopup('instance');
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
        visible: ($('#app').data('ro') == '1' ? false : true),
        //width: '100%',
        onClick: function (params) {
            var popup = $('#popPersonalDGA').dxPopup('instance');
            popup.hide();
        }
    };

    $scope.popPersonalDGASettings = {
        showTitle: true,
        title: 'Personal DGA',
        bindingOptions: {
            visible: 'visiblePopup'
        },
        //deferRendering: false,
        buttons: [
            {
                toolbar: 'bottom', location: 'after', widget: 'button', options: $scope.btnAceptarSettings
            },
            {
                toolbar: 'bottom', location: 'after', widget: 'button', options: $scope.btnCancelarSettings
            }
        ],
        onHidden: function () {
            var dataGrid = $('#grdTercerosDGA').dxDataGrid('instance');
            dataGrid.cancelEditData();

            if ($scope.cambioPersonal) {
                $('#grdTercerosDGA').dxDataGrid({
                    dataSource: grdTercerosDGADataSource
                });
            }

            $scope.cambioPersonal = false;
        },
    }

    $scope.ActividadEconomicaSettings = {
        dataSource: actividadEconomicaDataSource,
        placeholder: "[Seleccionar Actividad Económica]",
        title: "Actividad Económica",
        displayExpr: "S_NOMBRE_LOOKUP",
        valueExpr: "ID_ACTIVIDADECONOMICA",
        readOnly: ($('#app').data('ro') == '1' ? true : false),
        bindingOptions: { value: 'datosTerceroDGA.TERCERO.ID_ACTIVIDADECONOMICA' }
    };

    $scope.TipoDocumentoSettings = {
        dataSource: tiposIdentificacionDataSource,
        placeholder: "",
        displayExpr: "S_ABREVIATURA",
        valueExpr: "ID_TIPODOCUMENTO",
        readOnly: ($('#app').data('ro') == '1' ? true : false),
        bindingOptions: { value: 'datosTerceroDGA.TERCERO.ID_TIPODOCUMENTO', readOnly: 'datosTerceroDGA.ID_PERSONALDGA != 0' }
    };

    $scope.TipoPersonalSettings = {
        dataSource: TipoPersonalDataSource,
        placeholder: "[Tipo de Personal]",
        title: "Tipo de Personal",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_TIPOPERSONAL",
        readOnly: ($('#app').data('ro') == '1' ? true : false),
        bindingOptions: { value: 'datosTerceroDGA.ID_TIPOPERSONAL' }
    };

    $scope.ProfesionSettings = {
        dataSource: profesionesDataSource,
        placeholder: "[Seleccionar Profesión]",
        displayExpr: "S_NOMBRE_LOOKUP",
        valueExpr: "ID_PROFESION",
        readOnly: ($('#app').data('ro') == '1' ? true : false),
        bindingOptions: { value: 'datosTerceroDGA.TERCERO.ID_PROFESION' }
    };

    $scope.EsResponsableSettings = {
        dataSource: EsResponsableDataSource,
        placeholder: "",
        displayExpr: "S_NOMBRE",
        valueExpr: "S_ID",
        readOnly: ($('#app').data('ro') == '1' ? true : false),
        bindingOptions: { value: 'datosTerceroDGA.S_ESRESPONSABLE' }
    };

    $scope.TipoVinculacionSettings = {
        dataSource: tiposVinculacionDataSource,
        placeholder: "",
        displayExpr: "S_NOMBRE",
        valueExpr: "S_ID",
        readOnly: ($('#app').data('ro') == '1' ? true : false),
        bindingOptions: { value: 'datosTerceroDGA.S_TIPOPERSONAL' }
    };

    $scope.IdentificacionSettings = {
        onFocusOut: function (params) {
            if ($scope.nuevoTercero) {
                $scope.cargandoPersonalVisible = true;
                //$.getJSON($('#app').data('url') + 'General/api/DGAApi/TerceroDGAIdentificacion?idDGA=' + $('#app').data('iddga') + '&tipoDocumento=' + $scope.datosTerceroDGA.TERCERO.ID_TIPODOCUMENTO + '&identificacion=' + $scope.datosTerceroDGA.TERCERO.N_DOCUMENTON).done(function (data) {
                $.getJSON($('#app').data('url') + 'General/api/DGAApi/TerceroDGAIdentificacion?idDGA=' + idDGA + '&tipoDocumento=' + $scope.datosTerceroDGA.TERCERO.ID_TIPODOCUMENTO + '&identificacion=' + $scope.datosTerceroDGA.TERCERO.N_DOCUMENTON).done(function (data) {
                    if (data != null) {
                        $scope.datosTerceroDGA = data;

                        if ($scope.datosTerceroDGA.ID_TIPOPERSONAL == 0)
                            $scope.datosTerceroDGA.ID_TIPOPERSONAL = null;

                        if ($scope.datosTerceroDGA.TERCERO.D_NACIMIENTO != null)
                            $scope.datosTerceroDGA.TERCERO.D_NACIMIENTO = new Date($scope.datosTerceroDGA.TERCERO.D_NACIMIENTO.substring(0, 4), parseInt($scope.datosTerceroDGA.TERCERO.D_NACIMIENTO.substring(5, 7)) - 1, $scope.datosTerceroDGA.TERCERO.D_NACIMIENTO.substring(8, 10));
                        $scope.nombres['NOMBRES'] = $scope.datosTerceroDGA.TERCERO.S_NOMBRE1 + ($scope.datosTerceroDGA.TERCERO.S_NOMBRE2 === null ? '' : ' ' + $scope.datosTerceroDGA.TERCERO.S_NOMBRE2);
                        $scope.nombres['APELLIDOS'] = $scope.datosTerceroDGA.TERCERO.S_APELLIDO1 + ($scope.datosTerceroDGA.TERCERO.S_APELLIDO2 === null ? '' : ' ' + $scope.datosTerceroDGA.TERCERO.S_APELLIDO2);

                        $scope.terceroOK = true;
                    } else {
                        //$scope.datosTerceroDGA = { ID_PERSONALDGA: 0, ID_DGA: $scope.datosTerceroDGA.ID_DGA = $('#app').data('iddga'), TERCERO: { ID_TIPODOCUMENTO: $scope.datosTerceroDGA.TERCERO.ID_TIPODOCUMENTO, N_DOCUMENTON: $scope.datosTerceroDGA.TERCERO.N_DOCUMENTON, N_DIGITOVER: $scope.datosTerceroDGA.TERCERO.N_DIGITOVER } };
                        $scope.datosTerceroDGA = { ID_PERSONALDGA: 0, ID_DGA: $scope.datosTerceroDGA.ID_DGA = idDGA, TERCERO: { ID_TIPODOCUMENTO: $scope.datosTerceroDGA.TERCERO.ID_TIPODOCUMENTO, N_DOCUMENTON: $scope.datosTerceroDGA.TERCERO.N_DOCUMENTON, N_DIGITOVER: $scope.datosTerceroDGA.TERCERO.N_DIGITOVER } };
                        $scope.nombres = {};
                        $scope.nombres['NOMBRES'] = '';
                        $scope.nombres['APELLIDOS'] = '';

                        //$('#personalGroupValidator').dxValidationGroup('instance').reset();
                    }
                    $scope.nuevoTercero = false;
                    $scope.cargandoPersonalVisible = false;
                    var popupLoading = $('#popLoadingPersonal').dxLoadPanel('instance');
                    popupLoading.hide();
                }).fail(function (jqxhr, textStatus, error) {
                    $scope.cargandoPersonalVisible = false;
                    alert(error);
                });
            }
        },
        onChange: function (params) {
            $scope.terceroOK = false;
            $scope.nuevoTercero = true;

            //$scope.datosTerceroDGA = {};
            //$scope.datosTerceroDGA.TERCERO = {};
            //$scope.nombres = {};
            //$scope.nombres['NOMBRES'] = '';
            //$scope.nombres['APELLIDOS'] = '';
            $scope.CalcularDV();
        },
        readOnly: ($('#app').data('ro') == '1' ? true : false),
        bindingOptions: { value: 'datosTerceroDGA.TERCERO.N_DOCUMENTON', min: 0, max: 9, readOnly: 'datosTerceroDGA.ID_PERSONALDGA != 0' }
    };

    $scope.CalcularDV = function () {
        ObtenerDV($scope.datosTerceroDGA.TERCERO);
    };

    $scope.GeneroSettings = {
        dataSource: generosData,
        placeholder: "[Genero]",
        displayExpr: "NOMBRE",
        valueExpr: "ID",
        readOnly: ($('#app').data('ro') == '1' ? true : false),
        bindingOptions: { value: 'datosTerceroDGA.TERCERO.S_GENERO' }
    };

    $scope.grdTercerosDGASettings = {
        //dataSource: grdTercerosDGADataSource,
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
            insertEnabled: ($('#app').data('ro') == '1' ? false : true)
        },
        onInitNewRow: function (rowInfo) {
            rowInfo.cancel = true;

            $scope.datosTerceroDGA = {};
            $scope.datosTerceroDGA.TERCERO = {};
            $scope.nombres = {};
            $scope.nombres['NOMBRES'] = '';
            $scope.nombres['APELLIDOS'] = '';

            $scope.datosTerceroDGA.ID_PERSONALDGA = 0;
            
            //$scope.datosTerceroDGA.ID_DGA = $('#app').data('iddga');
            $scope.datosTerceroDGA.ID_DGA = idDGA;
            //var popup = $('#popPersonalDGA').dxPopup('instance');
            //popup.show();
            $scope.visiblePopup = true;

            var personalGroupValidator = $('#personalGroupValidator').dxValidationGroup('instance');
            if (personalGroupValidator != undefined)
                personalGroupValidator.reset();
        },
        onEditingStart: function (rowInfo) {
            rowInfo.cancel = true;
            $scope.cargandoPersonalVisible = true;
            $.getJSON($('#app').data('url') + 'General/api/DGAApi/TerceroDGA?idTerceroDGA=' + rowInfo.data.ID_PERSONALDGA).done(function (data) {
                if (data != null) {
                    $scope.datosTerceroDGA = data;

                    if ($scope.datosTerceroDGA.TERCERO.D_NACIMIENTO != null)
                        $scope.datosTerceroDGA.TERCERO.D_NACIMIENTO = new Date($scope.datosTerceroDGA.TERCERO.D_NACIMIENTO.substring(0, 4), parseInt($scope.datosTerceroDGA.TERCERO.D_NACIMIENTO.substring(5, 7)) - 1, $scope.datosTerceroDGA.TERCERO.D_NACIMIENTO.substring(8, 10));
                    $scope.nombres['NOMBRES'] = $scope.datosTerceroDGA.TERCERO.S_NOMBRE1 + ($scope.datosTerceroDGA.TERCERO.S_NOMBRE2 === null ? '' : ' ' + $scope.datosTerceroDGA.TERCERO.S_NOMBRE2);
                    $scope.nombres['APELLIDOS'] = $scope.datosTerceroDGA.TERCERO.S_APELLIDO1 + ($scope.datosTerceroDGA.TERCERO.S_APELLIDO2 === null ? '' : ' ' + $scope.datosTerceroDGA.TERCERO.S_APELLIDO2);

                    $scope.cargandoPersonalVisible = false;
                    $scope.terceroOK = true;
                } else {
                    $scope.cargandoPersonalVisible = false;
                    MostrarNotificacion('alert', 'error', 'Datos No Encontrados');
                }
                $scope.nuevoTercero = false;
                var popup = $('#popPersonalDGA').dxPopup('instance');
                popup.show();
            }).fail(function (jqxhr, textStatus, error) {
                $scope.cargandoPersonalVisible = false;
                alert(error);
            });
        },
        selection: {
            mode: 'single'
        },
        columns: [
            {
                dataField: "ID_PERSONALDGA",
                caption: 'ID',
                dataType: 'number',
                visible: false,
            }, {
                dataField: 'N_DOCUMENTO',
                width: '10%',
                caption: 'Documento',
                alignment: 'right',
                dataType: 'number',
            }, {
                dataField: 'S_RSOCIAL',
                width: '22%',
                caption: 'Nombre',
                dataType: 'string',
            }, {
                dataField: 'PROFESION',
                width: '15%',
                caption: 'Profesión',
                dataType: 'string',
            }, {
                dataField: 'N_DEDICACION',
                width: '7%',
                caption: '% Dedicación',
                dataType: 'string',
                encodeHtml: false,
            }, {
                dataField: 'N_EXPERIENCIA',
                width: '10%',
                caption: 'Exp Amb(meses)',
                dataType: 'string',
            }, {
                dataField: 'S_ESRESPONSABLE',
                width: '7%',
                caption: 'Resp.',
                dataType: 'string',
            }, {
                dataField: 'CORREO_ELECTRONICO',
                width: '8%',
                caption: 'Correo Electrónico',
                dataType: 'string',
            }, {
                dataField: 'TELEFONO',
                width: '7%',
                caption: 'Teléfono',
                dataType: 'string',
            }, {
                dataField: 'S_OBSERVACION',
                width: '14%',
                caption: 'Observación',
                dataType: 'string',
            }
        ],
    };
});

var grdTercerosDGADataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ID_TERCERO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = loadOptions.skip;
        var take = loadOptions.take;
        $.getJSON($('#app').data('url') + 'General/api/DGAApi/ProfesionalesDGA', {
            //id: $('#app').data('iddga'),
            id: idDGA,
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: true
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    }
});

var grdTercerosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ID_TERCERO","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = loadOptions.skip;
        var take = loadOptions.take;
        $.getJSON($('#app').data('url') + 'General/api/TerceroApi/Terceros', {
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: true
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        });
        return d.promise();
    }
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
            alert('falla2: ' + textStatus + ", " + error);
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

var generosData = [
        { ID: 'M', NOMBRE: 'Masculino' },
        { ID: 'F', NOMBRE: 'Femenino' },
];

var EsResponsableDataSource = [
        { S_ID: 'S', S_NOMBRE: 'Sí' },
        { S_ID: 'N', S_NOMBRE: 'No' },
];

var tiposVinculacionDataSource = [
        { S_ID: 'I', S_NOMBRE: 'Interno' },
        { S_ID: 'E', S_NOMBRE: 'Externo' },
];

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
            alert('falla2: ' + textStatus + ", " + error);
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

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'DGA');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}

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

var tabsDataNuevo = [
    { text: 'Empresa', pos: 0 },
    { text: 'Información DGA', pos: 2 },
    { text: 'Información Adicional', pos: 4 },
    { text: 'Seguimiento', pos: 5 },
];

var tabsData = [
    { text: 'Empresa', pos: 0 },
    { text: 'Personal DGA', pos: 1 },
    { text: 'Información DGA', pos: 2 },
    { text: 'Organigrama', pos: 3 },
    { text: 'Información Adicional', pos: 4 },
    { text: 'Seguimiento', pos: 5 },
];