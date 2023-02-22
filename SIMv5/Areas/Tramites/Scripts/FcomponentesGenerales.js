//*Variables Generales*/
var objetoMapgis;

/**funcion que me ubica un punto en el mapa*/
function UbicarPunto() {
    objetoMapgis = new MapGIS("mapa");
    objetoMapgis.initMapGIS();
    onLoad("tabUbicacion");
    objetoMapgis.drawPoint(rtaUbiPoint, 32, 203, 26, 'idPuntoUbi', 4326)
}

function ZoomXYmapa(x, y, contenedor) {

    onLoad(contenedor);
    try {
        objetoMapgis = new MapGIS("mapa");
        objetoMapgis.initMapGIS();

        objetoMapgis.zoomPto(x, y, 4326, 14);


        offLoad(contenedor);
    } catch (e) {
        setTimeout(function () {
            ZoomXYmapa(x, y, contenedor)
        }, 6000);
    }

}

/*Funcion para manejar el titulo en el encabezado(comportamiento el lapiz)*/
function nombreOpcion(tipo) {
    if (tipo == 1) {
        $("#div_nom_1").hide();
        $("#div_nom_2").show();
        $("#txtNombreTemp").val($("#txtNombreOficial").text())

    } else {
        $("#div_nom_2").hide();
        $("#div_nom_1").show();
        $("#txtNombreOficial").text($("#txtNombreTemp").val())
    }
}

function AbrirAyuda(texto) {
    $("#ContenidoAyuda").remove();
    var html = " <div class='col-md-12 ' id='ContenidoAyuda'>" + texto + "</div>";
    $("#dialogAyuda").append(html);
    $("#dialogAyuda").dialog();
}

function AbrirObservacion(id, texto) {
    $("#ContenidoObservacion").remove();

    var html = " <div class='col-md-12 ' id='ContenidoObservacion'><textarea id='txt_area_observacion'>" + $("#" + id).val() + "</textarea><br>"
    html += "<button onClick='GuardarobservacionEncuestas()' class='btn btn-default btn-sm botonObserva '>Aceptar</button><input type='text' style='display:none' value='" + id + "' id='txt_id_observacion'></div>";
    $("#dialogObservacion").append(html);
    $("#dialogObservacion").dialog();
    $("#txt_area_observacion").val($("#" + id).val());
}

function GuardarobservacionEncuestas() {
    var tex = $("#txt_area_observacion").val();
    var id = $("#txt_id_observacion").val();
    $("#" + id).val(tex);
    $("#dialogObservacion").dialog("close");
}
// cargando
function onLoad(contenedor) {
    $("#" + contenedor + " #divLoad").remove();
    html = "<div class='LoadG' id='divLoad'></div>"
    $("#" + contenedor).append(html);
}

function offLoad(contenedor) {
    $("#" + contenedor + " #divLoad").remove();
}
/////Funcion que crea html caracteristicas

var jsonDetallegenerales = new Array();
//funcion que arma el html de caracteristicas
function consultarDetalle(jsonEntrante, grupo, padre, idAcordeon) {

    if (idAcordeon != "CARAC_HIJO") {
        jsonDetallegenerales[grupo] = jsonEntrante;
        var json = jsonEntrante;
        //INICIO ACORDEON
        var htmlGeneral = ' <div class="panel-group acordeonVerde " id="' + idAcordeon + '">';
        /************************************************************************************/
        var acu0 = 0;
        var acu = 0;
        for (var r = 0; r < json.length; r++) {
            acu0++;
            htmlGeneral += '<div class="panel panel-default">'
            /*ENCABEZADOO*********************************************************************************************/
            if (json[r].ITEMCARACTERISTICA.length != 0) {
                htmlGeneral += '<div class="panel-heading  glyphicon-triangle-bottom fechaleft" id="cabezote_colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '" >';
            } else {
                htmlGeneral += '<div class="panel-heading  glyphicon-triangle-bottom not-flecha fechaleft" id="cabezote_colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '" >';
            }

            htmlGeneral += '<input type="text" style="display:none" id="txtCaracteristica_' + json[r].ID_CARACTERISTICA + '" value="' + json[r].ID_CARACTERISTICA + '"> ';
            htmlGeneral += '<a id="xyz' + acu0 + '" data-toggle="collapse" data-parent="#accordion" href="#colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '" onclick="CambiarEstadoAcordeon(\'#cabezote_colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '\')" class="titleAcordeon">' + json[r].S_DESCRIPCION + '</a>'



            /******boton de agregar o quitar*************/
            if (json[r].S_CARDINALIDAD == 1) {
                
                if (json[r].ITEMCARACTERISTICA.length == 0) {
                    acu++;
                    htmlGeneral += '<button id="btnAdd' + acu + '" class="agregar" title="Agregar Detalle" onclick="agregarAcordeonitem(' + padre + ',' + grupo + ',this,' + json[r].ID_CARACTERISTICA + ',\'colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '\')"></button> </div>';

                } else {
                    htmlGeneral += '<button class="agregar quitar" title="Quitar Detalle" onclick="quitarAcordeonitem(' + padre + ',' + grupo + ', this,' + json[r].ID_CARACTERISTICA + ',\'colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + ' \')"></button> </div>'; //&&&&&&&&&&&&&&

                }
            } else {
                acu++;
                htmlGeneral += '<button id="btnAdd' + acu + '" class="agregar" title="Agregar Detalle" onclick="agregarAcordeonitem(' + padre + ',' + grupo + ',this,' + json[r].ID_CARACTERISTICA + ',\'colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + ' \')"></button> </div>';

            }


            /***********fin condicion botones************/
            htmlGeneral += '</div>'
            /*FIN ENCABEZADO***************************************************************************************/
            /***************CUERPO COLAPSABLE***********************************************************/
            htmlGeneral += '<div id="colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '" class="panel-collapse collapse">';

            /**Cuerpo acordeon**/
            if (json[r].S_CARDINALIDAD == 1) {
                if (json[r].ITEMCARACTERISTICA.length != 0) {
                    htmlGeneral += '<div class="col-md-12 contenidoSub" >'
                    var fila = "EncuestaCol1";
                    for (var v = 0; v < json[r].ITEMCARACTERISTICA[0].VARIABLES.length; v++) {
                        if (fila == "EncuestaCol1") {
                            htmlGeneral += '<div class=" row col-md-12 EncuestaCol1" >'
                            fila = "EncuestaCol2";
                        } else {
                            htmlGeneral += '<div class=" row col-md-12 EncuestaCol2" >'
                            fila = "EncuestaCol1";
                        }
                        htmlGeneral += '<div class="col-md-5 " ><label>' + json[r].ITEMCARACTERISTICA[0].VARIABLES[v].S_NOMBRE + '</label></div>'
                        json[r].ITEMCARACTERISTICA[0].NOMBRE = json.S_DESCRIPCION + '(Nuevo )';
                        htmlGeneral += '<div class="col-md-7 " >' + generarControlDetalle(json[r].ITEMCARACTERISTICA[0].VARIABLES[v], "colapse" + json[r].ID_CARACTERISTICA + "_grupo" + grupo + "_variable" + json[r].ITEMCARACTERISTICA[0].VARIABLES[v].ID_VARIABLE) + '</div>'
                        htmlGeneral += "</div>"
                    }

                    htmlGeneral += consultarDetalle(json[r].CARACTERISTICAS, grupo, json[r].ID_CARACTERISTICA, "CARAC_HIJO");

                    htmlGeneral += "</div>"
                }

            } else {
                /*nivel 2**/
                htmlGeneral += ' <div class="panel-group acordeonVerde acordeonSecundario" id="acordionSecundario">'
                if (json[r].ITEMCARACTERISTICA.length != 0) {
                    for (var f = 0 ; f < json[r].ITEMCARACTERISTICA.length; f++) {
                        htmlGeneral += ' <div class="panel panel-default panel-tercero">'
                        htmlGeneral += '  <div class="panel-heading glyphicon-triangle-bottom" id="subcabezoteAcordeon_' + json[r].ID_CARACTERISTICA + '_' + f + '">'
                        if (json[r].ITEMCARACTERISTICA[f].NOMBRE != "") {
                            htmlGeneral += ' <a data-toggle="collapse" data-parent="#accordion" id="subcollapse_' + json[r].ID_CARACTERISTICA + '_' + f + '_title" href="#subcollapse_' + json[r].ID_CARACTERISTICA + '_' + f + '" onclick="CambiarEstadoAcordeon(\'#subcabezoteAcordeon_' + json[r].ID_CARACTERISTICA + '_' + f + '\')" class="titleAcordeon"><input class="tituloTexto" id="Caracteristicassub_' + json[r].ID_CARACTERISTICA + '_' + f + '" type="text" value="' + json[r].ITEMCARACTERISTICA[f].NOMBRE + '"></a>'

                        } else {
                            htmlGeneral += ' <a data-toggle="collapse" data-parent="#accordion" id="subcollapse_' + json[r].ID_CARACTERISTICA + '_' + f + '_title" href="#subcollapse_' + json[r].ID_CARACTERISTICA + '_' + f + '" onclick="CambiarEstadoAcordeon(\'#subcabezoteAcordeon_' + json[r].ID_CARACTERISTICA + '_' + f + '\')" class="titleAcordeon"><input class="tituloTexto" id="Caracteristicassub_' + f + '" type="text" value="' + json[r].S_DESCRIPCION + '(nuevo)"></a>'

                        }

                        htmlGeneral += ' <button class="agregar quitar" title="Quitar detalle" onclick="eliminarsubacordion(' + padre + ', \'colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '\' ,' + grupo + ',\'#subcabezoteAcordeon_' + json[r].ID_CARACTERISTICA + '_' + f + '\',' + json[r].ID_CARACTERISTICA + ', \'#subcollapse_' + json[r].ID_CARACTERISTICA + '_' + f + '\')"></button>'
                        htmlGeneral += '</div>'
                        htmlGeneral += '</div>'

                        htmlGeneral += '<div id="subcollapse_' + json[r].ID_CARACTERISTICA + '_' + f + '" class="panel-collapse collapse contenidoSub">'
                        /**/
                        var fila = "EncuestaCol1";
                        for (var v = 0; v < json[r].ITEMCARACTERISTICA[f].VARIABLES.length; v++) {
                            if (fila == "EncuestaCol1") {
                                htmlGeneral += '<div class=" row col-md-12 EncuestaCol1" >'
                                fila = "EncuestaCol2";
                            } else {
                                htmlGeneral += '<div class=" row col-md-12 EncuestaCol2" >'
                                fila = "EncuestaCol1";
                            }

                            htmlGeneral += '<div class="col-md-5 " ><label>' + json[r].ITEMCARACTERISTICA[f].VARIABLES[v].S_NOMBRE + '</label></div>'
                            htmlGeneral += '<div class="col-md-7 " >' + generarControlDetalle(json[r].ITEMCARACTERISTICA[f].VARIABLES[v], "colapse" + json[r].ID_CARACTERISTICA + "_grupo" + grupo + "_variable" + json[r].ITEMCARACTERISTICA[0].VARIABLES[v].ID_VARIABLE + "_" + f) + '</div>'
                            htmlGeneral += "</div>"
                        }


                        /***/
                        htmlGeneral += ' </div>'
                    } /**final nivel2*/
                }
                htmlGeneral += '</div>';
            }
            //div que cierra el de arriba

            /**fin cuerpo***/

            htmlGeneral += '</div>'

            /*************FIN CUERPO COLAPSABLE*****************************************************************/
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        htmlGeneral += '</div>';
        //FIN ACORDEON
        return htmlGeneral;
    }
    else {
        var jsonHijo = jsonEntrante;
        var htmlGeneralHijo = "<div class='CARAC_HIJO'>"
        for (var ri = 0; ri < jsonHijo.length; ri++) {

            htmlGeneralHijo += '<div class="panel panel-default">'
            /*ENCABEZADOO*********************************************************************************************/
            if (jsonHijo[ri].ITEMCARACTERISTICA.length != 0) {
                htmlGeneralHijo += '<div class="panel-heading  glyphicon-triangle-bottom fechaleft" id="cabezote_colapse' + jsonHijo[ri].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '" >';
            } else {
                htmlGeneralHijo += '<div class="panel-heading  glyphicon-triangle-bottom not-flecha fechaleft" id="cabezote_colapse' + jsonHijo[ri].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '" >';
            }

            htmlGeneralHijo += '<input type="text" style="display:none" id="txtCaracteristica_' + jsonHijo[ri].ID_CARACTERISTICA + '" value="' + jsonHijo[ri].ID_CARACTERISTICA + '"> ';
            htmlGeneralHijo += '<a  id="xyz" data-toggle="collapse" data-parent="#accordion" href="#colapse' + jsonHijo[ri].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '" onclick="CambiarEstadoAcordeon(\'#cabezote_colapse' + jsonHijo[ri].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '\')" class="titleAcordeon">' + jsonHijo[ri].S_DESCRIPCION + '</a>'
            /******boton de agregar o quitar*************/
            if (jsonHijo[ri].S_CARDINALIDAD == 1) {
                if (jsonHijo[ri].ITEMCARACTERISTICA.length == 0) {
                    htmlGeneralHijo += '<button class="agregar" title="Agregar Detalle" onclick="agregarAcordeonitem(' + padre + ',' + grupo + ',this,' + jsonHijo[ri].ID_CARACTERISTICA + ',\'colapse' + jsonHijo[ri].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '\')"></button> </div>';

                } else {
                    htmlGeneralHijo += '<button class="agregar quitar" title="Quitar Detalle" onclick="quitarAcordeonitem(' + padre + ',' + grupo + ', this, ' + jsonHijo[ri].ID_CARACTERISTICA + ',\'colapse' + jsonHijo[ri].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + ' \')"></button> </div>'; //&&&&&&&&&&&&&&

                }
            } else {
                htmlGeneralHijo += '<button class="agregar" title="Agregar Detalle" onclick="agregarAcordeonitem(' + padre + ',' + grupo + ',this,' + jsonHijo[ri].ID_CARACTERISTICA + ',\'colapse' + jsonHijo[ri].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + ' \')"></button> </div>';

            }
            /***********fin condicion botones************/
            htmlGeneralHijo += '</div>'
            /*FIN ENCABEZADO***************************************************************************************/
            /***************CUERPO COLAPSABLE***********************************************************/
            htmlGeneralHijo += '<div id="colapse' + jsonHijo[ri].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '" class="panel-collapse collapse">';

            /**Cuerpo acordeon**/
            if (jsonHijo[ri].S_CARDINALIDAD == 1) {
                if (jsonHijo[ri].ITEMCARACTERISTICA.length != 0) {
                    htmlGeneralHijo += '<div class="col-md-12 contenidoSub" >'
                    var fila = "EncuestaCol1";
                    for (var vi = 0; vi < jsonHijo[ri].ITEMCARACTERISTICA[0].VARIABLES.length; vi++) {
                        if (fila == "EncuestaCol1") {
                            htmlGeneralHijo += '<div class=" row col-md-12 EncuestaCol1" >'
                            fila = "EncuestaCol2";
                        } else {
                            htmlGeneralHijo += '<div class=" row col-md-12 EncuestaCol2" >'
                            fila = "EncuestaCol1";
                        }
                        htmlGeneralHijo += '<div class="col-md-5 " ><label>' + jsonHijo[ri].ITEMCARACTERISTICA[0].VARIABLES[vi].S_NOMBRE + '</label></div>'
                        jsonHijo[ri].ITEMCARACTERISTICA[0].NOMBRE = jsonHijo[ri].S_DESCRIPCION + '(Nuevo )';
                        htmlGeneralHijo += '<div class="col-md-7 " >' + generarControlDetalle(jsonHijo[ri].ITEMCARACTERISTICA[0].VARIABLES[vi], "colapse" + jsonHijo[ri].ID_CARACTERISTICA + "_grupo" + grupo + "_variable" + jsonHijo[ri].ITEMCARACTERISTICA[0].VARIABLES[vi].ID_VARIABLE) + '</div>'
                        htmlGeneralHijo += "</div>"
                    }

                    htmlGeneralHijo += consultarDetalleHijos(jsonHijo[ri].CARACTERISTICAS, grupo, jsonHijo[ri].ID_CARACTERISTICA, "CARAC_HIJO");

                    htmlGeneralHijo += "</div>"
                }

            } else {
                /*nivel 2**/
                htmlGeneralHijo += ' <div class="panel-group acordeonVerde acordeonSecundario" id="acordionSecundario">'
                if (jsonHijo[ri].ITEMCARACTERISTICA.length != 0) {
                    for (var fi = 0 ; f < jsonHijo[ri].ITEMCARACTERISTICA.length; fi++) {
                        htmlGeneralHijo += ' <div class="panel panel-default panel-tercero">'
                        htmlGeneralHijo += '  <div class="panel-heading glyphicon-triangle-bottom" id="subcabezoteAcordeon_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '">'
                        if (jsonHijo[ri].ITEMCARACTERISTICA[fi].NOMBRE != "") {
                            htmlGeneralHijo += ' <a data-toggle="collapse" data-parent="#accordion" id="subcollapse_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '_title" href="#subcollapse_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '" onclick="CambiarEstadoAcordeon(\'#subcabezoteAcordeon_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '\')" class="titleAcordeon"><input class="tituloTexto" id="Caracteristicassub_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '" type="text" value="' + jsonHijo[ri].ITEMCARACTERISTICA[fi].NOMBRE + '"></a>'

                        } else {
                            htmlGeneralHijo += ' <a data-toggle="collapse" data-parent="#accordion" id="subcollapse_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '_title" href="#subcollapse_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '" onclick="CambiarEstadoAcordeon(\'#subcabezoteAcordeon_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '\')" class="titleAcordeon"><input class="tituloTexto" id="Caracteristicassub_' + f + '" type="text" value="' + jsonHijo[ri].S_DESCRIPCION + '(nuevo)"></a>'

                        }

                        htmlGeneralHijo += ' <button class="agregar quitar" title="Quitar detalle" onclick="eliminarsubacordion(' + padre + ', \'colapse' + jsonHijo[ri].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '\' ,' + grupo + ',\'#subcabezoteAcordeon_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '\',' + jsonHijo[ri].ID_CARACTERISTICA + ', \'#subcollapse_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '\')"></button>'
                        htmlGeneralHijo += '</div>'
                        htmlGeneralHijo += '</div>'

                        htmlGeneralHijo += '<div id="subcollapse_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '" class="panel-collapse collapse contenidoSub">'
                        /**/
                        var fila = "EncuestaCol1";
                        for (var v = 0; v < jsonHijo[ri].ITEMCARACTERISTICA[fi].VARIABLES.length; v++) {
                            if (fila == "EncuestaCol1") {
                                htmlGeneralHijo += '<div class=" row col-md-12 EncuestaCol1" >'
                                fila = "EncuestaCol2";
                            } else {
                                htmlGeneralHijo += '<div class=" row col-md-12 EncuestaCol2" >'
                                fila = "EncuestaCol1";
                            }
                            htmlGeneralHijo += '<div class="col-md-5 " ><label>' + jsonHijo[ri].ITEMCARACTERISTICA[fi].VARIABLES[vi].S_NOMBRE + '</label></div>'
                            htmlGeneralHijo += '<div class="col-md-7 " >' + generarControlDetalle(jsonHijo[ri].ITEMCARACTERISTICA[fi].VARIABLES[vi], "colapse" + jsonHijo[ri].ID_CARACTERISTICA + "_grupo" + grupo + "_variable" + jsonHijo[ri].ITEMCARACTERISTICA[0].VARIABLES[vi].ID_VARIABLE + "_" + f) + '</div>'
                            htmlGeneralHijo += "</div>"
                        }


                        /***/
                        htmlGeneralHijo += ' </div>'
                    } /**final nivel2*/
                }
                htmlGeneralHijo += '</div>';
            }
            //div que cierra el de arriba

            /**fin cuerpo***/

            htmlGeneralHijo += '</div>'

            /*************FIN CUERPO COLAPSABLE*****************************************************************/
        }
        htmlGeneralHijo += '</div>'
        return htmlGeneralHijo;
    }
    /**RETORNO HTML**/


}

function consultarDetalleHijos(jsonEntrante, grupo, padre, idAcordeon) {

    if (idAcordeon != "CARAC_HIJO") {
        jsonDetallegenerales[grupo] = jsonEntrante;
        var json = jsonEntrante;
        //INICIO ACORDEON
        var htmlGeneral = ' <div class="panel-group acordeonVerde " id="' + idAcordeon + '">';
        /************************************************************************************/

        for (var r = 0; r < json.length; r++) {

            htmlGeneral += '<div class="panel panel-default">'
            /*ENCABEZADOO*********************************************************************************************/
            if (json[r].ITEMCARACTERISTICA.length != 0) {
                htmlGeneral += '<div class="panel-heading  glyphicon-triangle-bottom fechaleft" id="cabezote_colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '" >';
            } else {
                htmlGeneral += '<div class="panel-heading  glyphicon-triangle-bottom not-flecha fechaleft" id="cabezote_colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '" >';
            }

            htmlGeneral += '<input type="text" style="display:none" id="txtCaracteristica_' + json[r].ID_CARACTERISTICA + '" value="' + json[r].ID_CARACTERISTICA + '"> ';
            htmlGeneral += '<a  id="xyz" data-toggle="collapse" data-parent="#accordion" href="#colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '" onclick="CambiarEstadoAcordeon(\'#cabezote_colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '\')" class="titleAcordeon">' + json[r].S_DESCRIPCION + '</a>'
            /******boton de agregar o quitar*************/
            if (json[r].S_CARDINALIDAD == 1) {
                if (json[r].ITEMCARACTERISTICA.length == 0) {
                    htmlGeneral += '<button class="agregar" title="Agregar Detalle" onclick="agregarAcordeonitem(' + padre + ',' + grupo + ',this,' + json[r].ID_CARACTERISTICA + ',\'colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '\')"></button> </div>';

                } else {
                    htmlGeneral += '<button class="agregar quitar" title="Quitar Detalle" onclick="quitarAcordeonitem(' + padre + ',' + grupo + ', this, ' + json[r].ID_CARACTERISTICA + ',\'colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + ' \')"></button> </div>'; //&&&&&&&&&&&&&&

                }
            } else {
                htmlGeneral += '<button class="agregar" title="Agregar Detalle" onclick="agregarAcordeonitem(' + padre + ',' + grupo + ',this,' + json[r].ID_CARACTERISTICA + ',\'colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + ' \')"></button> </div>';

            }
            /***********fin condicion botones************/
            htmlGeneral += '</div>'
            /*FIN ENCABEZADO***************************************************************************************/
            /***************CUERPO COLAPSABLE***********************************************************/
            htmlGeneral += '<div id="colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '" class="panel-collapse collapse">';

            /**Cuerpo acordeon**/
            if (json[r].S_CARDINALIDAD == 1) {
                if (json[r].ITEMCARACTERISTICA.length != 0) {
                    htmlGeneral += '<div class="col-md-12 contenidoSub" >'
                    var fila = "EncuestaCol1";
                    for (var v = 0; v < json[r].ITEMCARACTERISTICA[0].VARIABLES.length; v++) {
                        if (fila == "EncuestaCol1") {
                            htmlGeneral += '<div class=" row col-md-12 EncuestaCol1" >'
                            fila = "EncuestaCol2";
                        } else {
                            htmlGeneral += '<div class=" row col-md-12 EncuestaCol2" >'
                            fila = "EncuestaCol1";
                        }
                        htmlGeneral += '<div class="col-md-5 " ><label>' + json[r].ITEMCARACTERISTICA[0].VARIABLES[v].S_NOMBRE + '</label></div>'
                        json[r].ITEMCARACTERISTICA[0].NOMBRE = json.S_DESCRIPCION + '(Nuevo )';
                        htmlGeneral += '<div class="col-md-7 " >' + generarControlDetalle(json[r].ITEMCARACTERISTICA[0].VARIABLES[v], "colapse" + json[r].ID_CARACTERISTICA + "_grupo" + grupo + "_variable" + json[r].ITEMCARACTERISTICA[0].VARIABLES[v].ID_VARIABLE) + '</div>'
                        htmlGeneral += "</div>"
                    }

                    htmlGeneral += consultarDetalle(json[r].CARACTERISTICAS, grupo, json[r].ID_CARACTERISTICA, "CARAC_HIJO");

                    htmlGeneral += "</div>"
                }

            } else {
                /*nivel 2**/
                htmlGeneral += ' <div class="panel-group acordeonVerde acordeonSecundario" id="acordionSecundario">'
                if (json[r].ITEMCARACTERISTICA.length != 0) {
                    for (var f = 0 ; f < json[r].ITEMCARACTERISTICA.length; f++) {
                        htmlGeneral += ' <div class="panel panel-default panel-tercero">'
                        htmlGeneral += '  <div class="panel-heading glyphicon-triangle-bottom" id="subcabezoteAcordeon_' + json[r].ID_CARACTERISTICA + '_' + f + '">'
                        if (json[r].ITEMCARACTERISTICA[f].NOMBRE != "") {
                            htmlGeneral += ' <a data-toggle="collapse" data-parent="#accordion" id="subcollapse_' + json[r].ID_CARACTERISTICA + '_' + f + '_title" href="#subcollapse_' + json[r].ID_CARACTERISTICA + '_' + f + '" onclick="CambiarEstadoAcordeon(\'#subcabezoteAcordeon_' + json[r].ID_CARACTERISTICA + '_' + f + '\')" class="titleAcordeon"><input class="tituloTexto" id="Caracteristicassub_' + json[r].ID_CARACTERISTICA + '_' + f + '" type="text" value="' + json[r].ITEMCARACTERISTICA[f].NOMBRE + '"></a>'

                        } else {
                            htmlGeneral += ' <a data-toggle="collapse" data-parent="#accordion" id="subcollapse_' + json[r].ID_CARACTERISTICA + '_' + f + '_title" href="#subcollapse_' + json[r].ID_CARACTERISTICA + '_' + f + '" onclick="CambiarEstadoAcordeon(\'#subcabezoteAcordeon_' + json[r].ID_CARACTERISTICA + '_' + f + '\')" class="titleAcordeon"><input class="tituloTexto" id="Caracteristicassub_' + f + '" type="text" value="' + json[r].S_DESCRIPCION + '(nuevo)"></a>'

                        }

                        htmlGeneral += ' <button class="agregar quitar" title="Quitar detalle" onclick="eliminarsubacordion(' + padre + ', \'colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '\' ,' + grupo + ',\'#subcabezoteAcordeon_' + json[r].ID_CARACTERISTICA + '_' + f + '\',' + json[r].ID_CARACTERISTICA + ', \'#subcollapse_' + json[r].ID_CARACTERISTICA + '_' + f + '\')"></button>'
                        htmlGeneral += '</div>'
                        htmlGeneral += '</div>'

                        htmlGeneral += '<div id="subcollapse_' + json[r].ID_CARACTERISTICA + '_' + f + '" class="panel-collapse collapse contenidoSub">'
                        /**/
                        var fila = "EncuestaCol1";
                        for (var v = 0; v < json[r].ITEMCARACTERISTICA[f].VARIABLES.length; v++) {
                            if (fila == "EncuestaCol1") {
                                htmlGeneral += '<div class=" row col-md-12 EncuestaCol1" >'
                                fila = "EncuestaCol2";
                            } else {
                                htmlGeneral += '<div class=" row col-md-12 EncuestaCol2" >'
                                fila = "EncuestaCol1";
                            }
                            htmlGeneral += '<div class=" row col-md-12"  >'
                            htmlGeneral += '<div class="col-md-5 " ><label>' + json[r].ITEMCARACTERISTICA[f].VARIABLES[v].S_NOMBRE + '</label></div>'
                            htmlGeneral += '<div class="col-md-7 " >' + generarControlDetalle(json[r].ITEMCARACTERISTICA[f].VARIABLES[v], "colapse" + json[r].ID_CARACTERISTICA + "_grupo" + grupo + "_variable" + json[r].ITEMCARACTERISTICA[0].VARIABLES[v].ID_VARIABLE + "_" + f) + '</div>'
                            htmlGeneral += "</div>"
                        }


                        /***/
                        htmlGeneral += ' </div>'
                    } /**final nivel2*/
                }
                htmlGeneral += '</div>';
            }
            //div que cierra el de arriba

            /**fin cuerpo***/

            htmlGeneral += '</div>'

            /*************FIN CUERPO COLAPSABLE*****************************************************************/
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        htmlGeneral += '</div>';
        //FIN ACORDEON
        return htmlGeneral;
    }
    else {
        var jsonHijo = jsonEntrante;
        var htmlGeneralHijo = "<div class='CARAC_HIJO2'>"
        for (var ri = 0; ri < jsonHijo.length; ri++) {

            htmlGeneralHijo += '<div class="panel panel-default col-md-12">'
            /*ENCABEZADOO*********************************************************************************************/
            if (jsonHijo[ri].ITEMCARACTERISTICA.length != 0) {
                htmlGeneralHijo += '<div class="panel-heading  glyphicon-triangle-bottom fechaleft" id="cabezote_colapse' + jsonHijo[ri].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '" >';
            } else {
                htmlGeneralHijo += '<div class="panel-heading  glyphicon-triangle-bottom not-flecha fechaleft" id="cabezote_colapse' + jsonHijo[ri].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '" >';
            }

            htmlGeneralHijo += '<input type="text" style="display:none" id="txtCaracteristica_' + jsonHijo[ri].ID_CARACTERISTICA + '" value="' + jsonHijo[ri].ID_CARACTERISTICA + '"> ';
            htmlGeneralHijo += '<a  id="xyz" data-toggle="collapse" data-parent="#accordion" href="#colapse' + jsonHijo[ri].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '" onclick="CambiarEstadoAcordeon(\'#cabezote_colapse' + jsonHijo[ri].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '\')" class="titleAcordeon">' + jsonHijo[ri].S_DESCRIPCION + '</a>'
            /******boton de agregar o quitar*************/
            if (jsonHijo[ri].S_CARDINALIDAD == 1) {
                if (jsonHijo[ri].ITEMCARACTERISTICA.length == 0) {
                    htmlGeneralHijo += '<button class="agregar" title="Agregar Detalle" onclick="agregarAcordeonitem(' + padre + ',' + grupo + ',this,' + jsonHijo[ri].ID_CARACTERISTICA + ',\'colapse' + jsonHijo[ri].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '\')"></button> </div>';

                } else {
                    htmlGeneralHijo += '<button class="agregar quitar" title="Quitar Detalle" onclick="quitarAcordeonitem(' + padre + ',' + grupo + ', this, ' + jsonHijo[ri].ID_CARACTERISTICA + ',\'colapse' + jsonHijo[ri].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + ' \')"></button> </div>'; //&&&&&&&&&&&&&&

                }
            } else {
                htmlGeneralHijo += '<button class="agregar" title="Agregar Detalle" onclick="agregarAcordeonitem(' + padre + ',' + grupo + ',this,' + jsonHijo[ri].ID_CARACTERISTICA + ',\'colapse' + jsonHijo[ri].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + ' \')"></button> </div>';

            }
            /***********fin condicion botones************/
            htmlGeneralHijo += '</div>'
            /*FIN ENCABEZADO***************************************************************************************/
            /***************CUERPO COLAPSABLE***********************************************************/
            htmlGeneralHijo += '<div id="colapse' + jsonHijo[ri].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '" class="panel-collapse collapse">';

            /**Cuerpo acordeon**/
            if (jsonHijo[ri].S_CARDINALIDAD == 1) {
                if (jsonHijo[ri].ITEMCARACTERISTICA.length != 0) {
                    htmlGeneralHijo += '<div class="col-md-12 contenidoSub" >'
                    var fila = "EncuestaCol1";
                    for (var vi = 0; vi < jsonHijo[ri].ITEMCARACTERISTICA[0].VARIABLES.length; vi++) {
                        if (fila == "EncuestaCol1") {
                            htmlGeneralHijo += '<div class=" row col-md-12 EncuestaCol1" >'
                            fila = "EncuestaCol2";
                        } else {
                            htmlGeneralHijo += '<div class=" row col-md-12 EncuestaCol2" >'
                            fila = "EncuestaCol1";
                        }
                        htmlGeneralHijo += '<div class="col-md-5 " ><label>' + jsonHijo[ri].ITEMCARACTERISTICA[0].VARIABLES[vi].S_NOMBRE + '</label></div>'
                        jsonHijo[ri].ITEMCARACTERISTICA[0].NOMBRE = jsonHijo[ri].S_DESCRIPCION + '(Nuevo )';
                        htmlGeneralHijo += '<div class="col-md-7 " >' + generarControlDetalle(jsonHijo[ri].ITEMCARACTERISTICA[0].VARIABLES[vi], "colapse" + jsonHijo[ri].ID_CARACTERISTICA + "_grupo" + grupo + "_variable" + jsonHijo[ri].ITEMCARACTERISTICA[0].VARIABLES[vi].ID_VARIABLE) + '</div>'
                        htmlGeneralHijo += "</div>"
                    }

                    //    htmlGeneralHijo += consultarDetalle(jsonHijo[ri].CARACTERISTICAS, grupo, jsonHijo[ri].ID_CARACTERISTICA, "CARAC_HIJO");

                    htmlGeneralHijo += "</div>"
                }

            } else {
                /*nivel 2**/
                htmlGeneralHijo += ' <div class="panel-group acordeonVerde acordeonSecundario" id="acordionSecundario">'
                if (jsonHijo[ri].ITEMCARACTERISTICA.length != 0) {
                    for (var fi = 0 ; f < jsonHijo[ri].ITEMCARACTERISTICA.length; fi++) {
                        htmlGeneralHijo += ' <div class="panel panel-default panel-tercero">'
                        htmlGeneralHijo += '  <div class="panel-heading glyphicon-triangle-bottom" id="subcabezoteAcordeon_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '">'
                        if (jsonHijo[ri].ITEMCARACTERISTICA[fi].NOMBRE != "") {
                            htmlGeneralHijo += ' <a data-toggle="collapse" data-parent="#accordion" id="subcollapse_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '_title" href="#subcollapse_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '" onclick="CambiarEstadoAcordeon(\'#subcabezoteAcordeon_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '\')" class="titleAcordeon"><input class="tituloTexto" id="Caracteristicassub_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '" type="text" value="' + jsonHijo[ri].ITEMCARACTERISTICA[fi].NOMBRE + '"></a>'

                        } else {
                            htmlGeneralHijo += ' <a data-toggle="collapse" data-parent="#accordion" id="subcollapse_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '_title" href="#subcollapse_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '" onclick="CambiarEstadoAcordeon(\'#subcabezoteAcordeon_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '\')" class="titleAcordeon"><input class="tituloTexto" id="Caracteristicassub_' + f + '" type="text" value="' + jsonHijo[ri].S_DESCRIPCION + '(nuevo)"></a>'

                        }

                        htmlGeneralHijo += ' <button class="agregar quitar" title="Quitar detalle" onclick="eliminarsubacordion(' + padre + ', \'colapse' + jsonHijo[ri].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre + '\' ,' + grupo + ',\'#subcabezoteAcordeon_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '\',' + jsonHijo[ri].ID_CARACTERISTICA + ', \'#subcollapse_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '\')"></button>'
                        htmlGeneralHijo += '</div>'
                        htmlGeneralHijo += '</div>'

                        htmlGeneralHijo += '<div id="subcollapse_' + jsonHijo[ri].ID_CARACTERISTICA + '_' + f + '" class="panel-collapse collapse contenidoSub">'
                        /**/
                        var fila = "EncuestaCol1";
                        for (var v = 0; v < jsonHijo[ri].ITEMCARACTERISTICA[fi].VARIABLES.length; v++) {
                            if (fila == "EncuestaCol1") {
                                htmlGeneralHijo += '<div class=" row col-md-12 EncuestaCol1" >'
                                fila = "EncuestaCol2";
                            } else {
                                htmlGeneralHijo += '<div class=" row col-md-12 EncuestaCol2" >'
                                fila = "EncuestaCol1";
                            }
                            htmlGeneralHijo += '<div class="col-md-5 " ><label>' + jsonHijo[ri].ITEMCARACTERISTICA[fi].VARIABLES[vi].S_NOMBRE + '</label></div>'
                            htmlGeneralHijo += '<div class="col-md-7 " >' + generarControlDetalle(jsonHijo[ri].ITEMCARACTERISTICA[fi].VARIABLES[vi], "colapse" + jsonHijo[ri].ID_CARACTERISTICA + "_grupo" + grupo + "_variable" + jsonHijo[ri].ITEMCARACTERISTICA[0].VARIABLES[vi].ID_VARIABLE + "_" + f) + '</div>'
                            htmlGeneralHijo += "</div>"
                        }


                        /***/
                        htmlGeneralHijo += ' </div>'
                    } /**final nivel2*/
                }
                htmlGeneralHijo += '</div>';
            }
            //div que cierra el de arriba

            /**fin cuerpo***/

            htmlGeneralHijo += '</div>'

            /*************FIN CUERPO COLAPSABLE*****************************************************************/
        }
        htmlGeneralHijo += '</div>'
        return htmlGeneralHijo;
    }
    /**RETORNO HTML**/


}

function agregarAcordeonitem(idpadre, id_grupo, boton, uso, id) {
    var jsonDetalle = jsonDetallegenerales[id_grupo];
    var posicionSub = 0;
    var nombre = $("#" + boton.parentElement.id)[0].className;
    var n = nombre.search("glyphicon-triangle-top");
    $("#" + boton.parentElement.id).removeClass("not-flecha");
    $("#" + boton.parentElement.id).removeClass("glyphicon-triangle-top");



    var json = null;


    var nivelHijo = true;
    if (idpadre != 0) {
        for (var z = 0; z < jsonDetalle.length; z++) {
            if (jsonDetalle[z].ID_CARACTERISTICA == idpadre) {
                jsonDetalle = jsonDetalle[z].CARACTERISTICAS;
                nivelHijo = false;
                break; //&&&&&&&&&&&&&&
            }
        }

        if (nivelHijo == true) {
            for (var z = 0; z < jsonDetalle.length; z++) {
                for (var c = 0; c < jsonDetalle[z].CARACTERISTICAS.length; c++) {
                    if (jsonDetalle[z].CARACTERISTICAS[c].ID_CARACTERISTICA == idpadre) {
                        jsonDetalle = jsonDetalle[z].CARACTERISTICAS[c].CARACTERISTICAS;
                        nivelHijo = false;
                        break; //&&&&&&&&&&&&&&
                    }
                }
            }

        }
        if (nivelHijo == true) {
            for (var z = 0; z < jsonDetalle.length; z++) {
                for (var c = 0; c < jsonDetalle[z].CARACTERISTICAS.length; c++) {
                    for (var cc = 0; cc < jsonDetalle[z].CARACTERISTICAS[c].CARACTERISTICAS.length; cc++) {
                        if (jsonDetalle[z].CARACTERISTICAS[c].CARACTERISTICAS[cc].ID_CARACTERISTICA == idpadre) {
                            jsonDetalle = jsonDetalle[z].CARACTERISTICAS[c].CARACTERISTICAS[cc].CARACTERISTICAS;
                            nivelHijo = false;
                            break; //&&&&&&&&&&&&&&
                        }
                    }
                }
            }
        }
    }


    for (var z = 0; z < jsonDetalle.length; z++) {
        if (jsonDetalle[z].ID_CARACTERISTICA == uso) {
            var njson = JSON.parse(JSON.stringify(jsonDetalle[z].PLANTILLA));
            jsonDetalle[z].ITEMCARACTERISTICA[jsonDetalle[z].ITEMCARACTERISTICA.length] = njson
            posicionSub = z;
            json = jsonDetalle[z];
        }
    }

    if (json == null) {
        return;
    }
    var htmlGeneral = "";
    if (json.S_CARDINALIDAD == 1) {
        $("#" + boton.parentElement.id + " a").click();
        htmlGeneral += '<div class="  col-md-12 contenidoSub" >'
        var fila = "EncuestaCol1";
        for (var v = 0; v < json.ITEMCARACTERISTICA[0].VARIABLES.length; v++) {
            if (fila == "EncuestaCol1") {
                htmlGeneral += '<div class=" row col-md-12 EncuestaCol1" >'
                fila = "EncuestaCol2";
            } else {
                htmlGeneral += '<div class=" row col-md-12 EncuestaCol2" >'
                fila = "EncuestaCol1";
            }
            htmlGeneral += '<div class="col-md-5 " ><label>' + json.ITEMCARACTERISTICA[0].VARIABLES[v].S_NOMBRE + '</label></div>'
            jsonDetalle[posicionSub].ITEMCARACTERISTICA[0].NOMBRE = json.S_DESCRIPCION + '(Nuevo )';
            htmlGeneral += '<div class="col-md-7 " >' + generarControlDetalle(json.ITEMCARACTERISTICA[0].VARIABLES[v], "colapse" + json.ID_CARACTERISTICA + "_grupo" + id_grupo + "_variable" + json.ITEMCARACTERISTICA[0].VARIABLES[v].ID_VARIABLE) + '</div>'
            htmlGeneral += "</div>"
        }
        htmlGeneral += consultarDetalle(json.CARACTERISTICAS, id_grupo, json.ID_CARACTERISTICA, "CARAC_HIJO");
        boton.outerHTML = '<button class="agregar quitar" title="Quitar Detalle" onclick="quitarAcordeonitem(' + idpadre + ',' + id_grupo + ',this,\'' + id + '\',' + uso + ')"></button> ';
        htmlGeneral += "</div>"
        $("#" + id + " ").append(htmlGeneral)
    } else {
        /*nivel 2**/
        htmlGeneral += ' <div class="panel-group acordeonVerde acordeonSecundario" id="acordionSecundario">'
        for (var f = json.ITEMCARACTERISTICA.length - 1; f < json.ITEMCARACTERISTICA.length; f++) {
            htmlGeneral += ' <div class="panel panel-default panel-tercero">'
            htmlGeneral += '  <div class="panel-heading glyphicon-triangle-bottom" id="subcabezoteAcordeon_' + uso + '_' + f + '">'
            htmlGeneral += ' <a data-toggle="collapse" data-parent="#accordion" id="subcollapse_' + uso + '_' + f + '_title" href="#subcollapse_' + uso + '_' + f + '" onclick="CambiarEstadoAcordeon(\'#subcabezoteAcordeon_' + uso + '_' + f + '\')" class="titleAcordeon"><input class="tituloTexto" id="Caracteristicassub_' + uso + '_' + f + '" type="text" value="' + json.S_DESCRIPCION + '(nuevo)"></a>'

            htmlGeneral += ' <button class="agregar quitar" title="Quitar detalle" onclick="eliminarsubacordion(' + idpadre + ', ' + id + ',' + id_grupo + ',\'#subcabezoteAcordeon_' + uso + '_' + f + '\',' + uso + ', \'#subcollapse_' + uso + '_' + f + '\')"></button>'
            htmlGeneral += '</div>'
            htmlGeneral += '</div>'

            htmlGeneral += '<div id="subcollapse_' + uso + '_' + f + '" class="panel-collapse collapse contenidoSub">'
            /**/
            var fila = "EncuestaCol1"
            for (var v = 0; v < json.ITEMCARACTERISTICA[f].VARIABLES.length; v++) {
                if (fila == "EncuestaCol1") {
                    htmlGeneral += '<div class=" row col-md-12 EncuestaCol1" >'
                    fila = "EncuestaCol2";
                } else {
                    htmlGeneral += '<div class=" row col-md-12 EncuestaCol2" >'
                    fila = "EncuestaCol1";
                }
                htmlGeneral += '<div class="col-md-5 " ><label>' + json.ITEMCARACTERISTICA[f].VARIABLES[v].S_NOMBRE + '</label></div>'
                htmlGeneral += '<div class="col-md-7 " >' + generarControlDetalle(json.ITEMCARACTERISTICA[f].VARIABLES[v], "colapse" + json.ID_CARACTERISTICA + "_grupo" + id_grupo + "_variable" + json.ITEMCARACTERISTICA[f].VARIABLES[v].ID_VARIABLE + "_" + f) + '</div>'
                htmlGeneral += "</div>"
            }
            htmlGeneral += consultarDetalleHijos(json.CARACTERISTICAS, id_grupo, json.ID_CARACTERISTICA, "CARAC_HIJO");
            /***/
            htmlGeneral += ' </div>'
        } /**final nivel2*/
        htmlGeneral += '</div>';
        //div que cierra el de arriba
        htmlGeneral += "</div>"
        $("#" + id + "").append(htmlGeneral)
    }





}

function quitarAcordeonitem(id_padre, id_grupo, boton, id, cara) {
    var jsonDetalle = jsonDetallegenerales[id_grupo];

    if (confirm("Esta seguro en desagregar la informacion del detalle")) {
        $("#" + boton.parentElement.id).addClass("not-flecha");
        $("#" + id + " div").remove();
        $("#" + id + "").append('<div style:"padding: 1px;"></div>');
        $("#" + boton.parentElement.id + " a").click();
        if (id_padre == 0) {
            for (var z = 0; z < jsonDetallegenerales[id_grupo].length; z++) {
                //if (jsonDetallegenerales[id_grupo][z].ID_CARACTERISTICA == cara) { //**** 20181121 Cambiado por el registro de abajo
                if (jsonDetallegenerales[id_grupo][z].ID_CARACTERISTICA == id) {
                    for (var i = 0; i < jsonDetallegenerales[id_grupo][z].ITEMCARACTERISTICA.length; i++) {

                        if (i == 0) {
                            jsonDetallegenerales[id_grupo][z].ITEMCARACTERISTICA[i].ID_CARACTERISTICA_ESTADO = jsonDetallegenerales[id_grupo][z].ITEMCARACTERISTICA[i].ID_CARACTERISTICA_ESTADO * -1;
                            break;
                        }
                    }
                }
                /******/
            }
        } else {
            for (var z = 0; z < jsonDetallegenerales[id_grupo].length; z++) {
                if (jsonDetallegenerales[id_grupo][z].ID_CARACTERISTICA == id_padre) {

                    for (var zi = 0; zi < jsonDetallegenerales[id_grupo][z].CARACTERISTICAS.length; zi++) {
                        //if (jsonDetallegenerales[id_grupo][z].CARACTERISTICAS[zi].ID_CARACTERISTICA == cara) { //**** 20181121 Cambiado por el registro de abajo
                        if (jsonDetallegenerales[id_grupo][z].CARACTERISTICAS[zi].ID_CARACTERISTICA == id) {
                            for (var i = 0; i < jsonDetallegenerales[id_grupo][z].CARACTERISTICAS[zi].ITEMCARACTERISTICA.length; i++) {

                                if (i == 0) {
                                    jsonDetallegenerales[id_grupo][z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[i].ID_CARACTERISTICA_ESTADO = jsonDetallegenerales[id_grupo][z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[i].ID_CARACTERISTICA_ESTADO * -1;
                                    break;
                                }
                            }
                        }
                        /******/
                    }
                }
                /******/
            }
        }



        $("#" + cara + " .contenidoSub").remove();
        //boton.outerHTML = '<button class="agregar" title="Agregar detalle" onclick="agregarAcordeonitem(' + id_padre + ',' + id_grupo + ',this,' + cara + ',\'' + id + '\')"></button> ';
        boton.outerHTML = '<button class="agregar" title="Agregar detalle" onclick="agregarAcordeonitem(' + id_padre + ',' + id_grupo + ',this, ' + id + ', \'' + cara + '\')"></button> ';

    } else {
        return;
    }
}

function eliminarsubacordion(id_padre, idpadre, id_grupo, id, subid, idconr) {
    var jsonDetalle = jsonDetallegenerales[id_grupo];
    if (confirm("Esta seguro en desagregar la informacion del detalle")) {
        var posicion = id.replace("#subcabezoteAcordeon_" + subid + "_", "");
        posicion = posicion * 1;
        if (id_padre != 0) {
            for (var t = 0; t < jsonDetalle.length; t++) {
                if (jsonDetalle[t].ID_CARACTERISTICA == id_padre) {
                    jsonDetalle = jsonDetalle[t].CARACTERISTICAS

                }
            }
        }
        for (var z = 0; z < jsonDetalle.length; z++) {
            if (jsonDetalle[z].ID_CARACTERISTICA == subid) {
                for (var i = 0; i < jsonDetalle[z].ITEMCARACTERISTICA.length; i++) {
                    if (jsonDetalle[z].ITEMCARACTERISTICA.length == 1) {
                        posicion = 0;
                    }
                    if (i == posicion) {
                        $(id).remove();
                        $(idconr).remove();
                        jsonDetalle[z].ITEMCARACTERISTICA[i].ID_CARACTERISTICA_ESTADO = jsonDetalle[z].ITEMCARACTERISTICA[i].ID_CARACTERISTICA_ESTADO * -1
                        if (jsonDetalle[posicion].ITEMCARACTERISTICA.length == 0) {
                            var texr = idpadre.id.replace("#", "");
                            $("#cabezote_" + texr).addClass("not-flecha");
                        }
                        break;
                    }
                }
            }
        }
        var texr = id.replace("#", "");
        $("#" + texr + " ").remove();

    } else {
        return;
    }


}

var arrayFormulas = new Array();
function generarControlDetalle(objeto, id) {
    var control = ""
    if (objeto.ID_TIPO_DATO == 1) {
        control = '<select id="' + id + '"  name="ID_VALOR" class="form-control"><option value="-1">Seleccione</option>'
        for (var i = 0; i < objeto.OPCIONES.length; i++) {
            control += '<option value="' + objeto.OPCIONES[i].ID_OPCION + '">' + objeto.OPCIONES[i].DESCRIPCION + '</option>'
        }
        control += '</select>'
        if (objeto.ID_VALOR != "") {
            setTimeout(function () { try { $("#" + id).val(objeto.ID_VALOR) } catch (e) { } }, 6000);
        }

    } else if (objeto.ID_TIPO_DATO == 2) {
        control = ' <input type="number"   class="form-control" onkeydown="return validarNumeros(event)" id="' + id + '" name="N_VALOR" value="' + objeto.N_VALOR + '">'
    } else if (objeto.ID_TIPO_DATO == 3) {
        control = ' <input type="text" class="form-control" id="' + id + '" name="S_VALOR" value="' + objeto.S_VALOR + '" >'
    } else if (objeto.ID_TIPO_DATO == 4) {
        control = ' <input type="date" class="form-control" id="' + id + '" name="D_VALOR" value="' + objeto.D_VALOR + '">'
    } else if (objeto.ID_TIPO_DATO == 5) {
        control = '<textarea class="form-control" rows="3" id="' + id + '" name="S_VALOR"  value="' + objeto.S_VALOR + '">' + objeto.S_VALOR + '</textarea>';
    } else if (objeto.ID_TIPO_DATO == 7) {
        control = '<table id="' + id + '"  name="ID_VALOR" class="form-control" style="border: none; box-shadow: none;">'
        for (var i = 0; i < objeto.OPCIONES.length; i++) {
            control += '<tr><td><input type="checkbox" name="ID_VALOR" id="ck_' + objeto.OPCIONES[i].ID_OPCION + '" value="' + objeto.OPCIONES[i].DESCRIPCION + '" ' + ((',' + objeto.ID_VALOR.toString() + ',').indexOf(',' + objeto.OPCIONES[i].ID_OPCION.toString() + ',') >= 0 ? ' checked ' : '') + '>' + objeto.OPCIONES[i].DESCRIPCION + '</tr></td>'
        }
        control += '</table>'


    } else if (objeto.ID_TIPO_DATO == 6) {

        control = '<label id="' + id + '" name="N_VALOR"  value=""></label> <button type="button" id="btnCerra" class="btn btn-default btn-sm" onclick="calcularFormula(\'' + id + '\')">Calcular</button>';
        arrayFormulas[id] = objeto.S_FORMULA;
        if (objeto.S_FORMULA != "") {
            setTimeout(function () {

                var valor = eval(objeto.S_FORMULA)
                $("#" + id).text(valor)

            }, 8000);
        }
    } else if (objeto.ID_TIPO_DATO == 11) {
        control = ' <input type="number"  step="0.0001" class="form-control" onkeydown="return validarNumerosD(event)" id="' + id + '" name="N_VALOR" value="' + objeto.N_VALOR + '">'
    }

    return control;
}

function calcularFormula(id) {
    var valor = eval(arrayFormulas[id]);
    $("#" + id).text(valor)
}

/*************guardar json detalle********************/
function guardarDetalle(idAcordeon, idgrupo) {
    var jsonDetalle = jsonDetallegenerales[idgrupo];
    for (var z = 0; z < jsonDetalle.length; z++) {

        for (var i = 0; i < jsonDetalle[z].ITEMCARACTERISTICA.length; i++) {
            if (jsonDetalle[z].ITEMCARACTERISTICA[i].ID_CARACTERISTICA_ESTADO >= 0) { //***** Condición para no obtener datos de las características eliminadas 20181121
                for (var X = 0; X < jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES.length; X++) {
                    var campo = "";
                    if (jsonDetalle[z].S_CARDINALIDAD == "1") {
                        campo = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE)[0].name;
                        /********/
                        if (campo == "ID_VALOR") {
                            jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VALOR = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE).val() * 1;

                        } else if (campo == "N_VALOR") {
                            jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].N_VALOR = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE).val() * 1;
                        } else if (campo == "S_VALOR") {
                            jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].S_VALOR = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE).val();
                        } else if (campo == "D_VALOR") {
                            jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].D_VALOR = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE).val();
                        } else if ($("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE)[0].tagName == "TABLE") {
                            var textoin = "";
                            for (var o = 0; o < jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].OPCIONES.length; o++) {
                                var valor = "#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE + " #ck_" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].OPCIONES[o].ID_OPCION;
                                if ($(valor).is(':checked')) {
                                    if (textoin == "") {
                                        textoin += "" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].OPCIONES[o].ID_OPCION;
                                    } else {
                                        textoin += "," + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].OPCIONES[o].ID_OPCION;
                                    }
                                }
                            }
                            jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VALOR = textoin;

                        }
                    } else {
                        if (jsonDetalle[z].ITEMCARACTERISTICA[i].ID_CARACTERISTICA_ESTADO + "" != "-0") {
                            if (jsonDetalle[z].ITEMCARACTERISTICA[i].ID_CARACTERISTICA_ESTADO >= 0) {
                                campo = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE + "_" + i)[0].name;
                                /***********/
                                if (campo == "ID_VALOR") {
                                    jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VALOR = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE + "_" + i).val() * 1;

                                } else if (campo == "N_VALOR") {
                                    jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].N_VALOR = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE + "_" + i).val() * 1;
                                } else if (campo == "S_VALOR") {
                                    jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].S_VALOR = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE + "_" + i).val();
                                } else if (campo == "D_VALOR") {
                                    jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].D_VALOR = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE + "_" + i).val();
                                } else if ($("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE)[0].tagName == "TABLE") {
                                    var textoin = "";
                                    for (var o = 0; o < jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].OPCIONES.length; o++) {
                                        var valor = "#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE + " #ck_" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].OPCIONES[o].ID_OPCION;
                                        if ($(valor).is(':checked')) {
                                            if (textoin == "") {
                                                textoin += "" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].OPCIONES[o].ID_OPCION;
                                            } else {
                                                textoin += "," + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].OPCIONES[o].ID_OPCION;
                                            }
                                        }
                                    }
                                    jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VALOR = textoin;
                                }
                            }
                        } else {
                            jsonDetalle[z].ITEMCARACTERISTICA[i].ID_CARACTERISTICA_ESTADO = -1;
                        }
                    }
                    var valorjson = jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X];
                    delete jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X];
                    jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X] = valorjson;
                    if (jsonDetalle[z].S_CARDINALIDAD != "1") {
                        jsonDetalle[z].ITEMCARACTERISTICA[i].NOMBRE = $("#Caracteristicassub_" + jsonDetalle[z].ID_CARACTERISTICA + "_" + i).val();
                    }
                }
            } //***** Cierra condición 20181121
        }
        /************subcaracteristicas*****/
        if (jsonDetalle[z].ITEMCARACTERISTICA.length != 0) {
            if (true) {
                guardarDetalleHijos(idAcordeon, idgrupo, jsonDetalle[z].CARACTERISTICAS);
            } else {
                for (var zi = 0; zi < jsonDetalle[z].CARACTERISTICAS.length; zi++) {
                    for (var ii = 0; ii < jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA.length; ii++) {
                        for (var Xx = 0; Xx < jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES.length; Xx++) {

                            var campo = "";
                            if (jsonDetalle[z].CARACTERISTICAS[zi].S_CARDINALIDAD == "1") {
                                campo = $("#colapse" + jsonDetalle[z].CARACTERISTICAS[zi].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].ID_VARIABLE)[0].name;
                                if (campo == "ID_VALOR") {
                                    jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].ID_VALOR = $("#colapse" + jsonDetalle[z].CARACTERISTICAS[zi].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].ID_VARIABLE).val() * 1;

                                } else if (campo == "N_VALOR") {
                                    jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].N_VALOR = $("#colapse" + jsonDetalle[z].CARACTERISTICAS[zi].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].ID_VARIABLE).val() * 1;
                                } else if (campo == "S_VALOR") {
                                    jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].S_VALOR = $("#colapse" + jsonDetalle[z].CARACTERISTICAS[zi].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].ID_VARIABLE).val();
                                } else if (campo == "D_VALOR") {
                                    jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].D_VALOR = $("#colapse" + jsonDetalle[z].CARACTERISTICAS[zi].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].ID_VARIABLE).val();
                                } else if ($("#colapse" + jsonDetalle[z].CARACTERISTICAS[zi].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].ID_VARIABLE)[0].tagName == "TABLE") {
                                    var textovalor = "";
                                    for (var o = 0; o < jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].OPCIONES.length; o++) {
                                        var valor = "#colapse" + jsonDetalle[z].CARACTERISTICAS[zi].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].ID_VARIABLE + " #ck_" + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].OPCIONES[o].ID_OPCION;
                                        if ($(valor).is(':checked')) {
                                            if (textovalor == "") {
                                                textovalor += "" + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].OPCIONES[o].ID_OPCION;
                                            } else {
                                                textovalor += "," + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].OPCIONES[o].ID_OPCION;
                                            }
                                        }
                                    }
                                    //&&&&&&&
                                    jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].ID_VALOR = textovalor;
                                }
                            } else {
                                if (jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].ID_CARACTERISTICA_ESTADO + "" != "-0") {
                                    if (jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].ID_CARACTERISTICA_ESTADO >= 0) {

                                        campo = $("#colapse" + jsonDetalle[z].CARACTERISTICAS[zi].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].ID_VARIABLE + "_" + ii)[0].name;
                                        if (campo == "ID_VALOR") {
                                            jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].ID_VALOR = $("#colapse" + jsonDetalle[z].CARACTERISTICAS[zi].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].ID_VARIABLE + "_" + ii).val() * 1;

                                        } else if (campo == "N_VALOR") {
                                            jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].N_VALOR = $("#colapse" + jsonDetalle[z].CARACTERISTICAS[zi].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].ID_VARIABLE + "_" + ii).val() * 1;
                                        } else if (campo == "S_VALOR") {
                                            jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].S_VALOR = $("#colapse" + jsonDetalle[z].CARACTERISTICAS[zi].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].ID_VARIABLE + "_" + ii).val();
                                        } else if (campo == "D_VALOR") {
                                            jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].D_VALOR = $("#colapse" + jsonDetalle[z].CARACTERISTICAS[zi].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].ID_VARIABLE + "_" + ii).val();
                                        } else if ($("#colapse" + jsonDetalle[z].CARACTERISTICAS[zi].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].ID_VARIABLE)[0].tagName == "TABLE") {
                                            var textovalor = "";
                                            for (var o = 0; o < jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].OPCIONES.length; o++) {
                                                var valor = "#colapse" + jsonDetalle[z].CARACTERISTICAS[zi].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].ID_VARIABLE + " #ck_" + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].OPCIONES[o].ID_OPCION;
                                                if ($(valor).is(':checked')) {
                                                    if (textovalor == "") {
                                                        textovalor += "" + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].OPCIONES[o].ID_OPCION;
                                                    } else {
                                                        textovalor += "," + jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].OPCIONES[o].ID_OPCION;
                                                    }
                                                }
                                            }
                                            //&&&&&&&
                                            jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].VARIABLES[Xx].ID_VALOR = textovalor;
                                        }

                                    }
                                } else {
                                    jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].ID_CARACTERISTICA_ESTADO = "-9999999"
                                }
                            }
                            if (jsonDetalle[z].CARACTERISTICAS[zi].S_CARDINALIDAD != "1") {
                                jsonDetalle[z].CARACTERISTICAS[zi].ITEMCARACTERISTICA[ii].NOMBRE = $("#Caracteristicassub_" + jsonDetalle[z].CARACTERISTICAS[zi].ID_CARACTERISTICA + "_" + ii).val();
                            }

                        }
                    }


                }
            }
        }

        /***************************************************/


    }

    return jsonDetalle;
}


/*************guardar json detalle********************/
function guardarDetalleHijos(idAcordeon, idgrupo, caracteristicas) {
    var jsonDetalle = caracteristicas;
    for (var z = 0; z < jsonDetalle.length; z++) {

        for (var i = 0; i < jsonDetalle[z].ITEMCARACTERISTICA.length; i++) {

            for (var X = 0; X < jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES.length; X++) {
                var campo = "";
                if (jsonDetalle[z].S_CARDINALIDAD == "1") {
                    campo = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE)[0].name;
                    /********/
                    if (campo == "ID_VALOR") {
                        jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VALOR = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE).val() * 1;

                    } else if (campo == "N_VALOR") {
                        jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].N_VALOR = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE).val() * 1;
                    } else if (campo == "S_VALOR") {
                        jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].S_VALOR = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE).val();
                    } else if (campo == "D_VALOR") {
                        jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].D_VALOR = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE).val();
                    } else if ($("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE)[0].tagName == "TABLE") {
                        var textoin = "";
                        for (var o = 0; o < jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].OPCIONES.length; o++) {
                            var valor = "#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE + " #ck_" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].OPCIONES[o].ID_OPCION;
                            if ($(valor).is(':checked')) {
                                if (textoin == "") {
                                    textoin += "" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].OPCIONES[o].ID_OPCION;
                                } else {
                                    textoin += "," + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].OPCIONES[o].ID_OPCION;
                                }
                            }
                        }
                        jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VALOR = textoin;

                    }
                } else {
                    if (jsonDetalle[z].ITEMCARACTERISTICA[i].ID_CARACTERISTICA_ESTADO + "" != "-0") {
                        if (jsonDetalle[z].ITEMCARACTERISTICA[i].ID_CARACTERISTICA_ESTADO >= 0) {
                            campo = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE + "_" + i)[0].name;
                            /***********/
                            if (campo == "ID_VALOR") {
                                jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VALOR = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE + "_" + i).val() * 1;

                            } else if (campo == "N_VALOR") {
                                jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].N_VALOR = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE + "_" + i).val() * 1;
                            } else if (campo == "S_VALOR") {
                                jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].S_VALOR = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE + "_" + i).val();
                            } else if (campo == "D_VALOR") {
                                jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].D_VALOR = $("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE + "_" + i).val();
                            } else if ($("#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE)[0].tagName == "TABLE") {
                                var textoin = "";
                                for (var o = 0; o < jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].OPCIONES.length; o++) {
                                    var valor = "#colapse" + jsonDetalle[z].ID_CARACTERISTICA + "_grupo" + idgrupo + "_variable" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE + " #ck_" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].OPCIONES[o].ID_OPCION;
                                    if ($(valor).is(':checked')) {
                                        if (textoin == "") {
                                            textoin += "" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].OPCIONES[o].ID_OPCION;
                                        } else {
                                            textoin += "," + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].OPCIONES[o].ID_OPCION;
                                        }
                                    }
                                }
                                jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VALOR = textoin;
                            }
                        }
                    } else {
                        jsonDetalle[z].ITEMCARACTERISTICA[i].ID_CARACTERISTICA_ESTADO = -1;
                    }
                }
                var valorjson = jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X];
                delete jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X];
                jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X] = valorjson;
                if (jsonDetalle[z].S_CARDINALIDAD != "1") {
                    jsonDetalle[z].ITEMCARACTERISTICA[i].NOMBRE = $("#Caracteristicassub_" + jsonDetalle[z].ID_CARACTERISTICA + "_" + i).val();
                }

            }
        }

        /************subcaracteristicas*****/
        if (jsonDetalle[z].ITEMCARACTERISTICA.length != 0) {
            guardarDetalleHijos(idAcordeon, idgrupo, jsonDetalle[z].CARACTERISTICAS);
        }
    }

    return jsonDetalle;
}


/*********************************************************/
function generarControlEncuesta(objeto, id) {
    var control = ""
    if (objeto.ID_TIPOPREGUNTA == 1) {
        control = ' <input type="checkbox" class="form-control" id="' + id + '" name="ID_VALOR" value="' + objeto.S_VALOR + '"'
        if (objeto.ID_VALOR == 1) {
            control += 'checked >'
        } else {
            control += '>'
        }

    } else if (objeto.ID_TIPOPREGUNTA == 2) {
        control = '<select id="' + id + '"  name="ID_VALOR" class="form-control"><option value="-1">Seleccione</option>'
        for (var i = 0; i < objeto.OPCIONES.length; i++) {
            control += '<option value="' + objeto.OPCIONES[i].ID_RESPUESTA + '">' + objeto.OPCIONES[i].S_VALOR + '</option>'
        }
        control += '</select>'
        if (objeto.ID_VALOR != null) {
            setTimeout(function () { try { $("#" + id).val(objeto.ID_VALOR) } catch (e) { } }, 7000);
        }

    } else if (objeto.ID_TIPOPREGUNTA == 3) {

        control = '<div ">'
        for (var i = 0; i < objeto.OPCIONES.length; i++) {
            control += '<div class="checkbox">  <label>   <input id="' + id + i + '" name="S_VALOR" value="' + objeto.OPCIONES[i].S_VALOR + '"'
            if (objeto.OPCIONES[i].SELECTED == 1) {
                control += 'checked  type="checkbox">' + objeto.OPCIONES[i].S_VALOR + '</label> </div>'
            } else {
                control += ' type="checkbox">' + objeto.OPCIONES[i].S_VALOR + '</label> </div>'
            }





        }
        control += '</div>'
    } else if (objeto.ID_TIPOPREGUNTA == 4) {
        control = ' <input type="number"   class="form-control" onkeydown="return validarNumeros(event)" id="' + id + '" name="N_VALOR" value="' + objeto.N_VALOR + '">'
    } else if (objeto.ID_TIPOPREGUNTA == 5) {
        control = ' <input type="text" class="form-control" id="' + id + '" name="S_VALOR" value="' + objeto.S_VALOR + '" >'
    } else if (objeto.ID_TIPOPREGUNTA == 6) {
        control = ' <input type="date" class="form-control" id="' + id + '" name="D_VALOR" value="' + objeto.D_VALOR + '">'
    } else if (objeto.ID_TIPOPREGUNTA == 7) {
        control = '<textarea class="form-control" rows="3" id="' + id + '" name="S_VALOR"  >' + objeto.S_VALOR + '</textarea>';
    }
    return control;
}

function agregarAcordeonEncuestaitem(boton, uso, id) {
    var nombre = $("#" + boton.parentElement.id)[0].className;
    var n = nombre.search("glyphicon-triangle-top");
    if (n < 0) {
        $("#" + boton.parentElement.id + " a").click();
    }
    var json = null;
    for (var z = 0; z < jsonEncuestas.length; z++) {
        if (jsonEncuestas[z].ID_ENCUESTA == uso) {
            json = jsonEncuestas[z];
            jsonEncuestas[z].ESTADO = 1;

        }
    }
    if (json == null) {
        return;
    }
    var htmlGeneral = "";
    htmlGeneral += '<div class="  col-md-12 contenidoSub" >'
    var fila = "EncuestaCol1";
    for (var v = 0; v < json.PREGUNTAS.length; v++) {
        if (fila == "EncuestaCol1") {
            htmlGeneral += '<div class=" row col-md-12 EncuestaCol1" >'
            fila = "EncuestaCol2";
        } else {
            htmlGeneral += '<div class=" row col-md-12 EncuestaCol2" >'
            fila = "EncuestaCol1";
        }
        htmlGeneral += '<div class="col-md-5 " ><label>' + json.PREGUNTAS[v].S_NOMBRE
        if (json.PREGUNTAS[v].S_REQUERIDA == "1") {
            htmlGeneral += " *"
        }
        htmlGeneral += '</label></div>'
        htmlGeneral += '<div class="col-md-6 " >' + generarControlEncuesta(json.PREGUNTAS[v], "encuesta_" + json.ID_ENCUESTA + "_pregunta_" + json.PREGUNTAS[v].ID_PREGUNTA) + '</div>'
        htmlGeneral += '<button title="Ayuda" onclick="AbrirAyuda(\'' + json.PREGUNTAS[v].S_AYUDA + '\')" class="iconAyudaEncuesta">?</button>'
        htmlGeneral += '<button onclick="AbrirObservacion(\'Encuesta_' + json.ID_ENCUESTA + '_Pregunta_' + json.PREGUNTAS[v].ID_PREGUNTA + '_observacion\',\'' + json.PREGUNTAS[v].S_OBSERVACION + '\')" class="iconAyudaEncuesta iconobservacionEncuesta">i</button>'

        //&&&&& Agregado para solucionar el problema que no almacenaba la observación en los nuevos (20181204)
        htmlGeneral += '<input type="text" style="display:none" value id="Encuesta_' + json.ID_ENCUESTA + '_Pregunta_' + json.PREGUNTAS[v].ID_PREGUNTA + '_observacion">';

        htmlGeneral += " </div><hr>"
    }
    htmlGeneral += "</div>"

    boton.outerHTML = '<button class="agregar quitar" title="Quitar Encuesta" onclick="quitarAcordeonEncuestaitem(this,\'' + id + '\',' + uso + ')"></button> ';
    $("#" + id).append(htmlGeneral)
}

function quitarAcordeonEncuestaitem(boton, id, uso) {
    if (confirm("Esta seguro en desagregar la informacion de la encuesta")) {
        var nombre = $("#" + boton.parentElement.id)[0].className;
        var n = nombre.search("glyphicon-triangle-top");
        if (n < 0) {
            $("#" + boton.parentElement.id + " a").click();
        }
        $("#" + id + " .contenidoSub").remove();
        boton.outerHTML = '<button class="agregar" title="Agregar Encuesta" onclick="agregarAcordeonEncuestaitem(this,' + uso + ',\'' + id + '\')"></button> ';
        for (var z = 0; z < jsonEncuestas.length; z++) {
            if (jsonEncuestas[z].ID_ENCUESTA == uso) {
                jsonEncuestas[z].ESTADO = 0;
            }
        }
    } else {
        return;
    }
}

function CambiarEstadoAcordeon(idEncabezado) {
    var nombre = $(idEncabezado)[0].className;
    var n = nombre.search("glyphicon-triangle-top");
    if (n > 0) {
        $(idEncabezado).removeClass("glyphicon-triangle-top");
    } else {
        $(idEncabezado).addClass("glyphicon-triangle-top");
    }

}

function validarNumeros(e) { // 1
    tecla = (document.all) ? e.keyCode : e.which; // 2
    if (tecla == 8) return true; // backspace
    if (tecla == 9) return true; // tab
    if (tecla == 109) return true; // menos
    if (tecla == 110) return true; // punto
    if (tecla == 189) return true; // guion
    if (e.ctrlKey && tecla == 86) { return false }; //Ctrl v
    if (e.ctrlKey && tecla == 17) { return false }; //Ctrl v
    if (e.ctrlKey && tecla == 67) { return true }; //Ctrl c
    if (e.ctrlKey && tecla == 88) { return true }; //Ctrl x
    if (tecla >= 96 && tecla <= 105) { return true; } //numpad

    patron = /[0-9]/; // patron

    te = String.fromCharCode(tecla);
    return patron.test(te); // prueba
}

function validarNumerosD(e) { // 1
    tecla = (document.all) ? e.keyCode : e.which; // 2
    if (tecla == 8) return true; // backspace
    if (tecla == 9) return true; // tab
    if (tecla == 190) return true; // punto
    if (tecla == 109) return true; // menos
    if (tecla == 189) return true; // guion
    if (e.ctrlKey && tecla == 86) { return false }; //Ctrl v
    if (e.ctrlKey && tecla == 17) { return false }; //Ctrl v
    if (e.ctrlKey && tecla == 67) { return true }; //Ctrl c
    if (e.ctrlKey && tecla == 88) { return true }; //Ctrl x
    if (tecla >= 96 && tecla <= 105) { return true; } //numpad

    patron = /^[0-9]+(\,[0-9]{1,5})?$/; // patron

    te = String.fromCharCode(tecla);
    return patron.test(te); // prueba
}



////Encuestas

function consultarEncuestas(jsonEn) {
    try {
        var json = jsonEn;
    } catch (e) {
        alert("Error al cargar las encuestas.");
        return;
    }

    if (json == null) {
        alert("Error al cargar las encuestas.");
        return;
    }
    /*****fin json*/
    var htmlGeneral = ' <div class="panel-group acordeonVerde " id="acordionEncuestaPrincipal">';
    var acu0 = 0;
    var acu = 0;
    for (var r = 0; r < json.length; r++) {
        acu0++;
        htmlGeneral += '<div class="panel panel-default">'
        htmlGeneral += '<div class="panel-heading glyphicon-triangle-bottom fechaleft" id="EncuestacabezoteAcordeon' + r + '" >';
        htmlGeneral += '<input type="text" style="display:none" id="txtEncuesta_' + r + '" value="' + json[r].ID_ENCUESTA + '"> <a id="xyz' + acu0 + '"  data-toggle="collapse" data-parent="#Encuestaaccordion" href="#Encuestacollapse' + r + '" onclick="CambiarEstadoAcordeon(\'#EncuestacabezoteAcordeon' + r + '\')" class="titleAcordeon">' + json[r].S_NOMBRE
        if (json[r].S_TIPO == "S") {
            if (json[r].N_VALOR == null) {
                htmlGeneral += '<label class="valorLabel">valor:0</label>'
            } else {
                htmlGeneral += '<label class="valorLabel">valor:' + json[r].N_VALOR + '</label>'
            }

        }
        htmlGeneral += '</a>'
        if (json[r].ESTADO == 0) {
            acu++;
            htmlGeneral += '<button id="btnAdd' + acu + '" class="agregar" title="Agregar Encuesta" onclick="agregarAcordeonEncuestaitem(this,' + json[r].ID_ENCUESTA + ',\'Encuestacollapse' + r + '\')"></button> </div>';
        } else {
            htmlGeneral += '<button class="agregar quitar" title="Quitar Encuesta" onclick="quitarAcordeonEncuestaitem(this,\'Encuestacollapse' + r + '\',' + json[r].ID_ENCUESTA + ')"></button> </div>';


        }


        htmlGeneral += '<div id="Encuestacollapse' + r + '" class="panel-collapse collapse">';
        if (json[r].ESTADO == 1) {
            htmlGeneral += '<div class="  col-md-12 contenidoSub" >'
            var fila = "EncuestaCol1";
            for (var v = 0; v < json[r].PREGUNTAS.length; v++) {
                if (fila == "EncuestaCol1") {
                    htmlGeneral += '<div class=" row col-md-12 EncuestaCol1" >'
                    fila = "EncuestaCol2";
                } else {
                    htmlGeneral += '<div class=" row col-md-12 EncuestaCol2" >'
                    fila = "EncuestaCol1";
                }

                htmlGeneral += '<div class="col-md-5 " ><label>' + json[r].PREGUNTAS[v].S_NOMBRE
                if (json[r].PREGUNTAS[v].S_REQUERIDA == "1") {
                    htmlGeneral += " *"
                }
                htmlGeneral += '</label></div>'
                htmlGeneral += '<div class="col-md-6 " >' + generarControlEncuesta(json[r].PREGUNTAS[v], "encuesta_" + json[r].ID_ENCUESTA + "_pregunta_" + json[r].PREGUNTAS[v].ID_PREGUNTA) + '</div>'
                htmlGeneral += '<button onclick="AbrirAyuda(\'' + json[r].PREGUNTAS[v].S_AYUDA + '\')" class="iconAyudaEncuesta">?</button>'
                htmlGeneral += '<button onclick="AbrirObservacion(\'Encuesta_' + json[r].ID_ENCUESTA + '_Pregunta_' + json[r].PREGUNTAS[v].ID_PREGUNTA + '_observacion\',\'' + json[r].PREGUNTAS[v].S_OBSERVACION + '\')" class="iconAyudaEncuesta iconobservacionEncuesta">i</button>'
                htmlGeneral += '<input type="text" style="display:none" value="' + json[r].PREGUNTAS[v].S_OBSERVACION + '" id="Encuesta_' + json[r].ID_ENCUESTA + '_Pregunta_' + json[r].PREGUNTAS[v].ID_PREGUNTA + '_observacion">';
                htmlGeneral += " </div><hr>"
            }
            htmlGeneral += "</div>"
        }
        htmlGeneral += '<div style="  padding: 1px;">';
        /*nivel 2**/
        htmlGeneral += ' <div class="panel-group acordeonVerde acordeonSecundario" id="EncuestaacordionSecundario">'

        htmlGeneral += '</div>'
        /**final nivel2*/
        htmlGeneral += '</div>';
        htmlGeneral += '</div>';
        htmlGeneral += ' </div>';
    }
    htmlGeneral += ' </div>';



    return htmlGeneral;
}


function guardarEncuesta(JsonEnc) {

    var jsonEncuestas = JsonEnc;

    var cont = 0;
    var texto = "";
    var tezttemp = ""
    for (var z = 0; z < jsonEncuestas.length; z++) {
        if (jsonEncuestas[z].ESTADO == 1) {
            for (var i = 0; i < jsonEncuestas[z].PREGUNTAS.length; i++) {

                jsonEncuestas[z].PREGUNTAS[i].S_OBSERVACION = $('#Encuesta_' + jsonEncuestas[z].ID_ENCUESTA + '_Pregunta_' + jsonEncuestas[z].PREGUNTAS[i].ID_PREGUNTA + '_observacion').val();

                if (jsonEncuestas[z].PREGUNTAS[i].ID_TIPOPREGUNTA != 3 && jsonEncuestas[z].PREGUNTAS[i].ID_TIPOPREGUNTA != 1) {
                    var campo = $("#encuesta_" + jsonEncuestas[z].ID_ENCUESTA + "_pregunta_" + jsonEncuestas[z].PREGUNTAS[i].ID_PREGUNTA)[0].name;
                    if (jsonEncuestas[z].PREGUNTAS[i].S_REQUERIDA == "1") {
                        var Pvalor = $("#encuesta_" + jsonEncuestas[z].ID_ENCUESTA + "_pregunta_" + jsonEncuestas[z].PREGUNTAS[i].ID_PREGUNTA).val();
                        if (Pvalor == "" || Pvalor == "-1") {
                            cont++;
                            $("#encuesta_" + jsonEncuestas[z].ID_ENCUESTA + "_pregunta_" + jsonEncuestas[z].PREGUNTAS[i].ID_PREGUNTA).css("boxShadow", "0px 1px 7px 2px #FA0D0D")
                            var tttemp = jsonEncuestas[z].S_NOMBRE;
                            if (tttemp != tezttemp) {
                                tezttemp = jsonEncuestas[z].S_NOMBRE;
                                texto += "-" + jsonEncuestas[z].S_NOMBRE + "\n";
                            }


                        } else {
                            $("#encuesta_" + jsonEncuestas[z].ID_ENCUESTA + "_pregunta_" + jsonEncuestas[z].PREGUNTAS[i].ID_PREGUNTA).css("boxShadow", "");
                        }
                    }
                    if (campo == "ID_VALOR") {
                        jsonEncuestas[z].PREGUNTAS[i].ID_VALOR = $("#encuesta_" + jsonEncuestas[z].ID_ENCUESTA + "_pregunta_" + jsonEncuestas[z].PREGUNTAS[i].ID_PREGUNTA).val() * 1;

                    } else if (campo == "N_VALOR") {
                        jsonEncuestas[z].PREGUNTAS[i].N_VALOR = $("#encuesta_" + jsonEncuestas[z].ID_ENCUESTA + "_pregunta_" + jsonEncuestas[z].PREGUNTAS[i].ID_PREGUNTA).val() * 1;
                    } else if (campo == "S_VALOR") {
                        jsonEncuestas[z].PREGUNTAS[i].S_VALOR = $("#encuesta_" + jsonEncuestas[z].ID_ENCUESTA + "_pregunta_" + jsonEncuestas[z].PREGUNTAS[i].ID_PREGUNTA).val();
                    } else if (campo == "D_VALOR") {
                        jsonEncuestas[z].PREGUNTAS[i].D_VALOR = $("#encuesta_" + jsonEncuestas[z].ID_ENCUESTA + "_pregunta_" + jsonEncuestas[z].PREGUNTAS[i].ID_PREGUNTA).val();
                    }
                } else if (jsonEncuestas[z].PREGUNTAS[i].ID_TIPOPREGUNTA == 3) {
                    for (var o = 0; o < jsonEncuestas[z].PREGUNTAS[i].OPCIONES.length; o++) {
                        var valor = "#encuesta_" + jsonEncuestas[z].ID_ENCUESTA + "_pregunta_" + jsonEncuestas[z].PREGUNTAS[i].ID_PREGUNTA + o
                        if ($(valor).is(':checked')) {
                            jsonEncuestas[z].PREGUNTAS[i].OPCIONES[o].SELECTED = 1
                        } else {
                            jsonEncuestas[z].PREGUNTAS[i].OPCIONES[o].SELECTED = 0
                        }

                    }
                } else {
                    var valor = "#encuesta_" + jsonEncuestas[z].ID_ENCUESTA + "_pregunta_" + jsonEncuestas[z].PREGUNTAS[i].ID_PREGUNTA
                    if ($(valor).is(':checked')) {
                        jsonEncuestas[z].PREGUNTAS[i].ID_VALOR = 1
                    } else {
                        jsonEncuestas[z].PREGUNTAS[i].ID_VALOR = 0
                    }
                    if (jsonEncuestas[z].PREGUNTAS[i].S_REQUERIDA == "1") {
                        var Pvalor = "#encuesta_" + jsonEncuestas[z].ID_ENCUESTA + "_pregunta_" + jsonEncuestas[z].PREGUNTAS[i].ID_PREGUNTA

                        if (!$(Pvalor).is(':checked')) {
                            cont++;
                            $("#encuesta_" + jsonEncuestas[z].ID_ENCUESTA + "_pregunta_" + jsonEncuestas[z].PREGUNTAS[i].ID_PREGUNTA).css("boxShadow", "0px 1px 7px 2px #FA0D0D")
                            var tttemp = jsonEncuestas[z].S_NOMBRE;
                            if (tttemp != tezttemp) {
                                tezttemp = jsonEncuestas[z].S_NOMBRE;
                                texto += "-" + jsonEncuestas[z].S_NOMBRE + "\n";
                            }
                        } else {
                            $("#encuesta_" + jsonEncuestas[z].ID_ENCUESTA + "_pregunta_" + jsonEncuestas[z].PREGUNTAS[i].ID_PREGUNTA).css("boxShadow", "");
                        }
                    }
                }
            }

        }
    }
    if (cont > 0) {
        totaltext = "Debe diligenciar todas las preguntas obligatorias de las encuestas:\n" + texto
        mensajeAlmacenamiento(totaltext)
        return;
    }
    return jsonEncuestas;
}

function CambiarGuardarbtn(id) {
    $("#btnCargarFoto").css("display", "none")
    $("#btnCargarDocumento").css("display", "none")
    for (k = 1; k <= 5; k++) {

        $("#btnGuardar" + k).css("display", "none")

    }
    if (id == 6) {
        $("#btnCargarDocumento").css("display", "inline-block")
    }
    if (id == 4) {
        $("#btnCargarFoto").css("display", "inline-block")
        setTimeout(' consultarFotos()', 2000);
    }

    $("#btnGuardar" + id).css("display", "inline-block")
}

function cerrarPestala() {
    window.opener.CargarJsonAcordeon();
    window.close()

}


function myfunc(e) {
    agregarAcordeonitem(padre, grupo, e, json[r].ID_CARACTERISTICA, 'colapse' + json[r].ID_CARACTERISTICA + '_grupo' + grupo + '_padre' + padre);
};