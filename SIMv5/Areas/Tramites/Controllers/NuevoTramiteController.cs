//using DevExpress.Pdf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using SIM.Areas.ControlVigilancia.Controllers;
using SIM.Areas.Seguridad.Models;
using SIM.Areas.Tramites.Models;
using SIM.Data;
using SIM.Data.Control;
using SIM.Data.Tramites;
using SIM.Models;
using SIM.Utilidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SIM.Areas.Tramites.Controllers
{
    public class NuevoTramiteController : Controller
    {
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        EntitiesSIMOracle db = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();
        EntitiesTramitesOracle dbTramites = new EntitiesTramitesOracle();

        Int32 idUsuario;
        decimal codFuncionario;

        public ActionResult Index(int? idTramite, int? idInstalacion, int? idTercero)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            }

            if (idTramite != null && idInstalacion != null && idTercero != null)
            {

                ViewBag.IdTercero = idTercero;
                ViewBag.IdInstalacion = idInstalacion;
                ViewBag.IdTramite = idTramite;
            }
            else
            {
                idInstalacion = 17763;
                var tct = db.TERMINOSCONDICIONES_TRAMITE.Where(t => t.ID_TERCERO == idUsuario && t.ID_INSTALACION == idInstalacion).FirstOrDefault();

                if (tct == null)
                {
                    ViewBag.IdTercero = idUsuario;
                    ViewBag.IdInstalacion = 0;
                    ViewBag.IdTramite = 0;
                }
                else
                {
                    ViewBag.IdTercero = idUsuario;
                    ViewBag.IdInstalacion = tct.ID_INSTALACION;
                    ViewBag.IdTramite = tct.ID_TRAMITE;
                }
            }

            return View();

        }

        public ActionResult NuevoTramite(int id)
        {
            return View();
        }

        //CARACTERISTICAS
        public class CoordenadasDir
        {
            decimal x { get; set; }
            decimal y { get; set; }
        }

        public decimal TruncateDecimal(decimal value, int precision)
        {
            decimal step = (decimal)Math.Pow(10, precision);
            decimal tmp = Math.Truncate(step * value);
            return tmp / step;

        }

        public ActionResult consultarJsonDetalle(int idTramite, int? idTramiteCreado, int idInstalacion, int idTercero)
        {
            try
            {

                idTramiteCreado = 0;
                int idFormu = 0;
                var infoFormularioxTramite = db.QRY_LISTADOTRAMITES.Where(f => f.ID_TRAMITE == idTramite).FirstOrDefault();
                ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));

                //
                idFormu = infoFormularioxTramite.ID_FORMULARIO;
                //idFormu = 7;

                //Me traigo el item de la tabla FormularioItem
                var formulario1 = db.FORMULARIO.Where(s => s.ID_FORMULARIO == idFormu).FirstOrDefault();
                string sql10 = "SELECT NVL( MAX (ID_ITEM), 0) Variable_2 FROM CONTROL.FORMULARIO_ITEM WHERE ID_FORMULARIO = " + formulario1.ID_FORMULARIO + " AND ID_INSTALACION  = " + idInstalacion + " AND ID_TERCERO = " + idTercero;
                dbControl.SP_GET_DATOS(sql10, jSONOUT);
                String Tbl_Estado = formulario1.TBL_ESTADOS;

                //Quito lo q hay antes del punto del nombre de la tabla para poder hacer la consulta posteriormente
                //String[] arraynomTabla = Tbl_Estado.ToString().Split('.');
                //string nomTablaEstado = arraynomTabla[1];

                var formularioItem = jSONOUT.Value.ToString();

                //valido si es la primera vez o si ya hay información creada, si ya se inició el tramite           
                DatosTramite formItem = new DatosTramite();
                formItem = JsonConvert.DeserializeObject<DatosTramite>(formularioItem.Substring(1, formularioItem.Length - 2));

                jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                string sql11 = "SELECT ID_VISITA AS Variable_2 FROM CONTROL.ITEM_VISITA WHERE ID_ITEM = " + formItem.Variable_2 + " AND ID_TRAMITE  = " + idTramite;
                dbControl.SP_GET_DATOS(sql10, jSONOUT);

                var idVisitaItem = jSONOUT.Value.ToString();

                //valido si es la primera vez o si ya hay información creada, si ya se inició el tramite           
                DatosTramite formVisista = new DatosTramite();
                formVisista = JsonConvert.DeserializeObject<DatosTramite>(idVisitaItem.Substring(1, idVisitaItem.Length - 2));

                int IDVIsita = formVisista.Variable_2;


                //Quito lo q hay antes del punto del nombre de la tabla para poder hacer la consulta posteriormente
                String[] arraynomTabla = Tbl_Estado.ToString().Split('.');
                string nomTablaEstado = arraynomTabla[1];

                //RIESGOS QUIMICOS
                if (idFormu == 8)
                {
                    if (formItem.Variable_2 > 0)
                    {
                        return Json("");
                    }
                    else
                    {

                        return Json("");

                    }
                }

                //INDUSTRIAS FORESTALES  // CDA // FUENTES FIJAS 
                else if (idFormu == 1 || idFormu == 10 || idFormu == 9)
                {
                    if (formItem.Variable_2 > 0)
                    {
                        string sqlPK = "SELECT cols.column_name AS Variable_1" +
                      " FROM all_constraints cons, all_cons_columns cols WHERE cols.table_name = '" + nomTablaEstado +
                      "' AND cons.constraint_type = 'P' " +
                      " AND cons.constraint_name = cols.constraint_name" +
                      " AND cons.owner = cols.owner";
                        dbControl.SP_GET_DATOS(sqlPK, jSONOUT);
                        var idnomcampo = jSONOUT.Value.ToString();

                        DatosTramite modItem = new DatosTramite();
                        modItem = JsonConvert.DeserializeObject<DatosTramite>(idnomcampo.Substring(1, idnomcampo.Length - 2));
                        string nombre_Item = modItem.Variable_1.ToString();


                        //Busco en la tabla formulario 
                        jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                        string sql = "SELECT " + nombre_Item + " AS Variable_2 FROM " + Tbl_Estado + " WHERE " + formulario1.S_CAMPO_ID_ITEM + " = " + formItem.Variable_2;
                        dbControl.SP_GET_DATOS(sql, jSONOUT);
                        var Item1 = jSONOUT.Value.ToString();

                        DatosTramite modItem2 = new DatosTramite();
                        modItem2 = JsonConvert.DeserializeObject<DatosTramite>(Item1.Substring(1, Item1.Length - 2));

                        int idItem = modItem2.Variable_2;

                        //Obtengo el Id de la visita
                        jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                        string sql2 = "SELECT ID_VISITA AS Variable_2 FROM ITEM_VISITA WHERE ID_ITEM = " + idItem;
                        dbControl.SP_GET_DATOS(sql2, jSONOUT);
                        var visita = jSONOUT.Value.ToString();

                        DatosTramite modIDvisita = new DatosTramite();
                        modIDvisita = JsonConvert.DeserializeObject<DatosTramite>(visita.Substring(1, visita.Length - 2));

                        int idVisita = modIDvisita.Variable_2;

                        jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                        dbControl.SP_GET_CARACTERISTICAS(idItem, Tbl_Estado, formulario1.ID_FORMULARIO, 0, 0, jSONOUT);
                        return Json(jSONOUT.Value + "|Detalle|" + idVisita);
                    }
                    else
                    {
                        jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                        dbControl.SP_GET_CARACTERISTICAS_TL(0, Tbl_Estado, formulario1.ID_FORMULARIO, 0, 0, jSONOUT);
                        return Json(jSONOUT.Value + "|Detalle");
                    }
                }
                //OCUPACION DE CAUSE // PROYECTOS CONSTRUCTIVOS
                else if (idFormu == 12 || idFormu == 7)
                {
                    if (formItem.Variable_2 > 0)
                    {
                        //Consulto cual es el campo de llave primaria de la tabla
                        string sqlPK = "SELECT cols.column_name AS Variable_1" +
                        " FROM all_constraints cons, all_cons_columns cols WHERE cols.table_name = '" + nomTablaEstado +
                        "' AND cons.constraint_type = 'P' " +
                        " AND cons.constraint_name = cols.constraint_name" +
                        " AND cons.owner = cols.owner";
                        dbControl.SP_GET_DATOS(sqlPK, jSONOUT);
                        var idnomcampo = jSONOUT.Value.ToString();

                        DatosTramite modItem = new DatosTramite();
                        modItem = JsonConvert.DeserializeObject<DatosTramite>(idnomcampo.Substring(1, idnomcampo.Length - 2));
                        string nombre_Item = modItem.Variable_1.ToString();


                        //Busco en la tabla formulario 
                        jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                        string sql = "SELECT " + nombre_Item + " AS Variable_2 FROM " + Tbl_Estado + " WHERE " + formulario1.S_CAMPO_ID_ITEM + " = " + formItem.Variable_2;
                        dbControl.SP_GET_DATOS(sql, jSONOUT);
                        var Item1 = jSONOUT.Value.ToString();

                        DatosTramite modItem2 = new DatosTramite();
                        modItem2 = JsonConvert.DeserializeObject<DatosTramite>(Item1.Substring(1, Item1.Length - 2));

                        int idItem = modItem2.Variable_2;

                        //Obtengo el Id de la visita
                        jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                        string sql2 = "SELECT ID_VISITA AS Variable_2 FROM ITEM_VISITA WHERE ID_ITEM = " + idItem;
                        dbControl.SP_GET_DATOS(sql2, jSONOUT);
                        var visita = jSONOUT.Value.ToString();

                        DatosTramite modIDvisita = new DatosTramite();
                        modIDvisita = JsonConvert.DeserializeObject<DatosTramite>(visita.Substring(1, visita.Length - 2));

                        int idVisita = modIDvisita.Variable_2;
                        //Obtengo la encuesta guardada
                        jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                        dbControl.SP_GET_ENCUESTAS(idItem, formulario1.ID_FORMULARIO, jSONOUT);
                        //db.SP_GET_CARACTERISTICAS_TL(idItem, Tbl_Estado, formulario1.ID_FORMULARIO, 0, 0, jSONOUT);

                        return Json(jSONOUT.Value + "|Encuesta|" + idVisita);
                    }
                    else
                    {
                        jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                        ObjectParameter jsonOutXY = new ObjectParameter("jsonOutXY", typeof(string));

                        //db.SP_GET_ITEM_L(idFormu, -1, jSONOUT, jsonOutXY);
                        //var result = Json(jSONOUT.Value + "||" + jsonOutXY.Value);

                        //db.SP_GET_CARACTERISTICAS_TL(0, Tbl_Estado, formulario1.ID_FORMULARIO, 0, 0, jSONOUT);
                        dbControl.SP_GET_ENCUESTAS(-1, idFormu, jSONOUT);
                        return Json(jSONOUT.Value + "|Encuesta");
                    }
                }

                //VERTIMIENTOS - AGUAS SUPERFICIALES - AGUAS SUBTERRANEAS
                else if (idFormu == 13 || idFormu == 4 || idFormu == 5)
                {
                    //Pregunto si ya hay información guardada     
                    if (formItem.Variable_2 > 0)
                    {
                        //
                        //Consulto cual es el campo de llave primaria de la tabla
                        string sqlPK = "SELECT cols.column_name AS Variable_1" +
                        " FROM all_constraints cons, all_cons_columns cols WHERE cols.table_name = '" + nomTablaEstado +
                        "' AND cons.constraint_type = 'P' " +
                        " AND cons.constraint_name = cols.constraint_name" +
                        " AND cons.owner = cols.owner";
                        dbControl.SP_GET_DATOS(sqlPK, jSONOUT);
                        var idnomcampo = jSONOUT.Value.ToString();

                        DatosTramite modItem = new DatosTramite();
                        modItem = JsonConvert.DeserializeObject<DatosTramite>(idnomcampo.Substring(1, idnomcampo.Length - 2));
                        string nombre_Item = modItem.Variable_1.ToString();


                        //Busco en la tabla formulario 
                        jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                        string sql = "SELECT " + nombre_Item + " AS Variable_2 FROM " + Tbl_Estado + " WHERE " + formulario1.S_CAMPO_ID_ITEM + " = " + formItem.Variable_2;
                        dbControl.SP_GET_DATOS(sql, jSONOUT);
                        var Item1 = jSONOUT.Value.ToString();

                        DatosTramite modItem2 = new DatosTramite();
                        modItem2 = JsonConvert.DeserializeObject<DatosTramite>(Item1.Substring(1, Item1.Length - 2));

                        int idItem = modItem2.Variable_2;

                        //Obtengo el Id de la visita
                        jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                        string sql2 = "SELECT ID_VISITA AS Variable_2 FROM ITEM_VISITA WHERE ID_ITEM = " + idItem;
                        dbControl.SP_GET_DATOS(sql2, jSONOUT);
                        var visita = jSONOUT.Value.ToString();

                        DatosTramite modIDvisita = new DatosTramite();
                        modIDvisita = JsonConvert.DeserializeObject<DatosTramite>(visita.Substring(1, visita.Length - 2));

                        int idVisita = modIDvisita.Variable_2;

                        jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                        dbControl.SP_GET_CARACTERISTICAS(idItem, Tbl_Estado, formulario1.ID_FORMULARIO, 0, 0, jSONOUT);
                        return Json(jSONOUT.Value + "|Detalle|" + idVisita);
                    }
                    else
                    {

                        jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                        dbControl.SP_GET_CARACTERISTICAS_TL(1, Tbl_Estado, formulario1.ID_FORMULARIO, 0, 0, jSONOUT);
                        return Json(jSONOUT.Value + "|Detalle");
                    }
                }
                //AGUAS SUPERFICIALES = 4 O AGUAS SUBTERRANEAS = 5
                //else if (idFormu == 4 || idFormu == 5)
                else
                {
                    if (formItem.Variable_2 > 0)
                    {
                        //
                        //Consulto cual es el campo de llave primaria de la tabla
                        //string sqlPK = "SELECT cols.column_name AS Variable_1" +
                        //" FROM all_constraints cons, all_cons_columns cols WHERE cols.table_name = '" + nomTablaEstado +
                        //"' AND cons.constraint_type = 'P' " +
                        //" AND cons.constraint_name = cols.constraint_name" +
                        //" AND cons.owner = cols.owner";
                        //dbControl.SP_GET_DATOS(sqlPK, jSONOUT);
                        //var idnomcampo = jSONOUT.Value.ToString();

                        //DatosTramite modItem = new DatosTramite();
                        //modItem = JsonConvert.DeserializeObject<DatosTramite>(idnomcampo.Substring(1, idnomcampo.Length - 2));
                        //string nombre_Item = modItem.Variable_1.ToString();


                        //Busco en la tabla formulario 
                        jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                        string sql = "SELECT " + formulario1.S_CAMPO_ID_ITEM + " AS Variable_2 FROM " + Tbl_Estado + " WHERE " + formulario1.S_CAMPO_ID_ITEM + " = " + formItem.Variable_2;
                        dbControl.SP_GET_DATOS(sql, jSONOUT);
                        var Item1 = jSONOUT.Value.ToString();

                        DatosTramite modItem2 = new DatosTramite();
                        modItem2 = JsonConvert.DeserializeObject<DatosTramite>(Item1.Substring(1, Item1.Length - 2));

                        int idItem = modItem2.Variable_2;


                        //Obtengo el Id de la visita
                        jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                        string sql2 = "SELECT ID_VISITA AS Variable_2 FROM ITEM_VISITA WHERE ID_ITEM = " + idItem;
                        dbControl.SP_GET_DATOS(sql2, jSONOUT);
                        var visita = jSONOUT.Value.ToString();

                        DatosTramite modIDvisita = new DatosTramite();
                        modIDvisita = JsonConvert.DeserializeObject<DatosTramite>(visita.Substring(1, visita.Length - 2));

                        int idVisita = modIDvisita.Variable_2;

                        jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                        dbControl.SP_GET_CARACTERISTICAS(idItem, Tbl_Estado, formulario1.ID_FORMULARIO, 0, 0, jSONOUT);
                        return Json(jSONOUT.Value + "|Detalle|" + idVisita);
                    }
                    else
                    {
                        jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                        dbControl.SP_GET_CARACTERISTICAS_TL(-1, Tbl_Estado, formulario1.ID_FORMULARIO, 0, 0, jSONOUT);
                        return Json(jSONOUT.Value + "|Detalle");
                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message.ToString();
                return Json("");
            }

        }

        public ActionResult GuardarInformacionDetalle()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // int idTecero = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            int idTecero = 532;
            int idTramite = Convert.ToInt32(Request.Params["idTramite"]);
            try
            {
                // Obtengo la dirección de la instalación 
                int idInstalacion = Convert.ToInt32(Request.Params["idInstalacion"]);
                var instalacion = db.QRYINSTALACION_TERCERO.Where(f => f.ID_INSTALACION == idInstalacion).FirstOrDefault();
                string direccionInstalacion = instalacion.DIRECCION;
                var direecionFinal = HttpUtility.UrlEncode(direccionInstalacion);

                //Con la direccion de la instalacíón, hago uso de la Geocodificación para obtener X y Y 
                coordenadas coorP = new coordenadas();
                coordenadas coorP2 = new coordenadas();
                var client = new System.Net.WebClient();
                var dato = client.DownloadString("https://www.medellin.gov.co/MapGIS/Geocod/AjaxGeocod?accion=2&valor=" + direecionFinal + "&f=json");
                String xp = "";
                String yp = "";
                Decimal x = 0;
                Decimal y = 0;
                double X = 0;
                double Y = 0;

                if (dato != "0")
                {
                    coorP = JsonConvert.DeserializeObject<coordenadas>(dato);
                    xp = coorP.x;
                    yp = coorP.y;

                    var dato2 = client.DownloadString("https://www.medellin.gov.co/mapas/rest/services/Utilities/Geometry/GeometryServer/project?inSR=6257&outSR=4326&geometries=" + xp + "," + yp + "&transformation=15738&transformForward=true&f=json");
                    Newtonsoft.Json.Linq.JObject o = JObject.Parse(dato2);
                    x = (Decimal)o.SelectToken("geometries[0].x");
                    y = (Decimal)o.SelectToken("geometries[0].y");

                    String[] varx = x.ToString().Split('M');
                    X = Convert.ToDouble(varx[0]);

                    String[] vary = y.ToString().Split('M');
                    Y = Convert.ToDouble(vary[0]);

                }
                else
                {
                    x = 2;
                    y = 3;
                    X = 2.2;
                    Y = 2.3;
                }

                //Crear la actuación y guardar en la tabla
                var tramite = db.QRY_LISTADOTRAMITES.Where(f => f.ID_TRAMITE == idTramite).FirstOrDefault();
                string nombreTramite = tramite.TRAMITE;
                //string nombreTramite = "prueba";
                string tipoUbicación = "";

                if (x != 0 && y != 0)
                    tipoUbicación = "Geocodificador";
                else
                    tipoUbicación = "Manual";

                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
                {
                    idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                }

                //Guardo la actuación
                dbControl.SP_SET_ACTUACION(nombreTramite, (decimal)X, (decimal)Y, tipoUbicación, "", idUsuario, "", "", 2, 0);

                ObjectParameter JSONOUT = new ObjectParameter("JSONOUT", typeof(string));

                string sql = "SELECT MAX (ID_VISITA) ID_VISITA FROM CONTROL.VISITA WHERE S_ASUNTO = '" + nombreTramite + "' AND ID_TIPOVISITA = 2";

                dbControl.SP_GET_DATOS(sql, JSONOUT);

                var idVisita = JSONOUT.Value.ToString();

                VisistaTramiteModel visita1 = new VisistaTramiteModel();
                visita1 = JsonConvert.DeserializeObject<VisistaTramiteModel>(idVisita.Substring(1, idVisita.Length - 2));

                var formulario = db.FORMULARIO.Where(s => s.ID_FORMULARIO == tramite.ID_FORMULARIO).FirstOrDefault();
                //var formulario = db.FORMULARIO.Where(s => s.ID_FORMULARIO == 7).FirstOrDefault();

                if (formulario.ID_FORMULARIO != 4 && formulario.ID_FORMULARIO != 5 && formulario.ID_FORMULARIO != 13 && formulario.ID_FORMULARIO != 12 && formulario.ID_FORMULARIO != 7)
                {
                    //Obetengo el item con el id fomulario y con id_item = -1
                    JSONOUT = new ObjectParameter("JSONOUT", typeof(string));
                    dbControl.SP_GET_ITEM(formulario.ID_FORMULARIO, -1, JSONOUT);
                    var objItems = JSONOUT.Value.ToString();

                    Items modItem = new Items();
                    modItem = JsonConvert.DeserializeObject<Items>(objItems);

                    //Creo el json array y le zeteo los valores de X y Y
                    var variable = new
                    {
                        X = X,
                        Y = Y,
                        ID = modItem.ID,
                        NOMBRE = nombreTramite
                    };

                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string jsonInfoItem = (js.Serialize(variable));

                    //int idForm = 13;
                    ObjectParameter rTA = new ObjectParameter("rta", typeof(string));
                    //Guardo el item
                    dbControl.SP_SET_ITEM(formulario.ID_FORMULARIO, jsonInfoItem, idUsuario, tipoUbicación, rTA);

                    // Crear el estado 
                    ObjectParameter respidEstado = new ObjectParameter("respidEstado", typeof(string));
                    //db.SP_CREATE_ESTADO_ITEM(idTecero, idInstalacion, idFormulario.ID_TRAMITE, Convert.ToInt32(formItem.Variable_1), 1, Convert.ToInt32(visita1.ID_VISITA), respidEstado);
                    dbControl.SP_CREATE_ESTADO_ITEM(idTecero, idInstalacion, formulario.ID_FORMULARIO, Convert.ToInt32(modItem.ID), 1, Convert.ToInt32(visita1.ID_VISITA), respidEstado);
                    var estadoId = respidEstado.Value.ToString();
                    int EstadoItem = Convert.ToInt32(estadoId);

                    ObjectParameter rTaA = new ObjectParameter("rta", typeof(string));
                    string jsonInfo = "";
                    jsonInfo = Request.Params["jsonInfo"];

                    //Inserto en la tabla Instalacion_Visita 
                    //int decUsuario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(db, idUsuario));
                    Decimal tipoestado = 1;
                    Decimal form = formulario.ID_FORMULARIO;
                    Decimal idVisita1 = Convert.ToDecimal(visita1.ID_VISITA);
                    Decimal instalacion1 = instalacion.ID_INSTALACION;
                    Decimal tercero = idTecero;

                    INSTALACION_VISITA itemInstalacion = new INSTALACION_VISITA();
                    itemInstalacion.ID_INSTALACION = (int)instalacion1;
                    itemInstalacion.ID_TERCERO = (int)tercero;
                    itemInstalacion.ID_VISITA = (int)idVisita1;
                    db.Entry(itemInstalacion).State = EntityState.Added;
                    db.SaveChanges();

                    //Inserto en la tabla item_visita
                    ObjectParameter rTA3 = new ObjectParameter("rtaI", typeof(string));
                    dbTramites.SP_SET_ITEMVISITA(idVisita1, EstadoItem, idTramite, rTA3);

                    dbControl.SP_SET_CARACTERISTICAS_TL(EstadoItem, formulario.TBL_ESTADOS, jsonInfo, rTaA);
                    return Content("" + rTaA.Value + "|" + modItem.ID + "|" + idVisita1);

                }
                else if (formulario.ID_FORMULARIO == 4 || formulario.ID_FORMULARIO == 5)
                {
                    //Obtengo la captacion
                    JSONOUT = new ObjectParameter("jsonOut", typeof(string));
                    dbControl.SP_GET_CAPTACION((decimal)-1, (decimal)X, (decimal)Y, JSONOUT);
                    var infoAguas = JSONOUT.Value.ToString();

                    Captacion ModCaptacion = new Captacion();
                    ModCaptacion = JsonConvert.DeserializeObject<Captacion>(infoAguas);
                    var idTipoCapatacion = 0;

                    if (formulario.ID_FORMULARIO == 4)
                    {
                        idTipoCapatacion = 1;
                    }
                    else
                    {
                        idTipoCapatacion = 2;
                    }
                    var variableCapt = new
                    {
                        X = X,
                        Y = Y,
                        Z = 0,
                        ID_CAPTACION = ModCaptacion.ID_CAPTACION,
                        DESCRIPCION = nombreTramite,
                        ID_MUNICIPIO = 0,
                        ID_TIPO_FUENTE = 0,
                        ID_AREA_HIDROGRAFICA = 0,
                        ID_ZONA_HIDRO = 0,
                        ID_SUB_ZH = 0,
                        NOMBRE_FUENTE = "",
                        NOMBRE_TRAMO = "",
                        TIPO_CAPTACION = idTipoCapatacion,
                        TIPO_VERTIMIENTO = 1,
                        NOMBRE = nombreTramite + " Prueba"

                    };


                    System.Web.Script.Serialization.JavaScriptSerializer jsVert = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string jsonInfo = jsVert.Serialize(variableCapt);

                    // var decUsuario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(db, idUsuario));

                    ObjectParameter rTA = new ObjectParameter("rta", typeof(string));
                    Decimal tipoestado = 1;
                    Decimal form = formulario.ID_FORMULARIO;
                    Decimal idVisita1 = Convert.ToDecimal(visita1.ID_VISITA);
                    Decimal instalacion1 = instalacion.ID_INSTALACION;
                    Decimal tercero = idTecero;

                    //Guardo la capatación
                    dbControl.SP_SET_CAPTACION(jsonInfo, idUsuario, tipoUbicación, rTA);

                    // Crear el estado 
                    ObjectParameter rTA2 = new ObjectParameter("rESPIDESTADO", typeof(string));
                    dbControl.SP_CREATE_ESTADO(form, tercero, instalacion1, ModCaptacion.ID_CAPTACION, tipoestado, idVisita1, rTA2);
                    int idCapEs = Convert.ToInt32(rTA2.Value);

                    //Inserto en la tabla Instalacion_Visita 
                    INSTALACION_VISITA itemInstalacion = new INSTALACION_VISITA();
                    itemInstalacion.ID_INSTALACION = (int)instalacion1;
                    itemInstalacion.ID_TERCERO = (int)tercero;
                    itemInstalacion.ID_VISITA = (int)idVisita1;
                    db.Entry(itemInstalacion).State = EntityState.Added;
                    db.SaveChanges();

                    //Inserto en la tabla item_visita
                    ObjectParameter rTA3 = new ObjectParameter("rtaI", typeof(string));
                    dbTramites.SP_SET_ITEMVISITA(idVisita1, idCapEs, (decimal)idTramite, rTA3);

                    jsonInfo = "";
                    jsonInfo = Request.Params["jsonInfo"];
                    ObjectParameter rTaA = new ObjectParameter("rta", typeof(string));

                    dbControl.SP_SET_CARACTERISTICAS_TL(idCapEs, formulario.TBL_ESTADOS, jsonInfo, rTaA);
                    return Content("" + rTaA.Value + "|" + idCapEs + "|" + idVisita1);
                }
                //VERTIMIENTOS
                else if (formulario.ID_FORMULARIO == 13)
                {
                    //Validamos si es la primera vez en guardar información en el formulario
                    JSONOUT = new ObjectParameter("jsonOut", typeof(string));
                    dbControl.SP_GET_VERTIMIENTO((decimal)-1, (decimal)X, (decimal)Y, JSONOUT);
                    var infoVertimiento = JSONOUT.Value.ToString();

                    //JavaScriptSerializer serializer = new JavaScriptSerializer();
                    //Result result = serializer.Deserialize<Result>(serializer.Serialize(JSONOUT.Value));

                    Vertimiento ModVertimiento = new Vertimiento();
                    ModVertimiento = JsonConvert.DeserializeObject<Vertimiento>(infoVertimiento);

                    var variableVert = new
                    {
                        X = X,
                        Y = Y,
                        Z = 0,
                        ID_VERTIMIENTO = ModVertimiento.ID_VERTIMIENTO,
                        DESCRIPCION = nombreTramite,
                        ID_MUNICIPIO = 0,
                        ID_TIPO_FUENTE = 0,
                        ID_AREA_HIDROGRAFICA = 0,
                        ID_ZONA_HIDRO = 0,
                        ID_SUB_ZH = 0,
                        NOMBRE_FUENTE = "",
                        NOMBRE_TRAMO = "",
                        TIPO_CAPTACION = 1,
                        TIPO_VERTIMIENTO = 1,
                        NOMBRE = nombreTramite

                    };

                    System.Web.Script.Serialization.JavaScriptSerializer jsVert = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string jsonInfo = jsVert.Serialize(variableVert);

                    JSONOUT = new ObjectParameter("jsonOut", typeof(string));
                    Decimal idCap = Convert.ToDecimal(Request.Params["idCapEstado"]);

                    if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                    {

                        ObjectParameter rTA = new ObjectParameter("rta", typeof(string));
                        Decimal tipoestado = 1;
                        Decimal form = formulario.ID_FORMULARIO;
                        Decimal idVisita1 = Convert.ToDecimal(visita1.ID_VISITA);
                        Decimal instalacion1 = instalacion.ID_INSTALACION;
                        Decimal tercero = idTecero;

                        dbControl.SP_SET_VERTIMIENTO(jsonInfo, 0, "MANUAL", rTA);
                        if (rTA.Value.Equals("Ok"))
                        {
                            ObjectParameter rTA2 = new ObjectParameter("rESPIDESTADO", typeof(string));
                            dbControl.SP_CREATE_ESTADO_V(form, tercero, instalacion1, ModVertimiento.ID_VERTIMIENTO, tipoestado, idVisita1, rTA2);
                            int idCapEs = Convert.ToInt32(rTA2.Value);

                            //Inserto en la tabla Instalacion_Visita 
                            INSTALACION_VISITA itemInstalacion = new INSTALACION_VISITA();
                            itemInstalacion.ID_INSTALACION = (int)instalacion1;
                            itemInstalacion.ID_TERCERO = (int)tercero;
                            itemInstalacion.ID_VISITA = (int)idVisita1;
                            db.Entry(itemInstalacion).State = EntityState.Added;
                            db.SaveChanges();

                            //Inserto en la tabla item_visita
                            ObjectParameter rTA3 = new ObjectParameter("rtaI", typeof(string));
                            dbTramites.SP_SET_ITEMVISITA(idVisita1, idCapEs, idTramite, rTA3);

                            jsonInfo = "";
                            jsonInfo = Request.Params["jsonInfo"];
                            ObjectParameter rTaA = new ObjectParameter("rta", typeof(string));

                            dbControl.SP_SET_CARACTERISTICAS_TL(idCapEs, formulario.TBL_ESTADOS, jsonInfo, rTaA);
                            return Content("" + rTaA.Value + "|" + idCapEs + "|" + idVisita1);
                        }
                        return Content("error");
                    }
                }
                //OCUPACIÓN DE CAUSES
                else if (formulario.ID_FORMULARIO == 12 || formulario.ID_FORMULARIO == 7)
                {
                    string jsonInfoEncuesta = Request.Params["jsonInfoEncuesta"];

                    //Obetengo el item con el id fomulario y con id_item = -1
                    ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                    ObjectParameter jsonOutXY = new ObjectParameter("jsonOutXY", typeof(string));
                    dbControl.SP_GET_ITEM_L(formulario.ID_FORMULARIO, -1, jSONOUT, jsonOutXY);

                    var jsonItem = jSONOUT.Value.ToString();
                    ItemOC modItem = new ItemOC();
                    modItem = JsonConvert.DeserializeObject<ItemOC>(jsonItem);

                    var variableVert = new
                    {
                        X = X,
                        Y = Y,

                    };

                    var infoItemNombre = new
                    {
                        ID_ITEM = modItem.ID_ITEM,
                        NOMBRE = "Ocupación Cauce (Nuevo)",
                    };

                    //var jsonInfo = Json(infoItemNombre);


                    System.Web.Script.Serialization.JavaScriptSerializer jsItem = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string JSONItemN = jsItem.Serialize(infoItemNombre);

                    System.Web.Script.Serialization.JavaScriptSerializer jsVert = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string jsonGeo2 = jsVert.Serialize(variableVert);

                    string ComillasI = "[";
                    string ComillasF = "]";
                    //string jsonInfo = jSONOUT.Value.ToString();
                    string jsonGeo = ComillasI + jsonGeo2 + ComillasF;

                    //var jsonOutt = Json(jSONOUT.Value);
                    //var jsonxy = Json(jsonOutXY.Value);
                    //Decimal decUsuario = clsGenerales.Obtener_Codigo_Funcionario(db, idUsuario);
                    ObjectParameter rTA = new ObjectParameter("rta", typeof(string));

                    dbControl.SP_SET_ITEM_L(formulario.ID_FORMULARIO, JSONItemN, jsonGeo, 0, "MANUAL", rTA);


                    Decimal tipoestado = 1;
                    Decimal form = formulario.ID_FORMULARIO;
                    Decimal idVisita1 = Convert.ToDecimal(visita1.ID_VISITA);
                    Decimal instalacion1 = instalacion.ID_INSTALACION;
                    Decimal tercero = idTecero;
                    //Inserto en la tabla Instalacion_Visita 
                    INSTALACION_VISITA itemInstalacion = new INSTALACION_VISITA();
                    itemInstalacion.ID_INSTALACION = (int)instalacion1;
                    itemInstalacion.ID_TERCERO = (int)tercero;
                    itemInstalacion.ID_VISITA = (int)idVisita1;
                    db.Entry(itemInstalacion).State = EntityState.Added;
                    db.SaveChanges();

                    // Crear el estado 
                    ObjectParameter respidEstado = new ObjectParameter("respidEstado", typeof(string));
                    //db.SP_CREATE_ESTADO_ITEM(idTecero, idInstalacion, idFormulario.ID_TRAMITE, Convert.ToInt32(formItem.Variable_1), 1, Convert.ToInt32(visita1.ID_VISITA), respidEstado);
                    dbControl.SP_CREATE_ESTADO_ITEM(idTecero, idInstalacion, formulario.ID_FORMULARIO, Convert.ToInt32(modItem.ID_ITEM), 1, Convert.ToInt32(visita1.ID_VISITA), respidEstado);
                    var estadoId = respidEstado.Value.ToString();
                    int EstadoItem = Convert.ToInt32(estadoId);

                    //Inserto en la tabla item_visita
                    ObjectParameter rTA3 = new ObjectParameter("rtaI", typeof(string));
                    dbTramites.SP_SET_ITEMVISITA(idVisita1, EstadoItem, idTramite, rTA3);

                    rTA = new ObjectParameter("rta", typeof(string));
                    dbControl.SP_SET_ENCUESTAS(EstadoItem, form, jsonInfoEncuesta, rTA);
                    // db.SP_SET_CARACTERISTICAS_TL(Convert.ToInt32(modItem.ID_ITEM), formulario.TBL_ESTADOS, jsonInfoEncuesta, rTA);

                    return Content(rTA.Value + "|" + EstadoItem + "|" + idVisita1);
                }

                string jsonInfo3 = Request.Params["jsonInfo"];
                ObjectParameter rTaA3 = new ObjectParameter("rta", typeof(string));

                dbControl.SP_SET_CARACTERISTICAS_TL(0, formulario.TBL_ESTADOS, jsonInfo3, rTaA3);
                return Content("" + rTaA3.Value);
            }
            catch (Exception ex)
            {
                var error = ex.Message.ToString();
                throw;
            }

        }
        //

        //REQUISITOS
        public ActionResult GetRequisitosXTramite(int id, int? idInstalacion, int? idRequisito)
        {

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            }


            //Verificar si ya hay requisitos cargados

            Tramites.Models.RequisitosTramiteModel mo2 = new Tramites.Models.RequisitosTramiteModel();

            List<Tramites.Models.RequisitosTramiteModel> listReqCargados = new List<Tramites.Models.RequisitosTramiteModel>();

            List<Tramites.Models.RequisitosTramiteModel> parcialReqCargados = new List<Tramites.Models.RequisitosTramiteModel>();

            var yaRequisitos = db.REQUISITOS_TRAMITE.Where(f => f.ID_INSTALACION == idInstalacion && f.ID_TERCERO == idUsuario && f.ID_TRAMITE == id).OrderBy(h => h.ID_REQUISITO).ToList();

            var nomRequisitos = db.QRY_REQUISITOS_TRAMITE.Where(b => b.ID_TRAMITE == id).OrderBy(h => h.ID_REQUISITO).ToList();

            if (yaRequisitos.Count > 0)
            {

                foreach (var item0 in yaRequisitos)
                {
                    foreach (var item01 in nomRequisitos)
                    {
                        if (item0.ID_REQUISITO == item01.ID_REQUISITO)
                        {
                            mo2.ID_REQUISITO = item0.ID_REQUISITO;
                            mo2.ID_TRAMITE = item0.ID_TRAMITE;
                            mo2.ID_ESTADO = 2;
                            mo2.NOMBRE_ESTADO = "Cargado";

                            if (item01.OBLIGATORIO == "1")
                                mo2.REQUISITO = item01.REQUISITO + "*";
                            else
                                mo2.REQUISITO = item01.REQUISITO;

                            mo2.FORMATO = item01.FORMATO;
                            mo2.ID_INSTALACION = (int)idInstalacion;

                            listReqCargados.Add(mo2);
                            mo2 = new Tramites.Models.RequisitosTramiteModel();
                        }

                    }
                }

                //foreach (var item in yaRequisitos)
                //{

                foreach (var item2 in nomRequisitos)
                {
                    //if (item.ID_REQUISITO != item2.ID_REQUISITO)
                    //{
                    mo2.ID_REQUISITO = item2.ID_REQUISITO;
                    mo2.ID_TRAMITE = (int)item2.ID_TRAMITE;
                    mo2.ID_ESTADO = 1;
                    mo2.NOMBRE_ESTADO = "Pendiente";

                    if (item2.OBLIGATORIO == "1")
                        mo2.REQUISITO = item2.REQUISITO + "*";
                    else
                        mo2.REQUISITO = item2.REQUISITO;

                    mo2.FORMATO = item2.FORMATO;
                    mo2.ID_INSTALACION = (int)idInstalacion;
                    //}

                    Tramites.Models.RequisitosTramiteModel tt = listReqCargados.Find(c => c.ID_REQUISITO == mo2.ID_REQUISITO);

                    if (tt == null)
                        listReqCargados.Add(mo2);

                    mo2 = new Tramites.Models.RequisitosTramiteModel();
                }
                //}
                string jsonq = JsonConvert.SerializeObject(listReqCargados);
                return Json(jsonq);
            }


            List<QRY_REQUISITOS_TRAMITE> model = new List<QRY_REQUISITOS_TRAMITE>();

            //pregunto si es un tramite nuevo ó van a continuar con el 
            //Ya existe documento subido
            if (idInstalacion > 0 && idRequisito > 0)
            {
                var requisito = db.REQUISITOS_TRAMITE.Where(f => f.ID_INSTALACION == idInstalacion && f.ID_REQUISITO == idRequisito && f.ID_TERCERO == idUsuario && f.ID_TRAMITE == id).FirstOrDefault();

                if (requisito != null && !String.IsNullOrEmpty(requisito.RUTA_DOCUMENTO))
                {
                    var query = from b in db.QRY_REQUISITOS_TRAMITE where (b.ID_TRAMITE == id) select new Tramites.Models.RequisitosTramiteModel { ID_TRAMITE = (int)b.ID_TRAMITE, ID_REQUISITO = b.ID_REQUISITO, REQUISITO = b.REQUISITO, ID_ESTADO = 1, NOMBRE_ESTADO = "Pendiente", FORMATO = b.FORMATO, OBLIGATORIO = b.OBLIGATORIO };

                    Tramites.Models.RequisitosTramiteModel mo = new Tramites.Models.RequisitosTramiteModel();

                    List<Tramites.Models.RequisitosTramiteModel> listReqTra = new List<Tramites.Models.RequisitosTramiteModel>();

                    foreach (var item in query)
                    {
                        mo.ID_TRAMITE = item.ID_TRAMITE;
                        mo.ID_REQUISITO = item.ID_REQUISITO;
                        if (item.OBLIGATORIO == "1")
                            mo.REQUISITO = item.REQUISITO + "*";
                        else
                            mo.REQUISITO = item.REQUISITO;
                        mo.FORMATO = item.FORMATO;

                        if (item.ID_REQUISITO == idRequisito)
                        {
                            mo.ID_ESTADO = 2;
                            mo.NOMBRE_ESTADO = "Cargado";
                        }
                        else
                        {
                            mo.ID_ESTADO = item.ID_ESTADO;
                            mo.NOMBRE_ESTADO = item.NOMBRE_ESTADO;
                        }
                        mo.ID_INSTALACION = (int)idInstalacion;

                        listReqTra.Add(mo);

                        mo = new Tramites.Models.RequisitosTramiteModel();
                    }
                    string json = JsonConvert.SerializeObject(listReqTra);
                    return Json(json);
                }
                else
                {
                    var query2 = from b in db.QRY_REQUISITOS_TRAMITE where (b.ID_TRAMITE == id) select new Tramites.Models.RequisitosTramiteModel { ID_TRAMITE = (int)b.ID_TRAMITE, ID_REQUISITO = b.ID_REQUISITO, REQUISITO = b.REQUISITO, ID_ESTADO = 1, NOMBRE_ESTADO = "Pendiente", FORMATO = b.FORMATO, OBLIGATORIO = b.OBLIGATORIO };

                    List<Tramites.Models.RequisitosTramiteModel> listReqTra = new List<Tramites.Models.RequisitosTramiteModel>();

                    foreach (var item2 in query2)
                    {
                        mo2.ID_REQUISITO = item2.ID_REQUISITO;
                        mo2.ID_TRAMITE = (int)item2.ID_TRAMITE;
                        mo2.ID_ESTADO = 1;
                        mo2.NOMBRE_ESTADO = "Pendiente";

                        if (item2.OBLIGATORIO == "1")
                            mo2.REQUISITO = item2.REQUISITO + "*";
                        else
                            mo2.REQUISITO = item2.REQUISITO;

                        mo2.FORMATO = item2.FORMATO;
                        mo2.ID_INSTALACION = (int)idInstalacion;

                        listReqTra.Add(mo2);

                        mo2 = new Tramites.Models.RequisitosTramiteModel();
                    }

                    string json = JsonConvert.SerializeObject(listReqTra);
                    return Json(json);
                }
            }
            //Primera vez del tramite
            else
            {
                var query2 = from b in db.QRY_REQUISITOS_TRAMITE where (b.ID_TRAMITE == id) select new Tramites.Models.RequisitosTramiteModel { ID_TRAMITE = (int)b.ID_TRAMITE, ID_REQUISITO = b.ID_REQUISITO, REQUISITO = b.REQUISITO, ID_ESTADO = 1, NOMBRE_ESTADO = "Pendiente", FORMATO = b.FORMATO, OBLIGATORIO = b.OBLIGATORIO };

                List<Tramites.Models.RequisitosTramiteModel> listReqTra = new List<Tramites.Models.RequisitosTramiteModel>();

                foreach (var item2 in query2)
                {
                    mo2.ID_REQUISITO = item2.ID_REQUISITO;
                    mo2.ID_TRAMITE = (int)item2.ID_TRAMITE;
                    mo2.ID_ESTADO = 1;
                    mo2.NOMBRE_ESTADO = "Pendiente";

                    if (item2.OBLIGATORIO == "1")
                        mo2.REQUISITO = item2.REQUISITO + "*";
                    else
                        mo2.REQUISITO = item2.REQUISITO;

                    mo2.FORMATO = item2.FORMATO;
                    mo2.ID_INSTALACION = (int)idInstalacion;

                    listReqTra.Add(mo2);

                    mo2 = new Tramites.Models.RequisitosTramiteModel();
                }

                string json = JsonConvert.SerializeObject(listReqTra);
                return Json(json);
            }
        }

        public ActionResult SubirDocumento()
        {
            int idTramite = Convert.ToInt32(Request.QueryString["idTramite"]);
            int idRequisito = Convert.ToInt32(Request.QueryString["idRequisito"]);
            int idTercero = Convert.ToInt32(Request.QueryString["idTercero"]);
            int idInstalacion = Convert.ToInt32(Request.QueryString["idInstalacion"]);

            ViewBag.idTramite = idTramite;
            ViewBag.idRequisito = idRequisito;
            ViewBag.idTercero = idTercero;
            ViewBag.idInstalacion = idInstalacion;

            return View();
        }

        private static bool HasFile(HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }

        public ActionResult guardarUrl(decimal idTramite, int idRequisito, int idTercero, int idInstalacion, String fecha)
        {

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            }

            string url = "";
            string filename = "";
            string rutaLeer = ConfigurationManager.AppSettings["dir_doc_inf_salidadGuardarTramite"];
            string url2 = ConfigurationManager.AppSettings["dir_doc_inf_leerTramite"];
            var ruta = "";
            var ruta_final = "";
            string extension = "";

            fecha = DateTime.Now.ToString("yyyyMMddHHmmss");

            try
            {
                foreach (string upload in Request.Files)
                {
                    if (!HasFile(Request.Files[upload]))
                        continue;

                    filename = Path.GetFileName(Request.Files[upload].FileName);
                    extension = Path.GetExtension(filename);

                    filename = filename.Replace(filename, "ins" + idInstalacion + "-tra" + idTramite + "-req" + idRequisito + "-ter" + idUsuario + "-" + fecha + extension);

                    string rutaDirectorio = rutaLeer + "\\" + Archivos.GetRutaDocumento(Convert.ToUInt64(idTramite), 100);

                    if (!Directory.Exists(rutaDirectorio))
                    {
                        Directory.CreateDirectory(rutaDirectorio);
                    }

                    Request.Files[upload].SaveAs(rutaDirectorio + filename);
                    ruta_final = rutaDirectorio + filename;

                }

                //Guardo en la tabla
                var requisito = db.REQUISITOS_TRAMITE.Where(f => f.ID_INSTALACION == idInstalacion && f.ID_REQUISITO == idRequisito && f.ID_TERCERO == idUsuario && f.ID_TRAMITE == idTramite).FirstOrDefault();

                if (requisito != null)
                {
                    requisito.ID_TRAMITE = (int)idTramite;
                    requisito.ID_REQUISITO = idRequisito;
                    requisito.ID_TERCERO = idUsuario;
                    requisito.ID_INSTALACION = idInstalacion;
                    requisito.D_CARGA = DateTime.Now;
                    requisito.RUTA_DOCUMENTO = ruta_final;
                    requisito.EXTENSION = extension;

                    db.Entry(requisito).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    db = new EntitiesSIMOracle();

                    REQUISITOS_TRAMITE item = new REQUISITOS_TRAMITE();
                    item.ID_TRAMITE = (int)idTramite;
                    item.ID_REQUISITO = idRequisito;
                    item.ID_TERCERO = idUsuario;
                    item.ID_INSTALACION = idInstalacion;
                    item.D_CARGA = DateTime.Now;
                    item.RUTA_DOCUMENTO = ruta_final;
                    item.EXTENSION = extension;

                    db.Entry(item).State = EntityState.Added;
                    db.SaveChanges();
                }



                return Content("1");
            }
            catch (Exception ex)
            {
                var error = ex.Message.ToString();
                return Content("0");
            }



        }

        public ActionResult DeleteRequisitoxTramite(int idTram, int idReq, int idTerce, int idInst)
        {

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            }

            try
            {
                var requisito = db.REQUISITOS_TRAMITE.Where(f => f.ID_INSTALACION == idInst && f.ID_REQUISITO == idReq && f.ID_TERCERO == idUsuario && f.ID_TRAMITE == idTram).FirstOrDefault();

                if (System.IO.File.Exists(requisito.RUTA_DOCUMENTO))
                {
                    System.IO.File.Delete(requisito.RUTA_DOCUMENTO);
                }

                if (requisito != null)
                {
                    db.Entry(requisito).State = EntityState.Deleted;
                    db.SaveChanges();
                }


                return Json("A");
            }
            catch (Exception ex)
            {

                return Json("B");
            }

        }

        public string ValidarRequisitosCargados(int idTramite, int idInstalacion, int idTercero)
        {
            try
            {
                //VALIDAMOS QUE HAYA ADJUNTADO LOS REQUSITOS QUE SON

                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
                {
                    idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
                }

                List<REQUISITOS_TRAMITE> listReqCargados = new List<REQUISITOS_TRAMITE>();
                REQUISITOS_TRAMITE objRequisitosTra = new REQUISITOS_TRAMITE();

                //var query = db.QRY_REQUISITOS_TRAMITE.Where(t => t.ID_TRAMITE == idTramite && t.obligatorio == 1).ToList();

                var query = db.QRY_REQUISITOS_TRAMITE.Where(t => t.ID_TRAMITE == idTramite && t.OBLIGATORIO == "1").ToList();

                foreach (var item in query)
                {
                    objRequisitosTra.ID_REQUISITO = item.ID_REQUISITO;

                    listReqCargados.Add(objRequisitosTra);
                    objRequisitosTra = new REQUISITOS_TRAMITE();

                }

                List<MisRequisitos> listRequisitosObliga = new List<MisRequisitos>();
                MisRequisitos x = new MisRequisitos();

                var query2 = db.REQUISITOS_TRAMITE.Where(r => r.ID_TRAMITE == idTramite && r.ID_TERCERO == idTercero && r.ID_INSTALACION == idInstalacion).ToList();

                if (query2.Count == 0)
                    return "No";
                else
                {

                    foreach (var item in query2)
                    {
                        x.Id = item.ID_REQUISITO;

                        listRequisitosObliga.Add(x);
                        x = new MisRequisitos();
                    }

                    //Validamos que esten cargados todos los requisitos para el tramite
                    bool existe = true;

                    foreach (var item2 in listReqCargados)
                    {
                        MisRequisitos tt = listRequisitosObliga.Find(e => e.Id == item2.ID_REQUISITO);

                        if (tt == null)
                        {
                            existe = false;
                            return "No";
                        }
                    }

                    if (existe == true)
                        return "Ok";
                    else
                        return "No";
                }
            }
            catch (Exception ex)
            {
                return "Error";
            }

        }

        //public string ValidarTramite(int idTramite, int idInstalacion, int idTercero, int? idVisita, int? idRadicado)
        //{

        //    //GENERAMOS EL REPORTE
        //    List<REQUISITOS_TRAMITE> listReqCargados = new List<REQUISITOS_TRAMITE>();

        //    if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
        //    {
        //        idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
        //    }

        //    String nombre = "";
        //    PdfDocument inputDocument;
        //    AppSettingsReader webConfigReader = new AppSettingsReader();
        //    var stream = new MemoryStream();

        //    var tramite = db.QRY_LISTADOTRAMITES.Where(f => f.ID_TRAMITE == idTramite).FirstOrDefault();
        //    string nombreTramite = tramite.TRAMITE;

        //    //var formulario = db.FORMULARIO.Where(f => f.ID_FORMULARIO == idTram).FirstOrDefault();

        //    PdfDocument outputDocument = new PdfDocument();

        //    var report1 = new SIM.Areas.Tramites.Reportes.R_Principal2();

        //    DevExpress.XtraReports.Parameters.Parameter paramIdTercero = report1.Parameters["prmIdTerceroPrpal"];
        //    DevExpress.XtraReports.Parameters.Parameter paramNombreTramite = report1.Parameters["prm_Nombre_Tramite"];
        //    DevExpress.XtraReports.Parameters.Parameter paramIdInstalacion = report1.Parameters["prmIdInstalacionP"];
        //    DevExpress.XtraReports.Parameters.Parameter paramIdVisita = report1.Parameters["prmidVisitaP"];


        //    paramIdTercero.Value = idUsuario;
        //    paramNombreTramite.Value = nombreTramite;
        //    paramIdInstalacion.Value = idInstalacion;
        //    paramIdVisita.Value = idVisita;

        //    //Valido si existen requisitos cargados para ese tramite
        //    listReqCargados = db.REQUISITOS_TRAMITE.Where(s => s.ID_INSTALACION == idInstalacion && s.ID_TRAMITE == idTramite && s.ID_TERCERO == idUsuario).OrderBy(r => r.ID_REQUISITO).ToList();
        //    report1.ExportToPdf(stream);
        //    report1.Dispose();

        //    PdfDocument docPlanti2 = new PdfDocument();
        //    docPlanti2 = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
        //    int numPaginas = 0;
        //    int countPlant2 = docPlanti2.PageCount;
        //    for (int idx = 0; idx < countPlant2; idx++)
        //    {
        //        PdfPage page = docPlanti2.Pages[idx];
        //        outputDocument.AddPage(page);
        //        numPaginas++;

        //        if (idx == 0) // Primera Página, se inserta el radicado
        //        {
        //            PdfPage pageRadicado = outputDocument.Pages[idx];
        //            Radicado01Report etiqueta = new Radicado01Report();
        //            MemoryStream imagenEtiqueta = etiqueta.GenerarEtiqueta((int)idRadicado, "png");

        //            XGraphics gfx = XGraphics.FromPdfPage(pageRadicado);
        //            DrawImage(gfx, imagenEtiqueta, 300, 60, 700, 100);
        //        }
        //    }

        //    if (listReqCargados.Count > 0)
        //    {
        //        for (int i = 0; i < listReqCargados.Count; i++)
        //        {

        //            PdfDocument docPlanti = new PdfDocument();
        //            docPlanti = PdfReader.Open(listReqCargados[i].RUTA_DOCUMENTO, PdfDocumentOpenMode.Import);

        //            int countPlant = docPlanti.PageCount;
        //            for (int idx = 0; idx < countPlant; idx++)
        //            {
        //                PdfPage page = docPlanti.Pages[idx];
        //                outputDocument.AddPage(page);
        //                numPaginas++;
        //            }
        //        }
        //    }

        //    MemoryStream ms = new MemoryStream();
        //    outputDocument.Save(ms);


        //    //Guardar sistema gestión documental
        //    string rutaDocumento;
        //    int idCodDocumento;
        //    int idproceso = 2521; //Pendiente de definir con jorge
        //    int codTramite = 993328; //Pendiente de definir con jorge


        //    TBRUTAPROCESO rutaProceso = db.TBRUTAPROCESO.Where(rp => rp.CODPROCESO == idproceso).FirstOrDefault();
        //    TBTRAMITEDOCUMENTO ultimoDocumento = db.TBTRAMITEDOCUMENTO.Where(td => td.CODTRAMITE == codTramite).OrderByDescending(td => td.CODDOCUMENTO).FirstOrDefault();
        //    RADICADO_DOCUMENTO radicado = db.RADICADO_DOCUMENTO.Where(r => r.ID_RADICADODOC == idRadicado).FirstOrDefault();

        //    if (ultimoDocumento == null)
        //        idCodDocumento = 1;
        //    else
        //        idCodDocumento = Convert.ToInt32(ultimoDocumento.CODDOCUMENTO) + 1;

        //    rutaDocumento = rutaProceso.PATH + "\\" + codTramite.ToString() + "-" + idCodDocumento.ToString() + ".pdf";



        //    TBTRAMITEDOCUMENTO documento = new TBTRAMITEDOCUMENTO();
        //    TBTRAMITE_DOC relDocTra = new TBTRAMITE_DOC();
        //    documento.CODDOCUMENTO = idCodDocumento;
        //    documento.CODTRAMITE = codTramite;
        //    documento.TIPODOCUMENTO = 1;
        //    documento.FECHACREACION = DateTime.Now;
        //    documento.CODFUNCIONARIO = codFuncionario;
        //    documento.ID_USUARIO = codFuncionario;
        //    documento.RUTA = rutaDocumento;
        //    documento.MAPAARCHIVO = "M";
        //    documento.MAPABD = "M";
        //    documento.PAGINAS = numPaginas;
        //    documento.CODSERIE = Convert.ToInt32(radicado.CODSERIE);

        //    db.Entry(documento).State = System.Data.Entity.EntityState.Added;
        //    db.SaveChanges();
        //    relDocTra.CODTRAMITE = codTramite;
        //    relDocTra.CODDOCUMENTO = idCodDocumento;
        //    relDocTra.ID_DOCUMENTO = documento.ID_DOCUMENTO;
        //    db.Entry(relDocTra).State = System.Data.Entity.EntityState.Added;
        //    db.SaveChanges();


        //    //SE AVANZA LA TAREA 
        //    //db.SP_AVANZA_TAREA_TRAMITE(codTramite, idUsuario, 4145, "Inicia Tramite TL", 0, idTercero, "", 0);

        //    //


        //    //ENVIA CORREO DE NOTIFICACIÓN AL SOLITANTE
        //    string emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
        //    string emailSMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
        //    string emailSMTPUser = ConfigurationManager.AppSettings["SMTPUser"];
        //    string emailSMTPPwd = ConfigurationManager.AppSettings["SMTPPwd"];
        //    string asunto = "Prueba";
        //    string contenido = "Informe actuación";
        //    string resultado = "";
        //    string emailDestino = ConfigurationManager.AppSettings["EmailDestino"];

        //    var tercero = db.TERCERO.Where(t => t.ID_TERCERO == idTercero).FirstOrDefault();
        //    string direccionEmail = tercero.S_CORREO;

        //    if (!string.IsNullOrEmpty(direccionEmail))
        //    {
        //        try
        //        {
        //            SIM.Utilidades.Email.EnviarEmail2(emailFrom, emailDestino, asunto, contenido, emailSMTPServer, true, emailSMTPUser, emailSMTPPwd, ms);
        //            resultado = "Ok|" + radicado.S_RADICADO;
        //        }
        //        catch (Exception ex)
        //        {
        //            resultado = ex.Message.ToString();
        //        }
        //    }

        //    return resultado;

        //}

        [HttpGet, ActionName("AbrirDocumento")]
        public ActionResult AbrirDocumento(int idTram, int idReq, int idTerce, int idInst)
        {

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            }

            var requisito = db.REQUISITOS_TRAMITE.Where(f => f.ID_INSTALACION == idInst && f.ID_REQUISITO == idReq && f.ID_TERCERO == idUsuario && f.ID_TRAMITE == idTram).FirstOrDefault();

            var nombreRequisito = db.QRY_REQUISITOS_TRAMITE.Where(b => b.ID_TRAMITE == idTram && b.ID_REQUISITO == idReq).FirstOrDefault();

            var ruta = "";

            if (requisito != null)
            {
                ruta = requisito.RUTA_DOCUMENTO;

                // Abro el documento 
                HttpResponse response = System.Web.HttpContext.Current.Response;

                switch (requisito.EXTENSION)
                {
                    case ".pdf":
                        return File(System.IO.File.ReadAllBytes(ruta), "application/pdf", nombreRequisito.REQUISITO + requisito.EXTENSION);

                    case ".doc":
                        return File(System.IO.File.ReadAllBytes(ruta), "application/msword", nombreRequisito.REQUISITO + requisito.EXTENSION);

                    case ".dot":
                        return File(System.IO.File.ReadAllBytes(ruta), "application/msword", nombreRequisito.REQUISITO + requisito.EXTENSION);

                    case ".dotx":
                        return File(System.IO.File.ReadAllBytes(ruta), "application/vnd.openxmlformats-officedocument.wordprocessingml.template", nombreRequisito.REQUISITO + requisito.EXTENSION);

                    case ".docm":
                        return File(System.IO.File.ReadAllBytes(ruta), "application/vnd.ms-word.document.macroEnabled.12", nombreRequisito.REQUISITO + requisito.EXTENSION);

                    case ".xls":
                        return File(System.IO.File.ReadAllBytes(ruta), "application/vnd.ms-excel", nombreRequisito.REQUISITO + requisito.EXTENSION);

                    case ".xlsx":
                        return File(System.IO.File.ReadAllBytes(ruta), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreRequisito.REQUISITO + requisito.EXTENSION);

                    case ".ppt":
                        return File(System.IO.File.ReadAllBytes(ruta), "application/vnd.ms-powerpoint", nombreRequisito.REQUISITO + requisito.EXTENSION);

                    case ".docx":
                        return File(System.IO.File.ReadAllBytes(ruta), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", nombreRequisito.REQUISITO + requisito.EXTENSION);

                    case ".PNG":
                        return File(System.IO.File.ReadAllBytes(ruta), "image/png", nombreRequisito.REQUISITO + requisito.EXTENSION);

                }

                return null;
            }
            else
            {
                return null;
            }
        }

        public ActionResult VistaPreviaPDF(int idTram, int idInstalacion, int? idVisita)
        {
            List<REQUISITOS_TRAMITE> listReqCargados = new List<REQUISITOS_TRAMITE>();

            //idInstalacion = 123533;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            }

            String nombre = "";
            PdfDocument inputDocument;
            AppSettingsReader webConfigReader = new AppSettingsReader();
            var stream = new MemoryStream();
            //idTram = 15;

            var tramite = db.QRY_LISTADOTRAMITES.Where(f => f.ID_TRAMITE == idTram).FirstOrDefault();
            string nombreTramite = tramite.TRAMITE;

            //var formulario = db.FORMULARIO.Where(f => f.ID_FORMULARIO == idTram).FirstOrDefault();

            PdfDocument outputDocument = new PdfDocument();

            var report1 = new SIM.Areas.Tramites.Reportes.R_Principal2();

            DevExpress.XtraReports.Parameters.Parameter paramIdTercero = report1.Parameters["prmIdTerceroPrpal"];
            DevExpress.XtraReports.Parameters.Parameter paramNombreTramite = report1.Parameters["prm_Nombre_Tramite"];
            DevExpress.XtraReports.Parameters.Parameter paramIdInstalacion = report1.Parameters["prmIdInstalacionP"];
            DevExpress.XtraReports.Parameters.Parameter paramIdVisita = report1.Parameters["prmidVisitaP"];


            paramIdTercero.Value = idUsuario;
            paramNombreTramite.Value = nombreTramite;
            paramIdInstalacion.Value = idInstalacion;
            paramIdVisita.Value = idVisita;
            //paramIdTram.Value = idTram;
            //paramIdInstalacion.Value = idInstalacion;

            //Valido si existen requisitos cargados para ese tramite
            listReqCargados = db.REQUISITOS_TRAMITE.Where(s => s.ID_INSTALACION == idInstalacion && s.ID_TRAMITE == idTram && s.ID_TERCERO == idUsuario).OrderBy(r => r.ID_REQUISITO).ToList();
            report1.ExportToPdf(stream);
            report1.Dispose();

            PdfDocument docPlanti2 = new PdfDocument();
            docPlanti2 = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
            int numPaginas2 = 0;
            int countPlant2 = docPlanti2.PageCount;
            for (int idx = 0; idx < countPlant2; idx++)
            {
                PdfPage page = docPlanti2.Pages[idx];
                outputDocument.AddPage(page);
                numPaginas2++;

            }


            if (listReqCargados.Count > 0)
            {
                for (int i = 0; i < listReqCargados.Count; i++)
                {

                    PdfDocument docPlanti = new PdfDocument();
                    docPlanti = PdfReader.Open(listReqCargados[i].RUTA_DOCUMENTO, PdfDocumentOpenMode.Import);
                    int numPaginas = 0;
                    int countPlant = docPlanti.PageCount;
                    for (int idx = 0; idx < countPlant; idx++)
                    {
                        PdfPage page = docPlanti.Pages[idx];
                        outputDocument.AddPage(page);
                        numPaginas++;
                    }
                }
            }

            MemoryStream ms = new MemoryStream();
            outputDocument.Save(ms);
            return File(ms.GetBuffer(), "application/pdf", "reporte.pdf");

        }

        private void DrawImage(XGraphics gfx, Stream imageEtiqueta, int x, int y, int width, int height)
        {
            XImage image = XImage.FromStream(imageEtiqueta);
            gfx.DrawImage(image, new System.Drawing.Point(x, y));
        }
        //
    }
}