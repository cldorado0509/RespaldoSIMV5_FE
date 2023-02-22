var SIMApp = angular.module('SIM', ['dx']);

SIMApp.controller("FoliarController", function ($scope, $http) {
    $scope.foliar = {};
    $scope.foliar.CBTomo = null;
    $scope.foliar.CBDocumento = null;
    $scope.foliar.folioInicial = null;
    $scope.foliar.folioFinal = null;
    $scope.cargandoVisible = false;
    $scope.selectedTab = 0;

    $('.my-cloak').removeClass('my-cloak');

    $scope.SoloNumeros = function (object) {
        var e = object.jQueryEvent;

        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A
            (e.keyCode == 65 && e.ctrlKey === true) ||
            // Allow: Ctrl+C
            (e.keyCode == 67 && e.ctrlKey === true) ||
            // Allow: Ctrl+X
            (e.keyCode == 88 && e.ctrlKey === true) ||
            // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    };

    $scope.tabsOptions = {
        dataSource: tabsData,
        onItemClick: (function (itemData) {
            $scope.setTab(itemData.itemIndex);
        }),
        selectedIndex: 0
    };

    $scope.setTab = function (tabIndex) {
        $scope.selectedTab = tabIndex;
    };

    $scope.isActive = function (tabIndex) {
        if ($scope.selectedTab === tabIndex)
            return true;
        else
            return false;
    };

    $scope.btnGenerarFolioSettings = {
        text: 'Generar Folios',
        type: 'success',
        onClick: function (params) {
            var result = params.validationGroup.validate();
            if (!result.isValid) return;

            $('#btnGenerarFolio').dxButton({
                disabled: true
            });

            $scope.cargandoVisible = true;

            $http.get($('#app').data('url') + 'GestionDocumental/api/RadicadorApi/Foliar?CBTomo=' + $scope.foliar.CBTomo + '&CBDocumento=' + $scope.foliar.CBDocumento + '&folioInicial=' + $scope.foliar.folioInicial + '&folioFinal=' + $scope.foliar.folioFinal).success(function (data, status, headers, config) {
                $('#btnGenerarFolio').dxButton({
                    disabled: false
                });

                $scope.cargandoVisible = false;
                if (data.tipoRespuesta == 'OK') {
                    MostrarNotificacion('alert', '', 'Folios Registrados Satisfactoriamente.');
                    $scope.foliar.CBDocumento = null;
                    $scope.foliar.folioInicial = null;
                    $scope.foliar.folioFinal = null;
                } else
                    MostrarNotificacion('alert', '', 'Error - ' + data.detalleRespuesta);
            }).error(function (data, status, headers, config) {
                $scope.cargandoVisible = false;
                MostrarNotificacion('alert', '', 'Error - ' + data);
                $('#btnGenerarFolio').dxButton({
                    disabled: false
                });
            });
        }
    };
});

var tabsData = [
        { text: 'Foliar' },
];

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Foliar');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }

}