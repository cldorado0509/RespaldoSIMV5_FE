using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.Models;
using System.Security.Claims;
using SIM.Areas.Tramites.Models;
using SIM.Utilidades;
using System.IO;
using System.Web.Hosting;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using SIM.Data.Tramites;
using SIM.Models;

namespace SIM.Areas.Tramites.Controllers
{
    public class AnulacionDocumentoApiController : ApiController
    {
        public class DatosAnulacion
        {
            public int id;
            public int idP;
            public int? ma;
            public int fS;
            public string nfS;
            public string tS;
            public int? fJ;
            public string nfJ;
            public string tJ;
            public int? fAP;
            public string nfAP;
            public string tAP;
            public int? fAT;
            public string nfAT;
            public string tAT;
        }

        public struct DatosRespuesta
        {
            public string tipoRespuesta; // OK, Error
            public string detalleRespuesta;
        }


        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesTramitesOracle dbTramites = new EntitiesTramitesOracle();

        [Authorize]
        [HttpGet, ActionName("ConsultaDocumentosGenerados")]
        public datosConsulta GetConsultaDocumentosGenerados(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, bool noFilterNoRecords)
        {
            bool administrador = false;
            bool noAnulacion = false;
            dynamic modelData;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            List<int> seriesNoAnulacion = new List<int>();

            var idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

            var funcionario = Convert.ToInt32((from uf in dbSIM.USUARIO_FUNCIONARIO
                                           join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                           where uf.ID_USUARIO == idUsuario
                                           select f.CODFUNCIONARIO).FirstOrDefault());

            var dependienciaFuncionario = dbSIM.FUNCIONARIO_DEPENDENCIA.Where(df => df.CODFUNCIONARIO == funcionario).FirstOrDefault();

            if (dependienciaFuncionario != null)
            {
                var configDependenciasNoAnulacion = ConfigurationManager.AppSettings["DependenciasNoAnulacion"];

                if (configDependenciasNoAnulacion != null)
                {
                    foreach (string config in configDependenciasNoAnulacion.Split(';'))
                    {
                        if (config.Split('|')[0].Trim() == dependienciaFuncionario.ID_DEPENDENCIA.ToString())
                        {
                            noAnulacion = true;
                            seriesNoAnulacion = config.Split('|')[1].Split(',').Select(int.Parse).ToList();
                            break;
                        }
                    }
                }
            }

            // Es el usuario administrador
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                administrador = claimPpal.IsInRole("XANULACIONDOCUMENTO");
            }

            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && administrador))
            {
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var model = (from pd in dbSIM.PROYECCION_DOC
                             join ud in dbSIM.TBSERIE on pd.CODSERIE equals ud.CODSERIE
                             join f in dbSIM.TBFUNCIONARIO on (decimal)pd.CODFUNCIONARIO equals f.CODFUNCIONARIO
                             join rd in dbSIM.RADICADO_DOCUMENTO on (int?)pd.ID_RADICADODOC equals (int)rd.ID_RADICADODOC into rdd
                             from pdt in rdd.DefaultIfEmpty()
                             join ad in dbSIM.ANULACION_DOC.Where(adi => adi.S_FORMULARIO != "54") on pd.ID_PROYECCION_DOC equals (int)ad.ID_PROYECCION_DOC into add
                             from adt in add.DefaultIfEmpty()
                                 //where (pd.CODFUNCIONARIO == funcionario || fi.CODFUNCIONARIO == funcionario) && pd.S_FORMULARIO == "22"
                             where pd.S_FORMULARIO == "22" && (administrador || pd.CODFUNCIONARIO == funcionario)
                             orderby pd.D_FECHA_TRAMITE descending
                             select new
                             {
                                 DIA = ((DateTime)pd.D_FECHA_TRAMITE).Day,
                                 MES = ((DateTime)pd.D_FECHA_TRAMITE).Month,
                                 ANO = ((DateTime)pd.D_FECHA_TRAMITE).Year,
                                 pd.ID_PROYECCION_DOC,
                                 pd.S_DESCRIPCION,
                                 S_SERIE = ud.NOMBRE,
                                 pd.S_TRAMITES,
                                 pd.S_PROCESOS,
                                 f.CODFUNCIONARIO,
                                 S_FUNCIONARIO = f.NOMBRES + " " + f.APELLIDOS,
                                 pd.ID_RADICADODOC,
                                 pdt.S_RADICADO,
                                 S_ESTADO = (pdt.S_ESTADO == "N" ? "ANULADO" : (adt != null ? "EN PROCESO" : "ACTIVO")),
                                 S_ESTADODOC = (pdt.S_ESTADO == "N" ? "A" : (adt != null ? "P" : "OK")),
                                 S_PERMITEANULAR = (noAnulacion && seriesNoAnulacion.Contains(pd.CODSERIE) ? "N" : "S")
                             });

                modelData = model;

                // Obtiene consulta linq dinámicamente de acuerdo a los filtros establecidos
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0)
                    resultado.datos = modelFiltered.ToList();
                else
                    resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        [Authorize]
        [HttpGet, ActionName("ConsultaSolicitudes")]
        public datosConsulta GetConsultaSolicitudes()
        {
            int idUsuario;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

                var funcionario = (from uf in dbSIM.USUARIO_FUNCIONARIO
                                   join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                   where uf.ID_USUARIO == idUsuario
                                   select f.CODFUNCIONARIO).FirstOrDefault();

                var model = (from ad in dbSIM.ANULACION_DOC
                             join pd in dbSIM.PROYECCION_DOC on ad.ID_PROYECCION_DOC equals pd.ID_PROYECCION_DOC
                             join ud in dbSIM.TBSERIE on pd.CODSERIE equals ud.CODSERIE
                             join rd in dbSIM.RADICADO_DOCUMENTO on (int?)pd.ID_RADICADODOC equals (int)rd.ID_RADICADODOC into rdd
                             from pdt in rdd.DefaultIfEmpty()
                             join uf in dbSIM.USUARIO_FUNCIONARIO on ad.CODFUNCIONARIO equals uf.CODFUNCIONARIO
                             join u in dbSIM.USUARIO on uf.ID_USUARIO equals u.ID_USUARIO
                             join ufa in dbSIM.USUARIO_FUNCIONARIO on ad.CODFUNCIONARIO_ACTUAL equals ufa.CODFUNCIONARIO
                             join ua in dbSIM.USUARIO on ufa.ID_USUARIO equals ua.ID_USUARIO
                             join f in dbSIM.TBFUNCIONARIO on (decimal)ad.CODFUNCIONARIO equals f.CODFUNCIONARIO
                             join fa in dbSIM.TBFUNCIONARIO on (decimal)ad.CODFUNCIONARIO_ACTUAL equals fa.CODFUNCIONARIO
                             where uf.ID_USUARIO == idUsuario
                             orderby ad.D_FECHA_SOLICITUD descending
                             select new
                             {
                                 ad.ID_ANULACION_DOC,
                                 ad.ID_PROYECCION_DOC,
                                 pd.S_DESCRIPCION,
                                 ad.D_FECHA_SOLICITUD,
                                 ad.D_FECHA_FINALIZACION,
                                 S_SERIE = ud.NOMBRE,
                                 pd.ID_RADICADODOC,
                                 pdt.S_RADICADO,
                                 ad.ID_MOTIVO_ANULACION,
                                 S_MOTIVO_ANULACION = ad.MOTIVO_ANULACION.S_DESCRIPCION,
                                 ad.S_TRAMITES,
                                 ad.S_PROCESOS,
                                 f.CODFUNCIONARIO,
                                 S_FUNCIONARIO = f.NOMBRES + " " + f.APELLIDOS,
                                 S_FUNCIONARIO_ACTUAL = fa.NOMBRES + " " + fa.APELLIDOS,
                                 ad.S_ESTADO,
                                 S_ESTADO_DESCRIPCION = (ad.S_FORMULARIO == "51" ? "JUSTIFICACIÓN" : (ad.S_FORMULARIO == "52" ? "AUTORIZACIÓN" : (ad.S_FORMULARIO == "53" ? "APROBACIÓN ATU" : (ad.S_ESTADO == "R" ? "RECHAZADO" : "ANULADO"))))
                             }).Distinct();

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = model.Count();
                resultado.datos = model.ToList();

                return resultado;
            }
            else
            {
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
        }

        [Authorize]
        [HttpGet, ActionName("ConsultaDocumentos")]
        public datosConsulta GetConsultaDocumentos(int tipo)
        {
            int idUsuario;
            string formulario = (tipo == 1 ? "51" : (tipo == 2 ? "52" : (tipo == 3 ? "53" : "00")));
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

                var funcionario = (from uf in dbSIM.USUARIO_FUNCIONARIO
                                   join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                   where uf.ID_USUARIO == idUsuario
                                   select f.CODFUNCIONARIO).FirstOrDefault();

                var model = (from ad in dbSIM.ANULACION_DOC
                             join pd in dbSIM.PROYECCION_DOC on ad.ID_PROYECCION_DOC equals pd.ID_PROYECCION_DOC
                             join ud in dbSIM.TBSERIE on pd.CODSERIE equals ud.CODSERIE
                             join rd in dbSIM.RADICADO_DOCUMENTO on (int?)pd.ID_RADICADODOC equals (int)rd.ID_RADICADODOC into rdd
                             from pdt in rdd.DefaultIfEmpty()
                             join uf in dbSIM.USUARIO_FUNCIONARIO on ad.CODFUNCIONARIO equals uf.CODFUNCIONARIO
                             join u in dbSIM.USUARIO on uf.ID_USUARIO equals u.ID_USUARIO
                             join ufa in dbSIM.USUARIO_FUNCIONARIO on ad.CODFUNCIONARIO_ACTUAL equals ufa.CODFUNCIONARIO
                             join ua in dbSIM.USUARIO on ufa.ID_USUARIO equals ua.ID_USUARIO
                             join f in dbSIM.TBFUNCIONARIO on (decimal)ad.CODFUNCIONARIO equals f.CODFUNCIONARIO
                             join fa in dbSIM.TBFUNCIONARIO on (decimal)ad.CODFUNCIONARIO_ACTUAL equals fa.CODFUNCIONARIO
                             where ufa.ID_USUARIO == idUsuario && ad.S_FORMULARIO == formulario
                             orderby ad.ID_ANULACION_DOC descending
                             select new
                             {
                                 ad.ID_ANULACION_DOC,
                                 ad.ID_PROYECCION_DOC,
                                 pd.S_DESCRIPCION,
                                 ad.D_FECHA_SOLICITUD,
                                 S_SERIE = ud.NOMBRE,
                                 pd.ID_RADICADODOC,
                                 pdt.S_RADICADO,
                                 ad.ID_MOTIVO_ANULACION,
                                 S_MOTIVO_ANULACION = ad.MOTIVO_ANULACION.S_DESCRIPCION,
                                 pd.S_TRAMITES,
                                 pd.S_PROCESOS,
                                 f.CODFUNCIONARIO,
                                 S_FUNCIONARIO = f.NOMBRES + " " + f.APELLIDOS,
                                 S_FUNCIONARIO_ACTUAL = fa.NOMBRES + " " + fa.APELLIDOS,
                                 S_ACTUAL = (funcionario == ad.CODFUNCIONARIO_ACTUAL ? "S" : "N"),
                             }).Distinct();

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = model.Count();
                resultado.datos = model.ToList();

                return resultado;
            }
            else
            {
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
        }

        [Authorize]
        [HttpGet, ActionName("ObtenerDatosAnulacionDocumento")]
        public dynamic GetObtenerDatosAnulacionDocumento(int id, int idP)
        {
            DatosAnulacion datosAnulacion = null;

            if (id > 0)
            {
                datosAnulacion = (from a in dbSIM.ANULACION_DOC
                                  where a.ID_ANULACION_DOC == id
                                  select new DatosAnulacion
                                  {
                                      id = a.ID_ANULACION_DOC,
                                      idP = (int)a.ID_PROYECCION_DOC,
                                      ma = a.ID_MOTIVO_ANULACION,
                                      fS = a.CODFUNCIONARIO,
                                      nfS = (from fsn in dbSIM.TBFUNCIONARIO where fsn.CODFUNCIONARIO == a.CODFUNCIONARIO select fsn.NOMBRES + " " + fsn.APELLIDOS).FirstOrDefault(),
                                      tS = a.S_SOLICITUD,
                                      fJ = a.CODFUNCIONARIO_JUSTIFICACION,
                                      nfJ = (from fjn in dbSIM.TBFUNCIONARIO where fjn.CODFUNCIONARIO == a.CODFUNCIONARIO_JUSTIFICACION select fjn.NOMBRES + " " + fjn.APELLIDOS).FirstOrDefault(),
                                      tJ = a.S_JUSTIFICACION,
                                      fAP = a.CODFUNCIONARIO_APROBACION,
                                      nfAP = (from fapn in dbSIM.TBFUNCIONARIO where fapn.CODFUNCIONARIO == a.CODFUNCIONARIO_APROBACION select fapn.NOMBRES + " " + fapn.APELLIDOS).FirstOrDefault(),
                                      tAP = a.S_APROBACION,
                                      fAT = a.CODFUNCIONARIO_ATU,
                                      nfAT = (from fatn in dbSIM.TBFUNCIONARIO where fatn.CODFUNCIONARIO == a.CODFUNCIONARIO_ATU select fatn.NOMBRES + " " + fatn.APELLIDOS).FirstOrDefault(),
                                      tAT = a.S_ATU
                                  }
                                    ).FirstOrDefault();
            }
            else
            {
                int idUsuario = 0;
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

                try
                {
                    if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                    {
                        idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                    }

                    var funcionario = (from f in dbSIM.TBFUNCIONARIO
                                      join uf in dbSIM.USUARIO_FUNCIONARIO on f.CODFUNCIONARIO equals uf.CODFUNCIONARIO
                                      where uf.ID_USUARIO == idUsuario
                                       select f).FirstOrDefault();

                    datosAnulacion = new DatosAnulacion
                    {
                        id = 0,
                        idP = idP,
                        ma = null,
                        fS = Convert.ToInt32(funcionario.CODFUNCIONARIO),
                        nfS = funcionario.NOMBRES + " " + funcionario.APELLIDOS,
                        tS = "",
                        fJ = 0,
                        nfJ = "",
                        tJ = "",
                        fAP = 0,
                        nfAP = "",
                        tAP = "",
                        fAT = 0,
                        nfAT = "",
                        tAT = ""
                    };
                }
                catch (Exception error)
                {
                    Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "AnulacionDocumento [GetObtenerDatosAnulacionDocumento] : Se presentó un error cargando los datos.\r\n" + Utilidades.LogErrores.ObtenerError(error));
                }
            }

            return datosAnulacion;
        }

        [Authorize]
        [HttpGet, ActionName("ObtenerMotivosAnulacion")]
        public dynamic GetObtenerMotivosAnulacion()
        {
            var motivosAnulacion = from ma in dbSIM.MOTIVO_ANULACION
                                   orderby ma.S_DESCRIPCION ascending
                                   select new
                                   {
                                       ma.ID_MOTIVO_ANULACION,
                                       ma.S_DESCRIPCION
                                   };

            return motivosAnulacion.ToList();
        }

        [Authorize]
        [HttpGet, ActionName("FuncionariosAutorizacion")]
        public dynamic GetFuncionariosAutorizacion()
        {
            var funcionariosAutorizacion = from tr in dbSIM.TBTAREARESPONSABLE
                                            join f in dbSIM.TBFUNCIONARIO on tr.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                           where tr.CODTAREA == 4929
                                           //*************&&&&where tr.CODTAREA == 4468 // Pruebas
                                           orderby f.NOMBRES + " " + f.APELLIDOS
                                           select new
                                            {
                                                f.CODFUNCIONARIO,
                                                S_FUNCIONARIO = f.NOMBRES + " " + f.APELLIDOS
                                            };

            return funcionariosAutorizacion.ToList();
        }

        [Authorize]
        [HttpGet, ActionName("FuncionariosATU")]
        public dynamic GetFuncionariosATU()
        {
            var funcionariosATU = from tr in dbSIM.TBTAREARESPONSABLE
                                           join f in dbSIM.TBFUNCIONARIO on tr.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                  where tr.CODTAREA == 4928
                                  //*************&&&&where tr.CODTAREA == 4469 // Pruebas
                                  orderby f.NOMBRES + " " + f.APELLIDOS
                                           select new
                                           {
                                               f.CODFUNCIONARIO,
                                               S_FUNCIONARIO = f.NOMBRES + " " + f.APELLIDOS
                                           };

            return funcionariosATU.ToList();
        }

        [Authorize]
        [HttpPost, ActionName("AlmacenarDatosAnulacionDocumento")]
        public string PostAlmacenarDatosAnulacionDocumento(DatosAnulacion datos)
        {
            int idUsuario = 0;
            int idAnulacionDocumento = 0;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            try
            {
                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                }

                if (idUsuario == 0)
                {
                    return "ERROR: El Usuario no se encuentra autenticado.";
                }

                ANULACION_DOC anulacionDocumento;

                if (datos.id == 0) // Nueva Anulacion
                {
                    var proyeccion = dbSIM.PROYECCION_DOC.Where(p => p.ID_PROYECCION_DOC == datos.idP).FirstOrDefault();

                    int codFuncionario = dbSIM.USUARIO_FUNCIONARIO.Where(uf => uf.ID_USUARIO == idUsuario).Select(uf => uf.CODFUNCIONARIO).FirstOrDefault();

                    var codProceso = Convert.ToDecimal(SIM.Utilidades.Data.ObtenerValorParametro("ProcesoAnulacionDocumento"));
                    var codFuncionarioTN = codFuncionario;
                    ObjectParameter respCodTramite = new ObjectParameter("respCodTramite", typeof(decimal));
                    ObjectParameter respCodTarea = new ObjectParameter("respCodTarea", typeof(decimal));
                    ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));

                    dbTramites.SP_NUEVO_TRAMITE(codProceso, 0, codFuncionarioTN, "Anulación Documento - " + proyeccion.S_DESCRIPCION + " - Solicitud", respCodTramite, respCodTarea, rtaResultado);
                    dbTramites.SP_AVANZA_TAREA_FORMULARIO(Convert.ToDecimal(respCodTramite.Value), Convert.ToDecimal(respCodTarea.Value), 0, proyeccion.CODFUNCIONARIO, "51", "0", "Anulación Documento - " + proyeccion.S_DESCRIPCION + " - Justificación", rtaResultado);

                    anulacionDocumento = new ANULACION_DOC();

                    anulacionDocumento.ID_PROYECCION_DOC = proyeccion.ID_PROYECCION_DOC;
                    anulacionDocumento.ID_MOTIVO_ANULACION = (int)datos.ma;
                    anulacionDocumento.S_FORMULARIO = "51";
                    anulacionDocumento.S_SOLICITUD = datos.tS;
                    anulacionDocumento.D_FECHA_SOLICITUD = DateTime.Now;
                    anulacionDocumento.CODFUNCIONARIO = codFuncionario;
                    anulacionDocumento.CODFUNCIONARIO_ACTUAL = proyeccion.CODFUNCIONARIO;
                    anulacionDocumento.CODFUNCIONARIO_JUSTIFICACION = proyeccion.CODFUNCIONARIO;
                    anulacionDocumento.S_ESTADO = "P";
                    anulacionDocumento.CODTRAMITE_ANULACION = Convert.ToInt32(respCodTramite.Value);

                    dbSIM.Entry(anulacionDocumento).State = EntityState.Added;

                    dbSIM.SaveChanges();

                    idAnulacionDocumento = anulacionDocumento.ID_ANULACION_DOC;
                }
                else
                {
                    idAnulacionDocumento = datos.id;

                    anulacionDocumento = dbSIM.ANULACION_DOC.Where(ad => ad.ID_ANULACION_DOC == datos.id).FirstOrDefault();

                    switch (anulacionDocumento.S_FORMULARIO)
                    {
                        case "50": // Solicitud
                            anulacionDocumento.ID_MOTIVO_ANULACION = (int)datos.ma;
                            anulacionDocumento.S_SOLICITUD = datos.tS;
                            anulacionDocumento.D_FECHA_SOLICITUD = DateTime.Now;
                            break;
                        case "51": // Justificación
                            anulacionDocumento.S_JUSTIFICACION = datos.tJ;
                            anulacionDocumento.CODFUNCIONARIO_APROBACION = datos.fAP;
                            break;
                        case "52": // Autorizacion
                            anulacionDocumento.S_APROBACION = datos.tAP;
                            anulacionDocumento.CODFUNCIONARIO_ATU = datos.fAT;
                            break;
                        case "53": // Aprobacion ATU
                            anulacionDocumento.S_ATU = datos.tAT;
                            break;
                    }

                    dbSIM.Entry(anulacionDocumento).State = EntityState.Modified;
                    dbSIM.SaveChanges();
                }

            }
            catch (Exception error)
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "AnulacionDocumento [PostAlmacenarDatosAnulacionDocumento] : Se presentó un error almacenando la información de la Anulación.\r\n" + Utilidades.LogErrores.ObtenerError(error));
                return "ERROR: " + idAnulacionDocumento.ToString() + "<br/>Se presentó un error almacenado los datos ingresados.";
            }

            return "OK";
        }

        [Authorize]
        [HttpGet, ActionName("AvanzarDocumento")]
        public string GetAvanzarDocumento(int id)
        {
            int idUsuario = 0;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            try
            {
                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                }

                if (idUsuario == 0)
                {
                    return "ERROR:El Usuario no se encuentra autenticado.";
                }

                ANULACION_DOC anulacionDocumento = (from ad in dbSIM.ANULACION_DOC
                                                    where ad.ID_ANULACION_DOC == id
                                                    select ad).FirstOrDefault();

                switch (anulacionDocumento.S_FORMULARIO)
                {
                    case "51": // Justificacion
                        if (anulacionDocumento.S_JUSTIFICACION == null || anulacionDocumento.S_JUSTIFICACION.Trim() == "")
                        {
                            return "ERROR:Las Observaciones de la Justificación son Requeridas.";
                        }

                        if (anulacionDocumento.CODFUNCIONARIO_APROBACION == null)
                        {
                            return "ERROR:Funcionario de Autorización Requerido.";
                        }
                        break;
                    case "52": // Autorización
                        if (anulacionDocumento.S_APROBACION == null || anulacionDocumento.S_APROBACION.Trim() == "")
                        {
                            return "ERROR:Las Observaciones de la Autorización son Requeridas.";
                        }

                        if (anulacionDocumento.CODFUNCIONARIO_ATU == null)
                        {
                            return "ERROR:Funcionario de Aprobación ATU Requerido.";
                        }
                        break;
                    case "53": // ATU
                        if (anulacionDocumento.S_ATU == null || anulacionDocumento.S_ATU.Trim() == "")
                        {
                            return "ERROR:Las Observaciones de la Aprobación de ATU son Requeridas.";
                        }
                        break;
                }

                var proyeccion = dbSIM.PROYECCION_DOC.Where(p => p.ID_PROYECCION_DOC == anulacionDocumento.ID_PROYECCION_DOC).FirstOrDefault();

                int codFuncionario = dbSIM.USUARIO_FUNCIONARIO.Where(uf => uf.ID_USUARIO == idUsuario).Select(uf => uf.CODFUNCIONARIO).FirstOrDefault();

                ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));

                string formularioSiguiente = (Convert.ToInt32(anulacionDocumento.S_FORMULARIO) + 1).ToString();
                string textoFormularioSiguiente = (formularioSiguiente == "52" ? "Autorización" : (formularioSiguiente == "53" ? "Aprobación ATU" : (formularioSiguiente == "54" ? "Fin Tramite" : "[Error]")));
                int funcionarioSiguiente = (formularioSiguiente == "52" ? (int)anulacionDocumento.CODFUNCIONARIO_APROBACION : (formularioSiguiente == "53" ? (int)anulacionDocumento.CODFUNCIONARIO_ATU : (formularioSiguiente == "54" ? (int)anulacionDocumento.CODFUNCIONARIO : -1)));

                if (funcionarioSiguiente == -1)
                {
                    return "ERROR: " + id.ToString() + "<br/>Se presentó un error avanzando el documento. Funcionario Inválido.";
                }
                else
                {
                    dbTramites.SP_AVANZA_TAREA_FORMULARIO(Convert.ToDecimal(anulacionDocumento.CODTRAMITE_ANULACION), 0, 0, funcionarioSiguiente, formularioSiguiente, "0", "Anulación Documento - " + proyeccion.S_DESCRIPCION + " - " + textoFormularioSiguiente, rtaResultado);

                    if (anulacionDocumento.S_FORMULARIO == "51")
                    {
                        anulacionDocumento.D_FECHA_JUSTIFICACION = DateTime.Now;
                        anulacionDocumento.S_FORMULARIO = "52";
                    }
                    else if (anulacionDocumento.S_FORMULARIO == "52")
                    {
                        anulacionDocumento.D_FECHA_APROBACION = DateTime.Now;
                        anulacionDocumento.S_FORMULARIO = "53";
                    }
                    else if (anulacionDocumento.S_FORMULARIO == "53")
                    {
                        anulacionDocumento.D_FECHA_FINALIZACION = DateTime.Now;
                        anulacionDocumento.S_FORMULARIO = "54";
                        anulacionDocumento.S_ESTADO = "A";

                        // Se marca como anulado y se regenera el documento con la marca de agua

                        if (proyeccion.ID_RADICADODOC != null && proyeccion.ID_RADICADODOC > 0)
                        {
                            var radicadoDocumento = (from dr in dbSIM.RADICADO_DOCUMENTO
                                                     where dr.ID_RADICADODOC == proyeccion.ID_RADICADODOC
                                                     select dr).FirstOrDefault();

                            radicadoDocumento.S_ESTADO = "N";

                            dbSIM.Entry(radicadoDocumento).State = EntityState.Modified;
                            // dbSIM.SaveChanges();
                        }

                        TramitesLibrary tramitesAnular = new TramitesLibrary();

                        // Se ingresa el comentario a cada trámite en la tarea en la que actualmente se encuentan
                        foreach (var tramiteProyeccion in proyeccion.TRAMITES_PROYECCION)
                        {
                            TBTRAMITETAREA tramiteTarea = (from tt in dbSIM.TBTRAMITETAREA
                                                           where tt.CODTRAMITE == tramiteProyeccion.CODTRAMITE && tt.COPIA == 0
                                                           orderby tt.FECHAINI descending
                                                           select tt).FirstOrDefault();

                            TBTAREACOMENTARIO tareaComentario = new TBTAREACOMENTARIO();
                            tareaComentario.CODTRAMITE = tramiteProyeccion.CODTRAMITE;
                            tareaComentario.CODTAREA = tramiteTarea.CODTAREA;
                            tareaComentario.FECHA = DateTime.Now;
                            tareaComentario.CODFUNCIONARIO = codFuncionario;
                            tareaComentario.IMPORTANCIA = "0";
                            tareaComentario.COMENTARIO = anulacionDocumento.S_ATU;

                            dbSIM.Entry(tareaComentario).State = EntityState.Added;
                            //dbSIM.SaveChanges();

                            var tramiteDocumento = (from td in dbSIM.TBTRAMITEDOCUMENTO
                                                where td.CODTRAMITE == tramiteProyeccion.CODTRAMITE && td.CODDOCUMENTO == tramiteProyeccion.CODDOCUMENTO
                                                select td).FirstOrDefault();

                            tramiteDocumento.S_ESTADO = "N";

                            dbSIM.Entry(tramiteDocumento).State = EntityState.Modified;
                            dbSIM.SaveChanges();

                            tramitesAnular.DocumentoMarcaAgua(tramiteDocumento.RUTA, "ANULADO", true);
                        }
                    }

                    anulacionDocumento.CODFUNCIONARIO_ACTUAL = funcionarioSiguiente;

                    dbSIM.Entry(anulacionDocumento).State = EntityState.Modified;

                    dbSIM.SaveChanges();
                }
            }
            catch (Exception error)
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "AnulacionDocumento [GetAvanzarDocumento] : Se presentó un error avanzando el trámite de la Anulación.\r\n" + Utilidades.LogErrores.ObtenerError(error));
                return "ERROR: " + id.ToString() + "<br/>Se presentó un error avanzando el documento.";
            }

            return "OK";
        }

        [Authorize]
        [HttpGet, ActionName("GenerarAnulacionDocumento")]
        public string GetGenerarAnulacionDocumento(int id, int ict) // ict = 1 Insertar Comentarios Trámite
        {
            int idUsuario = 0;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            try
            {
                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                }

                if (idUsuario == 0)
                {
                    return "ERROR:El Usuario no se encuentra autenticado.";
                }

                ANULACION_DOC anulacionDocumento = (from ad in dbSIM.ANULACION_DOC
                                                    where ad.ID_ANULACION_DOC == id
                                                    select ad).FirstOrDefault();

                if (anulacionDocumento.S_FORMULARIO != "54")
                    return "ERROR:Anulación aun pendiente por finalizar.";

                var proyeccion = dbSIM.PROYECCION_DOC.Where(p => p.ID_PROYECCION_DOC == anulacionDocumento.ID_PROYECCION_DOC).FirstOrDefault();

                int codFuncionario = (int)proyeccion.CODFUNCIONARIO_ACTUAL;
                int funcionarioSiguiente = (int)anulacionDocumento.CODFUNCIONARIO;

                // Se marca como anulado y se regenera el documento con la marca de agua

                if (proyeccion.ID_RADICADODOC != null && proyeccion.ID_RADICADODOC > 0)
                {
                    var radicadoDocumento = (from dr in dbSIM.RADICADO_DOCUMENTO
                                             where dr.ID_RADICADODOC == proyeccion.ID_RADICADODOC
                                             select dr).FirstOrDefault();

                    radicadoDocumento.S_ESTADO = "N";

                    dbSIM.Entry(radicadoDocumento).State = EntityState.Modified;
                }

                TramitesLibrary tramitesAnular = new TramitesLibrary();

                // Se ingresa el comentario a cada trámite en la tarea en la que actualmente se encuentan
                foreach (var tramiteProyeccion in proyeccion.TRAMITES_PROYECCION)
                {
                    TBTRAMITETAREA tramiteTarea = (from tt in dbSIM.TBTRAMITETAREA
                                                   where tt.CODTRAMITE == tramiteProyeccion.CODTRAMITE && tt.COPIA == 0
                                                   orderby tt.FECHAINI descending
                                                   select tt).FirstOrDefault();

                    if (ict == 1)
                    {
                        TBTAREACOMENTARIO tareaComentario = new TBTAREACOMENTARIO();
                        tareaComentario.CODTRAMITE = tramiteProyeccion.CODTRAMITE;
                        tareaComentario.CODTAREA = tramiteTarea.CODTAREA;
                        tareaComentario.FECHA = DateTime.Now;
                        tareaComentario.CODFUNCIONARIO = codFuncionario;
                        tareaComentario.IMPORTANCIA = "0";
                        tareaComentario.COMENTARIO = anulacionDocumento.S_ATU;

                        dbSIM.Entry(tareaComentario).State = EntityState.Added;
                    }

                    var tramiteDocumento = (from td in dbSIM.TBTRAMITEDOCUMENTO
                                            where td.CODTRAMITE == tramiteProyeccion.CODTRAMITE && td.CODDOCUMENTO == tramiteProyeccion.CODDOCUMENTO
                                            select td).FirstOrDefault();

                    tramiteDocumento.S_ESTADO = "N";

                    dbSIM.Entry(tramiteDocumento).State = EntityState.Modified;

                    tramitesAnular.DocumentoMarcaAgua(tramiteDocumento.RUTA, "ANULADO", true);
                }

                anulacionDocumento.CODFUNCIONARIO_ACTUAL = funcionarioSiguiente;

                dbSIM.Entry(anulacionDocumento).State = EntityState.Modified;

                dbSIM.SaveChanges();
            }
            catch (Exception error)
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "AnulacionDocumento [GetGenerarAnulacionDocumento (" + id.ToString() + "] : Se presentó un error generando el documento de anulación.\r\n" + Utilidades.LogErrores.ObtenerError(error));
                return "ERROR: " + id.ToString() + "<br/>Se presentó un error avanzando el documento.";
            }

            return "OK";
        }

        [HttpGet, ActionName("MarcaAguaAnulacionDocumento")]
        public string GetMarcaAguaAnulacionDocumento(int id) // ict = 1 Insertar Comentarios Trámite
        {
            try
            {
                ANULACION_DOC anulacionDocumento = (from ad in dbSIM.ANULACION_DOC
                                                    where ad.ID_ANULACION_DOC == id
                                                    select ad).FirstOrDefault();

                if (anulacionDocumento.S_FORMULARIO != "54")
                    return "ERROR:Anulación aun pendiente por finalizar.";

                var proyeccion = dbSIM.PROYECCION_DOC.Where(p => p.ID_PROYECCION_DOC == anulacionDocumento.ID_PROYECCION_DOC).FirstOrDefault();

                TramitesLibrary tramitesAnular = new TramitesLibrary();

                // Se ingresa el comentario a cada trámite en la tarea en la que actualmente se encuentan
                foreach (var tramiteProyeccion in proyeccion.TRAMITES_PROYECCION)
                {
                    var tramiteDocumento = (from td in dbSIM.TBTRAMITEDOCUMENTO
                                            where td.CODTRAMITE == tramiteProyeccion.CODTRAMITE && td.CODDOCUMENTO == tramiteProyeccion.CODDOCUMENTO
                                            select td).FirstOrDefault();

                    tramitesAnular.DocumentoMarcaAgua(tramiteDocumento.RUTA, "ANULADO", true);
                }
            }
            catch (Exception error)
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "AnulacionDocumento [GetGenerarAnulacionDocumento (" + id.ToString() + "] : Se presentó un error generando el documento de anulación.\r\n" + Utilidades.LogErrores.ObtenerError(error));
                return "ERROR: " + id.ToString() + "<br/>Se presentó un error con la marca de agua.";
            }

            return "OK";
        }

        [Authorize]
        [HttpGet, ActionName("RechazarDocumento")]
        public string GetRechazarDocumento(int id)
        {
            int idUsuario = 0;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            string comentarioRechazo = "";

            try
            {
                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                }

                if (idUsuario == 0)
                {
                    return "ERROR:El Usuario no se encuentra autenticado.";
                }

                ANULACION_DOC anulacionDocumento = (from ad in dbSIM.ANULACION_DOC
                                                    where ad.ID_ANULACION_DOC == id
                                                    select ad).FirstOrDefault();

                switch (anulacionDocumento.S_FORMULARIO)
                {
                    case "51": // Justificacion
                        if (anulacionDocumento.S_JUSTIFICACION == null || anulacionDocumento.S_JUSTIFICACION.Trim() == "")
                        {
                            return "ERROR:Las Observaciones de la Justificación son Requeridas.";
                        }

                        comentarioRechazo = anulacionDocumento.S_JUSTIFICACION;

                        break;
                    case "52": // Autorización
                        if (anulacionDocumento.S_APROBACION == null || anulacionDocumento.S_APROBACION.Trim() == "")
                        {
                            return "ERROR:Las Observaciones de la Autorización son Requeridas.";
                        }

                        comentarioRechazo = anulacionDocumento.S_APROBACION;
                        break;
                    case "53": // ATU
                        if (anulacionDocumento.S_ATU == null || anulacionDocumento.S_ATU.Trim() == "")
                        {
                            return "ERROR:Las Observaciones de la Aprobación de ATU son Requeridas.";
                        }

                        comentarioRechazo = anulacionDocumento.S_ATU;
                        break;
                }

                var proyeccion = dbSIM.PROYECCION_DOC.Where(p => p.ID_PROYECCION_DOC == anulacionDocumento.ID_PROYECCION_DOC).FirstOrDefault();

                int codFuncionario = dbSIM.USUARIO_FUNCIONARIO.Where(uf => uf.ID_USUARIO == idUsuario).Select(uf => uf.CODFUNCIONARIO).FirstOrDefault();

                ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));

                string formularioSiguiente = "54";
                string textoFormularioSiguiente = "Rechazo Anulación";
                int funcionarioSiguiente = (int)anulacionDocumento.CODFUNCIONARIO;

                if (funcionarioSiguiente == -1)
                {
                    return "ERROR: " + id.ToString() + "<br/>Se presentó un error rechazando la solicitud de anulación. Funcionario Inválido.";
                }
                else
                {
                    dbTramites.SP_AVANZA_TAREA_FORMULARIO(Convert.ToDecimal(anulacionDocumento.CODTRAMITE_ANULACION), 0, 0, funcionarioSiguiente, formularioSiguiente, "0", "Anulación Documento - " + proyeccion.S_DESCRIPCION + " - " + textoFormularioSiguiente, rtaResultado);

                    anulacionDocumento.D_FECHA_FINALIZACION = DateTime.Now;
                    //anulacionDocumento.CODFUNCIONARIO_ACTUAL = funcionarioSiguiente;
                    anulacionDocumento.S_FORMULARIO = "54";
                    anulacionDocumento.S_ESTADO = "R";

                    dbSIM.Entry(anulacionDocumento).State = EntityState.Modified;

                    dbSIM.SaveChanges();

                    // Se ingresa el comentario a cada trámite en la tarea en la que actualmente se encuentan
                    foreach (var tramiteProyeccion in proyeccion.TRAMITES_PROYECCION)
                    {
                        TBTRAMITETAREA tramiteTarea = (from tt in dbSIM.TBTRAMITETAREA
                                                       where tt.CODTRAMITE == tramiteProyeccion.CODTRAMITE && tt.COPIA == 0
                                                       orderby tt.FECHAINI descending
                                                       select tt).FirstOrDefault();

                        TBTAREACOMENTARIO tareaComentario = new TBTAREACOMENTARIO();
                        tareaComentario.CODTRAMITE = tramiteProyeccion.CODTRAMITE;
                        tareaComentario.CODTAREA = tramiteTarea.CODTAREA;
                        tareaComentario.FECHA = DateTime.Now;
                        tareaComentario.CODFUNCIONARIO = codFuncionario;
                        tareaComentario.IMPORTANCIA = "0";
                        tareaComentario.COMENTARIO = comentarioRechazo;

                        dbSIM.Entry(tareaComentario).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }
                }
            }
            catch (Exception error)
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "AnulacionDocumento [GetRechazarDocumento] : Se presentó un error rechazando la solicitud de anulación.\r\n" + Utilidades.LogErrores.ObtenerError(error));
                return "ERROR: " + id.ToString() + "<br/>Se presentó un error rechazando la solicitud de anulación.";
            }

            return "OK";
        }
    }
}
