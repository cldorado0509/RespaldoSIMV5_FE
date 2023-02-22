

var m;
var valores;
var valoresGEO;
var valoresCopia;
var valoresResponsable;
var usuario = "135";
var srid = 4326;
var nzoom = 14;
var tipoUbicacionM = "Manual";
var tipoUbicacionG = "Geocodificacion";
var capaLayerTramite = "Tramite";
var capaLayerVisita = "Visita";

function listener(nombre, parametros) {
    parametros = parametros.replace("$us$", usuario);
    eval(nombre + '(' + parametros + ' )')
}

function Refrescar(x, y) {

    conectar();

    //m.limpiarGraphics();
    m.noDraw();
    m.refreshLayer("Tramites_en reparto")
   
    //m.zoomScale(4622325);
}


    function Limpiar()
{
        try {
            conectar();
            m.limpiarGraphics();
        } catch (e) {

        }
    }

function conectar() {
    //cbpExample.PerformCallback();
    m = new MapGIS("mapa");
    var rta = m.initMapGIS(listener);
    return rta;
}

function zoom(x, y) {
    conectar();
    //x = x.replace(",", ".");
  //  y = y.replace(",", ".");
    m.zoomPto(x, y, srid, nzoom);
}

function pto() {
    conectar();
    m.drawPoint(function (g, g1) {
        //alert(g.x + "," + g.y)
        m.noDraw();
    }, 255, 0, 0, "ubica", srid);
}

function OnBeginCallback(s, e) {
    e.customArgs["xc"] = 1;
}
function OnEndCallback(s, e) {

}