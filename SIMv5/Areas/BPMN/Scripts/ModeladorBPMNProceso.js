
$(document).ready(function () {

    //DevExpress.ui.dialog.alert('aqui va la nueva pagina', 'Pendiente');
    var resultadoInfo = null
    

var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
    return false;
};


var ProcesosDataSource = function () {
        var d = $.Deferred();
    var _Ruta = "https://sim.metropol.gov.co/BPMN/api/Procesos/ObtenerUnProceso?idProceso=" + getUrlParameter("procesoid")
        //$.getJSON(_Ruta, { ID: options.data.ID })

    $.getJSON(_Ruta).done(function (data) {
        console.log(data[0])
            d.resolve(data, { totalCount: data.length });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d;
    }

    ProcesosDataSource().then(function (returndata) {
        resultadoInfo = returndata[0]
        controlEstados()

        console.log(returndata[0].modeloBPMN)

        if (returndata[0].modeloBPMN != "") {
            openDiagram(returndata[0].modeloBPMN)

        } else {
            openDiagram(`
                    <?xml version="1.0" encoding="UTF-8"?>
                    <definitions xmlns="http://www.omg.org/spec/BPMN/20100524/MODEL" xmlns:bpmndi="http://www.omg.org/spec/BPMN/20100524/DI" xmlns:omgdc="http://www.omg.org/spec/DD/20100524/DC" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" targetNamespace="" xsi:schemaLocation="http://www.omg.org/spec/BPMN/20100524/MODEL http://www.omg.org/spec/BPMN/2.0/20100501/BPMN20.xsd">
                      <process id="Process_12rftkk" />
                      <bpmndi:BPMNDiagram id="sid-74620812-92c4-44e5-949c-aa47393d3830">
                        <bpmndi:BPMNPlane id="sid-cdcae759-2af7-4a6d-bd02-53f3352a731d" bpmnElement="Process_12rftkk" />
                        <bpmndi:BPMNLabelStyle id="sid-e0502d32-f8d1-41cf-9c4a-cbb49fecf581">
                          <omgdc:Font name="Arial" size="11" isBold="false" isItalic="false" isUnderline="false" isStrikeThrough="false" />
                        </bpmndi:BPMNLabelStyle>
                        <bpmndi:BPMNLabelStyle id="sid-84cb49fd-2f7c-44fb-8950-83c3fa153d3b">
                          <omgdc:Font name="Arial" size="12" isBold="false" isItalic="false" isUnderline="false" isStrikeThrough="false" />
                        </bpmndi:BPMNLabelStyle>
                      </bpmndi:BPMNDiagram>
                    </definitions>`)
        }

        $('#content').html(returndata[0].nombreProceso + " | " + returndata[0].descripcion + " | " + returndata[0].nombreEstado + "<br> Version: " + returndata[0].versionNro  );
        
    });

    
    $("#btnGuardar").dxButton({
        stylingMode: "contained",
        text: "Guardar Diseno",
        type: "success",
        width: 200,
        height: 30,
        icon: '../Content/Images/new.png',
    });

    $("#btnRegresarEstado").dxButton({
        stylingMode: "contained",
        text: "Estado Anterior",
        type: "success",
        width: 200,
        height: 30,
        icon: '../Content/Images/new.png',
        onClick: function () {
            var params = { FuncionarioId: CodigoFuncionario, ProcesoVersionId: resultadoInfo.procesoVersionId, ProcesoEstadoVersionId: resultadoInfo.procesoEstadoVersionId, EstadoVersionId: resultadoInfo.estadoVersionId, Diagrama: '' };
            var _Ruta = "https://sim.metropol.gov.co/BPMN/api/Procesos/DevolverEstadoVersion"
            console.log(params)
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Guardar Datos');
                    else {
                        resultadoInfo = data[0]
                        controlEstados()
                        $('#content').html(data[0].nombreProceso + " | " + data[0].descripcion + " | " + data[0].nombreEstado + "<br> Version: " + data[0].versionNro);
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            })
        }
    });
    //visible: resultadoInfo.acualizaVersion == "N",
    $("#btnCambiarEstado").dxButton({
        stylingMode: "contained",
        text: "Siguiente Estado",
        type: "success",
        width: 200,
        height: 30,
        icon: '../Content/Images/new.png',
        onClick: function () {
            var params = { FuncionarioId: CodigoFuncionario, ProcesoVersionId: resultadoInfo.procesoVersionId, ProcesoEstadoVersionId: resultadoInfo.procesoEstadoVersionId, EstadoVersionId: resultadoInfo.estadoVersionId, Diagrama:'' };
            var _Ruta = "https://sim.metropol.gov.co/BPMN/api/Procesos/CambiarEstadoVersion"
            console.log(params)
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Guardar Datos');
                    else {
                        resultadoInfo = data[0]
                        controlEstados()
                        $('#content').html(data[0].nombreProceso + " | " + data[0].descripcion + " | " + data[0].nombreEstado + "<br> Version: " + data[0].versionNro);
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            })
        }
    });

    function controlEstados() {
        $("#btnCambiarEstado").css("visibility", resultadoInfo.acualizaVersion == "N" ? "visible" : "hidden");
        $("#btnRegresarEstado").css("visibility", resultadoInfo.acualizaVersion == "N" ? "visible" : "hidden");
        $("#btnGuardar").css("visibility", resultadoInfo.acualizaVersion == "N" ? "visible" : "hidden");
        $("#controlProcesos").css("visibility", resultadoInfo.acualizaVersion == "N" ? "visible" : "hidden");
    }





    // modeler instance
    var bpmnModeler = new BpmnJS({
        container: '#canvas',
        keyboard: {
            bindTo: window
        }
    });

    /**
     * Save diagram contents and print them to the console.
     */
    async function exportDiagram() {

        try {

            var result = await bpmnModeler.saveXML({ format: true });

            console.log(getUrlParameter("procesoestadoid"), result.xml);


            var params = { FuncionarioId: CodigoFuncionario, ProcesoEstadoVersionId: getUrlParameter("procesoestadoid"), Diagrama: result.xml, Nombre: '', Habilitado: '1', Descripcion: '' };
            var _Ruta = "https://sim.metropol.gov.co/BPMN/api/Procesos/ActualizarModelo"
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Guardar Datos');
                    else {
                        DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');

                        //cargamos la nueva pagina para el modelador nuevo del proceso creado
                        // < type="text/javascript" src="@Url.Content("../Areas/BPMN/Scripts/ModeladorBPMNProceso.js")"
                        //window.location.replace("/BPMN/ProcesosxArea/Index");

                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            })

        } catch (err) {

            console.error('could not save BPMN 2.0 diagram', err);
        }
    }

    /**
     * Open diagram in our modeler instance.
     *
     * param {String} bpmnXML diagram to display
     */
    async function openDiagram(bpmnXML) {

        // import diagram
        try {

            await bpmnModeler.importXML(bpmnXML);

            // access modeler components
            var canvas = bpmnModeler.get('canvas');
            var overlays = bpmnModeler.get('overlays');


            // zoom to fit full viewport
            canvas.zoom('fit-viewport');

            // attach an overlay to a node
            overlays.add('SCAN_OK', 'note', {
                position: {
                    bottom: 0,
                    right: 0
                },
                html: '<div class="diagram-note">Mixed up the labels?</div>'
            });

            // add marker
            canvas.addMarker('SCAN_OK', 'needs-discussion');
        } catch (err) {

            console.error('could not import BPMN 2.0 diagram', err);
        }
    }
 

    // load external diagram file via AJAX and open it
    //$.get(diagramUrl, openDiagram, 'text');

    // wire save button
    $('#btnGuardar').click(exportDiagram);

});
