$(function () {
    pageonload();
});

$('#DWTcontainer').hover(function () {
    $(document).bind('mousewheel DOMMouseScroll', function (event) {
        stopWheel(event);
    });
}, function () {
    $(document).unbind('mousewheel DOMMouseScroll');
});

var SIMApp = angular.module('SIM', ['dx']);

SIMApp.controller("DigitalizarController", function ($scope, $http) {
    $scope.digitalizar = {};
    $scope.digitalizar.CBDocumento = null;
    $scope.cargandoVisible = false;
    $scope.selectedTab = 0;
    $scope.indicesCargados = false;

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
        } else {
            $scope.indicesCargados = false;
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

    $scope.btnCargarIndicesSettings = {
        text: 'Cargar Indices',
        type: 'success',
        width: '100%',
        onClick: function (params) {
            $scope.indicesCargados = $scope.digitalizar.CBDocumento != null && $scope.digitalizar.CBDocumento != '';
        }
    }

    $scope.btnScanSettings = {
        text: 'Escanear Documento',
        type: 'success',
        width: '100%',
        onClick: function (params) {
            acquireImage();
        }
    }

    $scope.btnAlmacenarDocumentoSettings = {
        text: 'Almacenar Documento',
        type: 'success',
        width: '100%',
        onClick: function (params) {
            var result = params.validationGroup.validate();
            if (!result.isValid) return;

            $('#btnAlmacenarDocumento').dxButton({
                disabled: true
            });

            $scope.cargandoVisible = true;

            var file = $('#documentoDigitalizado').dxFileUploader('instance').option('value');

            if (file != null)
            {
                var data = new FormData();

                data.append('file', file);

                $http.post($('#app').data('url') + 'GestionDocumental/api/DigitalizacionApi/Digitalizar?documentoCB=' + $scope.digitalizar.CBDocumento,
                    data,
                    { transformRequest: angular.identity, headers: { 'Content-Type': undefined } }
                ).success(function (data, status, headers, config) {
                    $('#btnAlmacenarDocumento').dxButton({
                        disabled: false
                    });

                    $scope.cargandoVisible = false;

                    //if (data.id != null && data.id != 0) {
                    $scope.digitalizar.CBDocumento = null;
                    params.validationGroup.reset();

                    file = $('#documentoDigitalizado').dxFileUploader('instance').option('value', null);

                    MostrarNotificacion('alert', '', 'Digitalización Registrada Satisfactoriamente.');
                }).error(function (data, status, headers, config) {
                    $scope.cargandoVisible = false;
                    MostrarNotificacion('alert', '', data.Message);
                    $('#btnAlmacenarDocumento').dxButton({
                        disabled: false
                    });

                    $scope.$apply();
                });
            } else if (DWObject !== undefined && DWObject !== null && ((DWObject.SelectedImagesCount == 1) || (DWObject.SelectedImagesCount == DWObject.HowManyImagesInBuffer))) {
                DWObject.HTTPUploadAllThroughPostAsPDF(
                    $('#app').data('url'),
                    'GestionDocumental/api/DigitalizacionApi/Digitalizar?documentoCB=' + $scope.digitalizar.CBDocumento,
                    $scope.digitalizar.CBDocumento + '.pdf',
                    function (httpResponse) {
                        $('#btnAlmacenarDocumento').dxButton({
                            disabled: false
                        });

                        $scope.cargandoVisible = false;

                        //if (data.id != null && data.id != 0) {
                            $scope.digitalizar.CBDocumento = null;
                            params.validationGroup.reset();

                            MostrarNotificacion('alert', '', 'Digitalización Registrada Satisfactoriamente.');
                        //} else {
                        //    MostrarNotificacion('alert', '', 'Error - ' + data.detalleRespuesta);
                        //}
                    },
                    function (errorCode, errorString, httpResponse) {
                        $scope.cargandoVisible = false;
                        MostrarNotificacion('alert', '', errorString);
                        $('#btnAlmacenarDocumento').dxButton({
                            disabled: false
                        });

                        $scope.$apply();
                    }
                );
            }
            else
            {
                $scope.cargandoVisible = false;

                $('#btnAlmacenarDocumento').dxButton({
                    disabled: false
                });

                MostrarNotificacion('alert', '', 'No hay archivo para almacenar. Por favor selecione un archivo o escanee un documento.');
            }
        }
    };
});

var tabsData = [
        { text: 'Digitalizar' },
        { text: 'Indices' },
];

function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        DevExpress.ui.dialog.alert(msg, 'Digitalización');
    } else {
        DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}
