var SIMApp = angular.module('SIM', ['dx']);

var documentosSeleccionado = null;
var fechaInicialSel = null;
var fechaFinalSel = null;
var horaInicialSel = null;
var horaFinalSel = null;
var estacionesSel = null;
var idDocumentosAlmacenadosSel = null;
var listaDocumentosSel = null;

SIMApp.controller("TestExternoController", function ($scope, $location, $http) {
    $('.my-cloak').removeClass('my-cloak');

    $scope.parametros = {};
    $scope.parametros.ListaDocumentos = null;
    $scope.parametros.Estaciones = null;
    $scope.parametros.FechaInicial = null;
    $scope.parametros.FechaFinal = null;
    $scope.parametros.HoraInicial = null;
    $scope.parametros.HoraFinal = null;

    $scope.Resultado = '';
    $scope.cargandoVisible = false;

    $scope.consultarSettings = {
        text: 'Consultar',
        type: 'success',
        onClick: function (params) {
            /*$scope.cargandoVisible = true;

            $scope.parametros.ListaDocumentos = '79839056,43561488,3349950';
            $scope.parametros.Estaciones = '16,34,71,78,17,18,72,73,74,75,76,77,48,51,70,14,47,43,61,6,80,79,31,68,69,83,50,8,12,53,81,56,66,65,21,44,11,64,24,46,15,1,67,57,40,62,63,36,41,20,58,49,33,37,19,35,54,22,52,4,30,59,82,13,23,3,29,25,26,28,27,39,9,32,42,38,55,7,45,2,5,10,60,84,85,86,87,88';
            $scope.parametros.FechaInicial = '01/01/2018';
            $scope.parametros.FechaFinal = '01/04/2018';
            $scope.parametros.HoraInicial = '05:00';
            $scope.parametros.HoraFinal = '10:30';

            $.ajax({
                url:$('#app').data('url') + 'ServicioExterno/api/EnCiclaApi/ConsultaPrestamos',
                type:"POST",
                data: JSON.stringify($scope.parametros),
                contentType:"application/json; charset=utf-8",
                dataType:"json",
                success: function (data) {
                    $scope.cargandoVisible = false;
                    $scope.Resultado = JSON.stringify(data.datos);
                }
            });*/

            parametros = {};
            parametros.ListaDocumentos = '79839056,43561488,3349950';
            parametros.Estaciones = '16,34,71,78,17,18,72,73,74,75,76,77,48,51,70,14,47,43,61,6,80,79,31,68,69,83,50,8,12,53,81,56,66,65,21,44,11,64,24,46,15,1,67,57,40,62,63,36,41,20,58,49,33,37,19,35,54,22,52,4,30,59,82,13,23,3,29,25,26,28,27,39,9,32,42,38,55,7,45,2,5,10,60,84,85,86,87,88';
            parametros.FechaInicial = '01/01/2018';
            parametros.FechaFinal = '01/04/2018';
            parametros.HoraInicial = '05:00';
            parametros.HoraFinal = '10:30';

            $.ajax({
                url: $('#app').data('url') + 'ServicioExterno/api/EnCiclaApi/ConsultaPrestamos',
                type: "POST",
                data: JSON.stringify(parametros),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    alert(JSON.stringify(data.datos));
                }
            });
        }
    };
});