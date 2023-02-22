
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.EnCicla.Models;
using SIM.Data;
using System.IO;
using System.Net.Http.Headers;
using System.Data;
using System.Transactions;
using System.Xml.Linq;
using System.Drawing.Imaging;
using System.Data.Entity.SqlServer;
using System.Reflection;
using System.Security.Claims;
using System.Data.Entity.Core.Objects;
using System.Web;
using System.Threading.Tasks;
using SIM.Areas.Seguridad.Models;
using System.Data.Entity;
using System.Globalization;
using System.Text.RegularExpressions;
using SIM.Data.EnCicla;

namespace SIM.Areas.EnCicla.Controllers
{
    public class ConsultaApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesEnCiclaSQL dbSIMEnCicla = new EntitiesEnCiclaSQL();

        public static IEnumerable<int> StringToIntList(string str)
        {
            if (String.IsNullOrEmpty(str))
                yield break;

            var items = str.Split(',');

            foreach (var s in items)
            {
                int num;
                if (int.TryParse(s, out num))
                    yield return num;
            }
        }

        [HttpGet, ActionName("ConsultaPrestamos")]
        public datosConsulta GetConsultaPrestamos(int tipoReporte, string fechaInicial, string fechaFinal, string horaInicial, string horaFinal, string estaciones, int idDocumentosAlmacenados, string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero = -1;
            int idUsuario = -1;
            Claim claimTercero;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            datosConsulta resultado;
            dynamic modelData;
            DateTime fechaInicialSel;
            DateTime fechaFinalSel;
            TimeSpan horaInicialSel;
            TimeSpan horaFinalSel;

            if (estaciones != null)
                estaciones = estaciones.Replace(" ", "");

            claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

            if (claimTercero != null)
            {
                idTercero = int.Parse(claimTercero.Value);
            }

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            if (idTercero == -1 || idUsuario == -1)
            {
                resultado = new datosConsulta();
                    resultado.numRegistros = 0;
                    resultado.datos = null;

                    return resultado;
            }

            if ((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)
            {
                resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var filtrosConsulta = (from dc in dbSIM.DATOS_CONSULTA
                                  where dc.IDDATOSCONSULTA == idDocumentosAlmacenados
                                  select dc).FirstOrDefault();

                if (filtrosConsulta != null)
                {
                    var documentos = filtrosConsulta.DATOS.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var estacionesSel = StringToIntList(estaciones);

                    fechaInicialSel = DateTime.ParseExact(fechaInicial, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    fechaFinalSel = DateTime.ParseExact(fechaFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1);
                    horaInicialSel = new TimeSpan(Convert.ToInt32(horaInicial.Split(new char[] {':'})[0]), Convert.ToInt32(horaInicial.Split(new char[] {':'})[1]), 0);
                    horaFinalSel = new TimeSpan(Convert.ToInt32(horaFinal.Split(new char[] { ':' })[0]), Convert.ToInt32(horaFinal.Split(new char[] { ':' })[1]), 0);

                    if (tipoReporte == 0) // Resumido
                    {
                        var model = (from prestamo in dbSIMEnCicla.I_HISTORICO_PRESTAMO
                                     join usuario in dbSIMEnCicla.P_USUARIOS on prestamo.Id_Usuario equals usuario.Id_Usuario
                                     join estacionO in dbSIMEnCicla.B_APARCAMIENTOS on prestamo.Id_Aparcamiento_Origen equals estacionO.Id_Aparcamiento
                                     join estacionD in dbSIMEnCicla.B_APARCAMIENTOS on prestamo.Id_Aparcamiento_Destino equals estacionD.Id_Aparcamiento
                                     where estacionesSel.Contains((int)prestamo.Id_Aparcamiento_Destino) && 
                                        documentos.Contains(usuario.DNI) &&
                                        prestamo.Fecha_Devolucion >= fechaInicialSel &&
                                        prestamo.Fecha_Devolucion < fechaFinalSel &&
                                        DbFunctions.CreateTime(((DateTime)prestamo.Fecha_Devolucion).Hour, ((DateTime)prestamo.Fecha_Devolucion).Minute, ((DateTime)prestamo.Fecha_Devolucion).Second) >= horaInicialSel &&
                                        DbFunctions.CreateTime(((DateTime)prestamo.Fecha_Devolucion).Hour, ((DateTime)prestamo.Fecha_Devolucion).Minute, ((DateTime)prestamo.Fecha_Devolucion).Second) <= horaFinalSel
                                     orderby prestamo.Fecha_Devolucion, usuario.DNI //, usuario.Nombre + " " + usuario.Apellido1 + " " + usuario.Apellido2
                                     group prestamo by new { 
                                         //FECHA = DbFunctions.TruncateTime(prestamo.Fecha_Devolucion), 
                                         ESTACION_ORIGEN = estacionO.Descripcion, 
                                         ESTACION_DESTINO = estacionD.Descripcion, 
                                         DOCUMENTO = usuario.DNI, 
                                         //USUARIO = usuario.Nombre + " " + usuario.Apellido1 + " " + usuario.Apellido2 
                                     } into prestamoDia
                                     select new
                                     {
                                         //FECHA = prestamoDia.Key.FECHA,
                                         ESTACION_ORIGEN = prestamoDia.Key.ESTACION_ORIGEN,
                                         ESTACION_DESTINO = prestamoDia.Key.ESTACION_DESTINO,
                                         DOCUMENTO = prestamoDia.Key.DOCUMENTO,
                                         //USUARIO = prestamoDia.Key.USUARIO,
                                         CANTIDAD = prestamoDia.Count()
                                     });
                        modelData = model;
                    }
                    else
                    {
                        var model = (from prestamo in dbSIMEnCicla.I_HISTORICO_PRESTAMO
                                     join usuario in dbSIMEnCicla.P_USUARIOS on prestamo.Id_Usuario equals usuario.Id_Usuario
                                     join estacionO in dbSIMEnCicla.B_APARCAMIENTOS on prestamo.Id_Aparcamiento_Origen equals estacionO.Id_Aparcamiento
                                     join estacionD in dbSIMEnCicla.B_APARCAMIENTOS on prestamo.Id_Aparcamiento_Destino equals estacionD.Id_Aparcamiento
                                     where estacionesSel.Contains((int)prestamo.Id_Aparcamiento_Destino) &&
                                        documentos.Contains(usuario.DNI) &&
                                        prestamo.Fecha_Devolucion >= fechaInicialSel &&
                                        prestamo.Fecha_Devolucion < fechaFinalSel &&
                                        DbFunctions.CreateTime(((DateTime)prestamo.Fecha_Devolucion).Hour, ((DateTime)prestamo.Fecha_Devolucion).Minute, ((DateTime)prestamo.Fecha_Devolucion).Second) >= horaInicialSel &&
                                        DbFunctions.CreateTime(((DateTime)prestamo.Fecha_Devolucion).Hour, ((DateTime)prestamo.Fecha_Devolucion).Minute, ((DateTime)prestamo.Fecha_Devolucion).Second) <= horaFinalSel
                                     orderby prestamo.Fecha_Devolucion
                                     select new
                                     {
                                         FECHA_PRESTAMO = prestamo.Fecha_Prestamo,
                                         FECHA_DEVOLUCION = prestamo.Fecha_Devolucion,
                                         ESTACION_ORIGEN = estacionO.Descripcion,
                                         ESTACION_DESTINO = estacionD.Descripcion,
                                         DOCUMENTO = usuario.DNI//,
                                         //USUARIO = usuario.Nombre + " " + usuario.Apellido1 + " " + usuario.Apellido2
                                     });
                        modelData = model;
                    }
                }
                else
                {
                    resultado = new datosConsulta();
                    resultado.numRegistros = 0;
                    resultado.datos = null;

                    return resultado;
                }
            }

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado = new datosConsulta();
            if (take == 0) // retorna todos los registros, normalmente cuando se está exportando a excel
            {
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.ToList();
            }
            else
            {
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();
            }

            LOG_CONSULTA logConsulta = new LOG_CONSULTA();

            logConsulta.IDUSUARIO = idUsuario;
            logConsulta.IDTERCERO = idTercero;
            logConsulta.FECHACONSULTA = DateTime.Now;
            logConsulta.FECHAINICIAL = fechaInicialSel;
            logConsulta.FECHAFINAL = fechaFinalSel;
            logConsulta.HORAINICIAL = new DateTime(2000, 1, 1) + horaInicialSel;
            logConsulta.HORAFINAL = new DateTime(2000, 1, 1) + horaFinalSel;
            logConsulta.ESTACIONES = estaciones;
            logConsulta.IDDATOSCONSULTA = idDocumentosAlmacenados;

            dbSIM.Entry(logConsulta).State = System.Data.Entity.EntityState.Added;
            try
            {
                dbSIM.SaveChanges();
            }
            catch (Exception error)
            {
            }

            return resultado;
        }

        // GET api/<controller>
        [HttpGet]
        [ActionName("DocumentosAlmacenados")]
        public datosConsulta GetDocumentosAlmacenados()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero = -1;
            Claim claimTercero;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

            if (claimTercero != null)
            {
                idTercero = int.Parse(claimTercero.Value);
            }

            var model = (from da in dbSIM.DATOS_CONSULTA
                         where da.NOMBRE != null && da.IDTERCERO == idTercero
                         orderby da.FECHA descending
                         select new
                         {
                             IDDATOSCONSULTA = da.IDDATOSCONSULTA,
                             S_NOMBRE = da.NOMBRE
                         });

            datosConsulta resultado = new datosConsulta();
            resultado.datos = model.ToList();

            return resultado;
        }

        /*
        [HttpGet]
        [ActionName("DocumentosAlmacenados")]
        public datosConsulta GetDocumentosAlmacenados(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;

            if ((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)
            {
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                switch (tipoData)
                {
                    case "l": // (l) lookup
                    default:
                        {
                            var model = (from da in dbSIM.DATOS_CONSULTA
                                         select new
                                         {
                                             ID_LOOKUP = da.IDDATOSCONSULTA,
                                             S_NOMBRE_LOOKUP = da.NOMBRE,
                                             FECHA = da.FECHA
                                         });

                            modelData = model;
                        }
                        break;
                }

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }*/

        // GET api/<controller>
        [HttpGet]
        [ActionName("Estaciones")]
        public datosConsulta GetEstaciones()
        {
            datosConsulta resultado;

            resultado = new datosConsulta();

            var estaciones = from estacion in dbSIMEnCicla.B_APARCAMIENTOS
                             orderby estacion.Descripcion
                             select new
                             {
                                 ID_ESTACION = estacion.Id_Aparcamiento,
                                 ESTACION = estacion.Descripcion
                             };

            resultado.numRegistros = estaciones.Count();
            resultado.datos = estaciones.ToList();

            return resultado;
        }

        /*
        [HttpPost]
        [ActionName("ArchivoDocumentos")]
        public HttpResponseMessage PostArchivoDocumentos()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    docfiles.Add(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }
        */

        [HttpPost, ActionName("ListaDocumentos")]
        public dynamic PostListaDocumentos(ListaDoc listaDocumentos)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int? idTercero = null;
            Claim claimTercero;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

            if (claimTercero != null)
            {
                idTercero = int.Parse(claimTercero.Value);
            }
            try
            {
                // Se crea el registro de Documentos para Almacenar
                DATOS_CONSULTA datosConsulta = new DATOS_CONSULTA();
                datosConsulta.IDTERCERO = (int)idTercero;
                datosConsulta.NOMBRE = null;
                datosConsulta.FECHA = DateTime.Now;
                datosConsulta.DATOS = "CC" + Regex.Replace(listaDocumentos.listaDocumentos, @"\s+", "").Replace(",", ",CC");

                dbSIM.Entry(datosConsulta).State = System.Data.Entity.EntityState.Added;
                try
                {
                    dbSIM.SaveChanges();
                }
                catch (Exception error)
                {
                    return -1;
                }

                return datosConsulta.IDDATOSCONSULTA;
            }
            catch (System.Exception e)
            {
                return -1;
            }
        }

        [HttpPost, ActionName("ArchivoDocumentos")]
        public async Task<object> PostArchivoDocumentos()
        {
            string nombreDocumentos;
            string documentos;
            string pathDocumentos =  System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Uploads");
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int? idTercero = null;
            Claim claimTercero;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

            if (claimTercero != null)
            {
                idTercero = int.Parse(claimTercero.Value);
            }

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new EnCiclaMultipartFormDataStreamProvider(pathDocumentos);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                nombreDocumentos = provider.FormData["nombreDocumentos"].ToString().Trim().ToUpper();

                documentos = System.IO.File.ReadAllText(pathDocumentos + "\\" + provider.FileData[0].Headers.ContentDisposition.FileName.Replace("\"", string.Empty));
                documentos = Regex.Replace(documentos, @"\r\n?|\n", ",");
                documentos = Regex.Replace(documentos, @"\s+", "");
                documentos = "CC" + documentos.Replace(",", ",CC");

                System.IO.File.Delete(pathDocumentos + "\\" + provider.FileData[0].Headers.ContentDisposition.FileName.Replace("\"", string.Empty));

                // Se crea el registro de Documentos para Almacenar
                DATOS_CONSULTA datosConsulta = new DATOS_CONSULTA();
                datosConsulta.IDTERCERO = (int)idTercero;
                datosConsulta.NOMBRE = nombreDocumentos;
                datosConsulta.FECHA = DateTime.Now;
                datosConsulta.DATOS = documentos;

                dbSIM.Entry(datosConsulta).State = System.Data.Entity.EntityState.Added;
                try
                {
                    dbSIM.SaveChanges();
                }
                catch (Exception error)
                {
                    return new { resp = "Error", mensaje = "Error Almacenando Archivo. " + error.Message, id = -1 };
                }

                return new { resp = "OK", mensaje = "", id = datosConsulta.IDDATOSCONSULTA };
            }
            catch (System.Exception e)
            {
                return null; // Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }

    public class EnCiclaMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public EnCiclaMultipartFormDataStreamProvider(string path)
            : base(path)
        {
        }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            //var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") : "NoName";
            var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";
            return name.Replace("\"", string.Empty); //this is here because Chrome submits files in quotation marks which get treated as part of the filename and get escaped
        }
    }

    /// <summary>
    /// Estructura que almacena el resultado de una consulta, entregando los datos y número de registros en total.
    /// </summary>
    public struct datosConsulta
    {
        public int numRegistros;
        public IEnumerable<dynamic> datos;
    }

    public class PruebaParametro
    {
        public int tipoReporte { set; get; }
    }

    public class FiltrosReporte
    {
        public int tipoReporte { set; get; }
        public string fechaInicial { set; get; }
        public string fechaFinal { set; get; }
        public string horaInicial { set; get; }
        public string horaFinal { set; get; }
        public string estaciones { set; get; }
        public int idDocumentosAlmacenados { set; get; }
        public string filter { set; get; }
        public string sort { set; get; }
        public string group { set; get; }
        public int skip { set; get; }
        public int take { set; get; }
        public string searchValue { set; get; }
        public string searchExpr { set; get; }
        public string comparation { set; get; }
        public string tipoData { set; get; }
        public bool noFilterNoRecords { set; get; }
    }

    public class ListaDoc
    {
        public string listaDocumentos { get; set; }
    }
}
