/// <reference path="scripAguas.js" />
var jsonUbicacion = null;
var tipocaptacion = "";
var jsonUsos = null;
var jsonEncuestas = null;
var jsonDetalle = null;

function AbrirInfoUbi() {
    $("#dialogUbicacion").dialog();
}
function CambiarGuardarbtnA(id) {
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
function rtaUbiPoint(rta) {
    objetoMapgis.noDraw();
    $("#txtX").val(rta.x);
    $("#txtY").val(rta.y);
    $("#ubicarpuntobtn")[0].title = "Reubicar punto";

    if ($("#txtCaptacion").val() == "-1" && $("#txtEstadoBase").val() == "-1") {
        //reubico nueva captacion
        llamarGetCaptacionReubicar(-1, $("#txtX").val(), $("#txtY").val())

    } else if ($("#txtCaptacion").val() != "-1" && $("#txtTipoEstado").val() == "-1") {
        //reubico continuar
        llamarGetCaptacionReubicar($("#txtCaptacion").val(), -1, -1)
    } else if ($("#txtCaptacion").val() != "-1" && $("#txtTipoEstado").val() != "-1") {
        //reubico uno existente
        llamarGetCaptacionReubicar($("#txtCaptacion").val(), $("#txtX").val(), $("#txtY").val())

    }



}

function GuardarUbicacion(num) {
    if (num == 1) {
        guardarUbicacionoficial()
    } else if (num == 2) {
        guardarUsos();
    } else if (num == 3) {
        guardarDetalleOficial()
        
    } else if (num == 5) {
       
        guardarEncuesta()
    } else if (num == 4) {
        guardarFotosQuejas(Totalimg)
    }
}

function CargarComponentes() {
    consultarJsonUsos();
    consultarJsonEncuesta();
    consultarJsonDetalle();
    CargarGrid();
}

function cambiarTabs() {
    $("#Tab2").css('display', '');
    $("#Tab3").css('display', '');
    $("#Tab4").css('display', '');
    $("#Tab5").css('display', '');
    $("#Tab6").css('display', '');
}

/************PESTAÑA USOS***********/

function consultarUsos() {
    try {
        var json = jsonUsos;
    } catch (e) {
        alert("Error al cargar los usos.");
        return;
    }

    if (json == null) {
        alert("Error al cargar los usos.");
        return;
    }
    /*****fin json*/
    var htmlGeneral = ' <div class="panel-group acordeonVerde " id="acordionUsosPrincipal">';
    for (var r = 0; r < json.length; r++) {
        htmlGeneral += '<div class="panel panel-default">'
        htmlGeneral += '<div class="panel-heading glyphicon-triangle-bottom fechaleft" id="cabezoteAcordeon' + r + '" >';
        htmlGeneral += '<input type="text" style="display:none" id="txtuso_' + r + '" value="' + json[r].ID_USO + '"> <a data-toggle="collapse" data-parent="#accordion" href="#collapse' + r + '" onclick="CambiarEstadoAcordeon(\'#cabezoteAcordeon' + r + '\')" class="titleAcordeon">' + json[r].DESCRIPCION + '</a>'

        if (json[r].ESTADO == 0) {

            htmlGeneral += '<button class="agregar" title="Agregar Uso" onclick="agregarAcordeonUsoitem(this,' + json[r].ID_USO + ',\'collapse' + r + '\')"></button> </div>';
        } else {
            htmlGeneral += '<button class="agregar quitar" title="Quitar Uso" onclick="quitarAcordeonUsoitem(this,\'collapse' + r + '\',' + json[r].ID_USO + ')"></button> </div>';


        }


        htmlGeneral += '<div id="collapse' + r + '" class="panel-collapse collapse">';
        if (json[r].ESTADO == 1) {
            htmlGeneral += '<div class="  col-md-12 contenidoSub" >'
            var fila = "EncuestaCol1"
            for (var v = 0; v < json[r].VARIABLES.length; v++) {
                if (fila == "EncuestaCol1") {
                    htmlGeneral += '<div class=" row col-md-12 EncuestaCol1" >'
                    fila = "EncuestaCol2";
                } else {
                    htmlGeneral += '<div class=" row col-md-12 EncuestaCol2" >'
                    fila = "EncuestaCol1";
                }
                htmlGeneral += '<div class="col-md-5 " ><label>' + json[r].VARIABLES[v].DESCRIPCION + '</label></div>'
                htmlGeneral += '<div class="col-md-7 " >' + generarControl(json[r].VARIABLES[v], "uso_" + json[r].ID_USO + "_variable_" + json[r].VARIABLES[v].ID_VARIABLE) + '</div>'
                htmlGeneral += "</div>"
            }
            htmlGeneral += "</div>"
        }
        htmlGeneral += '<div style="  padding: 1px;">';
        /*nivel 2**/
        htmlGeneral += ' <div class="panel-group acordeonVerde acordeonSecundario" id="acordionSecundario">'

        htmlGeneral += '</div>'
        /**final nivel2*/
        htmlGeneral += '</div>';
        htmlGeneral += '</div>';
        htmlGeneral += ' </div>';
    }
    htmlGeneral += ' </div>';
    jsonUsos = json;

    $("#acordionUsosPrincipal").remove();
    $("#acordionUsosGeneral").append(htmlGeneral);
}
function agregarAcordeonUsoitem(boton, uso, id) {
    var nombre = $("#" + boton.parentElement.id)[0].className;
    var n = nombre.search("glyphicon-triangle-top");
    if (n < 0) {
        $("#" + boton.parentElement.id + " a").click();
    }
    var json = null;
    for (var z = 0; z < jsonUsos.length; z++) {
        var uso1 = jsonUsos[z].ID_USO;
            if (jsonUsos[z].ID_USO < 0) {
                uso1 = jsonUsos[z].ID_USO * (-1);
            }
            if (uso1 == uso) {
                json = jsonUsos[z];
                jsonUsos[z].ESTADO = 1;
            }
    }
    if (json == null) {
        return;
    }
    var htmlGeneral = "";
    htmlGeneral += '<div class="  col-md-12 contenidoSub" >'
    var fila = "EncuestaCol1"
    for (var v = 0; v < json.VARIABLES.length; v++) {
        if (fila == "EncuestaCol1") {
            htmlGeneral += '<div class=" row col-md-12 EncuestaCol1" >'
            fila = "EncuestaCol2";
        } else {
            htmlGeneral += '<div class=" row col-md-12 EncuestaCol2" >'
            fila = "EncuestaCol1";
        }
        htmlGeneral += '<div class="col-md-5 " ><label>' + json.VARIABLES[v].DESCRIPCION + '</label></div>'
        htmlGeneral += '<div class="col-md-7 " >' + generarControl(json.VARIABLES[v], "uso_" + json.ID_USO + "_variable_" + json.VARIABLES[v].ID_VARIABLE) + '</div>'
        htmlGeneral += "</div>"
    }
    htmlGeneral += "</div>"

    boton.outerHTML = '<button class="agregar quitar" title="Quitar Uso" onclick="quitarAcordeonUsoitem(this,\'' + id + '\',' + uso + ')"></button> ';
    $("#" + id).append(htmlGeneral)
}
function quitarAcordeonUsoitem(boton, id, uso) {
    if (confirm("Esta seguro en desagregar la informacion del uso")) {
        var nombre = $("#" + boton.parentElement.id)[0].className;
        var n = nombre.search("glyphicon-triangle-top");
        if (n < 0) {
            $("#" + boton.parentElement.id + " a").click();
        }
        $("#" + id + " .contenidoSub").remove();
        boton.outerHTML = '<button class="agregar" title="Agregar Uso" onclick="agregarAcordeonUsoitem(this,' + uso + ',\'' + id + '\')"></button> ';
        for (var z = 0; z < jsonUsos.length; z++) {
            if (jsonUsos[z].ID_USO == uso) {
                jsonUsos[z].ESTADO = 0;
                jsonUsos[z].ID_USO = jsonUsos[z].ID_USO * -1;
            }
        }
    } else {
        return;
    }
}
//function CambiarEstadoAcordeon(idEncabezado) {
//    var nombre = $(idEncabezado)[0].className;
//    var n = nombre.search("glyphicon-triangle-top");
//    if (n > 0) {
//        $(idEncabezado).removeClass("glyphicon-triangle-top");
//    } else {
//        $(idEncabezado).addClass("glyphicon-triangle-top");
//    }

//}
function validarNumeros(e) { // 1
    tecla = (document.all) ? e.keyCode : e.which; // 2
    if (tecla == 8) return true; // backspace
    if (tecla == 9) return true; // tab
    if (tecla == 109) return true; // menos
    if (tecla == 110) return true; // punto
    if (tecla == 189) return true; // guion
    if (e.ctrlKey && tecla == 86) { return false }; //Ctrl v
    if (e.ctrlKey && tecla == 86) { return false }; //Ctrl v
    if (e.ctrlKey && tecla == 67) { return true }; //Ctrl c
    if (e.ctrlKey && tecla == 88) { return true }; //Ctrl x
    if (tecla >= 96 && tecla <= 105) { return true; } //numpad
   

    patron = /[0-9]/; // patron

    te = String.fromCharCode(tecla);
    return patron.test(te); // prueba
}

function generarControl(objeto, id) {
    var control = ""
    if (objeto.TIPO_DATO == 1) {
        control = '<select id="' + id + '"  name="ID_VALOR" class="form-control"><option value="-1">Seleccione</option>'
        for (var i = 0; i < objeto.OPCIONES.length; i++) {
            control += '<option value="' + objeto.OPCIONES[i].ID_OPCION + '">' + objeto.OPCIONES[i].DESCRIPCION + '</option>'
        }
        control += '</select>'
        if (objeto.ID_VALOR != null) {
            setTimeout(function () { try { $("#" + id).val(objeto.ID_VALOR) } catch (e) { } }, 5000);
        }
        } else if (objeto.TIPO_DATO == 2) {
        control = ' <input type="number" class="form-control" onkeydown="return validarNumeros(event)"   id="' + id + '" name="N_VALOR" value="' + objeto.N_VALOR + '">'
    } else if (objeto.TIPO_DATO == 3) {
        control = ' <input type="text" class="form-control" id="' + id + '" name="S_VALOR" value="' + objeto.S_VALOR + '" >'
    } else if (objeto.TIPO_DATO == 4) {
        control = ' <input type="date" class="form-control" id="' + id + '" name="D_VALOR" value="' + objeto.D_VALOR + '">'
    } else if (objeto.TIPO_DATO == 5) {
        control = '<textarea class="form-control" rows="3" id="' + id + '" name="S_VALOR" >' + objeto.S_VALOR + '</textarea>';
    } else if (objeto.TIPO_DATO == 6) {
        control = ' <input type="number"  step="0.0001" class="form-control" onkeydown="return validarNumerosD(event)" id="' + id + '" name="N_VALOR" value="' + objeto.N_VALOR + '">'
    }

    return control;
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

//function generarControlDetalle(objeto, id) {
//    var control = ""
//    if (objeto.ID_TIPO_DATO == 1) {
//        control = '<select id="' + id + '"  name="ID_VALOR" class="form-control"><option value="-1">Seleccione</option>'
//        for (var i = 0; i < objeto.OPCIONES.length; i++) {
//            control += '<option value="' + objeto.OPCIONES[i].ID_OPCION + '">' + objeto.OPCIONES[i].DESCRIPCION + '</option>'
//        }
//        control += '</select>'
//        setTimeout(function () { try { $("#" + id).val(objeto.ID_VALOR) } catch (e) { } }, 6000);
//    } else if (objeto.ID_TIPO_DATO == 2) {
//        control = ' <input type="number"   class="form-control" onkeydown="return validarNumeros(event)" id="' + id + '" name="N_VALOR" value="' + objeto.N_VALOR + '">'
//    } else if (objeto.ID_TIPO_DATO == 3) {
//        control = ' <input type="text" class="form-control" id="' + id + '" name="S_VALOR" value="' + objeto.S_VALOR + '" >'
//    } else if (objeto.ID_TIPO_DATO == 4) {
//        control = ' <input type="date" class="form-control" id="' + id + '" name="D_VALOR" value="' + objeto.D_VALOR + '">'
//    } else if (objeto.ID_TIPO_DATO == 5) {
//        control = '<textarea class="form-control" rows="3" id="' + id + '" name="S_VALOR"  value="' + objeto.N_VALOR + '"></textarea>';
//    }
//    return control;
//}
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
            control += '<option value="' + objeto.OPCIONES[i].ID_RESPUESTA + '">' + objeto.OPCIONES[i].S_CODIGO + '</option>'
        }
        control += '</select>'
        setTimeout(function () { try { $("#" + id).val(objeto.ID_VALOR) } catch (e) { } }, 7000);
    } else if (objeto.ID_TIPOPREGUNTA == 3) {
        control = '<div ">'
        for (var i = 0; i < objeto.OPCIONES.length; i++) {
            control += '<div class="checkbox">  <label>   <input id="' + id + i + '" name="S_VALOR" value="' + objeto.OPCIONES[i].S_VALOR + '" type="checkbox">' + objeto.OPCIONES[i].S_CODIGO + '</label> </div>'

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
    for (var v = 0; v < json.PREGUNTAS.length; v++) {
        htmlGeneral += '<div class=" row col-md-12" >'
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
function guardarUsos() {
    for (var z = 0; z < jsonUsos.length; z++) {
        if (jsonUsos[z].ESTADO == 1) {
            for (var i = 0; i < jsonUsos[z].VARIABLES.length; i++) {
                var campo = $("#uso_" + jsonUsos[z].ID_USO + "_variable_" + jsonUsos[z].VARIABLES[i].ID_VARIABLE)[0].name;
                if (campo == "ID_VALOR") {
                    jsonUsos[z].VARIABLES[i].ID_VALOR = $("#uso_" + jsonUsos[z].ID_USO + "_variable_" + jsonUsos[z].VARIABLES[i].ID_VARIABLE).val() * 1;

                } else if (campo == "N_VALOR") {
                    jsonUsos[z].VARIABLES[i].N_VALOR = $("#uso_" + jsonUsos[z].ID_USO + "_variable_" + jsonUsos[z].VARIABLES[i].ID_VARIABLE).val() * 1;
                } else if (campo == "S_VALOR") {
                    jsonUsos[z].VARIABLES[i].S_VALOR = $("#uso_" + jsonUsos[z].ID_USO + "_variable_" + jsonUsos[z].VARIABLES[i].ID_VARIABLE).val();
                } else if (campo == "D_VALOR") {
                    jsonUsos[z].VARIABLES[i].D_VALOR = $("#uso_" + jsonUsos[z].ID_USO + "_variable_" + jsonUsos[z].VARIABLES[i].ID_VARIABLE).val();
                }

            }

        }
    }

    guardarUsosOficial();
}
/********************************/
/************PESTAÑA DETALLE***********/

//function consultarDetalle() {

//    var json = jsonDetalle;
//    /*****fin json*/
//    var htmlGeneral = ' <div class="panel-group acordeonVerde " id="acordionUsosPrincipal">';
//    for (var r = 0; r < json.length; r++) {
//        htmlGeneral += '<div class="panel panel-default">'
//        htmlGeneral += '<div class="panel-heading glyphicon-triangle-bottom fechaleft" id="DcabezoteAcordeon' + r + '" >';
//        htmlGeneral += '<input type="text" style="display:none" id="txtuso_' + r + '" value="' + json[r].ID_CARACTERISTICA + '"> <a data-toggle="collapse" data-parent="#accordion" href="#Dcollapse' + r + '" onclick="CambiarEstadoAcordeon(\'#DcabezoteAcordeon' + r + '\')" class="titleAcordeon">' + json[r].S_DESCRIPCION + '</a>'
//        if (json[r].S_CARDINALIDAD == 1) {
//            if (json[r].ITEMCARACTERISTICA.length == 0) {
//                htmlGeneral += '<button class="agregar" title="Agregar Detalle" onclick="agregarAcordeonitem(this,' + json[r].ID_CARACTERISTICA + ',\'Dcollapse' + r + '\')"></button> </div>';

//            } else {
//                htmlGeneral += '<button class="agregar quitar" title="Quitar Detalle" onclick="quitarAcordeonitem(this,' + json[r].ID_CARACTERISTICA + ',\'Dcollapse' + r + '\')"></button> </div>';

//            }
//        } else {
//            htmlGeneral += '<button class="agregar" title="Agregar Detalle" onclick="agregarAcordeonitem(this,' + json[r].ID_CARACTERISTICA + ',\'Dcollapse' + r + '\')"></button> </div>';

//        }





//        htmlGeneral += '<div id="Dcollapse' + r + '" class="panel-collapse collapse">';
//        htmlGeneral += '<div padding: 1px;">';
//        if (json[r].S_CARDINALIDAD == 1) {
//            if (json[r].ITEMCARACTERISTICA.length != 0) {
//                htmlGeneral += '<div class="  col-md-12 contenidoSub" >'

//                for (var v = 0; v < json[r].ITEMCARACTERISTICA[0].VARIABLES.length; v++) {
//                    htmlGeneral += '<div class=" row col-md-12" >'
//                    htmlGeneral += '<div class="col-md-5 " ><label>' + json[r].ITEMCARACTERISTICA[0].VARIABLES[v].S_NOMBRE + '</label></div>'
//                    // jsonDetalle[posicionSub].ITEMCARACTERISTICA[0].NOMBRE=json.S_DESCRIPCION+'(Nuevo )';

//                    htmlGeneral += '<div class="col-md-7 " >' + generarControlDetalle(json[r].ITEMCARACTERISTICA[0].VARIABLES[v], "caracteristica_" + json[r].ID_CARACTERISTICA + "_variable_" + json[r].ITEMCARACTERISTICA[0].VARIABLES[v].ID_VARIABLE) + '</div>'
//                    htmlGeneral += "</div>"
//                }
//            }
//            htmlGeneral += ' </div>'


//            htmlGeneral += '</div>'



//        } else {
//            /*nivel 2**/
//            /*nivel 2**/
//            htmlGeneral += ' <div class="panel-group acordeonVerde acordeonSecundario" id="acordionSecundario">'
//            for (var f = 0; f < json[r].ITEMCARACTERISTICA.length; f++) {
//                htmlGeneral += ' <div class="panel panel-default">'
//                htmlGeneral += ' <div class="panel-heading glyphicon-triangle-bottom" id="subcabezoteAcordeon_' + r + '_' + f + '">'
//                htmlGeneral += ' <a data-toggle="collapse" data-parent="#accordion" id="subcollapse_' + r + '_' + f + '_title" href="#subcollapse_' + r + '_' + f + '" onclick="CambiarEstadoAcordeon(\'#subcabezoteAcordeon_' + r + '_' + f + '\')" class="titleAcordeon"><input class="tituloTexto" id="Caracteristicassub_' + json[r].ITEMCARACTERISTICA[f].ID_CARACTERISTICA_ESTADO + '" type="text" value="' + json[r].ITEMCARACTERISTICA[f].NOMBRE + '"></a>'
//                htmlGeneral += ' <button class="agregar quitar" title="Quitar detalle" onclick="eliminarsubacordion( \'#subcabezoteAcordeon_' + r + '_' + f + '\',' + r + ', \'#subcollapse_' + r + '_' + f + '\')"></button>'
//                htmlGeneral += '</div>'
//                htmlGeneral += '<div id="subcollapse_' + r + '_' + f + '" class="panel-collapse collapse contenidoSub">'
//                /**/
//                for (var v = 0; v < json[r].ITEMCARACTERISTICA[f].VARIABLES.length; v++) {
//                    htmlGeneral += '<div class=" row col-md-12"  >'
//                    htmlGeneral += '<div class="col-md-5 " ><label>' + json[r].ITEMCARACTERISTICA[f].VARIABLES[v].S_NOMBRE + '</label></div>'
//                    htmlGeneral += '<div class="col-md-7 " >' + generarControlDetalle(json[r].ITEMCARACTERISTICA[f].VARIABLES[v], "caracteristica_" + json[r].ID_CARACTERISTICA + "_variable_" + json[r].ITEMCARACTERISTICA[f].VARIABLES[v].ID_VARIABLE) + '</div>'
//                    htmlGeneral += "</div>"
//                }

//                /***/
//                htmlGeneral += ' </div>'
//                htmlGeneral += '</div>'

//            }
//            htmlGeneral += '</div>';
//            htmlGeneral += '</div>';
//            /**final nivel2*/

//        }
//        htmlGeneral += ' </div>';

//    }

//    $("#acordionDetallePrincipal").remove();
//    $("#acordionDetalleGeneral").append(htmlGeneral);
//}
//function agregarAcordeonitem(boton, uso, id) {
//    var posicionSub = 0;
//    var nombre = $("#" + boton.parentElement.id)[0].className;
//    var n = nombre.search("glyphicon-triangle-top");
//    if (n < 0) {
//        $("#" + boton.parentElement.id + " a").click();
//    }
//    var json = null;
//    for (var z = 0; z < jsonDetalle.length; z++) {
//        if (jsonDetalle[z].ID_CARACTERISTICA == uso) {
//            jsonDetalle[z].ITEMCARACTERISTICA[jsonDetalle[z].ITEMCARACTERISTICA.length] = jsonDetalle[z].PLANTILLA
//            posicionSub = z;
//            json = jsonDetalle[z];



//        }
//    }
//    if (json == null) {
//        return;
//    }
//    var htmlGeneral = "";
//    if (json.S_CARDINALIDAD == 1) {
//        htmlGeneral += '<div class="  col-md-12 contenidoSub" >'
//        for (var v = 0; v < json.ITEMCARACTERISTICA[0].VARIABLES.length; v++) {
//            htmlGeneral += '<div class=" row col-md-12" >'
//            htmlGeneral += '<div class="col-md-5 " ><label>' + json.ITEMCARACTERISTICA[0].VARIABLES[v].S_NOMBRE + '</label></div>'
//            jsonDetalle[posicionSub].ITEMCARACTERISTICA[0].NOMBRE = json.S_DESCRIPCION + '(Nuevo )';

//            htmlGeneral += '<div class="col-md-7 " >' + generarControlDetalle(json.ITEMCARACTERISTICA[0].VARIABLES[v], "caracteristica_" + json.ID_CARACTERISTICA + "_variable_" + json.ITEMCARACTERISTICA[0].VARIABLES[v].ID_VARIABLE) + '</div>'
//            htmlGeneral += "</div>"
//        }
//        boton.outerHTML = '<button class="agregar quitar" title="Quitar Detalle" onclick="quitarAcordeonitem(this,\'' + id + '\',' + uso + ')"></button> ';

//    } else {
//        /*nivel 2**/
//        htmlGeneral += ' <div class="panel-group acordeonVerde acordeonSecundario" id="acordionSecundario">'
//        for (var f = json.ITEMCARACTERISTICA.length - 1; f < json.ITEMCARACTERISTICA.length; f++) {
//            htmlGeneral += ' <div class="panel panel-default">'
//            htmlGeneral += '                                        <div class="panel-heading glyphicon-triangle-bottom" id="subcabezoteAcordeon_' + uso + '_' + f + '">'
//            htmlGeneral += ' <a data-toggle="collapse" data-parent="#accordion" id="subcollapse_' + uso + '_' + f + '_title" href="#subcollapse_' + uso + '_' + f + '" onclick="CambiarEstadoAcordeon(\'#subcabezoteAcordeon_' + uso + '_' + f + '\')" class="titleAcordeon"><input class="tituloTexto" id="Caracteristicassub_' + json.ITEMCARACTERISTICA[f].ID_CARACTERISTICA_ESTADO + '" type="text" value="' + json.S_DESCRIPCION + '(nuevo)"></a>'

//            htmlGeneral += ' <button class="agregar quitar" title="Quitar detalle" onclick="eliminarsubacordion( \'#subcabezoteAcordeon_' + uso + '_' + f + '\',' + uso + ', \'#subcollapse_' + uso + '_' + f + '\')"></button>'
//            htmlGeneral += '</div>'
//            htmlGeneral += '<div id="subcollapse_' + uso + '_' + f + '" class="panel-collapse collapse contenidoSub">'
//            /**/
//            for (var v = 0; v < json.ITEMCARACTERISTICA[f].VARIABLES.length; v++) {
//                htmlGeneral += '<div class=" row col-md-12"  >'
//                htmlGeneral += '<div class="col-md-5 " ><label>' + json.ITEMCARACTERISTICA[f].VARIABLES[v].S_NOMBRE + '</label></div>'
//                htmlGeneral += '<div class="col-md-7 " >' + generarControlDetalle(json.ITEMCARACTERISTICA[f].VARIABLES[v], "caracteristica_" + json.ID_CARACTERISTICA + "_variable_" + json.ITEMCARACTERISTICA[f].VARIABLES[v].ID_VARIABLE) + '</div>'
//                htmlGeneral += "</div>"
//            }

//            /***/
//            htmlGeneral += ' </div>'
//            htmlGeneral += '</div>'
//        }
//        htmlGeneral += '</div>';
//        /**final nivel2*/
//    }
//    htmlGeneral += "</div>"



//    $("#" + id).append(htmlGeneral)
//}

//function quitarAcordeonitem(boton, id, cara) {
//    if (confirm("Esta seguro en desagregar la informacion del detalle")) {
//        var nombre = $("#" + boton.parentElement.id)[0].className;
//        var n = nombre.search("glyphicon-triangle-top");
//        if (n < 0) {
//            $("#" + boton.parentElement.id + " a").click();
//        }
//        for (var z = 0; z < jsonDetalle.length; z++) {
//            if (jsonDetalle[z].ID_CARACTERISTICA == id) {
//                for (var i = 0; i < jsonDetalle[z].ITEMCARACTERISTICA.length; i++) {

//                    if (i == 0) {
//                        jsonDetalle[z].ITEMCARACTERISTICA.splice(i - 1, 1);
//                        break;
//                    }
//                }
//            }
//        }


//        $("#" + cara + " .contenidoSub").remove();
//        boton.outerHTML = '<button class="agregar" title="Agregar detalle" onclick="agregarAcordeonitem(this,' + cara + ',\'' + id + '\')"></button> ';

//    } else {
//        return;
//    }
//}

//function eliminarsubacordion(id, subid, idconr) {
//    if (confirm("Esta seguro en desagregar la informacion del detalle")) {
//        var posicion = id.replace("#subcabezoteAcordeon_" + subid + "_", "");
//        posicion = posicion * 1;

//        for (var z = 0; z < jsonDetalle.length; z++) {
//            if (jsonDetalle[z].ID_CARACTERISTICA == subid) {
//                for (var i = 0; i < jsonDetalle[z].ITEMCARACTERISTICA.length; i++) {

//                    if (i == posicion) {
//                        $(id).remove();
//                        $(idconr).remove();
//                        jsonDetalle[z].ITEMCARACTERISTICA.splice(i - 1, 1);
//                        break;
//                    }
//                }
//            }
//        }

//        $("#" + id + " ").remove();
//    } else {
//        return;
//    }


//}
//function consultarEncuestas() {
//    try {
//        var json = jsonEncuestas;
//    } catch (e) {
//        alert("Error al cargar las encuestas.");
//        return;
//    }

//    if (json == null) {
//        alert("Error al cargar las encuestas.");
//        return;
//    }
//    /*****fin json*/
//    var htmlGeneral = ' <div class="panel-group acordeonVerde " id="acordionEncuestaPrincipal">';
//    for (var r = 0; r < json.length; r++) {
//        htmlGeneral += '<div class="panel panel-default">'
//        htmlGeneral += '<div class="panel-heading glyphicon-triangle-bottom fechaleft" id="EncuestacabezoteAcordeon' + r + '" >';
//        htmlGeneral += '<input type="text" style="display:none" id="txtEncuesta_' + r + '" value="' + json[r].ID_ENCUESTA + '"> <a data-toggle="collapse" data-parent="#Encuestaaccordion" href="#Encuestacollapse' + r + '" onclick="CambiarEstadoAcordeon(\'#EncuestacabezoteAcordeon' + r + '\')" class="titleAcordeon">' + json[r].S_NOMBRE
//        if (json[r].S_TIPO == "S") {
//            htmlGeneral += '<label class="valorLabel">valor:' + json[r].N_VALOR + '</label>'
//        }
//        htmlGeneral += '</a>'
//        if (json[r].ESTADO == 0) {

//            htmlGeneral += '<button class="agregar" title="Agregar Encuesta" onclick="agregarAcordeonEncuestaitem(this,' + json[r].ID_ENCUESTA + ',\'Encuestacollapse' + r + '\')"></button> </div>';
//        } else {
//            htmlGeneral += '<button class="agregar quitar" title="Quitar Encuesta" onclick="quitarAcordeonEncuestaitem(this,\'Encuestacollapse' + r + '\',' + json[r].ID_ENCUESTA + ')"></button> </div>';


//        }


//        htmlGeneral += '<div id="Encuestacollapse' + r + '" class="panel-collapse collapse">';
//        if (json[r].ESTADO == 1) {
//            htmlGeneral += '<div class="  col-md-12 contenidoSub" >'
//            for (var v = 0; v < json[r].PREGUNTAS.length; v++) {
//                htmlGeneral += '<div class=" row col-md-12" >'
//                htmlGeneral += '<div class="col-md-5 " ><label>' + json[r].PREGUNTAS[v].S_NOMBRE
//                if (json[r].PREGUNTAS[v].S_REQUERIDA == "1") {
//                    htmlGeneral += " *"
//                }
//                htmlGeneral += '</label></div>'
//                htmlGeneral += '<div class="col-md-6 " >' + generarControlEncuesta(json[r].PREGUNTAS[v], "encuesta_" + json[r].ID_ENCUESTA + "_pregunta_" + json[r].PREGUNTAS[v].ID_PREGUNTA) + '</div>'
//                htmlGeneral += '<button onclick="AbrirAyuda(\'' + json[r].PREGUNTAS[v].S_AYUDA + '\')" class="iconAyudaEncuesta">?</button>'
//                htmlGeneral += '<button onclick="AbrirObservacion(\'Encuesta_' + json[r].ID_ENCUESTA + '_Pregunta_' + json[r].PREGUNTAS[v].ID_PREGUNTA + '_observacion\',\'' + json[r].PREGUNTAS[v].S_OBSERVACION + '\')" class="iconAyudaEncuesta iconobservacionEncuesta">i</button>'
//                htmlGeneral += '<input type="text" style="display:none" value="' + json[r].PREGUNTAS[v].S_OBSERVACION + '" id="Encuesta_' + json[r].ID_ENCUESTA + '_Pregunta_' + json[r].PREGUNTAS[v].ID_PREGUNTA + '_observacion">';
//                htmlGeneral += " </div><hr>"
//            }
//            htmlGeneral += "</div>"
//        }
//        htmlGeneral += '<div style="  padding: 1px;">';
//        /*nivel 2**/
//        htmlGeneral += ' <div class="panel-group acordeonVerde acordeonSecundario" id="EncuestaacordionSecundario">'

//        htmlGeneral += '</div>'
//        /**final nivel2*/
//        htmlGeneral += '</div>';
//        htmlGeneral += '</div>';
//        htmlGeneral += ' </div>';
//    }
//    htmlGeneral += ' </div>';
//    jsonEncuestas = json;

//    $("#acordionEncuestaPrincipal").remove();
//    $("#acordionEncuestaGeneral").append(htmlGeneral);
//}
/******************************************/
/*****************fotos***********************/

var Totalimg = new Array();

/*********************************/

/***********************************************/


//function guardarDetalle() {
//    for (var z = 0; z < jsonDetalle.length; z++) {

//        for (var i = 0; i < jsonDetalle[z].ITEMCARACTERISTICA.length; i++) {
//            for (var X = 0; X < jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES.length; X++) {
//                var campo = $("#caracteristica_" + jsonDetalle[z].ID_CARACTERISTICA + "_variable_" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE)[0].name;
//                if (jsonDetalle[z].S_CARDINALIDAD != "1") {
//                    jsonDetalle[z].ITEMCARACTERISTICA[i].NOMBRE = $('#Caracteristicassub_' + jsonDetalle[z].ITEMCARACTERISTICA[i].ID_CARACTERISTICA_ESTADO).val();
//                }
//                if (campo == "ID_VALOR") {
//                    jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VALOR = $("#caracteristica_" + jsonDetalle[z].ID_CARACTERISTICA + "_variable_" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE).val() * 1;

//                } else if (campo == "N_VALOR") {
//                    jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].N_VALOR = $("#caracteristica_" + jsonDetalle[z].ID_CARACTERISTICA + "_variable_" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE).val() * 1;
//                } else if (campo == "S_VALOR") {
//                    jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].S_VALOR = $("#caracteristica_" + jsonDetalle[z].ID_CARACTERISTICA + "_variable_" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE).val();
//                } else if (campo == "D_VALOR") {
//                    jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].D_VALOR = $("#caracteristica_" + jsonDetalle[z].ID_CARACTERISTICA + "_variable_" + jsonDetalle[z].ITEMCARACTERISTICA[i].VARIABLES[X].ID_VARIABLE).val();
//                }
//            }
//        }


//    }

//    guardarDetalleOficial();
//}


function guardarEncuesta() {
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
                        var Pvalor = $("#encuesta_" + jsonEncuestas[z].ID_ENCUESTA + "_pregunta_" + jsonEncuestas[z].PREGUNTAS[i].ID_PREGUNTA).val()
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
        alert(totaltext)
        return;
    }
    guardarEncuestaOficial();
}

function cerrarPestala() {
    window.opener.CargarJsonAcordeon();
    window.close()

}