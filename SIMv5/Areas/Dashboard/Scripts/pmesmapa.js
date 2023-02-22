var SIMApp = angular.module('SIM', ['dx']);

SIMApp.controller("PMESMapaController", function ($scope, $location, $http) {
    $('.my-cloak').removeClass('my-cloak');

    $scope.instalacion = null;

    $scope.cargandoVisible = false;
    $scope.mapaCargado = false;

    $('#frmMapaPMES').attr('src', $('#app').data('url') + 'Dashboard/PMES/PMESMapa?e=' + $('#app').data('e') + '&v=' + $('#app').data('v') + '&t=' + $('#app').data('tercero'));

    $scope.RefrescarMapaSettings = {
        text: 'Refrescar Mapa',
        type: 'success',
        width: '100%',
        onClick: function (params) {
            var idModos = '';
            $('span[id^="mtId_"]').each(function (i, e) {
                if ($('#' + e.textContent).dxCheckBox('instance').option('value')) {
                    if (idModos == '')
                        idModos = e.id.substr(5) + '-' + $('#' + e.textContent.replace('chkmt_', 'mtRGB_')).text();
                    else
                        idModos += '|' + e.id.substr(5) + '-' + $('#' + e.textContent.replace('chkmt_', 'mtRGB_')).text();
                }
            });

            if (idModos == '') {
                $('span[id^="mtId_"]').each(function (i, e) {
                    if (idModos == '')
                        idModos = e.id.substr(5) + '-' + $('#' + e.textContent.replace('chkmt_', 'mtRGB_')).text();
                    else
                        idModos += '|' + e.id.substr(5) + '-' + $('#' + e.textContent.replace('chkmt_', 'mtRGB_')).text();
                });
            }
            $('#frmMapaPMES').attr('src', $('#app').data('url') + 'Dashboard/PMES/PMESMapa?e=' + $('#app').data('e') + '&v=' + $('#app').data('v') + '&t=' + $('#app').data('tercero') + '&i=' + ($scope.instalacion??'') + '&modos=' + idModos + '&todos=' + (idModos == '' ? 'S' : 'N'));
        }
    };

    $scope.InstalacionesSettings = {
        dataSource: instalacionDataSource,
        placeholder: "(Todas)",
        displayExpr: "S_NOMBRE",
        valueExpr: "ID_INSTALACION",
        searchEnabled: false,
        searchExpr: "S_NOMBRE",
        searchTimeout: 0,
        bindingOptions: { value: 'instalacion' }
    };

    $.get($('#app').data('url') + 'Dashboard/api/PMESApi/Instalaciones?e=' + $('#app').data('e'), function (data) {
        for (var i = 0; i < data.datos.length; i++) {
            instalacionDataSource.store().insert(data.datos[i]);
        }
        instalacionDataSource.load();
        $scope.$apply();
    }, "json");
});

var instalacionDataSource = new DevExpress.data.DataSource([]);

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'PMES');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}
