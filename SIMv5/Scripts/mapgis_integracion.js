function MapGIS(contenedor) {

var mapgis = null;
var contenedor = contenedor;

this.refreshMap = function () {
    mapgis.refreshMap()
}

this.refreshLayer = function (nombreLayer) {
    mapgis.refreshLayer(nombreLayer)
}

this.loginMapGIS = function(usuario,password){
	var login = document.getElementById(contenedor).contentWindow;
	if(login.testMapGIS()==1){
		login.ingresarAuto(usuario,password);
		return 1;
	}else{
		return -1;
	}
}

this.initMapGIS = function (flistener) {
	mapgis = document.getElementById(contenedor).contentWindow;
	if (mapgis.map != null) {
	    return mapgis.testMapGIS(flistener);
	}else{
		return -1;
	}
}

this.conectMapGIS = function() {
	mapgis = parent;
	if(mapgis.map != null){
		return mapgis.testMapGIS();
	}else{
		return -1;
	}
}

this.zoomPto = function(x,y,srid,level){
	 mapgis.zoomPto(x,y,srid,level);
}

this.zoomPtoRadio = function(x,y,srid,radio){
	  mapgis.zoomPtoRadio(x,y,srid,radio);
}

this.geocod = function(dir,zona,callback,srid){
    mapgis.geocodX(dir, zona, callback,srid);
	
}

this.zoomCoor = function(xmin,xmax,ymin,ymax,srid){
	mapgis.zoomCoor(xmin,xmax,ymin,ymax,srid);
}

this.zoomExtend = function(extend){
	mapgis.zoomExtend(extend);
}

this.zoomScale = function(escala){
	mapgis.zoomScale(escala);
}

this.drawPoint = function(frespuesta,r,g,b,nombre,srid){
	mapgis.drawPoint(frespuesta,r,g,b,nombre,srid);
}

this.drawRectangulo = function(frespuesta,r,g,b,nombre,srid){
	mapgis.drawRectangulo(frespuesta,r,g,b,nombre,srid);
}

this.drawPolyline = function(frespuesta,r,g,b,nombre,srid){
	mapgis.drawPolyline(frespuesta,r,g,b,nombre,srid);
}

this.drawPolygon = function(frespuesta,r,g,b,nombre,srid){
	mapgis.drawPolygon(frespuesta,r,g,b,nombre,srid);
}

this.drawCircle = function(frespuesta,r,g,b,nombre,srid){
    mapgis.drawCircle(frespuesta, r, g, b, nombre, srid);
}

//this.drawPoint = function (frespuesta, r, g, b, nombre, srid) {
//    mapgis.drawPoint(frespuesta, r, g, b, nombre, srid);
//}


this.noDraw = function(){
	mapgis.noDraw();
} 

this.limpiarGraphics = function() {
    mapgis.limpiarGraphics(null);
}
    
this.deleteGraphics = function(nombre) {
	mapgis.deleteGraphics(nombre)
}
    
this.pintarRombo4 = function(x,y,srid,colores,labels,size,titulos,formato,offsetX,offsetY){
	mapgis.pintarRombo4(x,y,srid,colores,labels,size,titulos,formato,offsetX,offsetY);
}

this.getSrid = function(){
  		return mapgis.getSrid();
}

this.verPoint = function(x,y,srid,r,g,b,nombre,atributos, titulo,formato){
	mapgis.verPoint(x,y,srid,r,g,b,nombre,atributos, titulo,formato);
}

this.verCircle = function(x,y,dist,srid,r,g,b,nombre,atributos, titulo,formato){
	mapgis.verCircle(x,y,dist,srid,r,g,b,nombre,atributos, titulo,formato);
}

this.verExtend = function(x,y,delta,srid,r,g,b,nombre,atributos, titulo,formato){
	mapgis.verExtend(x,y,delta,srid,r,g,b,nombre,atributos, titulo,formato)
}

this.estadoMapa = function(idMapa){
	return mapgis.estadoMapa(idMapa)
}

this.loadMapa = function(idMapa,estado){
	var mapa = mapgis.findMap(idMapa);
	mapa.checked = estado;
	mapgis.loadMap(mapa);
}

this.exportMap = function(ancho,alto,callback){
	mapgis.exportMap(ancho,alto,callback);
}

this.printMap = function(format,layout,title,author,scalebarUnit,ancho,alto,callback){
	mapgis.printMap(format,layout,title,author,scalebarUnit,ancho,alto,callback);
}

this.execQuery = function(capa,sql,returnGeometry,callback,failed,geom){
	mapgis.execQuery(capa,sql,returnGeometry,callback,failed,geom);
}

this.selectObjectId = function(capa,objectId,tipo,nombre,callback,failed){
	var sql = "ObjectId = " + objectId;
	mapgis.execQuery(capa,sql,true,function(result){
		for (f in result.features){
			var symbol = mapgis.getSymbol(tipo,0,255,0);
			var g =  result.features[f];
			 g.symbol = symbol;
			mapgis.addGraphics(nombre,g);
			mapgis.zoomExtend(result.features[f].geometry._extent);
		}
	},failed,null);
}

this.showform = function(id){
	for(var f in objFormularios){
		if(objFormularios[f].id == id){
			var ff = objFormularios[f];
			mapgis.showform(ff.id,ff.titulo,ff.url,ff.l,ff.t,ff.w,ff.h,ff.bits,ff.objs,ff.anclar);
		}
	}
}

this.closeform = function(id){
	mapgis.dijit.byId("div" + id).close();
}

this.swServicio = function(strServicio,isVisible){
	mapgis.swServicio(strServicio,isVisible);
}

this.showformExt = function(id,titulo,url,l,t,w,h,bits,objs,anclar){
	mapgis.showform(id,titulo,url,l,t,w,h,bits,objs,anclar);
}

this.buffer = function(geom,dist,unir,callback,pintar){
	mapgis.buffer(geom,dist,unir,callback,pintar);
}

this.customIdentify =  function(url,geom,capas,tolerancia,returnGeometry,callback){
	mapgis.customIdentify(url,geom,capas,tolerancia,returnGeometry,callback);
}

this.customFind = function(servicio,searchText,layerId,campos,callback){
	mapgis.customFind(servicio,searchText,layerId,campos,callback);
}

//Funciones integracion Sirena
//Variables de configuraci�n
	var urlServicio = "/arcgis/rest/services/Integracion_Sirena_Pub/MapServer";
	var idCapaDTM = 9;
	var idCapaMpio = 8;
	var idCapaCorregimientos = 6;
	var idCapaVereda = 7;
	var idCapaCuenca = 5;
	var idCapaDrenaje = 1;	
	var idCapaCatastro = 2;
	
	
this.localiza = function (data, success, error) 
{
	var ma = this;
	if(data.tipoGeometria == "punto"){
		this.drawPoint(function(g,g1){
			ma.rtaLocaliza(g,g1,data,success, error);
		},255,0,0,"localiza",data.srid);
	}else if(data.tipoGeometria == "linea"){
		this.drawPolyline(function(g,g1){
			ma.rtaLocaliza(g,g1,data,success, error);
		},255,0,0,"localiza",data.srid);
	}else if(data.tipoGeometria == "poligono"){
		this.drawPolygon(function(g,g1){
			ma.rtaLocaliza(g,g1,data,success, error);
		},255,0,0,"localiza",data.srid);
	}
}

this.rtaLocaliza = function(geom,geom1,data, success, error){
	var ma = this;
	if(data.nivelPrecision > 0){
		this.buffer(geom, data.nivelPrecision, true, function(g){
			ma.realizarConsultas(g,geom1,data, success, error)
		}, true)
	}else{
		ma.realizarConsultas(geom,geom1,data, success, error)
	}
}

this.realizarConsultas = function(geom,geom1,data, success, error){
	var capas = new Array();
	capas.push(idCapaMpio);
	capas.push(idCapaCorregimientos);
	capas.push(idCapaVereda);
	if(data.hidrico){
		capas.push(idCapaCuenca);
		capas.push(idCapaDrenaje);	
	}
	if(data.catastro){
		capas.push(idCapaCatastro);
	}
	var tolerancia = 1;
	if(data.tipoGeometria == "punto" && data.nivelPrecision == 0){
		tolerancia = 4;
	}
	this.customIdentify(urlServicio,geom1,capas,tolerancia,false,function(result){
		var resCapa = new Object();
		resCapa.name = "";
		var respuesta = new Object();
		c = new Array();
		respuesta.coordenadas = new Object();
		respuesta.coordenadas.x = geom.x;
		respuesta.coordenadas.y = geom.y;
		respuesta.coordenadas.z = 0;
		respuesta.coordenadas.srid = data.srid;
		respuesta.municipios = new Array();
		respuesta.hidrica = new Object();
		respuesta.hidrica.idSegmento = new Array();
		respuesta.hidrica.cuenca = new Array();
		respuesta.catastro = new Object();
		respuesta.id = 2;
 
	    	for (var i=0; i<result.length; i++) {
		    var idResult = result[i];
		    if(idResult.layerId == idCapaMpio){
			idResult.feature.attributes.corregimientos = new Array();
			idResult.feature.attributes.veredas = new Array();
	        	respuesta.municipios.push(idResult.feature.attributes);
		    }else if(idResult.layerId == idCapaCorregimientos ){
			for(var m in respuesta.municipios){
				if(idResult.feature.attributes.COD_MUNICIPIO == respuesta.municipios[m].COD_MUNICIPIO){
					idResult.feature.attributes.veredas = new Array();
					respuesta.municipios[m].corregimientos.push(idResult.feature.attributes);
					break;
				}
			}
		    }else if(idResult.layerId == idCapaVereda ){
			for(var m in respuesta.municipios){
				if(idResult.feature.attributes.COD_MUNICIPIO == respuesta.municipios[m].COD_MUNICIPIO){
					var encontroCorregimiento = false;
					for(var c in respuesta.municipios[m].corregimientos){
						if(idResult.feature.attributes.COD_CORREGIMIENTO == respuesta.municipios[m].corregimientos[c].COD_CORREGIMIENTO){
							encontroCorregimiento = true;
							respuesta.municipios[m].corregimientos[c].veredas.push(idResult.feature.attributes);
							break;
						}
					}
					if(!encontroCorregimiento){
						respuesta.municipios[m].veredas.push(idResult.feature.attributes);
						break;
					}
					break;
				}
			}
		    }else if(idResult.layerId == idCapaCuenca ){
	        	respuesta.hidrica.cuenca.push(idResult.feature.attributes);
		    }else if(idResult.layerId == idCapaDTM ){
		    	respuesta.coordenadas.z = Number(idResult.feature.attributes["Pixel Value"]);
		    }else if(idResult.layerId == idCapaDrenaje ){
	        	respuesta.hidrica.idSegmento.push(idResult.feature.attributes);
	            }
	        }
                 
	        success(respuesta);		
	});
}
}


