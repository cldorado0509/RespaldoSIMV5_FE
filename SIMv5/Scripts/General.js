var CodigoPreguntaSeleccionada;
var CodigoEncuestaSeleccionada;

function ValidarTexto(Caracter)
{
    var Resultado = true;
    var Expresion = new RegExp("^[a-zA-Z0-9 ]+$");
    var Tecla = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!Expresion.test(Tecla)) {
        Resultado =  false;
    }

    return Resultado;
}

function BuscarEncuestas(Control, Encuesta, Formulario, Responsable, Fecha) {
    //alert(RutaEncuestas + 'Encuestas/BuscarEncuestas?Nombre=' + Encuesta + '&Formulario=' + Formulario + '&Responsable=' + Responsable + '&FechaCreacion=' + Fecha);

    try {
        var source =
        {
            async: false,
            datatype: 'json',
            datafields: [
                { name: 'Codigo', type: 'int' },
                { name: 'NombreEncuesta', type: 'string' },
                { name: 'Descripcion', type: 'string' },
                { name: 'Responsable', type: 'string' },
                  { name: 'Formulario', type: 'string' },
                { name: 'FechaGrid', type: 'date' },
            ],
            id: 'Codigo',
            url: RutaEncuestas + 'Encuestas/BuscarEncuestas?Nombre=' + Encuesta + '&Formulario=' + Formulario + '&Responsable=' + Responsable + '&FechaCreacion=' + Fecha
        };

        var dataAdapter = new $.jqx.dataAdapter(source);

        dataAdapter.dataBind();

        $("#" + Control).jqxGrid(
                {
                    width: "98%",
                    height: 200,
                    source: dataAdapter,
                    theme: 'bootstrap',
                    columnsresize: true,
                    showdefaultloadelement: false,
                    columns: [
                        { text: 'Codigo', datafield: 'Codigo', width: '15%' },
                        { text: 'Nombre', datafield: 'NombreEncuesta', width: '15%' },
                        { text: 'Responsable', datafield: 'Responsable', width: '15%' },
                        { text: 'Formulario', datafield: 'Formulario', width: '15%' },
                        
                        { text: 'Fecha', datafield: 'FechaGrid', cellsformat: 'dd/MM/yyyy', width: '15%' },
                        {
                            text: 'Accion', width: '25%', cellsrenderer: function () {
                                return "<div id='divActions' valign='center' align='center'> \
                                                <img src='../../Content/Images/VerDetalle.png' style='width:25px;height:25px;  margin-right: 10px;' onclick='ClonarEncuesta()' class='btnGrid'/> \
                                                 <img src='../../Content/Images/edit.png' style='width:25px;height:25px' onclick='EditarEncuesta()'  class='btnGrid'/> \
                                        </div>";
                            }
                        }
                    ]
                });

       
       
        $("#" + Control).jqxGrid('clearselection');

        $("#" + Control).jqxGrid('hideloadelement');

        if (dataAdapter.records.length == 0) {
            alert('La consulto no obtuvo ningún resultado');
        }

        $("#" + Control).on('rowselect', function (event) {
            var Fila = event.args.rowindex;
            CodigoEncuestaSeleccionada = $("#" + Control).jqxGrid('getcelltext', Fila, "Codigo");
        });

    }
    catch (err)
    { alert(err); }
}

function BuscarPreguntas(Control, Pregunta, Encuesta, Responsable, Fecha) {
    try {
        var source =
        {
            async: false,
            datatype: 'json',
            datafields: [
                { name: 'Codigo', type: 'int' },
                { name: 'Nombre', type: 'string' },
                { name: 'NombreEncuesta', type: 'string' },
                { name: 'Responsable', type: 'string' },
                { name: 'FechaGrid', type: 'string' },

            ],
            id: 'Codigo',
            url: RutaEncuestas + 'Preguntas/BuscarPreguntasB?Pregunta=' + Pregunta + '&Encuesta=' + Encuesta + '&Responsable=' + Responsable + '&FechaCreacion=' + Fecha
        };

        var dataAdapter = new $.jqx.dataAdapter(source);

        $("#" + Control).jqxGrid(
                {
                    width: "98%",
                    height: 395,
                    source: dataAdapter,
                    theme: 'bootstrap',
                    columnsresize: true,
                    showdefaultloadelement: false,
                    columns: [
                        { text: 'Codigo', datafield: 'Codigo', width: '10%' },
                        { text: 'Pregunta', datafield: 'Nombre', width: '60%' },
                        { text: 'Encuesta', datafield: 'NombreEncuesta', width: '10%' },
                                 { text: 'Responsable', datafield: 'Responsable', width: '10%' },
                    { text: 'Fecha Creación', datafield: 'FechaGrid', width: '10%' },
                        {
                            text: 'Editar', width: '5%', cellsrenderer: function () {
                                return "<input onclick='EditarPregunta();' type='image' src='../../Content/Images/edit.png' width='20px' height='20px' title='Editar...' />";
                            }
                        },
                        {
                            text: 'Ver', width: '5%', cellsrenderer: function () {
                                return "<input onclick='EditarPregunta();' type='image' src='/Content/images/Ver.jpg' width='20px' height='20px' title='Ver...' />";
                            }
                        }
                    ]
                });

        $("#" + Control).jqxGrid('hideloadelement');

        $("#" + Control).on('rowselect', function (event) {
            var Fila = event.args.rowindex;
            CodigoPreguntaSeleccionada = $("#" + Control).jqxGrid('getcelltext', Fila, "Codigo");
        });

    }
    catch (err)
    { alert(err); }
}
function CargarPreguntasTodas(Control, Vincular, TipoEncuesta, CodigoEncuesta) {
    try {
        var source =
        {
            async: false,
            datatype: 'json',
            datafields: [
                { name: 'Codigo', type: 'int' },
                { name: 'Pregunta', type: 'string' },
                { name: 'Estado', type: 'string' },
                { name: 'Peso', type: 'short' },
                { name: 'Orden', type: 'short' },
                { name: 'Requerida', type: 'bool' },
                { name: 'Vincular', type: 'bool' },
            ],
            id: 'Codigo',
            url: RutaEncuestas + 'Preguntas/ObtenerPreguntastodas?Vincular=' + Vincular + '&TipoEncuesta=' + TipoEncuesta + '&CodigoEncuesta=' + CodigoEncuesta
        };

        var dataAdapter = new $.jqx.dataAdapter(source);

        if (Vincular == false)
        {
            $("#" + Control).jqxGrid(
                    {
                        width: "98%",
                        height: 250,
                        source: dataAdapter,
                        theme: 'bootstrap',
                        columnsresize: true,
                        showdefaultloadelement: false,
                        columns: [
                            { text: 'Codigo', datafield: 'Codigo', width: '10%' },
                            { text: 'Pregunta', datafield: 'Pregunta', width: '60%' },
                            { text: 'Estado', datafield: 'Estado', width: '10%' },
                            { text: 'Asociaciones', width: '10%' },
                            {
                                text: 'Editar', width: '5%', cellsrenderer: function () {
                                    return "<input onclick='EditarPregunta();' type='image' src='/Content/imagenes/Editar.png' width='20px' height='20px' title='Editar...' />";
                                }
                            },
                            {
                                text: 'Ver', width: '5%', cellsrenderer: function () {
                                    return "<input onclick='EditarPregunta();' type='image' src='/Content/imagenes/Ver.jpg' width='20px' height='20px' title='Ver...' />";
                                }
                            }
                        ]
                    });
        }
        else
        {
            $("#" + Control).on('bindingcomplete', function (event) {
                $("#" + Control).jqxGrid('setcolumnproperty', 'Codigo', 'editable', false);
                $("#" + Control).jqxGrid('setcolumnproperty', 'Pregunta', 'editable', false);
            });

            $("#" + Control).jqxGrid(
                    {
                        width: "98%",
                        height: 200,
                        source: dataAdapter,
                        theme: 'bootstrap',
                        selectionmode: 'multiplecellsadvanced',
                        columnsresize: true,
                        editable: true,
                        cellhover: function (element, pageX, pageY) {
                            X = pageX;
                            Y = pageY;
                        },
                        columns: [
                            { text: 'Vincular', datafield: 'Vincular', width: '10%', columntype: 'checkbox' },
                            { text: 'Codigo', datafield: 'Codigo', width: '10%' },                                                      
                            { text: 'Pregunta', datafield: 'Pregunta', width: '50%' },
                            {
                                text: 'Orden', datafield: 'Orden', width: '10%', columntype: 'numberinput',
                                    createeditor: function (row, cellvalue, editor, celltext, cellwidth, cellheight) {
                                        editor.jqxNumberInput({ decimalDigits: 0, digits: 3 });
                                    }
                            },
                            {
                                text: 'Peso', datafield: 'Peso', hidden: true, width: '10%'
                            },
                            { text: 'Requerida', datafield: 'Requerida', width: '10%', columntype: 'checkbox'},                            
                        ]
                    });

            try {
                $("#" + Control).on('cellbeginedit', function (event) {
                    $("#" + Control).jqxTooltip({
                        position: 'bottom',
                        theme: 'metrodark',
                        autoHide: false,
                        content: 'Presione Enter para guardar el valor.',
                        width: 150,
                        height: 10
                    });

                    $("#" + Control).jqxTooltip('open', X - 185, Y - 15);
                });
            }
            catch(err)
            {alert(err);}



            $("#" + Control).on('cellendedit', function (event) {
                $("#" + Control).jqxTooltip('close');
            });
        }



        $("#" + Control).jqxGrid('hideloadelement');
        

        $("#" + Control).on('rowselect', function (event) {
            var Fila = event.args.rowindex;
            CodigoPreguntaSeleccionada = $("#" + Control).jqxGrid('getcelltext', Fila, "Codigo");
        });

    }
    catch (err)
    { alert(err); }
}

function CargarEncuestasTodas(Control) {
    try {
        var source =
        {
            async: false,
            datatype: 'json',
            datafields: [
                { name: 'ID_ENCUESTA', type: 'int' },
                { name: 'S_NOMBRE', type: 'string' },
                { name: 'S_RESPONSABLE', type: 'string' },
                  { name: 'Formulario', type: 'string' },
                
                { name: 'Fecha', type: 'date' }
            ],
            id: 'ID_ENCUESTA',
            url: RutaEncuestas + 'Encuestas/ObtenerEncuestas'
        };

        var dataAdapter = new $.jqx.dataAdapter(source);

        $("#" + Control).jqxGrid(
        {
            width: "98%",
            height: 200,
            rowsheight: 40,
            source: dataAdapter,
            theme: 'bootstrap',
            columnsresize: true,
            showdefaultloadelement: false,
            columns: [
                { text: 'Codigo', datafield: 'ID_ENCUESTA', width: '15%' },
                { text: 'Encuesta', datafield: 'S_NOMBRE', width: '15%' },
                { text: 'Responsable', datafield: 'S_RESPONSABLE', width: '15%' },
                 { text: 'Formulario', datafield: 'Formulario', width: '15%' },
                { text: 'Fecha', datafield: 'Fecha', cellsformat: 'dd/MM/yyyy', width: '15%' },
                //{ text: 'Responsable', datafield: 'S_TIPO', width: 100 },
                //{ text: 'Estado', width: 200 },
                {
                    text: 'Accion', width: '25%', cellsrenderer: function () {
                        return "<div id='divActions' valign='center' align='center'> \
                                                       <img src='../../Content/Images/VerDetalle.png' style='width:25px;height:25px;  margin-right: 10px;' onclick='ClonarEncuesta()'/> \
                                                 <img src='../../Content/Images/edit.png' style='width:25px;height:25px' onclick='EditarEncuesta()'/> \
                                </div>";                            
                    }
                },
            ]
        });

        $("#" + Control).jqxGrid('clearselection');

        $("#" + Control).jqxGrid('hideloadelement');

        $("#" + Control).on('rowselect', function (event) {
            var Fila = event.args.rowindex;
            CodigoEncuestaSeleccionada = $("#" + Control).jqxGrid('getcelltext', Fila, "ID_ENCUESTA");
        });

    }
    catch (err)
    { alert(err); }
}

function ClonarEncuesta() {
    AbrirPopUp('Replicar Encuesta', RutaEncuestas + 'Encuestas/clonar/' + CodigoEncuestaSeleccionada, true, 500, 'Replicar');
}

function EditarEncuesta() {
    AbrirPopUp('Editar Encuesta', RutaEncuestas + 'Encuestas/edit/' + CodigoEncuestaSeleccionada, true, 500, 'Guardar Cambios');
}

function EditarPregunta()
{
    AbrirPopUp('Editar Pregunta', RutaEncuestas + 'preguntas/edit/' + CodigoPreguntaSeleccionada, true, 500, 'Guardar Cambios');
}

function CargarPreguntas(Control, CodigoEncuesta)
{    
    try {
        var source =
        {
            async: false,
            datatype: 'json',
            datafields: [
                { name: 'Codigo', type: 'int' },
                { name: 'Pregunta', type: 'string' },
                { name: 'Estado', type: 'string' },
                { name: 'Peso', type: 'short' },
                { name: 'Orden', type: 'short' },
                { name: 'Requerida', type: 'bool' },
                { name: 'Vincular', type: 'bool' },
            ],
            id: 'Codigo',
            url: RutaEncuestas + 'Preguntas/ObtenerPreguntasxEncuenta?CodigoEncuesta=' + CodigoEncuesta
        };

        var dataAdapter = new $.jqx.dataAdapter(source);

        $("#" + Control).jqxGrid(
        {
            width: "98%",
            height: 180,
            source: dataAdapter,
            theme: 'bootstrap',
            columnsresize: true,
            showdefaultloadelement: false,
            columns: [
                //{ text: 'Vincular', datafield: 'Vincular', width: 80, columntype: 'checkbox' },
                { text: 'Orden', datafield: 'Orden', width: 100 },
                { text: 'Codigo', datafield: 'Codigo', width: 100 },
                { text: 'Pregunta', datafield: 'Pregunta', width: 300 },
                { text: 'Peso', datafield: 'Peso', width: 100 },
                { text: 'Requerida', datafield: 'Requerida', width: 80, columntype: 'checkbox' },
            ]
        });

        $("#" + Control).jqxGrid('hideloadelement');
    }
    catch (err)
    { alert(err); }
}

function CargarRespuestasxPregunta(Control, CodigoPregunta)
{    
    try {
        var source =
        {
            async: false,
            datatype: 'json',
            datafields: [
                { name: 'Consecutivo', type: 'int' },
                { name: 'Orden', type: 'int' },
                { name: 'Codigo', type: 'string' },
                { name: 'Valor', type: 'string' },
            ],
            id: 'Codigo',
            url: RutaEncuestas + 'Respuestas/ObtenerRespuestasxPregunta?CodigoPregunta=' + CodigoPregunta
        };

        var dataAdapter = new $.jqx.dataAdapter(source);

        $("#" + Control).jqxGrid(
        {
            width: "98%",
            height: 220,
            source: dataAdapter,
            theme: 'bootstrap',
            columnsresize: true,
            showdefaultloadelement: false,
            columns: [
                { text: 'Codigo', datafield: 'Consecutivo', width: '10%' },
                { text: 'Orden', datafield: 'Orden', width: '10%' },
                { text: 'Valor', datafield: 'Codigo', width: '10%' },
                { text: 'Descripcion', datafield: 'Valor', width: '70%' }]
        });

        $("#" + Control).jqxGrid('hideloadelement');
    }
    catch (err)
    { alert(err); }
}

function VincularPreguntasEncuesta(CodigoEncuesta, Control)
{
    var Respuesta = 'Error';
    var Peso = 0;
    var Parametros = Array();
    var Filas = $('#' + Control).jqxGrid('getrows');

    for (var i = 0; i < Filas.length; i++) {
        var Fila = Filas[i];

        if (Fila.Vincular == true) {
            Peso += Fila.Peso;
            var Resultado = {
                "CodigoPregunta": Fila.Codigo,
                "Peso": Fila.Peso,
                "Orden": Fila.Orden,
                "Requerida": Fila.Requerida
            };

            Parametros.push(Resultado);
        }
    }    

    if (Peso <= 100) {
        $.ajax({
            dataType: 'json',
            async: false,
            type: "POST",
            url: RutaEncuestas + "Preguntas/VincularPreguntasEncuesta?CodigoEncuesta=" + CodigoEncuesta,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ CodigoEncuesta: CodigoEncuesta, PreguntasSeleccionadas: Parametros }),
            success: function (result) {
                var datos = result;
                if (datos == "Ok") {
                    Respuesta = datos;                    
                    window.parent.$('#popup').modal('hide');
                    window.parent.parent.$('#popup').modal('hide');
                    alert('Las preguntas han sido vinculadas con exito.');
                }
            }
        });

        return Respuesta;
    }
    else {
        alert('La suma de todos los pesos no puede ser mayor a 100');
    }
}

function CrearEncuesta(Nombre, Descripcion, Formulario, Tipo)
{    
    var Datos = "";
    $.ajax({
        dataType: 'json',
        async: false,
        type: "POST",
        url: RutaEncuestas + "Encuestas/CrearEncuesta?Nombre=" + Nombre + "&Descripcion=" + Descripcion + "&CodigoFormulario=" + Formulario + "&TipoEncuesta=" + Tipo ,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            Datos = result;
        }
    });

    return Datos;
}

function Enviar(UsarCallBack) {

    $('#popupboton').attr('disabled', 'disabled');

    if (typeof UsarCallBack === 'undefined')
    {
        $("#frmPopup").contents().find('form').submit();
    }
    else {       
        if (UsarCallBack == 'true')
        {
            try {                
                window.frames["frmPopup"].CallBackEnviar();
            }
            catch (err) {
                $("#frmPopup").contents().find('form').submit();
            }
        }
        else {
            $("#frmPopup").contents().find('form').submit();            
        }
    }    
}

function AbrirPopUp(Titulo, Url, MostrarGuardar, Alto, TextoBoton) {    
    $('#titulopopup').text(Titulo);
    $('#popup').modal({backdrop: 'static'},'show');
    $('#frmPopup').attr('src', Url);
    $('#popupAlto').attr('style', 'height:' + Alto + 'px')    
    $('#Rueda').show();
    $('#popupboton').removeAttr('disabled');

    if (typeof MostrarGuardar != 'undefined') {
        if (MostrarGuardar) {
            if (TextoBoton != 'undefined') {
                $('#popupboton').text(TextoBoton);
            }
            else {
                $('#popupboton').text('Crear');
            }

            $('#popupboton').show();
        }
        else {
            $('#popupboton').hide();
        }
    }
    else {
        $('#popupboton').hide();
    }    
}

function AgregarPopUp(Control, Ancho, CallBack, UsarCallBackEnviar) {
    if (CallBack == null) {
        CallBack = '';
    }
    else {
        CallBack = CallBack + ';';
    }

    if (Ancho == null) {
        Ancho = '70%';
    }

    var HtmlPopUp = '<div class="modal fade" id="popup" role="dialog" style="z-index: 10000;" aria-hidden="true"> \
                        <div class="modal-dialog" style="width:' + Ancho + ';"> \
                            <div class="modal-content"> \
                                <div class="modal-header" style="height:50px"> \
                                    <button type="button" class="close" data-dismiss="modal" onclick="$(\'#frmPopup\').attr(\'src\',\'about:blank\');' + CallBack + '" aria-hidden="true"> \
                                        &times; \
                                    </button> \
                                    <h4 class="modal-title" id="titulopopup"></h4> \
                                </div> \
                            <div class="modal-body" id="popupAlto" style="height:450px"> \
                                <iframe id="frmPopup" name="frmPopup" seamless="seamless" onload="$(\'#Rueda\').hide();" style="width:100%;height:100%;border:none"></iframe> \
                            </div> \
                            <div class="modal-footer"> \
                                <table style="width:100%"> \
                                <tr> \
                                    <td style="width:60%" align="left"> \
                                        <img id="Rueda" src="../Content/imagenes/ajax-loader.gif" /> \
                                    </td> \
                                    <td style="width:15%" align="right"> \
                                        <button type="button" id="popupboton" onclick="$(\'#Rueda\').show();Enviar(\'' + UsarCallBackEnviar + '\');" style="display:none" class="btn btn-primary btn-xs">Crear</button> \
                                    </td> \
                                    <td style="width:15%" align="right"> \
                                        <button type="button" class="btn btn-info btn-xs" onclick="$(\'#frmPopup\').attr(\'src\',\'about:blank\');' + CallBack + '" data-dismiss="modal">Cancelar</button> \
                                    </td> \
                                </tr> \
                                </table>   \
                            </div> \
                        </div> \
                    </div> \
                </div>';
    $('#' + Control).append(HtmlPopUp)
}

function ObtenerInfoBasicaEncuestas(CodigoFormulario, Control)
{
    $.ajax({
        dataType: 'json',
        async: false,
        type: "GET",
        url: RutaEncuestas + "ObtenerInfoBasicaEncuestas?CodigoFormulario=" + CodigoFormulario,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            var Contenido = "";
            for (var i = 0; i <= result.length -1; i++)
            {
                Contenido += '<input type="button" style="width:350px" class="btn btn-primary" id="Enc_' + result[i].Codigo + '" onclick="MostrarEncuesta(' + result[i].Codigo + ');" value="' + result[i].NombreEncuesta + '" /> <br /> <br />';
            }

            $("#" + Control).html(Contenido);
        }
    });
}

function ObtenerInformacionPreguntas(CodigoEncuesta, Control)
{
    $.ajax({
        dataType: 'json',
        async: false,
        type: "GET",
        url: RutaEncuestas + "Utilidades/ObtenerPreguntasEncuesta?CodigoEncuesta=" + CodigoEncuesta,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            var Contenido = "<table class='table table-condensed table-stripped' style='width:100%'>";
            for (var i = 0; i <= result.length - 1; i++) {
                Contenido += "<tr>";
                var Requerida = "";
                var Ayuda = "";

                var MarcaRequerido = "";
                if (result[i].Requerida) {
                    Requerida = "style='color:blue'";
                    MarcaRequerido = "pregunta-requerida='1'";
                }
                else {
                    Requerida = "style='color:black'";
                    MarcaRequerido = "pregunta-requerida='0'";
                }

                if (result[i].Ayuda != null) {
                    Ayuda = "<a tabindex='0' style='cursor:pointer;color:black;text-decoration:none' data-placement='bottom' class='glyphicon glyphicon-info-sign' data-toggle='popover' data-trigger='focus' data-original-title='Ayuda' data-content='" + result[i].Ayuda + "'/>&nbsp;";
                }

                Contenido += "<td " + Requerida + " style='width:50%'>" + Ayuda + result[i].Nombre + "</td>";
                switch (result[i].TipoPregunta)
                {
                    case 1://Binaria
                        {
                            Contenido += "<td style='width:50%'><select style='width:100%'" + MarcaRequerido + " pregunta-datos='" + result[i].Codigo + "|" + result[i].TipoPregunta + "' class='form-control'>";
                            for (var j = 0; j <= result[i].Respuestas.length - 1; j++) {
                                Contenido += "<option respuesta-codigo='" + result[i].Respuestas[j].Codigo + "' pregunta-codigo='" + result[i].Codigo + "' value=" + result[i].Respuestas[j].Valor + ">" + result[i].Respuestas[j].Descripcion + "</option>";
                            }
                            Contenido += "<option value='' selected='selected' ></option>";
                            Contenido += "</select>";
                            Contenido += "Observaciones &nbsp; <input id='chk_" + result[i].Codigo + "' type='checkbox' onclick='ActivarObservaciones(" + result[i].Codigo + ");'>"
                            Contenido += "<textarea id='obs_" + result[i].Codigo + "' pregunta-observaciones='" + result[i].Codigo + "' style='display:none;width:100%' class='form-control' rows='3'></textarea></td>"
                            break;
                        }
                    case 2://S. Simple
                        {
                            Contenido += "<td style='width:50%'><select style='width:100%'" + MarcaRequerido + " pregunta-datos='" + result[i].Codigo + "|" + result[i].TipoPregunta + "' class='form-control'>";
                            for (var j = 0; j <= result[i].Respuestas.length - 1; j++)
                            {
                                Contenido += "<option respuesta-codigo='" + result[i].Respuestas[j].Codigo + "' pregunta-codigo='" + result[i].Codigo + "' value=" + result[i].Respuestas[j].Valor + ">" + result[i].Respuestas[j].Descripcion + "</option>";
                            }
                            Contenido += "<option value='' selected='selected' ></option>";
                            Contenido += "</select>";
                            Contenido += "Observaciones &nbsp; <input id='chk_" + result[i].Codigo + "' type='checkbox' onclick='ActivarObservaciones(" + result[i].Codigo + ");'>"
                            Contenido += "<textarea id='obs_" + result[i].Codigo + "' pregunta-observaciones='" + result[i].Codigo + "' style='display:none;width:100%' class='form-control' rows='3'></textarea></td>"
                            break;
                        }
                    case 3://S. Multiple
                        {
                            Contenido += "<td style='width:50%'><div style='width:100%;height:100px;overflow:auto'>";
                            Contenido += "<table " + MarcaRequerido + " pregunta-datos='" + result[i].Codigo + "|" + result[i].TipoPregunta + "' style='width:100%' class='table table-condensed table-stripped'>";
                            for (var j = 0; j <= result[i].Respuestas.length - 1; j++) {
                                Contenido += "<tr><td style='width:80%'>" + result[i].Respuestas[j].Descripcion + "</td><td style='width:20%'><input valor-respuesta='" + result[i].Respuestas[j].Valor + "' codigo-respuesta='" + result[i].Respuestas[j].Consecutivo + "' type='checkbox' class='checkbox'/></td></tr><span>";
                            }
                            Contenido += "</table>";
                            Contenido += "</div>";
                            Contenido += "Observaciones &nbsp; <input id='chk_" + result[i].Codigo + "' type='checkbox' onclick='ActivarObservaciones(" + result[i].Codigo + ");'>"
                            Contenido += "<textarea id='obs_" + result[i].Codigo + "' pregunta-observaciones='" + result[i].Codigo + "' style='display:none;width:100%' class='form-control' rows='3'></textarea></td>"

                            break;
                        }
                    case 4://Numerica
                        {
                            Contenido += "<td style='width:50%'><input " + MarcaRequerido + " pregunta-datos='" + result[i].Codigo + "|" + result[i].TipoPregunta + "' type='number' class='form-control'/>";
                            Contenido += "Observaciones &nbsp; <input id='chk_" + result[i].Codigo + "' type='checkbox' onclick='ActivarObservaciones(" + result[i].Codigo + ");'>"
                            Contenido += "<textarea id='obs_" + result[i].Codigo + "' pregunta-observaciones='" + result[i].Codigo + "' style='display:none;width:100%' class='form-control' rows='3'></textarea></td>"

                            break;
                        }
                    case 5://Texto
                        {
                            Contenido += "<td style='width:50%'><textarea " + MarcaRequerido + " pregunta-datos='" + result[i].Codigo + "|" + result[i].TipoPregunta + "' rows='3' class='form-control'></textarea>";
                            Contenido += "Observaciones &nbsp; <input id='chk_" + result[i].Codigo + "' type='checkbox' onclick='ActivarObservaciones(" + result[i].Codigo + ");'>"
                            Contenido += "<textarea id='obs_" + result[i].Codigo + "' pregunta-observaciones='" + result[i].Codigo + "' style='display:none;width:100%' class='form-control' rows='3'></textarea></td>"

                            break;
                        }
                    case 6://Fecha
                        {
                            var Fecha = new Date();
                            var Dia = Fecha.getDate();
                            var Mes = Fecha.getMonth() + 1;
                            var Ano = Fecha.getFullYear();

                            if (Mes <= 9)
                            {
                                Mes = "0" + Mes;
                            }

                            if (Dia <= 9) {
                                Dia = "0" + Dia;
                            }

                            var FechaCompleta = Mes + '/' + Dia + '/' + Ano;
                            Contenido += "<td style='width:50%'><input " + MarcaRequerido + " pregunta-datos='" + result[i].Codigo + "|" + result[i].TipoPregunta + "' class='form-control' data-jqui-dpicker-changemonth='True' data-jqui-dpicker-changeyear='True' data-jqui-dpicker-dateformat='m/d/yy' data-jqui-type='datepicker' data-val='true' data-val-date='Debe ingresar una fecha valida.' readonly='readonly' type='text' value='" + FechaCompleta + "' />";
                            Contenido += "Observaciones &nbsp; <input id='chk_" + result[i].Codigo + "' type='checkbox' onclick='ActivarObservaciones(" + result[i].Codigo + ");'>"
                            Contenido += "<textarea id='obs_" + result[i].Codigo + "' pregunta-observaciones='" + result[i].Codigo + "' style='display:none;width:100%' class='form-control' rows='3'></textarea></td>"

                            break;
                        }
                }                
                Contenido += "</tr>";
            }
            Contenido += "</table>"
            $("#" + Control).html(Contenido);
            $('#Guardar').removeAttr('disabled');
        }
    });
    
}

function ProcesarEncuesta(CodigoEncuesta) {

    var Soluciones = Array();
    var Textos = $('input[pregunta-datos]');
    var Simple = $('select[pregunta-datos]');
    var Multiples = $('table[pregunta-datos]');
    var Abierta = $('textarea[pregunta-datos]');

    //Se procesan las preguntas numericas,fecha
    for (var i = 0; i <= Textos.length -1; i++ )
    {
        var Valor;
        var Info = $(Textos[i]).attr('pregunta-datos');
        if (Textos[i].type == 'checkbox') {
            if ($(Textos[i]).is(':checked')) {
                Valor = true;
            }
            else
            {
                Valor = false;
            }
        }
        else {
            Valor = $(Textos[i]).val();
        }

        var InfoSplit = Info.split("|");

        switch (Number(InfoSplit[1]))
        {
            case 4: //Numerica 
                {

                var Valida = ValidarValor(Textos[i]);
                if (Valida == false) {
                    return;
                }

                var Solucion = {
                    "CodigoPregunta": InfoSplit[0],
                    "ValorNumero": Valor
                }
                Soluciones.push(Solucion);
                break; 
            }
            case 6: //Fecha 
                {
                    var Valida = ValidarValor(Textos[i]);
                    if (Valida == false) {
                        return;
                    }

                var Solucion = {
                    "CodigoPregunta": InfoSplit[0],
                    "ValorFecha": Valor
                }
                Soluciones.push(Solucion);
                break;
            }
        }        
    }

    //Se procesan las preguntas simples  y binarias       
    for (var i = 0; i <= Simple.length - 1; i++) {
        var Info = $(Simple[i]).attr('pregunta-datos');
        var Texto = $(Simple[i]).find(':selected').text();
        var Valor = $(Simple[i]).find(':selected').val();
        var CodigoPregunta = $(Simple[i]).find(':selected').attr('pregunta-codigo');
        var CodigoRespuesta = $(Simple[i]).find(':selected').attr('respuesta-codigo');

        var Valida = ValidarValor(Simple[i]);
        if (Valida == false) {
            return;
        }

        var InfoSplit = Info.split("|");

        if (Texto != '' && Valor != '')
        {
            var Solucion = {
                "CodigoPregunta": CodigoPregunta,
                "CodigoRespuesta": CodigoRespuesta,
                "ValorTexto": Texto,
                "ValorNumero": Valor
            }

            Soluciones.push(Solucion);
        }
    }

    //Se procesan las preguntas multiples
    for (var i = 0; i <= Multiples.length - 1; i++) {
        var Info = $(Multiples[i]).attr('pregunta-datos');
        var SolucionesTmp = $(Multiples[i]).find('input:checkbox');

        //Se valida que sea requerida.
        if ($(Multiples[i]).attr('pregunta-requerida') == '1') {
            var Seleccionados = 0;
            for (var x = 0; x <= SolucionesTmp.length - 1; x++) {
                if ($(SolucionesTmp[x]).is(':checked'))
                {
                    Seleccionados++;
                }
            }

            if (Seleccionados == 0)
            {
                alert('El valor es requerido');
                try {
                    $(SolucionesTmp[0]).focus();
                }
                catch(err){}                
                return;
            }
        }

        var InfoSplit = Info.split("|");

        for (var x = 0; x <= SolucionesTmp.length - 1; x++)
        {
            if ($(SolucionesTmp[x]).is(':checked'))
            {
                var Solucion = {
                    "CodigoPregunta": InfoSplit[0],
                    "CodigoRespuesta": $(SolucionesTmp[x]).attr('codigo-respuesta'),
                    "ValorTexto": $(SolucionesTmp[x]).attr('valor-respuesta')
                }

                Soluciones.push(Solucion);
            }
        }
    }
    
    //Se procesan las preguntas texto
    for (var i = 0; i <= Abierta.length - 1; i++) {
        var Info = $(Abierta[i]).attr('pregunta-datos');
        var Valor = $(Abierta[i]).val();

        var Valida = ValidarValor(Abierta[i]);
        if (Valida == false)
        {
            return;
        }

        var InfoSplit = Info.split("|");

        var Solucion = {
            "CodigoPregunta": InfoSplit[0],
            "ValorTexto": Valor
        }

        Soluciones.push(Solucion);
    }

    //Se envian las soluciones la servidor para ser almacenadas
    $.ajax({
        dataType: 'json',
        async: false,
        type: "POST",
        url: RutaEncuestas + "Utilidades/GuardarSolucionEncuesta?CodigoEncuesta=" + CodigoEncuesta + "&CodigoObjeto=1",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ CodigoEncuesta: CodigoEncuesta, CodigoObjeto: 1, Solucion: Soluciones }),
        success: function (result) {
            if (result == true) {
                alert('Los datos de la encuesta han sido almacenados.')
                document.location.href = 'ListarEncuestas';
            }
        }
    });
}

function ValidarValor(Control)
{
    if ($(Control).attr('pregunta-requerida') == '1') {
        if ($(Control).val() == '') {
            alert('El valor es requerido');
            $(Control).focus();
            return false;
        }
        else {
            return true;
        }
    }
}

function MostrarEncuesta(Codigo) {
    document.location.href = "GenerarEncuesta?Codigo=" + Codigo;
}