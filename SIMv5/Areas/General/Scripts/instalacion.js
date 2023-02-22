//Globalize.culture("es-CO");
//Globalize.locale(navigator.language || navigator.browserLanguage);

var SIMApp = angular.module('SIM', ['dx']);

SIMApp.controller("InstalacionController", function ($scope, $location, $http) {
    $('.my-cloak').removeClass('my-cloak');
    $scope.selectedTab = 0;
    $scope.tiposIdentificacion = {};
    $scope.datosInstalacion = {};
    $scope.instalacionTipos = [];
    //$scope.datosInstalacion.TERCERO_INSTALACION = [{}];
    $scope.ft = true;

    $scope.TerceroSettings = {
        dataSource: terceroDataSource,
        placeholder: "[Seleccionar Propietario]",
        title: "Propietario",
        displayExpr: "S_NOMBRE_LOOKUP",
        valueExpr: "ID_TERCERO",
        readOnly: $('#app').data('idtercero') != 0 ? true : false,
        //value: $('#app').data('idtercero'),
        cancelButtonText: "Cancelar",
        pageLoadingText: "Cargando...",
        refreshingText: "Refrescando...",
        searchPlaceholder: "Buscar",
        noDataText: "Sin Datos",
        bindingOptions: { value: 'datosInstalacion.TERCERO_INSTALACION[0].ID_TERCERO' },
    };

    $scope.municipioSettings = {
        dataSource: municipiosDataSource,
        placeholder: "",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_DIVIPOLA",
        bindingOptions: { value: 'datosInstalacion.ID_DIVIPOLA' }
    };

    $scope.tipoInstalacionSettings = {
        dataSource: tiposInstalacionesDataSource,
        placeholder: "",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_TIPOINSTALACION",
        bindingOptions: { value: 'datosInstalacion.TERCERO_INSTALACION[0].ID_TIPOINSTALACION' }
    };

    $scope.tipoViaPpalSettings = {
        dataSource: tiposViaDataSource,
        placeholder: "",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_TIPOVIA",
        bindingOptions: { value: 'datosInstalacion.ID_TIPOVIAPPAL' },
        tabIndex: 1,
        searchEnabled: true,
    };

    $scope.letraViaPpalSettings = {
        dataSource: letrasViaDataSource,
        placeholder: "",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_LETRAVIA",
        bindingOptions: { value: 'datosInstalacion.ID_LETRAVIAPPAL' },
        tabIndex: 3,
    };

    $scope.sentidoViaPpalSettings = {
        dataSource: sentidosViaDataSource,
        placeholder: "",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_SENTIDO",
        bindingOptions: { value: 'datosInstalacion.S_SENTIDOVIAPPAL' },
        tabIndex: 4,
    };

    $scope.tipoViaSecSettings = {
        dataSource: tiposViaDataSource,
        placeholder: "",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_TIPOVIA",
        bindingOptions: { value: 'datosInstalacion.ID_TIPOVIASEC' },
        tabIndex: 5,
    };

    $scope.letraViaSecSettings = {
        dataSource: letrasViaDataSource,
        placeholder: "",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_LETRAVIA",
        bindingOptions: { value: 'datosInstalacion.ID_LETRAVIASEC' },
        tabIndex: 7,
    };

    $scope.sentidoViaSecSettings = {
        dataSource: sentidosViaDataSource,
        placeholder: "",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_SENTIDO",
        bindingOptions: { value: 'datosInstalacion.S_SENTIDOVIASEC' },
        tabIndex: 8,
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
        bindingOptions: { value: 'datosInstalacion.TERCERO_INSTALACION[0].ID_ACTIVIDADECONOMICA' }
    };

    $scope.btnUbicarDireccionSettings = {
        text: '',
        icon: 'globe',
        type: 'normal',
        onClick: function (params) {

            var direccion = (($('#tipoViaPpal').dxSelectBox('instance').option('text') ?? '').replace('(CL)', '').replace('(CR)', '').replace('(AV)', '').replace('(CQ)', '').replace('(DG)', '').replace('(TV)', '').replace('(OT)', '') + ' ' + ($('#numeroPpal').dxTextBox('instance').option('text') ?? '') + ' ' + ($('#letraViaPpal').dxSelectBox('instance').option('text') ?? '') + ' ' + ($('#sentidoViaPpal').dxSelectBox('instance').option('text') ?? '') + ' ' + ($('#numeroSec').dxTextBox('instance').option('text') ?? '') + ' ' + ($('#letraViaSec').dxSelectBox('instance').option('text') ?? '') + ' ' + ($('#sentidoViaSec').dxSelectBox('instance').option('text') ?? '') + '-' + ($('#placa').dxTextBox('instance').option('text') ?? '')).replace('  ', ' ').replace('  ', ' ').replace('  ', ' ');

            abrirUrbanoRuralLite(direccion, $('#cboMunicipio').dxSelectBox('instance').option('text'), $scope.datosInstalacion.N_COORDX ?? '', $scope.datosInstalacion.N_COORDY ?? '');
        }
    };

    $scope.btnAlmacenarSettings = {
        text: 'Almacenar',
        type: 'success',
        onClick: function (params) {
            //alert(JSON.stringify($scope.datosInstalacion));

            var result = params.validationGroup.validate();

            if (!result.isValid) return;

            $('#btnAlmacenar').dxButton({
                disabled: true
            });

            $scope.instalacionTipos = [];
            $('span[id^="tiId_"]').each(function (i, e) {
                if ($('#' + e.textContent).dxCheckBox('instance').option('value')) {
                    $scope.instalacionTipos.push(parseInt(e.id.substr(5)));
                }
            });

            var params = {
                instalacionTipo: ($scope.instalacionTipos.length > 0 ? $scope.instalacionTipos.join(',') : ',')
            };

            var config = {
                params: params
            };
            
            $http.post($('#app').data('url') + 'General/api/InstalacionApi/Instalacion', JSON.stringify($scope.datosInstalacion)).success(function (data, status, headers, config) {
                MostrarNotificacion('alert', (data.resp === 'OK' ? 'success' : 'error'), data.mensaje);
                if (data.resp === 'OK')
                {
                    $scope.datosInstalacion = data.datos;
                    //alert(JSON.stringify(data.datos));
                    $scope.$apply();

                    $('span[id^="tiId_"]').each(function (i, e) {
                        if ($.inArray(Number(e.id.substr(5)), $scope.datosInstalacion.INSTALACION_TIPO) >= 0) {
                            $('#' + e.textContent).dxCheckBox('instance').option('value', true);
                        } else {
                            $('#' + e.textContent).dxCheckBox('instance').option('value', false);
                        }
                    });

                    $scope.datosInstalacion.INSTALACION_TIPO = [];

                    $scope.mensajeValidacion = '';
                    $scope.ValidarAE($scope.datosInstalacion.TERCERO_INSTALACION[0]);
                }

                $('#btnAlmacenar').dxButton({
                    disabled: false
                });
            }).error(function (data, status, headers, config) {
                MostrarNotificacion('alert', (data.resp === 'OK' ? 'success' : 'error'), data.mensaje);
                $('#btnAlmacenar').dxButton({
                    disabled: false
                });
            });
        }
    };

    $scope.btnCancelarSettings = {
        text: ($('#app').data('t') == 0 || $('#app').data('t') == null) && ($('#app').data('i') == 0 || $('#app').data('i') == null)  ? 'Cancelar' : 'Volver',
        type: 'danger',
        onClick: function (params) {
            if (($('#app').data('t') == 0 || $('#app').data('t') == null) && ($('#app').data('i') == 0 || $('#app').data('i') == null))
                window.location = $('#app').data('url');
            else if ($('#app').data('i') != 0 && $('#app').data('i') != null)
                window.location = $('#app').data('url') + '/General/Instalacion';
            else
                window.location = $('#app').data('url') + 'General/Tercero/Tercero/' + $('#app').data('t')
            
        }
    };

    $scope.dxTabsOptions = {
        dataSource: tabsData,
        onItemClick: (function (itemData) {
            $scope.setTab(itemData.itemIndex);
        }),
        selectedIndex: 0
    };

    if ($('#app').data('idinstalacion') != '0') {
        $.getJSON($('#app').data('url') + 'General/api/InstalacionApi/Instalacion', {
            id: $('#app').data('idinstalacion'), idTercero: 0,
        }).done(function (data) {
            if (data.D_REGISTRO != null)
                data.D_REGISTRO = new Date(data.D_REGISTRO.substring(0, 4), parseInt(data.D_REGISTRO.substring(5, 7)) - 1, data.D_REGISTRO.substring(8, 10));

            if (data.TERCERO_INSTALACION[0].D_INICIO != null)
                data.TERCERO_INSTALACION[0].D_INICIO = new Date(data.TERCERO_INSTALACION[0].D_INICIO.substring(0, 4), parseInt(data.TERCERO_INSTALACION[0].D_INICIO.substring(5, 7)) - 1, data.TERCERO_INSTALACION[0].D_INICIO.substring(8, 10));

            $scope.datosInstalacion = data;

            $('span[id^="tiId_"]').each(function (i, e) {
                if ($.inArray(Number(e.id.substr(5)), $scope.datosInstalacion.INSTALACION_TIPO) >= 0) {
                    $('#' + e.textContent).dxCheckBox('instance').option('value', true);
                } else {
                    $('#' + e.textContent).dxCheckBox('instance').option('value', false);
                }
            });

            $scope.datosInstalacion.INSTALACION_TIPO = [];

            $scope.ValidarAE($scope.datosInstalacion.TERCERO_INSTALACION[0]);

            $scope.$apply();
        }).fail(function (jqxhr, textStatus, error) {
            alert(error);
        });
    } else {
        if ($('#app').data('idtercero') != '0') {
            //alert($('#app').data('idtercero') * (-1));
            $.getJSON($('#app').data('url') + 'General/api/InstalacionApi/Instalacion', {
                id: 0, idTercero: $('#app').data('idtercero'),
            }).done(function (data) {
                if (data.D_REGISTRO != null)
                    data.D_REGISTRO = new Date(data.D_REGISTRO.substring(0, 4), parseInt(data.D_REGISTRO.substring(5, 7)) - 1, data.D_REGISTRO.substring(8, 10));

                if (data.TERCERO_INSTALACION[0].D_INICIO != null)
                    data.TERCERO_INSTALACION[0].D_INICIO = new Date(data.TERCERO_INSTALACION[0].D_INICIO.substring(0, 4), parseInt(data.TERCERO_INSTALACION[0].D_INICIO.substring(5, 7)) - 1, data.TERCERO_INSTALACION[0].D_INICIO.substring(8, 10));

                $scope.datosInstalacion = data;

                $('#instalacionGroupValidator').dxValidationGroup('instance').reset();

                $scope.ValidarAE($scope.datosInstalacion.TERCERO_INSTALACION[0]);

                //$scope.$apply();
            }).fail(function (jqxhr, textStatus, error) {
                alert(error);
            });
            //$scope.datosInstalacion.TERCERO_INSTALACION[0].ID_TERCERO = $('#app').data('idtercero');
        }
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

    $scope.setTab = function (tabIndex) {
        $scope.selectedTab = tabIndex;
    };

    $scope.isActive = function (tabIndex) {
        if ($scope.selectedTab === tabIndex)
            return true;
        else
            return false;
    };
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
            alert('falla: ' + textStatus + ", " + error);
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
            //alert(JSON.stringify(data.datos));
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('falla: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
});

var terceroDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var searchValueOptions = loadOptions.searchValue;
        var searchExprOptions = loadOptions.searchExpr;

        var skip = loadOptions.skip;
        var take = loadOptions.take;

        $.getJSON($('#app').data('url') + 'General/api/TerceroApi/Terceros', {
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
            alert('falla: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
    byKey: function (key, extra) {
        //alert('hols');
        //alert(key);
        //alert(JSON.stringify(extra));
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
            //alert(JSON.stringify(data.datos));
            //alert(data.datos[0].S_NOMBRE_LOOKUP);
            d.resolve(data.datos, { totalCount: data.numRegistros });
            //$scope.datosInstalacion.TERCERO_INSTALACION[0].ID_TERCERO = 
            //datosInstalacion.TERCERO_INSTALACION[0].ID_TERCERO
        }).fail(function (jqxhr, textStatus, error) {
            alert('falla: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
});

var tabsData = [
    { text: 'Información Básica' },
    { text: 'Información Técnica' },
    { text: 'Tipos' },
    /*{ text: 'Localización' },*/
];

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Instalación');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}

function wait(ms) {
    var deferred = $.Deferred();
    setTimeout(function () { deferred.resolve() }, ms);
    return deferred.promise();
}

function abrirUrbanoRuralLite(direccion, municipio, x, y)
{
    $("#divUbicacionUrbana").dialog(
        {

            width: 900,
            height: 500,
            modal: true
        });
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", "../../../LimiteUrbano/Limite/UbicacionUrbanaLite");

    form.setAttribute("target", "frm_UbicacionUrbana");

    var campo1 = document.createElement("input");
    campo1.setAttribute("type", "hidden");
    campo1.setAttribute("name", "direccion");
    campo1.setAttribute("value", direccion);
    form.appendChild(campo1);
    var campo2 = document.createElement("input");
    campo2.setAttribute("type", "hidden");
    campo2.setAttribute("name", "municipio");
    campo2.setAttribute("value", municipio);
    form.appendChild(campo2);
    var campo3 = document.createElement("input");
    campo3.setAttribute("type", "hidden");
    campo3.setAttribute("name", "tipo");
    campo3.setAttribute("value", 2);
    form.appendChild(campo3);
    var campo4 = document.createElement("input");
    campo4.setAttribute("type", "hidden");
    campo4.setAttribute("name", "id");
    campo4.setAttribute("value", 0);
    form.appendChild(campo4);
    var campo5 = document.createElement("input");
    campo5.setAttribute("type", "hidden");
    campo5.setAttribute("name", "x");
    campo5.setAttribute("value", x);
    form.appendChild(campo5);
    var campo6 = document.createElement("input");
    campo6.setAttribute("type", "hidden");
    campo6.setAttribute("name", "y");
    campo6.setAttribute("value", y);
    form.appendChild(campo6);
    
    document.body.appendChild(form);
    form.submit();
}

function getgeografico(x, y, idPregunta, idCampo, direccion, municipio) {
    $("#coordenadaX").dxTextBox('instance').option('value', x);
    $("#coordenadaY").dxTextBox('instance').option('value', y);
    $("#divUbicacionUrbana").dialog("close");
}