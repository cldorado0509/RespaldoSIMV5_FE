
var rol;
var coorX = "";
var coorY = "";
var TipoUbicacion = "";
var idVisita = "";
var estados = "";
var tipoUbicacion = "";
$(document).ready(function () {

    $("#webcam").webcam({
        width: 320,
        height: 240,
        mode: "save",
        swffile: "/Scripts/webcam/jscam_canvas_only.swf",
        onTick: function () { },
        onSave: function (data) {


        },
        onCapture: function () {
            //webcam.save('/ControlVigilancia/Visitas/Capture');
            $.ajax({
                type: "GET",
                url: "/ControlVigilancia/Visitas/Capture",

                success: function (r) {
                    $("[id*=imgCapture]").css("visibility", "visible");
                    $("[id*=imgCapture]").attr("src", r.d);
                },
                failure: function (response) {
                    alert(response.d);
                }
            });


        },
        debug: function () { },
        onLoad: function () { }
    });

    $("#txtFechaInicial").datepicker();
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();
    $("#txtFechaInicio").val(dd + '/' + mm + '/' + yyyy);
    $("#txtFechaAsignacion").val(dd + '/' + mm + '/' + yyyy);

    $("#txtEstadoVisita").val("@ViewBag.txtEstadoVisita");
    $("#txtRadicado").val("@ViewBag.txtRadicado");
    $("#txtAsunto").val("@ViewBag.txtAsunto");
    $("#IdVisita").val("@ViewBag.IdVisita");
    $("#txtTipoUbicacion").val("@ViewBag.txtTipoUbicacion");
    $("#txtObservacion").val("@ViewBag.txtObservacion");
    $("#txtResponsable").val("@ViewBag.txtResponsable");
    $("#txtcopias").val("@ViewBag.txtcopias");



    estados = $("#txtEstadoVisita").val();


    switch (Number(estados)) {
        case 1: //asignado
            $("#ibtabAtiende").css({ "display": "none" });
            $("#idImdEncuesta").css({ "display": "none" });
            $("#btnAddAtiende").css({ "display": "none" });
            $("#btnIniciarVisita").css({ "display": "none" });

            $("#btnEnviarCertificado").css({ "display": "none" });
            $("#btnTerminarVisita").css({ "display": "none" });
            $("#btnPreview").css({ "display": "none" });

            $("#btnGuardar").text("Aceptar Visita");

            break;
        case 2://aceptado
            $("#ibtabAtiende").css({ "display": "block" });
            $("#idImdEncuesta").css({ "display": "block" });
            $("#btnAddAtiende").css({ "display": "block" });
            $("#btnIniciarVisita").css({ "display": "block" });

            $("#btnGuardar").text("Guardar");

            $("#btnEnviarCertificado").css({ "display": "none" });
            $("#btnTerminarVisita").css({ "display": "none" });
            $("#btnPreview").css({ "display": "none" });
            consultarDatosRealizarVisitas();
            break;
        case 3://en proceso

            $("#ibtabAtiende").css({ "display": "block" });
            $("#idImdEncuesta").css({ "display": "block" });
            $("#btnAddAtiende").css({ "display": "block" });
            $("#btnEnviarCertificado").css({ "display": "block" });

            $("#btnGuardar").text("Guardar");
            $("#btnIniciarVisita").css({ "display": "none" });
            $("#btnTerminarVisita").css({ "display": "none" });
            consultarDatosRealizarVisitas();
            break;
        case 4://en revision

            $("#ibtabAtiende").css({ "display": "block" });
            $("#idImdEncuesta").css({ "display": "block" });
            $("#btnAddAtiende").css({ "display": "block" });
            $("#btnPreview").css({ "display": "block" });
            $("#btnTerminarVisita").css({ "display": "block" });
            $("#btnEnviarCertificado").css({ "display": "none" });
            $("#btnIniciarVisita").css({ "display": "none" });
            $("#btnGuardar").text("Guardar");

            consultarDatosRealizarVisitas();

            break;

    }




});
function adicionar() {

    var table = document.getElementById(tableID);
    var rowCount = table.rows.length;
    var row = table.insertRow(rowCount);
    var cell1 = row.insertCell(0);
    var element1 = document.createElement("input"); element1.type = "checkbox";
    element1.name = "chkbox[]"; cell1.appendChild(element1);
    var cell2 = row.insertCell(1); cell2.innerHTML = rowCount + 1;
    var cell3 = row.insertCell(2);
    var element2 = document.createElement("input");
    element2.type = "text";
    element2.name = "txtbox[]"; cell3.appendChild(element2);
}
function agregartab(datos) {

    var idLi = datos.id + tab;
    var nombre = datos.value;
    if (datos.checked == true) {
        var num = 0;
        var tab = document.getElementById("tbId");
        var li = document.createElement("li");
        li.id = idLi;
        li.innerHTML = "<a href=" + '#' + idLi + " data-toggle='tab'>" + nombre + "</a>"
        tab.appendChild(li);
        var divcontenedor = document.getElementById("myTabContent");
        var divTab = document.createElement("div");
        divTab.className = 'tab-pane fade';
        divTab.id = idLi;
        divcontenedor.appendChild(divTab);
    } else {
        document.getElementById(idLi).remove();
        var elem = document.getElementById(idLi);
        elem.parentNode.removeChild(elem);

    }

}




function showDialog() {
    $("#pantallaEmpresa").dialog(
    {

        width: 600,
        height: 400
    });
    $("#frm_dialog").attr("src", "/ControlVigilancia/Visitas/RealizarVisitaAtiende");
}
function abrirAtiende() {
    $("#atiendePantalla").dialog(
    {

        width: 600,
        height: 400
    });
    $("#frm_atiende").attr("src", "/ControlVigilancia/Visitas/GridAtiendeTabla");
}
function abrirAsignacionTramites() {
    $("#pantallaAsignacionTramites").dialog(
    {

        width: 900,
        height: 400
    });
    $("#AsignacionTramites").attr("src", "/ControlVigilancia/Visitas/RegistrarVisitasAsignacionTramites");
}

function myFunction(nombre, id) {
    var table = document.getElementById("atiende");
    var row = table.insertRow(1);
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    var cell3 = row.insertCell(2);
    var cell4 = row.insertCell(3);
    cell1.innerHTML = id;
    cell2.innerHTML = nombre;
    cell3.innerHTML = "<select id=" + table.rows.length + "></select>";
    cell4.innerHTML = "<button  class='btn btn-default'>Editar</button>";
    ConsultarRolVista(table.rows.length, '-1');
}
function cargarEmpresa(nombre, id) {
    $("#txtEmpresa").val(nombre);
    $("#txtId").val(id);
    ConsultarIntalacion(id);
}

function ConsultarIntalacion(id) {


    $.ajax({


        type: "GET",
        url: "/ControlVigilancia/api/VisitasWebAPI/GetConsultarInstalacion?id=" + id,


        success: function (result) {
            var datos = result;
            var comb = document.getElementById("txtInstalacion");
            $("#txtInstalacion").children().remove();
            comb.innerHTML = "<option value='-1'>--seleccionar--</option>";
            if (datos.length > 0) {



                for (var i = 0; i < datos.length; i++) {
                    comb.innerHTML = "<option value=" + datos[i].ID_INSTALACION + ">" + datos[i].S_NOMBRE + "</option>";
                    //datos[0].ID_INSTALACION

                }
            }
        }
    });


}
function ConsultarRolVista(combo, id) {


    $.ajax({


        type: "GET",
        url: "/ControlVigilancia/api/VisitasWebAPI/GetConsultarConsultarRolVisita",


        success: function (result) {
            var datos = result;
            $("#" + combo).find("option").remove();
            $("#" + combo).append(
                $("<option></option>").attr("value", "-1").text(
                    "--Seleccionar--"));
            $.each(datos, function (i, Dominio) {
                $("#" + combo).append(
                    $("<option></option>").attr("value",
                        Dominio.ID_ROL).text(
                        Dominio.S_NOMBRE));
            });
            if (id != "-1")
                $("#" + combo).val(id);
        }
    });


}
function guardar() {
    var radicado = $("#txtRadicado").val();
    var asunto = $("#txtAsunto").val();
    var observacion = $("#txtObservacion").val();
    var idEmpresa = $("#txtId").val();
    var idIntalacion = $("#txtInstalacion").val();
    var tabla = document.getElementById("atiende");
    var id = "";
    var k = 2;
    var j = 0;

    for (var i = 1; i < document.getElementById('atiende').rows.length ; i++) {
        if ($("#" + k).val() == "-1") {
            j++;
        }
        if (document.getElementById('atiende').rows.length - 1 == i) {
            id += document.getElementById('atiende').rows[i].cells[0].innerHTML + "_" + $("#" + k).val();
        } else {
            id += document.getElementById('atiende').rows[i].cells[0].innerHTML + "_" + $("#" + k).val() + ",";
        }
        k++;
    }
    if (j > 0) {
        alert("selecione el usuario");
        return;
    }

    var atendio = "";
    if (id == "") {
        atendio = "0";
    } else {
        atendio = id;
    }

    $.ajax({
        type: "GET",
        url: '/ControlVigilancia/api/VisitasWebAPI/guardarRealizarVisita',
        data: {
            p_Asunto: asunto, p_cx: 0,
            p_cy: 0, p_IdVisita: $("#IdVisita").val(), idTercero: idEmpresa, idInstalacion: idIntalacion, atiende: atendio, observacion: observacion
        },
        beforeSend: function () {

        },
        success: function (response) {
            var dato = response;
            if (dato == "1") {
                alert("almacenamiento exitoso");
                if (estados == 1)
                    validarForm(2);
            } else {
                alert("Error");
            }
        },
    });
}


function consultarDatosRealizarVisitas() {


    $.ajax({


        type: "GET",
        url: "/ControlVigilancia/api/VisitasWebAPI/GETConsultarDatosRealizarVisita?id=" + $("#IdVisita").val(),


        success: function (result) {
            var datos = result;

            if (datos.length > 0) {
                $("#txtEmpresa").val(datos[0].NOMBRETERCERO);
                $("#txtId").val(datos[0].IDTERCERO);
                $("#txtAsunto").val(datos[0].ASUNTO);
                $("#txtObservacion").val(datos[0].OBSEVACION);

                var comb = document.getElementById("txtInstalacion");
                $("#txtInstalacion").children().remove();
                comb.innerHTML = "<option value='-1'>--seleccionar--</option>";
                comb.innerHTML = "<option value=" + datos[0].IDINSTALACION + ">" + datos[0].INSTALACION + "</option>";


            }
            AtiendeRealizarVisitas();
        }
    });

}


function AtiendeRealizarVisitas() {


    $.ajax({


        type: "GET",
        url: "/ControlVigilancia/api/VisitasWebAPI/GETConsultarAtiendeRealizarVisita?id=" + $("#IdVisita").val(),


        success: function (result) {
            var datos = result;

            if (datos.length > 0) {
                for (var i = 0; i < datos.length; i++) {
                    var table = document.getElementById("atiende");
                    var row = table.insertRow(1);
                    var cell1 = row.insertCell(0);
                    var cell2 = row.insertCell(1);
                    var cell3 = row.insertCell(2);
                    var cell4 = row.insertCell(3);
                    cell1.innerHTML = datos[i].ID_TERCERO;
                    cell2.innerHTML = datos[i].S_RSOCIAL;
                    cell3.innerHTML = "<select id=" + table.rows.length + "></select>";
                    cell4.innerHTML = "<button class='btn btn-default'>Editar</button>";
                    ConsultarRolVista(table.rows.length, datos[i].ID_ROL);
                }
            }
        }
    });

}

function validarForm(idEstado) {
    estados = idEstado;

    switch (idEstado) {
        case 1: //asignado
            $("#ibtabAtiende").css({ "display": "none" });
            $("#idImdEncuesta").css({ "display": "none" });
            $("#btnAddAtiende").css({ "display": "none" });
            $("#btnIniciarVisita").css({ "display": "none" });

            $("#btnEnviarCertificado").css({ "display": "none" });
            $("#btnTerminarVisita").css({ "display": "none" });
            $("#btnPreview").css({ "display": "none" });

            $("#btnGuardar").text("Aceptar Visita");

            break;
        case 2://aceptado
            $("#ibtabAtiende").css({ "display": "block" });
            $("#idImdEncuesta").css({ "display": "block" });
            $("#btnAddAtiende").css({ "display": "block" });
            $("#btnIniciarVisita").css({ "display": "block" });

            $("#btnGuardar").text("Guardar");

            $("#btnEnviarCertificado").css({ "display": "none" });
            $("#btnTerminarVisita").css({ "display": "none" });
            $("#btnPreview").css({ "display": "none" });
            consultarDatosRealizarVisitas();
            break;
        case 3://en proceso

            $("#ibtabAtiende").css({ "display": "block" });
            $("#idImdEncuesta").css({ "display": "block" });
            $("#btnAddAtiende").css({ "display": "block" });
            $("#btnEnviarCertificado").css({ "display": "block" });

            $("#btnGuardar").text("Guardar");
            $("#btnIniciarVisita").css({ "display": "none" });
            $("#btnTerminarVisita").css({ "display": "none" });
            break;
        case 4://en revision

            $("#ibtabAtiende").css({ "display": "block" });
            $("#idImdEncuesta").css({ "display": "block" });
            $("#btnAddAtiende").css({ "display": "block" });
            $("#btnPreview").css({ "display": "block" });
            $("#btnTerminarVisita").css({ "display": "block" });
            $("#btnEnviarCertificado").css({ "display": "none" });
            $("#btnIniciarVisita").css({ "display": "none" });
            $("#btnGuardar").text("Guardar");



            break;

    }
}

function iniciarVisita() {
    $.ajax({
        type: "GET",
        url: '/ControlVigilancia/api/VisitasWebAPI/guardarIniciarVisita',
        data: {
            p_IdVisita: $("#IdVisita").val()
        },
        beforeSend: function () {

        },
        success: function (response) {
            var dato = response;
            if (dato == "1") {
                alert("almacenamiento exitoso");

                validarForm(3);
            } else {
                alert("Error");
            }
        },
    });

}

function CerrarVisita() {
    $.ajax({
        type: "GET",
        url: '/ControlVigilancia/api/VisitasWebAPI/CerrarVisita',
        data: {
            p_IdVisita: $("#IdVisita").val(), radicado: 1
        },
        beforeSend: function () {

        },
        success: function (response) {
            var dato = response;
            if (dato == "1") {
                alert("almacenamiento exitoso");

                validarForm(4);
            } else {
                alert("Error");
            }
        },
    });

}
function FinalizarVisita() {
    $.ajax({
        type: "GET",
        url: '/ControlVigilancia/api/VisitasWebAPI/FinalizarVisita',
        data: {
            p_IdVisita: $("#IdVisita").val(), radicado: 1, pdf: "//", comentario: "pruevas", responsable: 1, copias: "0"
        },
        beforeSend: function () {

        },
        success: function (response) {
            var dato = response;
            if (dato == "1") {
                alert("almacenamiento exitoso");

                validarForm(4);
                window.close();
                // window.open("/ControlVigilancia/Visitas/Visitas", "_self");
            } else {
                alert("Error");
            }
        },
    });

}
function abrirEnviarCertificado() {
    $("#pantallaEnviarCertificado").dialog(
    {

        width: 600,
        height: 400
    });
    $("#pdfEnviarCertificado").attr("src", "/ControlVigilancia/Visitas/EnviarCertificado");
}
function abrirPreviePDF() {
    $("#pantallaPreview").dialog(
    {

        width: 600,
        height: 400
    });
    $("#pdfPreview").attr("src", "/ControlVigilancia/Visitas/previePDF");


}