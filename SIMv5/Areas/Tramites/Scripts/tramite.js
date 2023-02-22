var tabsDataNuevo;
var tabsData;

var SIMApp = angular.module('SIM', ['dx']);

//Globalize.culture("es");
//Globalize.locale(navigator.language || navigator.browserLanguage);

SIMApp.controller("TramiteController", function ($scope, $http, $rootScope) {
    $('.my-cloak').removeClass('my-cloak');
    $scope.cargandoVisible = false;

    function init() {
        //consultarJsonDetalle();

    }

    $scope.idTramite = $('#app').data('idtramite');
    $scope.IdTercero = $('#app').data('idtercero');
    $scope.idInstalacion = $('#app').data('idinstalacion');


    $scope.NuevoTramite = function (idTramite, idInstalacion, idTercero) {

        $scope.idTramite = idTramite;
        $scope.IdTercero = idTercero;
        $scope.idInstalacion = idInstalacion;


        //
        $.getJSON($('#app').data('url') + 'Tramites/api/TramitesApi/Tramite', {
            id: $scope.idTramite, idInstalacion: $scope.idInstalacion, idTercero: $scope.IdTercero  //Los parametros que vienen de Index.cshtml
        }).done(function (data) {
            var datos = data.split("||");
            $scope.respuesta = datos[1];

            var JSONArray = [];
            JSONArray = JSON.parse(datos[0]);

            $('[id$=txtnombretramite]').text(JSONArray[0].TRAMITE);
            $('[id$=txtDescripcion]').text(JSONArray[0].DESCRIPCION);


            if ($scope.respuesta == "Ok") {
                $('#btnSiguiente').dxButton({
                    disabled: true

                });

                $scope.setTab(2, 2);
                $scope.isActive(2);
                tabsData[0].disabled = true;
                tabsData[1].disabled = true;
                tabsData[2].disabled = false;
                tabsData[3].disabled = false;
                $scope.mostrarBtnCancelar();

                $("#btnVistaPrev").css("display", "block");

                //Consultar
                $scope.cargandoVisible = true;
                $http({
                    method: "POST",
                    url: "../Tramites/NuevoTramite/consultarJsonDetalle",
                    params: { idTramite: $scope.idTramite, idInstalacion: $scope.idInstalacion, idTercero: $scope.IdTercero },
                }).then(function mySuccess(response) {

                    var x = response.data.split("|");

                    $scope.jsonDetalle = JSON.parse(x[0]);
                    $("#txtVisita").val(x[2]);
                    if (x[1] == "Detalle") {
                        $scope.html = consultarDetalle($scope.jsonDetalle, 0, 0, "acordionDetallePrincipal");
                        $("#acordionDetallePrincipal").remove();
                        $("#acordionDetalleGeneral").accordion(
                            {
                                collapsible: true,
                                heightStyle: "content",
                                navigation: true,
                                active: true
                            });
                        $("#acordionDetalleGeneral").append($scope.html);

                        //
                        var cont = 0;
                        var flag = 0;
                        for (var i = 0; i < $scope.jsonDetalle.length; i++) {
                            if ($scope.jsonDetalle[i].ITEMCARACTERISTICA.length == 0) {
                                cont++;
                                $("#btnAdd" + cont).click();
                                $("#xyz" + i).click();
                            }
                            else {
                                flag++;
                                $('#btnSiguiente').dxButton({
                                    disabled: false

                                });
                                $("#xyz" + i).click();
                            }
                        }

                        //
                        if (flag > 0) {
                            $("#btnVistaPrev").css("display", "block");
                        }
                        else {
                            $("#btnVistaPrev").css("display", "none");
                        }

                    }
                    else {
                        jsonEncuestas = eval('(' + x[0] + ')');
                        var html = consultarEncuestas(jsonEncuestas);
                        $("#acordionEncuestaPrincipal").remove();
                        $("#acordionEncuestaGeneral").append(html);

                        var cont = 0;
                        for (var i = 0; i < jsonEncuestas.length; i++) {
                            cont++;
                            $("#btnAdd" + cont).click();
                            $("#xyz" + i).click();

                        }
                    }
                    $scope.cargandoVisible = false;

                }, function myError(response) {
                    $scope.myWelcome = response.statusText;
                    $scope.cargandoVisible = false;
                });
                //
            }
            else {
                tabsData[0].disabled = true;
                tabsData[1].disabled = false;
                tabsData[2].disabled = true;
                tabsData[3].disabled = true;
                $scope.setTab(1, 1);
                $scope.isActive(1);

                //Consultar
                $scope.cargandoVisible = true;
                $http({
                    method: "POST",
                    url: "../Tramites/NuevoTramite/consultarJsonDetalle",
                    params: { idTramite: $scope.idTramite, idInstalacion: $scope.idInstalacion, idTercero: $scope.IdTercero },
                }).then(function mySuccess(response) {

                    var x = response.data.split("|");

                    $scope.jsonDetalle = JSON.parse(x[0]);
                    $("#txtVisita").val(x[2]);
                    if (x[1] == "Detalle") {
                        $scope.html = consultarDetalle($scope.jsonDetalle, 0, 0, "acordionDetallePrincipal");
                        $("#acordionDetallePrincipal").remove();
                        $("#acordionDetalleGeneral").accordion(
                            {
                                collapsible: true,
                                heightStyle: "content",
                                navigation: true,
                                active: true
                            });
                        $("#acordionDetalleGeneral").append($scope.html);

                        //
                        var cont = 0;
                        for (var i = 0; i < $scope.jsonDetalle.length; i++) {
                            if ($scope.jsonDetalle[i].ITEMCARACTERISTICA.length == 0) {
                                cont++;
                                $("#btnAdd" + cont).click();
                                $("#xyz" + i).click();
                            }
                            else {
                                $('#btnSiguiente').dxButton({
                                    disabled: false

                                });
                                $("#xyz" + i).click();
                            }
                        }

                    }
                    else {
                        jsonEncuestas = eval('(' + x[0] + ')');
                        var html = consultarEncuestas(jsonEncuestas);
                        $("#acordionEncuestaPrincipal").remove();
                        $("#acordionEncuestaGeneral").append(html);

                        var cont = 0;
                        for (var i = 0; i < jsonEncuestas.length; i++) {
                            cont++;
                            $("#btnAdd" + cont).click();
                            $("#xyz" + i).click();

                        }
                    }
                    $scope.cargandoVisible = false;

                }, function myError(response) {
                    $scope.myWelcome = response.statusText;
                    $scope.cargandoVisible = false;
                });

            }

        }).fail(function (jqxhr, textStatus, error) {
            alert(error);
        });
        //

    };



    $scope.selectedTab = 0;
    $scope.siAcepto;
    $scope.noAcepto;



    $scope.idRequisito = 0;

    $("#btnVistaPrev").css("display", "none");
    //  $("#btnGuarPar").css("display", "none");

    tabsData = [
        { text: 'Información Tercero', pos: 0 },
        { text: 'Trámite', pos: 1 },
        { text: 'Características', pos: 2 },
        { text: 'Requisitos', pos: 3 },
    ];

    tabsData[1].disabled = true;
    tabsData[2].disabled = true;
    tabsData[3].disabled = true;

    // Le doy funcionamiento a los Tabs
    $scope.setTab = function (tabIndex, pos) {
        $scope.selectedTab = tabIndex;
    };

    // Determina si el indice corresponde al tab activo
    $scope.isActive = function (tabIndex) {
        if ($scope.selectedTab === tabIndex)
            return true;
        else
            return false;
    };

    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////TERMINOS Y CONDICIONES///////////////////////////////////////////////      
    //////////////////////////////////////////////////////////////////////////////////////////////////////////

    // cargo del api los datos que ya estan almacenados en la tabla de la DB y se lo asigno a una variable
    if ($scope.idInstalacion > 0) {
        $.getJSON($('#app').data('url') + 'Tramites/api/TramitesApi/Tramite', {
            id: $('#app').data('idtramite'), idInstalacion: $scope.idInstalacion, idTercero: $scope.IdTercero  //Los parametros que vienen de Index.cshtml
        }).done(function (data) {
            var datos = data.split("||");
            $scope.respuesta = datos[1];

            var JSONArray = [];
            JSONArray = JSON.parse(datos[0]);

            $('[id$=txtnombretramite]').text(JSONArray[0].TRAMITE);
            $('[id$=txtDescripcion]').text(JSONArray[0].DESCRIPCION);


            if ($scope.respuesta == "Ok") {
                $('#btnSiguiente').dxButton({
                    disabled: true

                });

                $scope.setTab(2, 2);
                $scope.isActive(2);
                tabsData[0].disabled = true;
                tabsData[1].disabled = true;
                tabsData[2].disabled = false;
                tabsData[3].disabled = false;
                $scope.mostrarBtnCancelar();

                $("#btnVistaPrev").css("display", "block");
                // $("#btnGuarPar").css("display", "block");
            }

        }).fail(function (jqxhr, textStatus, error) {
            alert(error);
        });
    }
    $scope.checkboxSi = function (e) {

        $('#btnSiguiente').dxButton({
            disabled: true
        });

        if (e.value == true) {
            $scope.siAcepto == true;
            $scope.noAcepto = false;

            $('#btnSiguiente').dxButton({
                disabled: false
            });
        }
        else {
            $scope.siAcepto == false;
            $scope.noAcepto = true;
        }
        $('#chkNO').dxCheckBox({
            value: false
        });

    };

    $scope.checkboxNo = function (e) {
        if (e.value == true) {
            $scope.siAcepto == false;
            $scope.noAcepto = true;
        }
        else {
            $scope.siAcepto == true;
            $scope.noAcepto = false;
        }

        $('#chkSI').dxCheckBox({
            value: false
        });
    };

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////

    //Asignamos la posición al Tab
    $scope.setTab = function (tabIndex, pos) {
        $scope.selectedTab = pos;
    }

    //mostrar o esconder el boton siguiente
    $scope.mostrarBtnSiguiente = function () {
        if ($scope.selectedTab == 0) {
            return false;
        } else if ($scope.selectedTab == 1) {
            return true;
        } else if ($scope.selectedTab == 2) {
            return true;
        } else if ($scope.selectedTab == 3) {
            return true;
        }
    };

    // mostrar o esconder el botón cancelar
    $scope.mostrarBtnCancelar = function () {
        if ($scope.selectedTab == 0) {
            return false;
        } else if ($scope.selectedTab == 1) {
            return true;
        } else if ($scope.selectedTab == 2) {
            return true;
        } else if ($scope.selectedTab == 3) {
            return true;
        }
    }


    //Configuro los tabs
    $scope.dxTabsOptions = {
        dataSource: tabsData,
        onItemClick: (function (itemData) {
            $scope.setTab(itemData.itemIndex, tabsData[itemData.itemIndex].pos) // que tome el indice y la posición;

            if ($scope.selectedTab == 0) {
                $("#btnVistaPrev").css("display", "none");
                //    $("#btnGuarPar").css("display", "none");
            }
            else if ($scope.selectedTab == 1) {
                $("#btnSiguiente").text("Siguiente");
                $("#btnCancelar").text("Cancelar");
                $("#btnVistaPrev").text("Vista Previa");
                // $("#btnGuarPar").text("Guardar Parcial");
                $("#btnVistaPrev").css("display", "none");
                // $("#btnGuarPar").css("display", "none");
                $('#btnSiguiente').css('padding-top', '4px');
                $('#btnCancelar').css('padding-top', '4px');
            }
            else if ($scope.selectedTab == 2) {
                $("#btnSiguiente").text("Siguiente");
                $("#btnCancelar").text("Cancelar");
                $("#btnVistaPrev").text("Vista Previa");
                //  $("#btnGuarPar").text("Guardar Parcial");
                $("#btnVistaPrev").css("display", "block");
                // $("#btnGuarPar").css("display", "block");

                $('#btnSiguiente').css('padding-top', '4px');
                $('#btnCancelar').css('padding-top', '4px');
                $('#btnVistaPrev').css('padding-top', '4px');
            }
            else if ($scope.selectedTab == 3) {

                $('#btnSiguiente').dxButton({
                    disabled: false

                });

                $("#btnSiguiente").text("Validar");
                $("#btnCancelar").text("Regresar");
                $("#btnVistaPrev").text("Vista Previa");
                //   $("#btnGuarPar").text("Guardar Parcial");
                $("#btnVistaPrev").css("display", "block");
                //  $("#btnGuarPar").css("display", "block");
                $('#btnSiguiente').css('padding-top', '4px');
                $('#btnCancelar').css('padding-top', '4px');
                $('#btnVistaPrev').css('padding-top', '4px');

                mostrarRequisitos($scope.idTramite, $scope.idInstalacion, $scope.idRequisito);



            }
        }),
        selectedIndex: 0
    };


    $scope.isActive = function (tabIndex) {
        if ($scope.selectedTab === tabIndex)
            return true;
        else
            return false;
    };

    ////////////Boton validar y enviar el tramite/////////////
    $scope.btnValidaryEnviar = {

        type: 'success',
        text: 'Aceptar',
        onClick: function (params) {
            //$('#btnEnviarT').dxButton({
            //    disabled: true

            //});
            parent.$("#CargarVistaReporte").dialog('close');
            $scope.cargandoVisible = true;

            //Codigo de la tarea = 4145 primera vez
            // codigo debe avanzar 4146

            //Generar radicado
            var idRadicado = 0;
            var unidadDocumental = 10   //preguntar a Jorge cual es mi unidad documental

            $.ajax({
                type: "GET",
                url: "../Tramites/api/RadicadorUDApi/Radicar",
                data: { idUnidadDocumental: unidadDocumental, tipoRetorno: 'key', tipoRadicado: 'FuncionarioTL' },
                success: function (response) {
                    var dato = response;
                    idRadicado = dato.IdRadicado;
                    $scope.idVisita = parseInt($("#txtVisita").val());

                    $.ajax({
                        type: "POST",
                        url: "../Tramites/NuevoTramite/ValidarTramite",
                        data: { idTramite: $scope.idTramite, idInstalacion: $scope.idInstalacion, idTercero: $scope.IdTercero, idVisita: $scope.idVisita, idRadicado: idRadicado },
                        beforeSend: function () { },
                        success: function (response) {
                            var respuesta = response.split("|");
                            if (respuesta[0] == "Ok") {
                                //
                                $scope.cargandoVisible = false;
                                mensajeAlmacenamientoFin("Su solicitud de trámite ha sido enviada con éxito, Su numero de radicado es " + respuesta[1] + ", fue enviada una copia de solicitud a su correo electronico");
                                $(".ui-dialog-titlebar-close").css('display', 'none');
                                //
                            }
                            else {
                                parent.$("#CargarVistaReporte").dialog('close');
                                mensajeAlmacenamiento(response);
                            }
                        },
                        error: function (request, status, error) {
                            alert(request.responseText);
                        }
                    });
                    //

                },
            });
        }
    };

    $scope.btnCancelarValidar = {
        text: 'Cancelar',
        type: 'danger',
        onClick: function (params) {
            parent.$("#CargarVistaReporte").dialog('close');
        }
    };

    //////////////////////////////////////////////////

    //Propiedades para el botón siguiente
    $scope.btnSiguientePestana = {

        type: 'success',
        text: 'Siguiente',
        onClick: function (params) {


            if ($scope.selectedTab == 1) {
                //$scope.almacenandoVisible = true;

                $('#btnSiguiente').dxButton({
                    disabled: true
                });

                $scope.setTab(2, 2);
                $scope.isActive(2);
                tabsData[0].disabled = true;
                tabsData[1].disabled = true;
                tabsData[2].disabled = false;
                tabsData[3].disabled = false;
                $scope.mostrarBtnCancelar();

                $("#btnVistaPrev").css("display", "block");

                //Guardo la información de los terminos y condiciones luego de acpetar y dar click en siguiente
                var textoTerminosC = parent.$("#textoTYC").val();

                $.getJSON($('#app').data('url') + 'Tramites/api/TramitesApi/GuardarInfoTyC', {
                    idTramite: $scope.idTramite, idInstalacion: $scope.idInstalacion, idTercero: $scope.IdTercero, TYC: "Prueba TL"  //$("#textoTYC").text() valor del div
                }).done(function (data) {

                })

                //Consultar
                $scope.cargandoVisible = true;
                $http({
                    method: "POST",
                    url: "../Tramites/NuevoTramite/consultarJsonDetalle",
                    params: { idTramite: $scope.idTramite, idInstalacion: $scope.idInstalacion, idTercero: $scope.IdTercero },
                }).then(function mySuccess(response) {

                    var x = response.data.split("|");

                    $scope.jsonDetalle = JSON.parse(x[0]);
                    $("#txtVisita").val(x[2]);
                    if (x[1] == "Detalle") {
                        $scope.html = consultarDetalle($scope.jsonDetalle, 0, 0, "acordionDetallePrincipal");
                        $("#acordionDetallePrincipal").remove();
                        $("#acordionDetalleGeneral").accordion(
                            {
                                collapsible: true,
                                heightStyle: "content",
                                navigation: true,
                                active: true
                            });
                        $("#acordionDetalleGeneral").append($scope.html);

                        //
                        var cont = 0;
                        var flag = 0;
                        for (var i = 0; i < $scope.jsonDetalle.length; i++) {
                            if ($scope.jsonDetalle[i].ITEMCARACTERISTICA.length == 0) {
                                cont++;
                                $("#btnAdd" + cont).click();
                                $("#xyz" + i).click();
                            }
                            else {
                                flag++;
                                $('#btnSiguiente').dxButton({
                                    disabled: false

                                });
                                $("#xyz" + i).click();
                            }
                        }

                        //
                        if (flag > 0) {
                            $("#btnVistaPrev").css("display", "block");
                        }
                        else {
                            $("#btnVistaPrev").css("display", "none");
                        }

                    }
                    else {
                        jsonEncuestas = eval('(' + x[0] + ')');
                        var html = consultarEncuestas(jsonEncuestas);
                        $("#acordionEncuestaPrincipal").remove();
                        $("#acordionEncuestaGeneral").append(html);

                        var cont = 0;
                        for (var i = 0; i < jsonEncuestas.length; i++) {
                            cont++;
                            $("#btnAdd" + cont).click();
                            $("#xyz" + i).click();

                        }


                    }
                    $scope.cargandoVisible = false;

                }, function myError(response) {
                    $scope.myWelcome = response.statusText;
                    $scope.cargandoVisible = false;
                });
                //

            }

            else if ($scope.selectedTab == 2) {
                //$('#btnSiguiente').dxButton({
                //    disabled: true

                //});

                $scope.setTab(3, 3);
                $scope.isActive(3);
                $scope.mostrarBtnCancelar();

                //Luego de guardar las caracteristicas y dar clic en el botón siguiente, se muestran los requisitos

                $("#btnSiguiente").text("Validar");
                $("#btnCancelar").text("Regresar");
                $('#btnSiguiente').css('padding-top', '4px');
                $('#btnCancelar').css('padding-top', '4px');
                $('#btnVistaPrev').css('padding-top', '4px');


                mostrarRequisitos($scope.idTramite, $scope.idInstalacion, $scope.idRequisito);

            }
            else if ($scope.selectedTab == 3) {
                $('#btnSiguiente').dxButton({
                    disabled: false

                });

                //Validar si los requisitos estan cargados 
                $.ajax({
                    type: "POST",
                    url: "../Tramites/NuevoTramite/ValidarRequisitosCargados",
                    data: { idTramite: $scope.idTramite, idInstalacion: $scope.idInstalacion, idTercero: $scope.IdTercero },
                    beforeSend: function () { },
                    success: function (response) {

                        //Validamos si se cargaron todos los requisitos obligatorios
                        if (response == "Ok") {

                            $("#CargarVistaReporte").dialog(
                           {
                               width: 350,
                               height: 100,
                               modal: true,
                               close: function () {
                               }
                           });
                            $(".ui-dialog-titlebar-close").css('display', 'none');

                            $scope.idVisita = parseInt($("#txtVisita").val());

                            window.open('../Tramites/NuevoTramite/VistaPreviaPDF?idTram=' + $scope.idTramite + '&idInstalacion=' + $scope.idInstalacion + '&idVisita=' + $scope.idVisita);

                        }
                        else if (response == "No") {
                            mensajeAlmacenamiento("Faltan 1 ó varios requsitos por adjuntar");
                            return;
                        }
                        else {
                            mensajeAlmacenamiento("Ocurrió un error");
                            return;
                        }
                    },
                    error: function (request, status, error) {
                        alert(request.responseText);
                    }
                });


            }
        }
    };

    $scope.btnCancelarSettings = {
        text: 'Cancelar',
        type: 'danger',
        onClick: function (params) {

            if ($scope.selectedTab == 1 || $scope.selectedTab == 2) {
                window.location = $('#app').data('url');

            }
            else if ($scope.selectedTab == 3) {
                $scope.setTab(2, 2);
                $scope.isActive(2);
                $("#btnCancelar").text("Cancelar");
                $("#btnSiguiente").text("Siguiente");
            }



        }
    };

    $scope.mostrarbtnVistaPrev = function () {
        return true;
    };

    $scope.mostrarbtnGuardarPar = function () {
        return true;
    };

    $scope.btnVistaPrevia = {

        type: 'test',
        text: 'Vista Previa',
        onClick: function (params) {


            if ($scope.selectedTab == 2 || $scope.selectedTab == 3) {

                $('#btnVistaPrev').dxButton({
                    disabled: false
                });

                $scope.idVisista = parseInt($("#txtVisita").val());

                window.open('../Tramites/NuevoTramite/VistaPreviaPDF?idTram=' + $scope.idTramite + '&idInstalacion=' + $scope.idInstalacion + '&idVisita=' + $scope.idVisista);


            }

        }
    };

    $scope.btnGuardaParcial = {

        type: 'test',
        text: 'Guardar Parcial',
        onClick: function (params) {

        }
    };

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////CARACTERISTICAS////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    //Cargo el acordeon segun el idestado, tablaestado y id formulario
    if ($scope.idInstalacion > 0) {
        $scope.cargandoVisible = true;
        $http({
            method: "POST",
            url: "../Tramites/NuevoTramite/consultarJsonDetalle",
            params: { idTramite: $scope.idTramite, idInstalacion: $scope.idInstalacion, idTercero: $scope.IdTercero },
        }).then(function mySuccess(response) {

            var x = response.data.split("|");

            $scope.jsonDetalle = JSON.parse(x[0]);
            $("#txtVisita").val(x[2]);
            if (x[1] == "Detalle") {
                $scope.html = consultarDetalle($scope.jsonDetalle, 0, 0, "acordionDetallePrincipal");
                $("#acordionDetallePrincipal").remove();
                $("#acordionDetalleGeneral").accordion(
                    {
                        collapsible: true,
                        heightStyle: "content",
                        navigation: true,
                        active: true
                    });
                $("#acordionDetalleGeneral").append($scope.html);

                //
                var cont = 0;
                var flag = 0;
                for (var i = 0; i < $scope.jsonDetalle.length; i++) {
                    if ($scope.jsonDetalle[i].ITEMCARACTERISTICA.length == 0) {
                        cont++;
                        $("#btnAdd" + cont).click();
                        $("#xyz" + i).click();
                    }
                    else {
                        flag++;
                        $('#btnSiguiente').dxButton({
                            disabled: false

                        });
                        $("#xyz" + i).click();
                    }
                }

                if (flag > 0) {
                    $("#btnVistaPrev").css("display", "block");
                }
                else {
                    $("#btnVistaPrev").css("display", "none");
                }

            }
            else {
                jsonEncuestas = eval('(' + x[0] + ')');
                var html = consultarEncuestas(jsonEncuestas);
                $("#acordionEncuestaPrincipal").remove();
                $("#acordionEncuestaGeneral").append(html);

                var cont = 0;
                for (var i = 0; i < jsonEncuestas.length; i++) {
                    cont++;
                    $("#btnAdd" + cont).click();
                    $("#xyz" + i).click();

                }
            }
            $scope.cargandoVisible = false;

        }, function myError(response) {
            $scope.myWelcome = response.statusText;
            $scope.cargandoVisible = false;
        });
    }

    //Guardo la información de las caracteristicas
    $scope.guardarDetalleCaracteristicas = {
        text: 'Guardar Detalle',
        onClick: function () {

            //DETALLE
            if ($('#acordionDetallePrincipal').children().length != 0) {
                var jsoE = guardarDetalle("acordionDetallePrincipal", 0);
                var jsonOficial = JSON.stringify(jsoE);
                var jsonOficialEncuesta = "";
            }

            //ENCUESTA
            if ($('#acordionEncuestaPrincipal').children().length != 0) {
                var jsoEncuesta = guardarEncuesta(jsonEncuestas);
                var jsonOficialEncuesta = JSON.stringify(jsoEncuesta);
                var jsonOficial = "";
            }

            $.ajax({
                type: "POST",
                url: "../Tramites/NuevoTramite/GuardarInformacionDetalle",
                data: { jsonInfo: jsonOficial, idCapEstado: "-1", tblEstados: "122", idInstalacion: $scope.idInstalacion, idTramite: $scope.idTramite, jsonInfoEncuesta: jsonOficialEncuesta },
                beforeSend: function () { },
                success: function (response) {
                    var x = response.split("|");
                    if (x[0] == "Ok") {
                        mensajeAlmacenamiento("Almacenamiento Exitoso");
                        $("#btnVistaPrev").css("display", "block");
                        $("#idCaptacionEstado").val(x[1]);
                        $("#txtEstadoBase").val(x[1]);
                        $("#txtVisita").val(x[2]);
                        $('#btnSiguiente').dxButton({
                            disabled: false

                        });

                    } else {
                        mensajeAlmacenamiento("Error al almacenar la informacion detalle")

                    }
                },
                error: function (request, status, error) {
                    alert(request.responseText);
                }
            });
        }
    };

    //Mensaje de almacenamiento cuando se guardan las caracteristicas
    function mensajeAlmacenamiento(mensaje) {
        $("#msText").text(mensaje);

        $("#msAlmacenamiento").dialog({

            buttons: [
      {
          text: "Aceptar",

          click: function () { $(this).dialog("close"); },

          class: "btn btn-default "
      },
            ]
        });


    };

    function mensajeAlmacenamientoFin(mensaje) {
        $("#msText").text(mensaje);

        $("#msAlmacenamiento").dialog({

            buttons: [
      {
          text: "Aceptar",

          click: function () {
              //$(this).dialog("close");
              window.location = $('#app').data('url');
          },

          class: "btn btn-default "
      },
            ]
        });


    };

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

});


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////Iniciar Tramite//////////////////////////////////////////////////////////////////////////////

function IniciarTramite(idTramite, idInstalacion, idTercero) {

    if (idTramite == 14 || idTramite == 15 || idTramite == 16 || idTramite == 17 || idTramite == 30 || idTramite == 32 || idTramite == 74) {
        angular.element(document.getElementById('MyAngularControl')).scope().NuevoTramite(idTramite, idInstalacion, idTercero);
    }
    else {
        alert("Tramite no válido, escoge otro por favor");
        return;
    }
};

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////REQUISITOS////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




//Funcion para subir los documentos al grid 
function SubirDoC(idTramite, idRequisito, idTercero, idInstala) {

    $("#pantallaCargarImg").dialog("option", "modal", true);

    //$("#pantallaCargarImg").dialog(
    //{
    //    width: 500,
    //    height: 200,
    //    modal: true,
    //    close: function () {
    //        //some code
    //    }
    //});
    $("#fotos").attr("src", "../Tramites/NuevoTramite/SubirDocumento?idTramite=" + idTramite + "&idRequisito=" + idRequisito + "&idTercero=" + idTercero + "&idInstalacion=" + idInstala);


};

var idRequ = 0
var idInst = 0;
//Obtenemos la información del grid
//if ($scope.selectedTab == 3) {
function mostrarRequisitos(idTram, idInst, idRequ) {

    $.ajax({
        type: "POST",
        url: '../Tramites/NuevoTramite/GetRequisitosXTramite',
        data: { id: idTram, idInstalacion: idInst, idRequisito: idRequ }, //id del tramite
        success: function (response) {
            var obj = JSON.parse(response);
            gridRequisitos(obj);

        }, error: function (request, status, error) {
            alert(request.responseText);
        }

    });
}



//Ver el documento adjutado al requisito
function VerRequisito(idTram, idReq, idTerce, idInst) {

    $.ajax({
        type: "POST",
        url: '../Tramites/NuevoTramite/AbrirDocumento',
        data: { idTram: idTram, idReq: idReq, idTerce: idTerce, idInst: idInst },
        success: function (response) {

            //var url = response;
            //url.replace(/\\/g, "/");

            //window.open("@" + url);

        }, error: function (request, status, error) {
            alert(request.responseText);
        }
    });
}

//Eliminar el documento cargado
function EliminarRequisito(idTram, idReq, idTerce, idInst) {

    $.ajax({
        type: "POST",
        url: '../Tramites/NuevoTramite/DeleteRequisitoxTramite',
        data: { idTram: idTram, idReq: idReq, idTerce: idTerce, idInst: idInst },
        success: function (response) {

            alert("Documento eliminado correctamente.");
            parent.$("#cargaDocu" + idReq).attr('src', '/SIM/Content/Images/EditarVisita.png')

        }, error: function (request, status, error) {
            alert(request.responseText);
        }
    });
}


function gridRequisitos(jsonRequisitos) {

    var tercero = $('#app').data('idtercero');
    var instalacion = $scope.idInstalacion;
    var idTramite = $('#app').data('idtramite');

    $("#GrdRequisitos").dxDataGrid({
        dataSource: jsonRequisitos,
        filterRow: { visible: false },
        columns: [
       { dataField: 'ID_REQUISITO', visible: false, allowGrouping: true, caption: '', width: '10%', allowFiltering: true, dataType: 'number', allowEditing: false },
       { dataField: 'REQUISITO', caption: 'Requisito', allowGrouping: true, width: '60%', dataType: 'string', allowEditing: false },
       { dataField: 'ID_ESTADO', visible: false, allowGrouping: true, caption: '', width: '10%', allowFiltering: true, dataType: 'number', allowEditing: false },
       { dataField: 'NOMBRE_ESTADO', caption: 'Estado', alignment: 'center', allowGrouping: true, width: '10%', dataType: 'string', allowEditing: false },
       { dataField: 'ID_TRAMITE', visible: false, caption: '', allowGrouping: true, width: '20%', dataType: 'string', allowEditing: false },
         { dataField: 'FORMATO', visible: false, caption: '', allowGrouping: true, width: '20%', dataType: 'string', allowEditing: false },
          { dataField: 'ID_INSTALACION', visible: false, caption: '', allowGrouping: true, width: '20%', dataType: 'number', allowEditing: false },
             {
                 dataField: 'cargar', alignment: 'center', allowGrouping: true, caption: 'Cargar Documento', width: '10%', allowEditing: false, cellTemplate: function (container, options) {

                     $('<img src="/SIM/Content/Images/EditarVisita.png" style="width:25px;height:25px" class="btnGuardar" id="cargaDocu' + options.data.ID_REQUISITO + '" />')

                                 .attr('src', options.value)
                                 .appendTo(container);
                 }
             },
                       {
                           dataField: 'verDoc', alignment: 'center', allowGrouping: true, caption: 'Ver', width: '10%', allowEditing: false, cellTemplate: function (container, options) {

                               $('<img src="/SIM/Content/Images/Ver_Doc.png" style="width:25px;height:25px" class="btnGuardar" id="verDocureq' + options.data.ID_REQUISITO + 'tra' + options.data.ID_TRAMITE + 'terc' + tercero + '" />')

                                               .attr('src', options.value)
                                               .appendTo(container);
                           }
                       }
             , {
                 dataField: 'eliminar', alignment: 'center', allowGrouping: true, caption: 'Eliminar', width: '10%', allowEditing: false, cellTemplate: function (container, options) {

                     $('<img src="/SIM/Content/Images/delete.png" style="width:25px;height:25px" class="btnGuardar"  />')

                                 .attr('src', options.value)
                                 .appendTo(container);
                 }
             }

        ],
        height: 300,

        setCellValue: function (rowData, value) {
            rowData.ID_ESTADO = 2;
            rowData.NOMBRE_ESTADO = "Aprobado";
        },

        onCellClick: function (e) {

            var idTramite = e.data.ID_TRAMITE;
            var idRequisito = e.data.ID_REQUISITO;
            var idTercero = tercero
            var idInstala = e.data.ID_INSTALACION;
            var idEstado = e.ID_ESTADO;

            var tipoBoton = e.columnIndex;
            switch (tipoBoton) {

                case 2: //subir doc
                    SubirDoC(idTramite, idRequisito, idTercero, idInstala);
                    break;

                case 3: //ver  documento

                    VerRequisito(idTramite, idRequisito, idTercero, idInstala);

                    break;

                case 4: //eliminar documento

                    EliminarRequisito(idTramite, idRequisito, idTercero, idInstala);
                    break;

            }

        },
        scrolling: {
            mode: 'virtual',
            scrollByContent: false,
            scrollByThumb: false,
            showScrollbar: 'never'
        },

        //columnChooser: { enabled: false },
        //allowColumnReordering: true,
        //sorting: { mode: 'single' },
        //pager: { visible: true },
        //paging: { pageSize: 6 },
        //filterRow: { visible: false },
        onCellPrepared: function (cellInfo) {

            if (cellInfo.rowType == "data" && cellInfo.column.dataField === 'cargarDoc') {

                if (cellInfo.row.key.URL == "") {
                    cellInfo.cellElement.addClass('btnEditar');
                    cellInfo.column.allowEditing = false;

                } else {


                    cellInfo.cellElement.addClass('btnGuardar');

                }


            }
        }




    });

};