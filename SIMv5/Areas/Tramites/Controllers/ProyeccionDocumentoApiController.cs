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
using SIM.Data;
using SIM.Data.Tramites;
using SIM.Models;
using co.com.certicamara.encryption3DES.code;
using SIM.Utilidades.FirmaDigital;
using System.Text;
using SIM.Data.Documental;
//using DevExpress.Utils.Extensions;

namespace SIM.Areas.Tramites.Controllers
{
    public class ProyeccionDocumentoApiController : ApiController
    {
        public class Tramite
        {
            public int CODTRAMITE { get; set; }
            public decimal CODTAREA { get; set; }
            public string S_ASUNTO { get; set; }
        }

        public class Grupo
        {
            public int ID_GUPOMEMO { get; set; }
            public string S_NOMBRE { get; set; }
        }

        public class ValorLista
        {
            public int ID { get; set; }
            public string NOMBRE { get; set; }
        }

        public class Archivo
        {
            public int ID_PROYECCION_DOC_ARCHIVOS { get; set; }
            public string S_DESCRIPCION { get; set; }
            public int N_TIPO { get; set; } // 1 Principal, 2 Adjunto
            public string S_ACTIVO { get; set; }
            public Nullable<int> N_ORDEN { get; set; }
            public string MODIFICADO { get; set; } // Si se modifica el Orden del Archivo (modificación del registro del archivo, no un cambio de archivo)
            public string ARCHIVONUEVO { get; set; } // Si el archivo es nuevo, por lo tanto hay que crear el registro al almacenar la Proyección del Documento
            public string ARCHIVOACTUALIZADO { get; set; } // Si el archivo que había se cambió por otro
            public string S_NOMBRE_ARCHIVO { get; set; }
        }

        public class Indice
        {
            public int CODINDICE { get; set; }
            public string INDICE { get; set; }
            public byte TIPO { get; set; }
            public long LONGITUD { get; set; }
            public int OBLIGA { get; set; }
            public string VALORDEFECTO { get; set; }
            public string VALOR { get; set; }
            public int? ID_VALOR { get; set; }
            public Nullable<int> ID_LISTA { get; set; }
            public Nullable<int> TIPO_LISTA { get; set; }
            public string CAMPO_NOMBRE { get; set; }
        }

        public class IndiceProceso
        {
            public decimal CODINDICE { get; set; }
            public string INDICE { get; set; }
            public decimal TIPO { get; set; }
            public decimal LONGITUD { get; set; }
            public decimal OBLIGA { get; set; }
            public string VALORDEFECTO { get; set; }
            public string VALOR { get; set; }
            public Nullable<decimal> ID_LISTA { get; set; }
            public Nullable<int> TIPO_LISTA { get; set; }
            public string CAMPO_NOMBRE { get; set; }
        }

        public class Firma
        {
            public decimal CODFUNCIONARIO { get; set; }
            public string FUNCIONARIO { get; set; }
            public int ORDEN { get; set; }
            public Nullable<DateTime> D_FECHA_FIRMA { get; set; }
            public string S_ESTADO { get; set; }
            public string S_TIPO { get; set; }
            public string S_REVISA { get; set; }
            public string S_APRUEBA { get; set; }
            public string S_ACTIVO { get; set; }
        }

        public class DatosProyeccion
        {
            public int Id { get; set; }
            public int TipoSeleccionTramites { get; set; }
            public string CentroCostos { get; set; }
            public string Descripcion { get; set; }
            public bool NoAvanzaTramites { get; set; }
            public Nullable<int> SerieDocumental { get; set; }
            public Nullable<int> Grupo { get; set; }
            public List<Tramite> TramitesSeleccionados { get; set; }
            public List<Indice> Indices { get; set; }
            public List<Archivo> Archivos { get; set; }
            public List<Firma> Firmas { get; set; }
        }

        public class DatosAvanzar
        {
            public int Id { get; set; }
            public int Siguiente { get; set; }
            public string Comentario { get; set; }
            public int? Cargo { get; set; }
            public string TipoFirma { get; set; }
        }

        public class Funcionario
        {
            public decimal CodFuncionario { get; set; }
            public string Nombre { get; set; }
            public string CentroCostos { get; set; }
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
            dynamic modelData;
            int idUsuario = 0;
            int funcionario = 0;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

                funcionario = Convert.ToInt32((from uf in dbSIM.USUARIO_FUNCIONARIO
                                               join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                               where uf.ID_USUARIO == idUsuario
                                               select f.CODFUNCIONARIO).FirstOrDefault());
            }
            else
            {
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }

            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords))
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
                             join f in dbSIM.TBFUNCIONARIO on (decimal)pd.CODFUNCIONARIO_ACTUAL equals f.CODFUNCIONARIO
                             join fi in dbSIM.PROYECCION_DOC_FIRMAS.Where(fd => fd.CODFUNCIONARIO == funcionario) on pd.ID_PROYECCION_DOC equals fi.ID_PROYECCION_DOC into fip
                             from fipt in fip.DefaultIfEmpty()
                                 //join tp in dbSIM.TRAMITES_PROYECCION on pd.ID_PROYECCION_DOC equals tp.ID_PROYECCION_DOC into tpd
                                 //from tpt in tpd.DefaultIfEmpty()
                                 //join c in dbSIM.PROYECCION_DOC_COM.Where(cp => cp.S_ACTIVO == "S") on pd.ID_PROYECCION_DOC equals c.ID_PROYECCION_DOC into cd
                                 //from pdt in cd.DefaultIfEmpty()
                             join rd in dbSIM.RADICADO_DOCUMENTO on (int)pd.ID_RADICADODOC equals (int)rd.ID_RADICADODOC into rdd
                             from pdt in rdd.DefaultIfEmpty()
                                 //join tt in dbSIM.TBTRAMITETAREA on tpt.CODTRAMITE equals (int)tt.CODTRAMITE into ttd
                                 //from ttt in ttd.DefaultIfEmpty()
                                 //join ta in dbSIM.TBTAREA on ttt.CODTAREA equals ta.CODTAREA into tad
                                 //from tat in tad.DefaultIfEmpty()
                                 //join pr in dbSIM.TBPROCESO on tat.CODPROCESO equals pr.CODPROCESO into prd
                                 //from prt in prd.DefaultIfEmpty()
                                 //join ft in dbSIM.TBFUNCIONARIO on ttt.CODFUNCIONARIO equals ft.CODFUNCIONARIO into ftd
                                 //from ftt in ftd.DefaultIfEmpty()
                             where (pd.CODFUNCIONARIO == funcionario || (fipt.CODFUNCIONARIO == funcionario && (fipt.S_ESTADO == "S" || fipt.D_FECHA_FIRMA != null))) && pd.S_FORMULARIO != "25"
                             orderby (pd.S_FORMULARIO == "22" ? "S" : "N"), pd.D_FECHA_TRAMITE ?? pd.D_FECHA_CREACION descending
                             select new
                             {
                                 D_FECHA_TRAMITE = pd.D_FECHA_TRAMITE ?? pd.D_FECHA_CREACION,
                                 DIA = ((DateTime)(pd.D_FECHA_TRAMITE ?? pd.D_FECHA_CREACION)).Day,
                                 MES = ((DateTime)(pd.D_FECHA_TRAMITE ?? pd.D_FECHA_CREACION)).Month,
                                 ANO = ((DateTime)(pd.D_FECHA_TRAMITE ?? pd.D_FECHA_CREACION)).Year,
                                 pd.ID_PROYECCION_DOC,
                                 pd.S_DESCRIPCION,
                                 S_SERIE = ud.NOMBRE,
                                 pd.S_TRAMITES,
                                 pd.S_PROCESOS,
                                 pdt.S_RADICADO,
                                 pdt.S_ESTADO,
                                 S_ESTADO_FLUJO = (fipt.D_FECHA_FIRMA == null ? (pd.S_FORMULARIO == "20" ? "ELABORACIÓN" : (pd.S_FORMULARIO == "21" ? "FIRMAS" : "")) : (pd.S_FORMULARIO == "20" ? "ELAB. (DEV)" : (pd.S_FORMULARIO == "21" ? "FIRMAS (DEV)" : ""))),
                                 CODFUNCIONARIO = (decimal?)f.CODFUNCIONARIO,
                                 S_FUNCIONARIO = f.NOMBRES + " " + f.APELLIDOS,
                                 PF = (pd.CODFUNCIONARIO == funcionario ? "P" : "") + (fipt.CODFUNCIONARIO == funcionario ? "F" : ""),
                                 S_FINALIZADO = (pd.S_FORMULARIO == "22" ? "S" : "N"),
                                 S_ESTADODOC = (pdt.S_ESTADO == "N" ? "A" : "")
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
        [HttpGet, ActionName("ConsultaDocumentos")]
        public datosConsulta GetConsultaDocumentos(int tipo)
        {
            int idUsuario;
            string formulario = (tipo == 1 ? "20" : (tipo == 2 ? "21" : "22"));
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

                var funcionario = (from uf in dbSIM.USUARIO_FUNCIONARIO
                                   join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                   where uf.ID_USUARIO == idUsuario
                                   select f.CODFUNCIONARIO).FirstOrDefault();

                var model = (from pd in dbSIM.PROYECCION_DOC
                                 //join fpd in dbSIM.PROYECCION_DOC_FIRMAS on new { pd.ID_PROYECCION_DOC, pd.CODFUNCIONARIO_ACTUAL } equals new { fpd.ID_PROYECCION_DOC, fpd.CODFUNCIONARIO }
                             join ud in dbSIM.TBSERIE on pd.CODSERIE equals ud.CODSERIE
                             join uf in dbSIM.USUARIO_FUNCIONARIO on pd.CODFUNCIONARIO equals uf.CODFUNCIONARIO
                             join u in dbSIM.USUARIO on uf.ID_USUARIO equals u.ID_USUARIO
                             join ufa in dbSIM.USUARIO_FUNCIONARIO on pd.CODFUNCIONARIO_ACTUAL equals ufa.CODFUNCIONARIO
                             join ua in dbSIM.USUARIO on ufa.ID_USUARIO equals ua.ID_USUARIO
                             join f in dbSIM.TBFUNCIONARIO on (decimal)pd.CODFUNCIONARIO equals f.CODFUNCIONARIO
                             join fa in dbSIM.TBFUNCIONARIO on (decimal)pd.CODFUNCIONARIO_ACTUAL equals fa.CODFUNCIONARIO
                             join c in dbSIM.PROYECCION_DOC_COM.Where(cp => cp.S_ACTIVO == "S") on pd.ID_PROYECCION_DOC equals c.ID_PROYECCION_DOC into cd
                             from pdt in cd.DefaultIfEmpty()
                             where (ufa.ID_USUARIO == idUsuario || uf.ID_USUARIO == idUsuario) && pd.S_FORMULARIO == formulario// && u.S_ESTADO == "A" && ua.S_ESTADO == "A"
                             //orderby (funcionario == pd.CODFUNCIONARIO_ACTUAL ? "S" : "N") descending, pd.ID_PROYECCION_DOC descending
                             select new
                             {
                                 pd.ID_PROYECCION_DOC,
                                 pd.S_DESCRIPCION,
                                 S_SERIE = ud.NOMBRE,
                                 pd.S_TRAMITES,
                                 pd.S_PROCESOS,
                                 pd.D_FECHA_TRAMITE,
                                 f.CODFUNCIONARIO,
                                 S_FUNCIONARIO = f.NOMBRES + " " + f.APELLIDOS,
                                 S_FUNCIONARIO_ACTUAL = fa.NOMBRES + " " + fa.APELLIDOS,
                                 S_DEVUELTO = (pdt != null ? "S" : "N"),
                                 S_ACTUAL = (funcionario == pd.CODFUNCIONARIO_ACTUAL ? "S" : "N"),
                                 S_COMENTARIO = (pdt != null ? "S" : ""),
                                 //fpd.S_REVISA,
                                 //fpd.S_APRUEBA
                             }).Distinct().OrderBy(f => f.D_FECHA_TRAMITE);

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
        [HttpGet, ActionName("ObtenerDatosProyeccionDocumento")]
        public DatosProyeccion GetObtenerDatosProyeccionDocumento(int id)
        {
            DatosProyeccion resultado = new DatosProyeccion();

            if (id > 0)
            {
                var proyeccionDocumento = (from pd in dbSIM.PROYECCION_DOC where pd.ID_PROYECCION_DOC == id select pd).FirstOrDefault();

                if (proyeccionDocumento != null)
                {
                    resultado.CentroCostos = proyeccionDocumento.S_CENTRO_COSTOS;
                    resultado.Descripcion = proyeccionDocumento.S_DESCRIPCION;
                    resultado.SerieDocumental = proyeccionDocumento.CODSERIE;
                    resultado.Grupo = proyeccionDocumento.ID_GRUPO;
                    resultado.TipoSeleccionTramites = (proyeccionDocumento.S_TRAMITE_NUEVO == "S" ? 2 : 1);
                    resultado.NoAvanzaTramites = (proyeccionDocumento.S_NO_AVANZAR == "S");

                    var tramitesProyeccion = (
                        from trp in dbSIM.TRAMITES_PROYECCION
                        join tp in dbSIM.VW_TAREAS_PROYECCION on trp.CODTRAMITE equals tp.CODTRAMITE
                        where trp.ID_PROYECCION_DOC == id
                        orderby trp.CODTRAMITE
                        select new Tramite
                        {
                            CODTRAMITE = trp.CODTRAMITE,
                            CODTAREA = tp.CODTAREA,
                            S_ASUNTO = tp.S_ASUNTO
                        }).ToList();

                    resultado.TramitesSeleccionados = tramitesProyeccion;

                    var indicesProyeccion = (
                        from pdi in dbSIM.PROYECCION_DOC_INDICES
                        join i in dbSIM.TBINDICESERIE on pdi.CODINDICE equals i.CODINDICE
                        join lista in dbSIM.TBSUBSERIE on (decimal)i.CODIGO_SUBSERIE equals lista.CODIGO_SUBSERIE into l
                        from pdis in l.DefaultIfEmpty()
                        where pdi.ID_PROYECCION_DOC == id && pdi.ID_PROYECCION_DOC == id && i.MOSTRAR == "1" && i.INDICE_RADICADO == null
                        orderby i.ORDEN
                        select new Indice
                        {
                            CODINDICE = i.CODINDICE,
                            INDICE = i.INDICE,
                            TIPO = i.TIPO,
                            LONGITUD = i.LONGITUD,
                            OBLIGA = i.OBLIGA,
                            VALORDEFECTO = i.VALORDEFECTO,
                            VALOR = pdi.S_VALOR,
                            ID_VALOR = pdi.ID_VALOR,
                            ID_LISTA = i.CODIGO_SUBSERIE,
                            TIPO_LISTA = pdis.TIPO,
                            CAMPO_NOMBRE = pdis.CAMPO_NOMBRE
                        }).ToList();


                    resultado.Indices = indicesProyeccion;

                    var archivosProyeccion = (
                        from a in dbSIM.PROYECCION_DOC_ARCHIVOS
                        join da in dbSIM.PROYECCION_DOC_DET_ARCH on a.ID_PROYECCION_DOC_ARCHIVOS equals da.ID_PROYECCION_DOC_ARCHIVOS
                        where a.ID_PROYECCION_DOC == id && a.S_ACTIVO == "S" && da.S_ACTIVO == "S"
                        orderby a.N_TIPO, a.N_ORDEN
                        select new Archivo
                        {
                            ID_PROYECCION_DOC_ARCHIVOS = a.ID_PROYECCION_DOC_ARCHIVOS,
                            S_DESCRIPCION = a.S_DESCRIPCION,
                            N_TIPO = a.N_TIPO,
                            N_ORDEN = a.N_ORDEN,
                            S_ACTIVO = a.S_ACTIVO,
                            MODIFICADO = "N",
                            ARCHIVONUEVO = "N",
                            ARCHIVOACTUALIZADO = "N",
                            S_NOMBRE_ARCHIVO = ""
                        }).ToList();


                    resultado.Archivos = archivosProyeccion;

                    var firmasProyeccion = (
                        from pf in dbSIM.PROYECCION_DOC_FIRMAS
                        join f in dbSIM.TBFUNCIONARIO on pf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                        where pf.ID_PROYECCION_DOC == id
                        orderby pf.N_ORDEN
                        select new Firma
                        {
                            CODFUNCIONARIO = pf.CODFUNCIONARIO,
                            FUNCIONARIO = f.NOMBRES + " " + f.APELLIDOS,
                            ORDEN = pf.N_ORDEN,
                            D_FECHA_FIRMA = pf.D_FECHA_FIRMA,
                            S_ESTADO = pf.S_ESTADO,
                            S_ACTIVO = "S",
                            S_TIPO = pf.S_TIPO,
                            S_REVISA = pf.S_REVISA,
                            S_APRUEBA = pf.S_APRUEBA
                        }).ToList();


                    resultado.Firmas = firmasProyeccion;
                }
                else
                {
                    resultado = null;
                }
            }
            else
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
                                       select new Funcionario
                                       {
                                           CodFuncionario = f.CODFUNCIONARIO,
                                           Nombre = f.NOMBRES + " " + f.APELLIDOS
                                       }).FirstOrDefault();

                    // Centro de Costos Funcionario
                    var centroCostosUsuario = dbSIM.Database.SqlQuery<string>(
                    "SELECT  d.S_CODOFICINA " +
                    "FROM SEGURIDAD.USUARIO u INNER JOIN " +
                    "SEGURIDAD.USUARIO_FUNCIONARIO uf ON u.ID_USUARIO = uf.ID_USUARIO INNER JOIN " +
                    "SEGURIDAD.FUNCIONARIO_DEPENDENCIA fd ON uf.CODFUNCIONARIO = fd.CODFUNCIONARIO INNER JOIN " +
                    "SEGURIDAD.DEPENDENCIA d ON fd.ID_DEPENDENCIA = d.ID_DEPENDENCIA " +
                    "WHERE u.ID_USUARIO = " + idUsuario.ToString()).FirstOrDefault();

                    resultado.CentroCostos = centroCostosUsuario ?? "";
                    resultado.Firmas = new List<Firma>();
                    resultado.Firmas.Add(new Firma() { CODFUNCIONARIO = funcionario.CodFuncionario, FUNCIONARIO = funcionario.Nombre, ORDEN = 1, S_ESTADO = "N", S_ACTIVO = "S", S_TIPO = "Firma" });
                }
                else
                {
                    resultado.CentroCostos = "";
                }

                resultado.TipoSeleccionTramites = 1;
                resultado.Descripcion = "";
                resultado.SerieDocumental = null;
                resultado.TramitesSeleccionados = new List<Tramite>();
                resultado.Archivos = new List<Archivo>();
                resultado.Indices = new List<Indice>();
            }

            return resultado;
        }

        [Authorize]
        [HttpGet, ActionName("ObtenerIndicesProceso")]
        public dynamic GetObtenerIndicesProceso(int codProceso)
        {
            var indicesProceso = from ip in dbSIM.TBINDICEPROCESO
                                 join lista in dbSIM.TBSUBSERIE on (decimal)ip.CODIGO_SUBSERIE equals lista.CODIGO_SUBSERIE into l
                                 from pdis in l.DefaultIfEmpty()
                                 where ip.CODPROCESO == codProceso && ip.MOSTRAR == "1"
                                 orderby ip.ORDEN
                                 select new IndiceProceso
                                 {
                                     CODINDICE = ip.CODINDICE,
                                     INDICE = ip.INDICE,
                                     TIPO = (decimal)ip.TIPO,
                                     LONGITUD = (decimal)ip.LONGITUD,
                                     OBLIGA = (decimal)ip.OBLIGA,
                                     VALORDEFECTO = ip.VALORDEFECTO,
                                     VALOR = "",
                                     ID_LISTA = ip.CODIGO_SUBSERIE,
                                     TIPO_LISTA = pdis.TIPO,
                                     CAMPO_NOMBRE = pdis.CAMPO_NOMBRE
                                 };

            return indicesProceso.ToList();
        }

        [Authorize]
        [HttpGet, ActionName("ObtenerIndicesTramite")]
        public dynamic GetObtenerIndicesTramite(int t)
        {
            var indicesTramite = (
                from tr in dbSIM.TBTRAMITE
                join ip in dbSIM.TBINDICEPROCESO on tr.CODPROCESO equals ip.CODPROCESO
                join it in dbSIM.TBINDICETRAMITE on new { tr.CODTRAMITE, ip.CODINDICE } equals new { it.CODTRAMITE, it.CODINDICE } into ipt
                from iptt in ipt.DefaultIfEmpty()
                join lista in dbSIM.TBSUBSERIE on (decimal)ip.CODIGO_SUBSERIE equals lista.CODIGO_SUBSERIE into l
                from pdis in l.DefaultIfEmpty()
                where tr.CODTRAMITE == t && ip.MOSTRAR == "1"
                orderby ip.ORDEN
                select new IndiceProceso
                {
                    CODINDICE = ip.CODINDICE,
                    INDICE = ip.INDICE,
                    TIPO = (decimal)ip.TIPO,
                    LONGITUD = (decimal)ip.LONGITUD,
                    OBLIGA = (decimal)ip.OBLIGA,
                    VALORDEFECTO = ip.VALORDEFECTO,
                    VALOR = iptt.VALOR,
                    ID_LISTA = ip.CODIGO_SUBSERIE,
                    TIPO_LISTA = pdis.TIPO,
                    CAMPO_NOMBRE = pdis.CAMPO_NOMBRE
                }).ToList();

            return indicesTramite;
        }

        [Authorize]
        [HttpPost, ActionName("AlmacenarDatosProyeccionDocumento")]
        public string PostAlmacenarDatosProyeccionDocumento(DatosProyeccion datos)
        {
            int idUsuario = 0;
            int idProyeccionDocumento = 0;
            string respuestaAdicional = "";
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
                    return "ERROR:" + idProyeccionDocumento.ToString() + ":El Usuario no se encuentra autenticado.";
                }

                // Validar si el DE y PARA asociados al documento pueden proyectar la serie (esto es para memorandos)
                string[] seriesCargo = Utilidades.Data.ObtenerValorParametro("PD_SeriesValidarCargo").ToString().Split(',');
                string[] indicePARA = Utilidades.Data.ObtenerValorParametro("PD_IndicesProyeccionGruposPARA").ToString().Split(',');
                foreach (string serie in seriesCargo)
                {
                    if (serie.Trim() != "")
                    {
                        if (Convert.ToInt32(serie) == datos.SerieDocumental)
                        {
                            // Valida el último qué firma para sabir si puede radicar memorandos
                            var codFuncionarioRadica = datos.Firmas.Where(f => f.S_ACTIVO == "S").OrderByDescending(f => f.ORDEN).Select(f => f.CODFUNCIONARIO).FirstOrDefault();

                            var cargoFuncionarioRadica = (
                                    from f in dbSIM.TBFUNCIONARIO join 
                                        c in dbSIM.TBCARGO on f.CODCARGO equals c.CODCARGO
                                    where f.CODFUNCIONARIO == codFuncionarioRadica
                                    select c
                                ).FirstOrDefault();

                            if (cargoFuncionarioRadica.RADICA_MEMORANDO == null || cargoFuncionarioRadica.RADICA_MEMORANDO.Trim() != "1")
                            {
                                return "ERROR:" + idProyeccionDocumento.ToString() + ":El funcionario correspondiente a la última firma, no está configurado para radicar documentos de esta serie documental.";
                            }

                            // Valida el indide PARA para saber si pueden recibir memorandos
                            foreach (Indice indice in datos.Indices)
                            {
                                if (indicePARA.Contains(indice.CODINDICE.ToString()))
                                {
                                    // Obtene el cargo del funcionario asociado en el índice
                                    var cargoFuncionario = (from f in dbSIM.TBFUNCIONARIO
                                                            join
                                                             c in dbSIM.TBCARGO on f.CODCARGO equals c.CODCARGO
                                                            where f.CODFUNCIONARIO == indice.ID_VALOR
                                                            select c).FirstOrDefault();

                                    if (cargoFuncionario.RECIBE_MEMORANDO == null || cargoFuncionario.RECIBE_MEMORANDO.Trim() != "1")
                                    {
                                        return "ERROR:" + idProyeccionDocumento.ToString() + ":El funcionario relacionado en el Indice 'PARA', no está configurado para recibir documentos de esta serie documental.";
                                    }
                                }
                            }

                            if (datos.Grupo != null)
                            {
                                // Valida los funcionarios del grupo para saber si pueden recibir copias de memorandos
                                var sqlFuncionariosNoRecibe = "SELECT SUM(CASE WHEN c.RECIBE_COPIAMEMO IS NULL OR TRIM(c.RECIBE_COPIAMEMO) <> '1' THEN 1 ELSE 0 END) AS NumNoRecibe " +
                                    "FROM TRAMITES.MEMORANDO_FUNCGRUPO mf INNER JOIN " +
                                    "   TRAMITES.TBFUNCIONARIO f ON mf.CODFUNCIONARIO = f.CODFUNCIONARIO INNER JOIN " +
                                    "   TRAMITES.TBCARGO c ON f.CODCARGO = c.CODCARGO " +
                                    "WHERE ID_GRUPOMEMO = " + datos.Grupo.ToString();

                                int? numFuncionariosNoRecibe = dbSIM.Database.SqlQuery<int?>(sqlFuncionariosNoRecibe).FirstOrDefault();

                                if ((numFuncionariosNoRecibe ?? 0) > 0)
                                {
                                    return "ERROR:" + idProyeccionDocumento.ToString() + ":Por lo menos un funcionario del grupo seleccionado, no está configurado para recibir copias de documentos de esta serie documental.";
                                }

                                break;
                            }
                        }
                    }
                }

                PROYECCION_DOC proyeccionDocumento;

                if (datos.Id == 0) // Nuevo Documento
                {
                    int codFuncionario = dbSIM.USUARIO_FUNCIONARIO.Where(uf => uf.ID_USUARIO == idUsuario).Select(uf => uf.CODFUNCIONARIO).FirstOrDefault();

                    proyeccionDocumento = new PROYECCION_DOC();

                    proyeccionDocumento.S_CENTRO_COSTOS = (datos.CentroCostos == null || datos.CentroCostos == "" ? "00000" : datos.CentroCostos);
                    proyeccionDocumento.CODSERIE = (int)datos.SerieDocumental;
                    proyeccionDocumento.ID_GRUPO = (int?)(datos.Grupo == -1 ? null : datos.Grupo);
                    proyeccionDocumento.S_DESCRIPCION = datos.Descripcion;
                    proyeccionDocumento.S_NO_AVANZAR = datos.NoAvanzaTramites ? "S" : "N";
                    proyeccionDocumento.S_FORMULARIO = "20";
                    proyeccionDocumento.D_FECHA_CREACION = DateTime.Now;
                    proyeccionDocumento.CODFUNCIONARIO = codFuncionario;
                    proyeccionDocumento.CODFUNCIONARIO_ACTUAL = codFuncionario;
                    proyeccionDocumento.D_FECHA_TRAMITE = DateTime.Now;
                    proyeccionDocumento.S_TRAMITE_NUEVO = datos.TipoSeleccionTramites == 2 ? "S" : "N";

                    dbSIM.Entry(proyeccionDocumento).State = EntityState.Added;

                    dbSIM.SaveChanges();
                }
                else
                {
                    proyeccionDocumento = dbSIM.PROYECCION_DOC.Where(pd => pd.ID_PROYECCION_DOC == datos.Id).FirstOrDefault();

                    proyeccionDocumento.S_CENTRO_COSTOS = datos.CentroCostos;
                    proyeccionDocumento.CODSERIE = (int)datos.SerieDocumental;
                    proyeccionDocumento.ID_GRUPO = (int?)(datos.Grupo == -1 ? null : datos.Grupo);
                    proyeccionDocumento.S_DESCRIPCION = datos.Descripcion;
                    proyeccionDocumento.S_NO_AVANZAR = datos.NoAvanzaTramites ? "S" : "N";
                    proyeccionDocumento.S_TRAMITE_NUEVO = datos.TipoSeleccionTramites == 2 ? "S" : "N";

                    dbSIM.Entry(proyeccionDocumento).State = EntityState.Modified;

                    dbSIM.SaveChanges();
                }

                idProyeccionDocumento = proyeccionDocumento.ID_PROYECCION_DOC;

                // Tramites Documento
                if (datos.TramitesSeleccionados != null && datos.TipoSeleccionTramites == 1)
                {
                    var tramitesSeleccionados = datos.TramitesSeleccionados.Select(ts => ts.CODTRAMITE).ToList();
                    List<TRAMITES_PROYECCION> tramitesProyeccion = dbSIM.TRAMITES_PROYECCION.Where(tp => tp.ID_PROYECCION_DOC == proyeccionDocumento.ID_PROYECCION_DOC && !tramitesSeleccionados.Contains(tp.CODTRAMITE)).ToList();

                    foreach (TRAMITES_PROYECCION tramiteProyeccion in tramitesProyeccion)
                    {
                        dbSIM.Entry(tramiteProyeccion).State = EntityState.Deleted;
                        dbSIM.SaveChanges();
                    }

                    tramitesProyeccion = dbSIM.TRAMITES_PROYECCION.Where(tp => tp.ID_PROYECCION_DOC == proyeccionDocumento.ID_PROYECCION_DOC).ToList();
                    var tramitesActuales = tramitesProyeccion.Select(ts => ts.CODTRAMITE).ToList();

                    var tramitesNuevos = datos.TramitesSeleccionados.Where(ts => !tramitesActuales.Contains(ts.CODTRAMITE));

                    foreach (Tramite tramiteProyeccionNuevo in tramitesNuevos)
                    {
                        TRAMITES_PROYECCION tramiteProyeccion = new TRAMITES_PROYECCION();

                        tramiteProyeccion.ID_PROYECCION_DOC = proyeccionDocumento.ID_PROYECCION_DOC;
                        tramiteProyeccion.CODTRAMITE = tramiteProyeccionNuevo.CODTRAMITE;
                        tramiteProyeccion.CODTAREA_INICIAL = Convert.ToInt32(tramiteProyeccionNuevo.CODTAREA);

                        dbSIM.Entry(tramiteProyeccion).State = EntityState.Added;

                        dbSIM.SaveChanges();
                    }
                }
                else
                {
                    List<TRAMITES_PROYECCION> tramitesProyeccion = dbSIM.TRAMITES_PROYECCION.Where(tp => tp.ID_PROYECCION_DOC == proyeccionDocumento.ID_PROYECCION_DOC).ToList();

                    foreach (TRAMITES_PROYECCION tramiteProyeccion in tramitesProyeccion)
                    {
                        dbSIM.Entry(tramiteProyeccion).State = EntityState.Deleted;
                        dbSIM.SaveChanges();
                    }
                }

                if (datos.TipoSeleccionTramites == 1)
                {
                    string listaTareasTramites = string.Join(", ", proyeccionDocumento.TRAMITES_PROYECCION.Select<TRAMITES_PROYECCION, string>(tp => tp.CODTAREA_INICIAL.ToString()));

                    if (listaTareasTramites != null && listaTareasTramites.Trim() != "")
                    {
                        var codProcesos = dbSIM.Database.SqlQuery<int>("SELECT CODPROCESO FROM TRAMITES.TBTAREA WHERE CODTAREA IN (" + listaTareasTramites + ")").ToList();

                        string listaTramites = string.Join(", ", proyeccionDocumento.TRAMITES_PROYECCION.Select<TRAMITES_PROYECCION, string>(tp => tp.CODTRAMITE.ToString()));
                        string listaProcesos = string.Join(", ", dbSIM.TBPROCESO.Where(p => codProcesos.Contains((int)p.CODPROCESO)).Select<TBPROCESO, string>(p => p.NOMBRE).Distinct());

                        proyeccionDocumento.S_TRAMITES = listaTramites;
                        proyeccionDocumento.S_PROCESOS = listaProcesos;

                        dbSIM.Entry(proyeccionDocumento).State = System.Data.Entity.EntityState.Modified;
                        dbSIM.SaveChanges();
                    }
                }
                else
                {
                    proyeccionDocumento.S_TRAMITES = "";
                    proyeccionDocumento.S_PROCESOS = "";

                    dbSIM.Entry(proyeccionDocumento).State = System.Data.Entity.EntityState.Modified;
                    dbSIM.SaveChanges();
                }

                // Archivos
                if (datos.Archivos != null)
                {
                    foreach (Archivo archivo in datos.Archivos)
                    {
                        int idArchivo = archivo.ID_PROYECCION_DOC_ARCHIVOS;

                        if (archivo.MODIFICADO == "S" || archivo.ARCHIVONUEVO == "S")
                        {
                            PROYECCION_DOC_ARCHIVOS archivoProyeccion;

                            if (archivo.ARCHIVONUEVO == "S" && archivo.S_ACTIVO == "S") // Nuevo Archivo
                            {
                                archivoProyeccion = new PROYECCION_DOC_ARCHIVOS();

                                archivoProyeccion.ID_PROYECCION_DOC = proyeccionDocumento.ID_PROYECCION_DOC;
                                archivoProyeccion.S_DESCRIPCION = archivo.S_DESCRIPCION;
                                archivoProyeccion.S_ACTIVO = "S";
                                archivoProyeccion.N_TIPO = archivo.N_TIPO;
                                archivoProyeccion.N_ORDEN = archivo.N_ORDEN;

                                dbSIM.Entry(archivoProyeccion).State = EntityState.Added;
                                dbSIM.SaveChanges();

                                archivo.ID_PROYECCION_DOC_ARCHIVOS = archivoProyeccion.ID_PROYECCION_DOC_ARCHIVOS;
                            }
                            else if (archivo.ARCHIVONUEVO == "N" && archivo.MODIFICADO == "S")
                            {
                                archivoProyeccion = dbSIM.PROYECCION_DOC_ARCHIVOS.Where(pa => pa.ID_PROYECCION_DOC_ARCHIVOS == archivo.ID_PROYECCION_DOC_ARCHIVOS).FirstOrDefault();

                                archivoProyeccion.S_ACTIVO = archivo.S_ACTIVO;
                                archivoProyeccion.N_ORDEN = archivo.N_ORDEN;

                                dbSIM.Entry(archivoProyeccion).State = EntityState.Modified;
                                dbSIM.SaveChanges();
                            }

                            if ((archivo.ARCHIVONUEVO == "S" || archivo.ARCHIVOACTUALIZADO == "S") && archivo.S_ACTIVO == "S") // Si se cambió el archivo o se actualizó uno existente
                            {
                                PROYECCION_DOC_DET_ARCH archivoProyeccionDet;

                                archivoProyeccion = dbSIM.PROYECCION_DOC_ARCHIVOS.Where(pa => pa.ID_PROYECCION_DOC_ARCHIVOS == archivo.ID_PROYECCION_DOC_ARCHIVOS).FirstOrDefault();

                                if (archivo.ARCHIVOACTUALIZADO == "S")
                                {
                                    // Se obtiene el activo actual y se desactiva
                                    archivoProyeccionDet = dbSIM.PROYECCION_DOC_DET_ARCH.Where(pad => pad.ID_PROYECCION_DOC_ARCHIVOS == archivo.ID_PROYECCION_DOC_ARCHIVOS && pad.S_ACTIVO == "S").FirstOrDefault();

                                    if (archivoProyeccionDet != null)
                                    {
                                        archivoProyeccionDet.S_ACTIVO = "N";

                                        dbSIM.Entry(archivoProyeccionDet).State = EntityState.Modified;
                                        dbSIM.SaveChanges();
                                    }
                                }

                                // Se crea el archivo en Temporales
                                string rutaBaseTemporal = ConfigurationManager.AppSettings["RutaProyeccionDocumentos"] + "\\" + Archivos.GetRutaDocumento(Convert.ToUInt64(proyeccionDocumento.ID_PROYECCION_DOC), 100);
                                string ruta = rutaBaseTemporal + archivoProyeccion.ID_PROYECCION_DOC_ARCHIVOS.ToString() + "_" + archivo.S_NOMBRE_ARCHIVO.ToString();

                                if (!Directory.Exists(rutaBaseTemporal))
                                    Directory.CreateDirectory(rutaBaseTemporal);

                                string rutaOriginal;

                                rutaOriginal = HostingEnvironment.MapPath("~/App_Data/Temporal/PD/" + idUsuario.ToString() + "__" + idArchivo.ToString() + "__" + archivo.S_NOMBRE_ARCHIVO);

                                File.Copy(rutaOriginal, ruta, true);

                                // Se crea el registro del archivo nuevo y se activa
                                archivoProyeccionDet = new PROYECCION_DOC_DET_ARCH();

                                archivoProyeccionDet.ID_PROYECCION_DOC_ARCHIVOS = archivoProyeccion.ID_PROYECCION_DOC_ARCHIVOS;
                                archivoProyeccionDet.D_FECHA_CARGA = DateTime.Now;
                                archivoProyeccionDet.S_RUTA_ARCHIVO = ruta;
                                archivoProyeccionDet.S_ACTIVO = "S";

                                dbSIM.Entry(archivoProyeccionDet).State = EntityState.Added;
                                dbSIM.SaveChanges();
                            }
                        }
                    }
                }

                // Indices
                if (datos.Indices != null)
                {
                    var indicesSeleccionados = datos.Indices.Select(isel => isel.CODINDICE).ToList();

                    List<PROYECCION_DOC_INDICES> indicesProyeccion = dbSIM.PROYECCION_DOC_INDICES.Where(ip => ip.ID_PROYECCION_DOC == proyeccionDocumento.ID_PROYECCION_DOC && !indicesSeleccionados.Contains(ip.CODINDICE)).ToList();

                    foreach (PROYECCION_DOC_INDICES indiceProyeccion in indicesProyeccion)
                    {
                        dbSIM.Entry(indiceProyeccion).State = EntityState.Deleted;
                        dbSIM.SaveChanges();
                    }

                    foreach (Indice indice in datos.Indices)
                    {
                        PROYECCION_DOC_INDICES indiceProyeccion = dbSIM.PROYECCION_DOC_INDICES.Where(i => i.ID_PROYECCION_DOC == proyeccionDocumento.ID_PROYECCION_DOC && i.CODINDICE == indice.CODINDICE).FirstOrDefault();

                        if (indiceProyeccion != null)
                        {
                            indiceProyeccion.S_VALOR = indice.VALOR ?? "";
                            indiceProyeccion.ID_VALOR = indice.ID_VALOR;
                            dbSIM.Entry(indiceProyeccion).State = EntityState.Modified;
                        }
                        else
                        {
                            indiceProyeccion = new PROYECCION_DOC_INDICES();

                            indiceProyeccion.ID_PROYECCION_DOC = proyeccionDocumento.ID_PROYECCION_DOC;
                            indiceProyeccion.CODINDICE = indice.CODINDICE;
                            indiceProyeccion.S_VALOR = indice.VALOR ?? "";
                            indiceProyeccion.ID_VALOR = indice.ID_VALOR;

                            dbSIM.Entry(indiceProyeccion).State = EntityState.Added;
                        }

                        dbSIM.SaveChanges();
                    }
                }

                // Firmas
                if (datos.Firmas != null)
                {
                    int numRevision = 0;
                    int numAprobacion = 0;

                    foreach (Firma firma in datos.Firmas)
                    {
                        if (firma.S_ACTIVO == "N")
                        {
                            PROYECCION_DOC_FIRMAS firmaProyeccion = dbSIM.PROYECCION_DOC_FIRMAS.Where(f => f.ID_PROYECCION_DOC == proyeccionDocumento.ID_PROYECCION_DOC && f.CODFUNCIONARIO == firma.CODFUNCIONARIO).FirstOrDefault();

                            if (firmaProyeccion != null)
                            {
                                dbSIM.Entry(firmaProyeccion).State = EntityState.Deleted;
                            }

                            dbSIM.SaveChanges();
                        }
                        else
                        {
                            bool _EsFirmaSerie = true;

                            if (firma.S_REVISA == "S")
                                numRevision++;

                            if (firma.S_APRUEBA == "S")
                                numAprobacion++;

                            PROYECCION_DOC_FIRMAS firmaProyeccion = dbSIM.PROYECCION_DOC_FIRMAS.Where(f => f.ID_PROYECCION_DOC == proyeccionDocumento.ID_PROYECCION_DOC && f.CODFUNCIONARIO == firma.CODFUNCIONARIO).FirstOrDefault();
                            if (firma.S_TIPO == "Digital")
                            {
                                string _SeriesFirma = Utilidades.Data.ObtenerValorParametro("SeriesFirmaDigital").ToString();
                                string[] _SerFirma = _SeriesFirma.Split(',');
                                _EsFirmaSerie = false;
                                foreach (string CodSerie in _SerFirma)
                                {
                                    if (proyeccionDocumento.CODSERIE.ToString().Trim() == CodSerie) _EsFirmaSerie = true;
                                }
                            }
                            if (_EsFirmaSerie)
                            {
                                if (firmaProyeccion != null)
                                {
                                    firmaProyeccion.S_ESTADO = firma.S_ESTADO;
                                    firmaProyeccion.N_ORDEN = firma.ORDEN;
                                    firmaProyeccion.S_TIPO = firma.S_TIPO;
                                    firmaProyeccion.S_REVISA = firma.S_REVISA;
                                    firmaProyeccion.S_APRUEBA = firma.S_APRUEBA;
                                    dbSIM.Entry(firmaProyeccion).State = EntityState.Modified;
                                }
                                else
                                {
                                    firmaProyeccion = new PROYECCION_DOC_FIRMAS();

                                    firmaProyeccion.ID_PROYECCION_DOC = proyeccionDocumento.ID_PROYECCION_DOC;
                                    firmaProyeccion.CODFUNCIONARIO = Convert.ToInt32(firma.CODFUNCIONARIO);
                                    firmaProyeccion.S_ESTADO = firma.S_ESTADO;
                                    firmaProyeccion.N_ORDEN = firma.ORDEN;
                                    firmaProyeccion.S_TIPO = firma.S_TIPO;
                                    firmaProyeccion.S_REVISA = firma.S_REVISA;
                                    firmaProyeccion.S_APRUEBA = firma.S_APRUEBA;

                                    dbSIM.Entry(firmaProyeccion).State = EntityState.Added;
                                }

                                dbSIM.SaveChanges();
                            }
                            else return "ERROR:" + idProyeccionDocumento.ToString() + ":La unidad documental no esta destinada para firma digital, por tanto no puede tener tipo de firma Digital";
                        }
                    }

                    if (proyeccionDocumento.CODSERIE == 11 || proyeccionDocumento.CODSERIE == 17 || proyeccionDocumento.CODSERIE == 722 || proyeccionDocumento.CODSERIE == 1082 || proyeccionDocumento.CODSERIE == 1102)
                    {
                        if (numAprobacion == 0 || numRevision == 0)
                        {
                            return "ERROR:" + idProyeccionDocumento.ToString() + ":Las Resoluciones y Autos deben tener por lo menos un usuario que Revise y uno que Apruebe.";
                        }
                    }
                }

                // Validación que el documento principal cargado tenga consistencia en las firmas ingresadas y configuradas
                // 1. No hayan etiquetas de firmas repetidas.
                // 2. La cantidad de etiquetas de firmas coincide con las firmas configuradas
                // 3. No tenga la etiqueta [Proyecta] para Resoluciones y Autos
                TramitesLibrary utilidad = new TramitesLibrary();
                respuestaAdicional = utilidad.ValidarFirmasDocumento(idProyeccionDocumento);
            }
            catch (Exception error)
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "ProyeccionDocumento [PostAlmacenarDatosProyeccionDocumento - " + datos.Id.ToString() + " ] : Se presentó un error. Se pudo haber almacenado parcialmente la Información.\r\n" + Utilidades.LogErrores.ObtenerError(error));
                return "ERROR:" + idProyeccionDocumento.ToString() + ":Se presentó un error. Se pudo haber almacenado parcialmente la Información.";
            }

            return "OK:" + idProyeccionDocumento.ToString() + ":" + respuestaAdicional;
        }

        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet, ActionName("ObtenerSeriesDocumentales")]
        public dynamic GetObtenerSeriesDocumentales()
        {
            //var seriesHabilitadas = ConfigurationManager.AppSettings["SeriesProyeccionDocumento"];
            var seriesHabilitadas = Utilidades.Data.ObtenerValorParametro("PD_SeriesProyeccionDocumento");
            //var seriesGrupos = ConfigurationManager.AppSettings["SeriesProyeccionGrupos"];
            var seriesGrupos = Utilidades.Data.ObtenerValorParametro("PD_SeriesProyeccionGrupos");

            if (seriesHabilitadas == null || seriesHabilitadas.Trim() == "")
            {
                var unidadDocumental = from ud in dbSIM.TBSERIE
                                       where ud.ACTIVO == "1" && ud.RADICADO == "1"
                                       orderby ud.NOMBRE
                                       select new
                                       {
                                           ud.CODSERIE,
                                           ud.NOMBRE
                                       };

                return unidadDocumental.ToList();
            }
            else
            {
                var series = seriesHabilitadas.Split(',');
                var codSeries = series.Select(s => decimal.Parse(s));
                var codSeriesGrupos = seriesGrupos.Split(',').Select(s => decimal.Parse(s));

                // var codSeries = Array.ConvertAll<int>(series, s => Convert.ToInt32(s));

                var unidadDocumental = from ud in dbSIM.TBSERIE
                                       where ud.ACTIVO == "1" && codSeries.Contains(ud.CODSERIE) //&& ud.RADICADO == "1"
                                       orderby ud.NOMBRE
                                       select new
                                       {
                                           ud.CODSERIE,
                                           ud.NOMBRE,
                                           GRUPO = (codSeriesGrupos.Contains(ud.CODSERIE) ? "S" : "N")
                                       };

                return unidadDocumental.ToList();
            }
        }

        [Authorize]
        [HttpGet, ActionName("ObtenerGrupos")]
        public dynamic GetObtenerGrupos()
        {
            var sql = "SELECT -1 AS ID_GUPOMEMO, n' -- NINGUNO -- ' AS S_NOMBRE FROM DUAL UNION SELECT ID_GUPOMEMO, S_NOMBRE FROM TRAMITES.MEMORANDO_GRUPO WHERE S_ACTIVO = '1' ORDER BY S_NOMBRE";

            var resultadoConsulta = dbSIM.Database.SqlQuery<Grupo>(sql).ToList();
            return resultadoConsulta;
        }

        [Authorize]
        [HttpGet, ActionName("ObtenerPrimerRegistro")]
        public ValorLista GetObtenerPrimerRegistro(int idGrupo)
        {
            //var sql = "SELECT CODFUNCIONARIO AS ID FROM TRAMITES.MEMORANDO_FUNCGRUPO WHERE ID_GRUPOMEMO = " + idGrupo.ToString() + " FETCH FIRST 1 ROWS ONLY";

            var sql = "SELECT CODFUNCIONARIO AS ID, NOMBRES AS NOMBRE " +
                "FROM(SELECT * FROM TRAMITES.QRY_FUNCIONARIO WHERE CODFUNCIONARIO = (SELECT CODFUNCIONARIO AS ID FROM TRAMITES.MEMORANDO_FUNCGRUPO WHERE ID_GRUPOMEMO = " + idGrupo.ToString() + " FETCH FIRST 1 ROWS ONLY)) datos";
           
            var resultadoConsulta = dbSIM.Database.SqlQuery<ValorLista>(sql).FirstOrDefault();
            return resultadoConsulta;
        }

        [Authorize]
        [HttpGet, ActionName("ObtenerIndicesSerieDocumental")]
        public dynamic GetObtenerIndicesSerieDocumental(int codSerie)
        {
            var indicesSerieDocumental = from i in dbSIM.TBINDICESERIE
                                         join lista in dbSIM.TBSUBSERIE on (decimal)i.CODIGO_SUBSERIE equals lista.CODIGO_SUBSERIE into l
                                         from pdis in l.DefaultIfEmpty()
                                         where i.CODSERIE == codSerie && i.MOSTRAR == "1" && i.INDICE_RADICADO == null
                                         orderby i.ORDEN
                                         select new Indice
                                         {
                                             CODINDICE = i.CODINDICE,
                                             INDICE = i.INDICE,
                                             TIPO = i.TIPO,
                                             LONGITUD = i.LONGITUD,
                                             OBLIGA = i.OBLIGA,
                                             VALORDEFECTO = i.VALORDEFECTO,
                                             VALOR = "",
                                             ID_VALOR = null,
                                             ID_LISTA = i.CODIGO_SUBSERIE,
                                             TIPO_LISTA = pdis.TIPO,
                                             CAMPO_NOMBRE = pdis.CAMPO_NOMBRE
                                         };

            return indicesSerieDocumental.ToList();
        }

        [Authorize]
        [HttpGet, ActionName("TareasProyeccion")]
        public datosConsulta GetTareasProyeccion(string f)
        {
            int idUsuario;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                return new datosConsulta() { numRegistros = 0, datos = null };
            }

            var model = (from tp in dbSIM.VW_TAREAS_PROYECCION
                         where tp.ID_USUARIO == idUsuario && tp.S_FORMULARIO == f
                         orderby tp.CODTRAMITE
                         select tp);

            datosConsulta resultado = new datosConsulta();
            resultado.numRegistros = model.Count();
            resultado.datos = model.ToList();

            return resultado;
        }

        [Authorize]
        [HttpGet, ActionName("ValidarDatosProyeccion")] // Para validar si se puede avanzar siempre y cuando cumpla con los datos mínimos
        public bool GetValidarDatosProyeccion(int id)
        {
            return true;
        }

        // GET api/<controller>
        [Authorize]
        [HttpGet, ActionName("Funcionarios")]
        public datosConsulta GetFuncionarios(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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
                var model = (from f in dbSIM.QRY_FUNCIONARIO_ALL
                             where f.ACTIVO == "1"
                             select new
                             {
                                 f.CODFUNCIONARIO,
                                 FUNCIONARIO = f.NOMBRES,
                                 FIRMADIG = f.FIRMA_DIGITAL
                             });

                modelData = model;

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        // GET api/<controller>
        [Authorize]
        [HttpGet, ActionName("Cargos")]
        public datosConsulta GetCargos(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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
                var model = (from c in dbSIM.TBCARGO

                             select new
                             {
                                 c.CODCARGO,
                                 c.NOMBRE
                             });

                modelData = model;

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        [HttpGet, ActionName("PruebaDocumento")]
        public dynamic GetPruebaDocumento()
        {
            TramitesLibrary utilidad = new TramitesLibrary();

            utilidad.PruebaFirmas();

            return "OK - " + DateTime.Now.ToString("HH:mm:ss");
        }

        [Authorize]
        [HttpPost, ActionName("AvanzarDocumento")]
        public dynamic PostAvanzarDocumento(DatosAvanzar datos)
        {
            Respuesta respuesta = new Respuesta { tipoRespuesta = "OK", detalleRespuesta = "" };
            DatosAvanzaTareaTramiteFormulario datosTareaTramiteFormulario = new DatosAvanzaTareaTramiteFormulario();
            PROYECCION_DOC documento = dbSIM.PROYECCION_DOC.Where(pd => pd.ID_PROYECCION_DOC == datos.Id).FirstOrDefault();
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            Encrypt3DES ed3des = new Encrypt3DES();
            Comun FirmaDig = new Comun();
            int idUsuario = 0;
            string tramitesAvanzar = "";
            string tareasInicialesTramites = "";
            string tramiteNuevo = "";

            try
            {
                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                }

                if (idUsuario == 0)
                {
                    return new { Respuesta = "ERROR:El Usuario no se encuentra autenticado.", Avanzar = "0" };
                }

                int codFuncionario = dbSIM.USUARIO_FUNCIONARIO.Where(uf => uf.ID_USUARIO == idUsuario).Select(uf => uf.CODFUNCIONARIO).FirstOrDefault();

                if (documento != null)
                {
                    string avanzar = "0";
                    string radicadoGenerado = "0";

                    if (documento.S_FORMULARIO == "20")
                    {
                        // Validar que los datos sean consistentes y se cumplan las restricciones mínimas
                        var indicesDocumento = (from idp in dbSIM.PROYECCION_DOC_INDICES
                                                join i in dbSIM.TBINDICESERIE on idp.CODINDICE equals i.CODINDICE
                                                where idp.ID_PROYECCION_DOC == datos.Id && i.OBLIGA == 1 && (idp.S_VALOR == null || idp.S_VALOR.Trim() == "")
                                                select idp
                                                ).ToList();

                        if (indicesDocumento.Count > 0)
                        {
                            return new { Respuesta = "ERROR:Falta Ingresar Valores en Todos los Indices Requeridos.", Avanzar = "0" };
                        }
                    }


                    // Ubicar el formulario en el que se encuentra el documento
                    switch (documento.S_FORMULARIO)
                    {
                        case "20": // En elaboracion
                            {
                                // Buscar la primera firma asignada
                                var firma = dbSIM.PROYECCION_DOC_FIRMAS.Where(f => f.ID_PROYECCION_DOC == datos.Id).OrderBy(f => f.N_ORDEN).FirstOrDefault();

                                var funcionariosSiguiente = dbSIM.TBFUNCIONARIO.Where(f => f.CODFUNCIONARIO == firma.CODFUNCIONARIO).FirstOrDefault();

                                if (funcionariosSiguiente.ACTIVO == "0")
                                {
                                    return new { Respuesta = "Error:¡Advertencia!<br>No puede avanzar el documento.<br>Actualmente el usuario " + funcionariosSiguiente.NOMBRES + " " + funcionariosSiguiente.APELLIDOS + " se encuentra inhabilitado en el sistema y no puede firmar este documento.<br>Debe cambiar el responsable de la firma para que el documento pueda avanzar.", Avanzar = "0" };
                                }

                                // Se validan las firmas y las etiquetas del documento
                                int numRevision = 0;
                                int numAprobacion = 0;

                                var firmas = dbSIM.PROYECCION_DOC_FIRMAS.Where(f => f.ID_PROYECCION_DOC == datos.Id).OrderBy(f => f.N_ORDEN);

                                foreach (PROYECCION_DOC_FIRMAS firmaDoc in firmas)
                                {
                                    bool _EsFirmaSerie = true;

                                    if (firmaDoc.S_REVISA == "S")
                                        numRevision++;

                                    if (firmaDoc.S_APRUEBA == "S")
                                        numAprobacion++;

                                    if (firmaDoc.S_TIPO == "Digital")
                                    {
                                        string _SeriesFirma = Utilidades.Data.ObtenerValorParametro("SeriesFirmaDigital").ToString();
                                        string[] _SerFirma = _SeriesFirma.Split(',');
                                        _EsFirmaSerie = false;
                                        foreach (string CodSerie in _SerFirma)
                                        {
                                            if (documento.CODSERIE.ToString().Trim() == CodSerie) _EsFirmaSerie = true;
                                        }
                                    }
                                    if (!_EsFirmaSerie)
                                        return new { Respuesta = "Error:¡Advertencia!<br>No puede avanzar el documento.<br>La unidad documental no esta destinada para firma digital, por tanto no puede tener tipo de firma Digital.", Avanzar = "0" };
                                }

                                if (documento.CODSERIE == 11 || documento.CODSERIE == 17 || documento.CODSERIE == 722 || documento.CODSERIE == 1082 || documento.CODSERIE == 1102)
                                {
                                    if (numAprobacion == 0 || numRevision == 0)
                                    {
                                        return new { Respuesta = "Error:¡Advertencia!<br>No puede avanzar el documento.<br>Las Resoluciones y Autos deben tener por lo menos un usuario que Revise y uno que Apruebe.", Avanzar = "0" };
                                    }
                                }

                                // Validación que el documento principal cargado tenga consistencia en las firmas ingresadas y configuradas
                                // 1. No hayan etiquetas de firmas repetidas.
                                // 2. La cantidad de etiquetas de firmas coincide con las firmas configuradas
                                // 3. No tenga la etiqueta [Proyecta] para Resoluciones y Autos
                                TramitesLibrary firmasDocumento = new TramitesLibrary();
                                var respuestaFirmasDocumento = firmasDocumento.ValidarFirmasDocumento(datos.Id);

                                if (respuestaFirmasDocumento != "")
                                    return new { Respuesta = "Error:¡Advertencia!<br>No puede avanzar el documento.<br>" + respuestaFirmasDocumento, Avanzar = "0" };

                                if (documento.S_TRAMITE_NUEVO != "S")
                                {
                                    var tramites = dbSIM.TRAMITES_PROYECCION.Where(tp => tp.ID_PROYECCION_DOC == datos.Id);
                                    tramitesAvanzar = "";

                                    foreach (var tramite in tramites)
                                    {
                                        if (tramitesAvanzar == "")
                                            tramitesAvanzar = tramite.CODTRAMITE.ToString();
                                        else
                                            tramitesAvanzar += "," + tramite.CODTRAMITE.ToString();
                                    }

                                    datosTareaTramiteFormulario = new DatosAvanzaTareaTramiteFormulario()
                                    {
                                        codTramites = tramitesAvanzar,
                                        codTarea = 0,
                                        codTareaSiguiente = 0,
                                        codFuncionario = firma.CODFUNCIONARIO,
                                        formularioSiguiente = "21",
                                        copias = "",
                                        comentario = "Documento Enviado Para ser Firmado."
                                    };

                                    TramitesLibrary utilidad = new TramitesLibrary();

                                    respuesta = utilidad.AvanzaTareaTramiteFormulario(datosTareaTramiteFormulario);

                                    if (respuesta.tipoRespuesta == "OK")
                                    {
                                        //string procesosIndicesProyeccion = ConfigurationManager.AppSettings["ProcesosIndicesProyeccion"];
                                        string procesosIndicesProyeccion = Utilidades.Data.ObtenerValorParametro("PD_ProcesosIndicesProyeccion");

                                        if (procesosIndicesProyeccion != null && procesosIndicesProyeccion.Trim() != "")
                                        {
                                            var codTramites = tramites.Select(t => (decimal)t.CODTRAMITE).ToList();
                                            var codProcesos = procesosIndicesProyeccion.Split(',').Select(decimal.Parse).ToList();

                                            string tramitesIndices = string.Join(",", dbSIM.TBTRAMITE.Where(t => codTramites.Contains(t.CODTRAMITE) && codProcesos.Contains(t.CODPROCESO)).Select(t => t.CODTRAMITE).ToArray());
                                            //string tramitesIndices = string.Join(",", dbSIM.TBTRAMITE.Where(t => codTramites.Contains(t.CODTRAMITE)).Select(t => t.CODTRAMITE).ToArray());

                                            respuesta.datosAdicionales = tramitesIndices;
                                        }
                                    }
                                }
                                else
                                {
                                    respuesta = new Respuesta() { tipoRespuesta = "OK", detalleRespuesta = "" };
                                }

                                documento.CODFUNCIONARIO_ACTUAL = firma.CODFUNCIONARIO;
                                documento.S_FORMULARIO = "21";
                                documento.D_FECHA_TRAMITE = DateTime.Now;
                                dbSIM.Entry(documento).State = EntityState.Modified;
                                dbSIM.SaveChanges();
                            }
                            break;
                        case "21": // Pendiente Firmas
                            {
                                List<TRAMITES_PROYECCION> tramites = null;

                                TramitesLibrary utilidad = new TramitesLibrary();

                                // Buscar la siguiente firma
                                var firma = dbSIM.PROYECCION_DOC_FIRMAS.Where(f => f.ID_PROYECCION_DOC == datos.Id && f.S_ESTADO == "N" && f.CODFUNCIONARIO != codFuncionario).OrderBy(f => f.N_ORDEN).FirstOrDefault();

                                if (documento.S_TRAMITE_NUEVO != "S")
                                {
                                    tramites = dbSIM.TRAMITES_PROYECCION.Where(tp => tp.ID_PROYECCION_DOC == datos.Id).ToList();
                                    tramitesAvanzar = "";
                                    tareasInicialesTramites = "";

                                    foreach (var tramite in tramites)
                                    {
                                        if (tramitesAvanzar == "")
                                            tramitesAvanzar = tramite.CODTRAMITE.ToString();
                                        else
                                            tramitesAvanzar += "," + tramite.CODTRAMITE.ToString();

                                        if (tareasInicialesTramites == "")
                                            tareasInicialesTramites = tramite.CODTRAMITE.ToString() + "|" + tramite.CODTAREA_INICIAL.ToString();
                                        else
                                            tareasInicialesTramites += "," + tramite.CODTRAMITE.ToString() + "|" + tramite.CODTAREA_INICIAL.ToString();
                                    }
                                }

                                if (datos.Siguiente == 1)
                                {
                                    string _Password = "";

                                    var FuncFirma = (from Firma in dbSIM.PROYECCION_DOC_FIRMAS
                                                     join Fun in dbSIM.TBFUNCIONARIO on Firma.CODFUNCIONARIO equals Fun.CODFUNCIONARIO
                                                     where Firma.ID_PROYECCION_DOC == datos.Id && Firma.CODFUNCIONARIO == codFuncionario
                                                     select new
                                                     {
                                                         Firma.S_TIPO,
                                                         Fun.FIRMA_DIGITAL,
                                                         Fun.USUARIO_FIRMA,
                                                         Fun.FECHAFIN_FIRMA,
                                                         Firma.N_ORDEN
                                                     }).FirstOrDefault();
                                    if (FuncFirma.S_TIPO == "Digital")
                                    {
                                        if (FuncFirma.FIRMA_DIGITAL == "1")
                                        {
                                            if (datos.Comentario == null) return new { Respuesta = "Error:Firma Digital", Avanzar = "0" };
                                        }
                                        if (FuncFirma.USUARIO_FIRMA == "" || FuncFirma.USUARIO_FIRMA == null)
                                        {
                                            return new { Respuesta = "Error:El funcionario no tiene registrado el usuario de la firma digital, por favor corrija!!", Avanzar = "0" };
                                        }
                                        if (FuncFirma.FECHAFIN_FIRMA != null)
                                        {
                                            if (FuncFirma.FECHAFIN_FIRMA <= DateTime.Now)
                                            {
                                                return new { Respuesta = "Error:Su firma digital ha caducado, si necesita firmar el documento con firma electrónica modifique el tipo de firma!!", Avanzar = "0" };
                                            }
                                        }
                                        else return new { Respuesta = "Error:No se encuentra registrada la fecha de vencimiento de su firma digital!!", Avanzar = "0" };
                                        if (datos.Comentario.Contains(";"))
                                        {
                                            string[] _Aux = datos.Comentario.Split(';');
                                            _Password = _Aux[1].ToString();

                                            Utilidades.FirmaDigital.Valida _val = FirmaDig.ValidaUsuario(FuncFirma.USUARIO_FIRMA.Trim(), _Password.Trim()).Result;

                                            if (!_val.Exito) return new { Respuesta = _val.Mensaje, Avanzar = "0" };
                                        }
                                    }

                                    if (firma == null) // No hay mas firmas y hay que avanzar los trámites
                                    {
                                        /*var funcionariosInhabilitados = (from pf in dbSIM.PROYECCION_DOC_FIRMAS
                                                                        join fi in dbSIM.TBFUNCIONARIO on pf.CODFUNCIONARIO equals fi.CODFUNCIONARIO
                                                                        where pf.ID_PROYECCION_DOC == datos.Id && fi.ACTIVO == "0"
                                                                        select new { FUNCIONARIO = fi.NOMBRES + " " + fi.APELLIDOS }).ToList();

                                        if (funcionariosInhabilitados.Count > 0)
                                        {
                                            string listaFuncionarios = string.Join(", ", funcionariosInhabilitados.Select(f => f.FUNCIONARIO));

                                            return new { Respuesta = "Error:¡Advertencia!<br>No puede radicarse el documento.<br>Actualmente el usuario " + listaFuncionarios + " se encuentra inhabilitado en el sistema y no puede firmar este documento.<br>Si desea cambiar el responsable de la firma puede devolver la tarea a quién proyectó el documento.", Avanzar = "0" };
                                        }*/


                                        DatosRadicado radicado;

                                        bool puedeRadicar = (new Radicador()).SePuedeGenerarRadicado(DateTime.Now);

                                        if (documento.ID_RADICADODOC == null)
                                        {
                                            if (puedeRadicar)
                                            {
                                                radicado = RadicarDocumento(documento.ID_PROYECCION_DOC);
                                                documento.ID_RADICADODOC = radicado.IdRadicado;
                                                dbSIM.Entry(documento).State = EntityState.Modified;
                                                dbSIM.SaveChanges();
                                            }
                                            else
                                            {
                                                return new { Respuesta = "ERROR:Horario No Válido para Radicar Documentos.", Avanzar = "0" };
                                            }
                                        }
                                        else
                                        {
                                            radicado = new DatosRadicado();

                                            if ((int)documento.ID_RADICADODOC == -1)
                                            {
                                                radicado.IdRadicado = -1;
                                                radicado.Radicado = "SERIE DOCUMENTAL SIN RADICADO";
                                                radicado.Fecha = DateTime.Now;
                                            }
                                            else
                                            {
                                                RADICADO_DOCUMENTO radicadoCreado = dbSIM.RADICADO_DOCUMENTO.Where(rd => rd.ID_RADICADODOC == (int)documento.ID_RADICADODOC).FirstOrDefault();
                                                radicado.IdRadicado = (int)documento.ID_RADICADODOC;
                                                radicado.Radicado = radicadoCreado.S_RADICADO;
                                                radicado.Fecha = radicadoCreado.D_RADICADO;
                                            }
                                        }

                                        radicadoGenerado = radicado.Radicado;

                                        //var seriesGrupos = ConfigurationManager.AppSettings["SeriesProyeccionGrupos"];
                                        var seriesGrupos = Utilidades.Data.ObtenerValorParametro("PD_SeriesProyeccionGrupos");
                                        var codSeriesGrupos = seriesGrupos.Split(',').Select(s => int.Parse(s));
                                        //var indicesPara = ConfigurationManager.AppSettings["IndicesProyeccionGrupos"];
                                        var indicesPara = Utilidades.Data.ObtenerValorParametro("PD_IndicesProyeccionGruposPARA");
                                        var codindicesPara = indicesPara.Split(',').Select(s => int.Parse(s));
                                        //var indicesAsunto = ConfigurationManager.AppSettings["IndiceProyeccionGruposAsunto"];
                                        var indicesAsunto = Utilidades.Data.ObtenerValorParametro("PD_IndiceProyeccionGruposAsunto");
                                        var codindicesAsunto = indicesAsunto.Split(',').Select(s => int.Parse(s));

                                        PROYECCION_DOC_INDICES paraProyeccion = dbSIM.PROYECCION_DOC_INDICES.Where(p => p.ID_PROYECCION_DOC == documento.ID_PROYECCION_DOC && codindicesPara.Contains(p.CODINDICE)).FirstOrDefault();
                                        PROYECCION_DOC_INDICES asuntoProyeccion = dbSIM.PROYECCION_DOC_INDICES.Where(p => p.ID_PROYECCION_DOC == documento.ID_PROYECCION_DOC && codindicesAsunto.Contains(p.CODINDICE)).FirstOrDefault();

                                        bool noAvanzaFuncionarioDiferente = false;

                                        if (documento.S_TRAMITE_NUEVO == "S")
                                        {
                                            if (documento.S_TRAMITES == null)
                                            {
                                                decimal codProceso = 0;
                                                decimal codTarea = 0;
                                                // Crea el trámite y lo relaciona con la Proyección

                                                string codProcesoTareaSerieDocumental = SIM.Utilidades.Data.ObtenerValorParametro("PD_ProcesoTareaNuevoTramite");

                                                if (codProcesoTareaSerieDocumental != null && codProcesoTareaSerieDocumental != "")
                                                {
                                                    var confSeries = codProcesoTareaSerieDocumental.Split('|');

                                                    foreach (string confSerie in confSeries)
                                                    {
                                                        var procesoTareaSerie = confSerie.Split(';');

                                                        if (procesoTareaSerie[0] == "*")
                                                        {
                                                            codProceso = Convert.ToDecimal(procesoTareaSerie[1]);
                                                            codTarea = Convert.ToDecimal(procesoTareaSerie[2]);
                                                        }
                                                        else if (procesoTareaSerie[0] == documento.CODSERIE.ToString())
                                                        {
                                                            codProceso = Convert.ToDecimal(procesoTareaSerie[1]);
                                                            codTarea = Convert.ToDecimal(procesoTareaSerie[2]);
                                                            break;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    codProceso = Convert.ToDecimal(SIM.Utilidades.Data.ObtenerValorParametro("ProcesoNuevoTramite"));
                                                    codTarea = Convert.ToDecimal(SIM.Utilidades.Data.ObtenerValorParametro("TareaNuevoTramite"));
                                                }

                                                var codFuncionarioTN = documento.CODFUNCIONARIO_ACTUAL;
                                                //ObjectParameter respCodTramite = new ObjectParameter("respCodTramite", typeof(decimal));
                                                //ObjectParameter respCodTarea = new ObjectParameter("respCodTarea", typeof(decimal));
                                                //ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));

                                                int[] respNuevoTramite;
                                                int respCodTramite;
                                                int respCodTarea;
                                                int funcionarioPrincipal = -1;

                                                //dbTramites.SP_NUEVO_TRAMITE(codProceso, codTarea, codFuncionarioTN, documento.S_DESCRIPCION, respCodTramite, respCodTarea, rtaResultado);
                                                if (documento.ID_GRUPO == null) {
                                                    funcionarioPrincipal = (paraProyeccion != null && paraProyeccion.ID_VALOR != null ? (int)paraProyeccion.ID_VALOR : (int)codFuncionarioTN);

                                                    respNuevoTramite = Utilidades.Tramites.CrearTramite(Convert.ToInt32(codProceso), Convert.ToInt32(codTarea), null, documento.S_DESCRIPCION, documento.S_DESCRIPCION, (int)codFuncionarioTN, funcionarioPrincipal, null, null, false);
                                                    respCodTramite = respNuevoTramite[0];
                                                    respCodTarea = respNuevoTramite[1];
                                                }
                                                else
                                                {
                                                    var sqlFuncionariosGrupo = "SELECT mf.CODFUNCIONARIO " +
                                                        "FROM TRAMITES.MEMORANDO_FUNCGRUPO mf " +
                                                        "WHERE mf.ID_GRUPOMEMO = " + documento.ID_GRUPO.ToString() + " AND mf.CODFUNCIONARIO <> " + codFuncionarioTN.ToString();

                                                    var funcionariosGrupo = dbSIM.Database.SqlQuery<int>(sqlFuncionariosGrupo).ToArray();
                                                    var funcionarioPrincipalGrupo = funcionariosGrupo[0];

                                                    funcionarioPrincipal = (paraProyeccion != null && paraProyeccion.ID_VALOR != null ? (int)paraProyeccion.ID_VALOR : funcionarioPrincipalGrupo);

                                                    respNuevoTramite = Utilidades.Tramites.CrearTramite(Convert.ToInt32(codProceso), Convert.ToInt32(codTarea), null, documento.S_DESCRIPCION, documento.S_DESCRIPCION, (int)codFuncionarioTN, funcionarioPrincipal, funcionariosGrupo, null, false);
                                                    respCodTramite = respNuevoTramite[0];
                                                    respCodTarea = respNuevoTramite[1];
                                                }

                                                noAvanzaFuncionarioDiferente = (funcionarioPrincipal != codFuncionarioTN);

                                                TRAMITES_PROYECCION nuevoTramite = new TRAMITES_PROYECCION();
                                                nuevoTramite.ID_PROYECCION_DOC = documento.ID_PROYECCION_DOC;
                                                nuevoTramite.CODTRAMITE = respCodTramite;
                                                nuevoTramite.CODTAREA_INICIAL = respCodTarea;

                                                dbSIM.Entry(nuevoTramite).State = System.Data.Entity.EntityState.Added;
                                                dbSIM.SaveChanges();

                                                int codTramite = respCodTramite;

                                                documento.S_TRAMITES = respCodTramite.ToString();
                                                //documentoProyeccion.D_FECHA_TRAMITE = dbSIM.TBTRAMITE.Where(t => t.CODTRAMITE == codTramite).Select(t => t.FECHAINI).FirstOrDefault();
                                                int codProcesoNuevoTramite = Convert.ToInt32(codProceso);
                                                documento.S_PROCESOS = dbSIM.TBPROCESO.Where(p => p.CODPROCESO == codProcesoNuevoTramite).Select(p => p.NOMBRE).FirstOrDefault();

                                                dbSIM.Entry(documento).State = System.Data.Entity.EntityState.Modified;
                                                dbSIM.SaveChanges();

                                                tramiteNuevo = nuevoTramite.CODTRAMITE.ToString();
                                            }
                                            else
                                            {
                                                tramiteNuevo = documento.S_TRAMITES;
                                            }
                                        }

                                        var firmaActual = dbSIM.PROYECCION_DOC_FIRMAS.Where(f => f.ID_PROYECCION_DOC == datos.Id && f.CODFUNCIONARIO == codFuncionario).FirstOrDefault();
                                        firmaActual.S_ESTADO = "S";
                                        firmaActual.CODCARGO = datos.Cargo;
                                        firmaActual.S_TIPOFIRMA = datos.TipoFirma;
                                        firmaActual.D_FECHA_FIRMA = DateTime.Now;
                                        firmaActual.TMP_PASS = ed3des.encrypt(_Password);

                                        dbSIM.Entry(firmaActual).State = EntityState.Modified;
                                        dbSIM.SaveChanges();

                                        var documentoRadicado = utilidad.DocumentoRadicado(documento.ID_PROYECCION_DOC, (radicado.IdRadicado > 0), radicado.IdRadicado, false, "Firmado");
                                        //tramiteNuevo = RegistrarDocumento(documento.ID_PROYECCION_DOC, documentoRadicado.documentoBinario, documentoRadicado.numPaginas);

                                        if (documentoRadicado.exito)
                                        {
                                            RegistrarDocumento(documento.ID_PROYECCION_DOC, documentoRadicado.documentoBinario, documentoRadicado.numPaginas);

                                            if (codSeriesGrupos.Contains(documento.CODSERIE))
                                            {
                                                var serieDocumental = dbSIM.TBSERIE.FirstOrDefault(s => s.CODSERIE == documento.CODSERIE).NOMBRE;

                                                if (documento.ID_GRUPO != null)
                                                {
                                                    EnviarEmailGrupo(serieDocumental, documento.CODFUNCIONARIO, documento.ID_GRUPO, null, radicado.IdRadicado, (asuntoProyeccion != null ? asuntoProyeccion.S_VALOR : documento.S_DESCRIPCION));
                                                }
                                                else
                                                {
                                                    EnviarEmailGrupo(serieDocumental, documento.CODFUNCIONARIO, null, (paraProyeccion != null ? paraProyeccion.ID_VALOR : null) , radicado.IdRadicado, (asuntoProyeccion != null ? asuntoProyeccion.S_VALOR : documento.S_DESCRIPCION));
                                                }
                                            }

                                            if (documento.S_TRAMITE_NUEVO != "S")
                                            {
                                                int codFuncionarioTramite;

                                                if (paraProyeccion != null && paraProyeccion.ID_VALOR != null)
                                                {
                                                    codFuncionarioTramite = (int)paraProyeccion.ID_VALOR;

                                                    noAvanzaFuncionarioDiferente = ((int)paraProyeccion.ID_VALOR != documento.CODFUNCIONARIO);
                                                }
                                                else
                                                {
                                                    codFuncionarioTramite = documento.CODFUNCIONARIO;
                                                }

                                                string comentarioTramite;
                                                if (noAvanzaFuncionarioDiferente)
                                                {
                                                    comentarioTramite = documento.S_DESCRIPCION;
                                                }
                                                else
                                                {
                                                    comentarioTramite = "Documento " + (radicado.IdRadicado > 0 ? "Radicado" : "Generado") + " y Enviado Nuevamente al Usuario que Proyectó el Documento.";
                                                }

                                                datosTareaTramiteFormulario = new DatosAvanzaTareaTramiteFormulario()
                                                {
                                                    codTramites = tareasInicialesTramites,
                                                    codTarea = 0,
                                                    codTareaSiguiente = 0,
                                                    codFuncionario = codFuncionarioTramite,
                                                    formularioSiguiente = "20",
                                                    copias = "",
                                                    idGrupo = documento.ID_GRUPO,
                                                    comentario = comentarioTramite
                                                };
                                            }
                                            else
                                            {
                                                tramitesAvanzar = tramiteNuevo;
                                            }

                                            if (documento.S_NO_AVANZAR == "S" || noAvanzaFuncionarioDiferente)
                                                avanzar = "0";
                                            else
                                                avanzar = "1";
                                        }
                                        else
                                        {
                                            Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "ProyeccionDocumento [PostAvanzarDocumento - " + datos.Id.ToString() + " ] : Error generando documento. " + documentoRadicado.mensaje);
                                            return new { Respuesta = "Error:¡Advertencia!<br>No puede avanzar el documento.<br>Ocurrió un problema radicando el documento y no puede firmar este documento.<br>" + documentoRadicado.mensaje, Avanzar = "0" };
                                        }
                                    }
                                    else // Hay más firmas, por lo tanto se queda en el mismo nodo del flujo para recoletar las demás firmas
                                    {
                                        var funcionariosSiguiente = dbSIM.TBFUNCIONARIO.Where(f => f.CODFUNCIONARIO == firma.CODFUNCIONARIO).FirstOrDefault();

                                        if (funcionariosSiguiente.ACTIVO == "0")
                                        {
                                            return new { Respuesta = "Error:¡Advertencia!<br>No puede avanzar el documento.<br>Actualmente el usuario " + funcionariosSiguiente.NOMBRES + " " + funcionariosSiguiente.APELLIDOS + " se encuentra inhabilitado en el sistema y no puede firmar este documento.<br>Si desea cambiar el responsable de la firma puede devolver la tarea a quién proyectó el documento.", Avanzar = "0" };
                                        }

                                        if (documento.S_TRAMITE_NUEVO != "S")
                                        {
                                            int codTramite = tramites.FirstOrDefault().CODTRAMITE;

                                            TBTRAMITETAREA tramiteTarea = dbSIM.TBTRAMITETAREA.Where(tt => tt.CODTRAMITE == codTramite && tt.ESTADO == 0).FirstOrDefault();

                                            int codTarea = Convert.ToInt32(tramiteTarea.CODTAREA);

                                            datosTareaTramiteFormulario = new DatosAvanzaTareaTramiteFormulario()
                                            {
                                                codTramites = tramitesAvanzar,
                                                codTarea = 0,
                                                codTareaSiguiente = 0,
                                                codFuncionario = firma.CODFUNCIONARIO,
                                                formularioSiguiente = "21",
                                                copias = "",
                                                comentario = "Documento Enviado Para ser Firmado."
                                            };
                                        }

                                        var firmaActual = dbSIM.PROYECCION_DOC_FIRMAS.Where(f => f.ID_PROYECCION_DOC == datos.Id && f.CODFUNCIONARIO == codFuncionario).FirstOrDefault();
                                        firmaActual.S_ESTADO = "S";
                                        firmaActual.S_TIPOFIRMA = datos.TipoFirma;
                                        firmaActual.CODCARGO = datos.Cargo;
                                        firmaActual.TMP_PASS = ed3des.encrypt(_Password);
                                        firmaActual.D_FECHA_FIRMA = DateTime.Now;

                                        dbSIM.Entry(firmaActual).State = EntityState.Modified;
                                        dbSIM.SaveChanges();
                                    }

                                    if (documento.S_TRAMITE_NUEVO != "S")
                                    {
                                        respuesta = utilidad.AvanzaTareaTramiteFormulario(datosTareaTramiteFormulario);
                                    }
                                    else
                                    {
                                        if (firma == null)
                                            respuesta = new Respuesta() { tipoRespuesta = "OK", detalleRespuesta = "Trámite Creado -> " + tramiteNuevo };
                                        else
                                            respuesta = new Respuesta() { tipoRespuesta = "OK", detalleRespuesta = "" };
                                    }

                                    if (firma == null)
                                    {
                                        documento.S_FORMULARIO = "22";
                                        documento.CODFUNCIONARIO_ACTUAL = documento.CODFUNCIONARIO;
                                        documento.D_FECHA_CREACION = DateTime.Now;
                                        documento.D_FECHA_TRAMITE = DateTime.Now;
                                        dbSIM.Entry(documento).State = EntityState.Modified;
                                        dbSIM.SaveChanges();
                                    }
                                    else
                                    {
                                        documento.CODFUNCIONARIO_ACTUAL = firma.CODFUNCIONARIO;
                                        documento.D_FECHA_TRAMITE = DateTime.Now;
                                        dbSIM.Entry(documento).State = EntityState.Modified;
                                        dbSIM.SaveChanges();
                                    }
                                }
                                else
                                {
                                    if (documento.S_TRAMITE_NUEVO != "S")
                                    {
                                        datosTareaTramiteFormulario = new DatosAvanzaTareaTramiteFormulario()
                                        {
                                            codTramites = tareasInicialesTramites,
                                            codTarea = 0,
                                            codTareaSiguiente = 0,
                                            codFuncionario = documento.CODFUNCIONARIO,
                                            formularioSiguiente = "20",
                                            copias = "",
                                            comentario = "Documento Devuelto Para ser Corregido."
                                        };

                                        respuesta = utilidad.AvanzaTareaTramiteFormulario(datosTareaTramiteFormulario);
                                    }
                                    else
                                    {
                                        respuesta = new Respuesta() { tipoRespuesta = "OK", detalleRespuesta = "" };
                                    }

                                    documento.S_FORMULARIO = "20";
                                    documento.CODFUNCIONARIO_ACTUAL = documento.CODFUNCIONARIO;
                                    documento.D_FECHA_TRAMITE = DateTime.Now;
                                    dbSIM.Entry(documento).State = EntityState.Modified;
                                    dbSIM.SaveChanges();

                                    var firmas = dbSIM.PROYECCION_DOC_FIRMAS.Where(pf => pf.ID_PROYECCION_DOC == datos.Id).ToList();

                                    foreach (var firmaInicial in firmas)
                                    {
                                        firmaInicial.S_ESTADO = "N";
                                        firmaInicial.S_TIPOFIRMA = null;
                                        firmaInicial.CODCARGO = null;

                                        dbSIM.Entry(firmaInicial).State = EntityState.Modified;
                                        dbSIM.SaveChanges();
                                    }
                                }
                            }
                            break;
                    }

                    if (datos.Comentario != null && datos.Comentario.Trim() != "" && !datos.Comentario.Contains(";"))
                    {
                        var comentarios = dbSIM.PROYECCION_DOC_COM.Where(c => c.ID_PROYECCION_DOC == documento.ID_PROYECCION_DOC && c.S_ACTIVO == "S").ToList();

                        foreach (var cp in comentarios)
                        {
                            cp.S_ACTIVO = "N";

                            dbSIM.Entry(cp).State = EntityState.Modified;
                            dbSIM.SaveChanges();
                        }

                        PROYECCION_DOC_COM comentario = new PROYECCION_DOC_COM();

                        comentario.ID_PROYECCION_DOC = documento.ID_PROYECCION_DOC;
                        comentario.FECHA = DateTime.Now;
                        comentario.CODFUNCIONARIO = codFuncionario;
                        comentario.S_COMENTARIO = datos.Comentario;
                        comentario.S_ACTIVO = "S";

                        dbSIM.Entry(comentario).State = EntityState.Added;
                        dbSIM.SaveChanges();

                        // Se ingresa el comentario a cada trámite en la tarea en la que actualmente se encuentan
                        foreach (var tramiteProyeccion in documento.TRAMITES_PROYECCION)
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
                            tareaComentario.COMENTARIO = datos.Comentario ?? "";

                            dbSIM.Entry(tareaComentario).State = EntityState.Added;
                            dbSIM.SaveChanges();
                        }
                    }

                    // Avanzar documento
                    return new { Respuesta = "OK:" + respuesta.detalleRespuesta, Avanzar = avanzar, Tramites = tramitesAvanzar, TramitesIndices = respuesta.datosAdicionales, Radicado = radicadoGenerado };
                }
                else
                {
                    return new { Respuesta = "ERROR:Documento NO Encontrado.", Avanzar = "0" };
                }
            }
            catch (Exception error)
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "ProyeccionDocumento [PostAvanzarDocumento - " + datos.Id.ToString() + " ] : " + Utilidades.LogErrores.ObtenerError(error));

                return new { Respuesta = "ERROR: No se pudo realizar la operación. Contactar a soporte para la verificación del caso.", Avanzar = "0" };
            }
        }

        private bool EnviarEmailGrupo(string serieDocumental, int codFuncionario, int? idGrupo, int? codFuncionarioPara, int idRadicado, string asunto)
        {
            string emailFrom;
            string emailSMTPServer;
            string emailSMTPUser;
            string emailSMTPPwd;
            string emailPara;
            StringBuilder emailHtml;
            TBFUNCIONARIO funcionarioProyecta;
            string funcionariosGrupo = "";
            string funcionarioPara = "";
            //string asunto = serieDocumental + " Radicado No. " + idRadicado.ToString() + " - " + descripcion;

            if (idGrupo != null || codFuncionarioPara != null)
            {
                emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
                emailSMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
                emailSMTPUser = ConfigurationManager.AppSettings["SMTPUser"];
                emailSMTPPwd = ConfigurationManager.AppSettings["SMTPPwd"];

                try
                {
                    funcionarioProyecta = dbSIM.TBFUNCIONARIO.Where(f => f.CODFUNCIONARIO == codFuncionario).FirstOrDefault();

                    if (idGrupo != null && idGrupo != -1)
                    {
                        var sqlFuncionariosGrupo = "SELECT f.EMAIL " +
                                "FROM TRAMITES.MEMORANDO_FUNCGRUPO mf INNER JOIN " +
                                "   TRAMITES.TBFUNCIONARIO f ON mf.CODFUNCIONARIO = f.CODFUNCIONARIO " +
                                "WHERE ID_GRUPOMEMO = " + idGrupo.ToString();

                        funcionariosGrupo = String.Join(";", dbSIM.Database.SqlQuery<string>(sqlFuncionariosGrupo).ToList());

                        if (funcionariosGrupo.Trim() == ";")
                            funcionariosGrupo = "";

                        emailPara = funcionariosGrupo;
                    }
                    else
                    {
                        var sqlFuncionarioPara = "SELECT f.EMAIL " +
                                "FROM TRAMITES.TBFUNCIONARIO f " +
                                "WHERE CODFUNCIONARIO = " + codFuncionarioPara.ToString();

                        funcionarioPara = dbSIM.Database.SqlQuery<string>(sqlFuncionarioPara).FirstOrDefault();

                        emailPara = funcionarioPara;
                    }

                    var radicado = dbSIM.RADICADO_DOCUMENTO.Where(rd => rd.ID_RADICADODOC == idRadicado).Select(rd => rd.S_RADICADO).FirstOrDefault();

                    string saludo = "";

                    if (DateTime.Now.Hour < 12) saludo = "Buenos D&iacute;as";
                    else if (DateTime.Now.Hour < 18) saludo = "Buenas Tardes";
                    else if (DateTime.Now.Hour < 24) saludo = "Buenas Noches";

                    emailHtml = new StringBuilder(File.ReadAllText(HostingEnvironment.MapPath("~/Content/plantillas/PlantillaNotificacionProyeccion.html")));
                    emailHtml.Replace("[Tipo Proyeccion]", serieDocumental);
                    emailHtml.Replace("[Radicado]", (radicado ?? ""));
                    emailHtml.Replace("[Saludo]", saludo);
                    emailHtml.Replace("[De]", funcionarioProyecta.NOMBRES + " " + funcionarioProyecta.APELLIDOS);
                    emailHtml.Replace("[Asunto]", asunto);
                }
                catch (Exception error)
                {
                    Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), Utilidades.LogErrores.ObtenerError(error));
                    return false;
                }

                try
                {
                    Utilidades.Email.EnviarEmail(emailFrom, funcionarioProyecta.EMAIL, emailPara, "", asunto, emailHtml.ToString(), emailSMTPServer, true, emailSMTPUser, emailSMTPPwd, null);
                }
                catch (Exception error)
                {
                    Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), Utilidades.LogErrores.ObtenerError(error));
                    return false;
                }
            }

            return true;
        }

        [Authorize]
        [HttpGet, ActionName("ObtenerIndiceValoresLista")]
        public dynamic GetObtenerIndiceValoresLista(int id)
        {
            List<ValorLista> resultadoConsulta;
            string sql;
            TBSUBSERIE lista = dbSIM.TBSUBSERIE.Where(ss => ss.CODIGO_SUBSERIE == id).FirstOrDefault();

            if (lista != null)
            {
                if (lista.TIPO == 0) // La lista se toma de la tabla TBDETALLE_SUBSERIE
                {
                    sql = "SELECT CODIGO_DETALLE AS ID, NOMBRE FROM TRAMITES.TBDETALLE_SUBSERIE WHERE CODIGO_SUBSERIE = " + lista.CODIGO_SUBSERIE.ToString() + " ORDER BY NOMBRE";

                    resultadoConsulta = dbSIM.Database.SqlQuery<ValorLista>(sql).ToList<ValorLista>();
                }
                else
                {
                    //sql = lista.SQL;

                    resultadoConsulta = dbSIM.Database.SqlQuery<ValorLista>("SELECT " + lista.CAMPO_ID + " AS ID, " + lista.CAMPO_NOMBRE + " AS NOMBRE FROM (" + lista.SQL + ") datos").ToList<ValorLista>();
                }

                //ObjectParameter jsonOut = new ObjectParameter("jSONOUT", typeof(string));
                //dbSIM.SP_GET_DATOS(sql, jsonOut);
                //return Json(jsonOut.Value);
                return resultadoConsulta;
            }
            else
            {
                return null;
            }
        }

        private DatosRadicado RadicarDocumento(int idDocumento)
        {
            var documentoProyeccion = dbSIM.PROYECCION_DOC.Where(pd => pd.ID_PROYECCION_DOC == idDocumento).FirstOrDefault();
            var usuario = dbSIM.USUARIO_FUNCIONARIO.Where(uf => uf.CODFUNCIONARIO == documentoProyeccion.CODFUNCIONARIO).FirstOrDefault();

            if (documentoProyeccion.ID_RADICADODOC == null)
            {
                var fechaCreacion = DateTime.Now;
                Radicador radicador = new Radicador();
                DatosRadicado radicadoGenerado = radicador.GenerarRadicado(dbSIM, documentoProyeccion.CODSERIE, usuario.ID_USUARIO, fechaCreacion);

                return radicadoGenerado;
            }
            else
            {
                int idRadicadoAsignado = (int)documentoProyeccion.ID_RADICADODOC;

                if (idRadicadoAsignado == -1)
                {
                    return new DatosRadicado() { IdRadicado = idRadicadoAsignado, Radicado = "SERIE DOCUMENTAL SIN RADICADO", Etiqueta = "", Fecha = DateTime.Now };
                }
                else
                {
                    RADICADO_DOCUMENTO nuevoRadicado = dbSIM.RADICADO_DOCUMENTO.Where(r => r.ID_RADICADODOC == idRadicadoAsignado).FirstOrDefault();

                    return new DatosRadicado() { IdRadicado = idRadicadoAsignado, Radicado = nuevoRadicado.S_RADICADO, Etiqueta = nuevoRadicado.S_ETIQUETA, Fecha = nuevoRadicado.D_RADICADO };
                }
            }
        }

        private string RegistrarDocumento(int idDocumento, byte[] documentoBinario, int numPaginas)
        {
            TBINDICEDOCUMENTO indiceDocumento;
            int idCodDocumento;
            string rutaDocumento = "";
            string tramiteNuevo = "";

            var documentoProyeccion = dbSIM.PROYECCION_DOC.Where(pd => pd.ID_PROYECCION_DOC == idDocumento).FirstOrDefault();

            //if (documentoProyeccion.S_TRAMITE_NUEVO == "S")
            //{
            //    // Crea el trámite y lo relaciona con la Proyección

            //    var codProceso = Convert.ToDecimal(Data.ObtenerValorParametro("ProcesoNuevoTramite"));
            //    var codTarea = Convert.ToDecimal(Data.ObtenerValorParametro("TareaNuevoTramite"));
            //    var codFuncionario = documentoProyeccion.CODFUNCIONARIO;
            //    ObjectParameter respCodTramite = new ObjectParameter("respCodTramite", typeof(decimal));
            //    ObjectParameter respCodTarea = new ObjectParameter("respCodTarea", typeof(decimal));
            //    ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));

            //    dbSIM.SP_NUEVO_TRAMITE(codProceso, codTarea, codFuncionario, documentoProyeccion.S_DESCRIPCION, respCodTramite, respCodTarea, rtaResultado);

            //    TRAMITES_PROYECCION nuevoTramite = new TRAMITES_PROYECCION();
            //    nuevoTramite.ID_PROYECCION_DOC = idDocumento;
            //    nuevoTramite.CODTRAMITE = Convert.ToInt32(respCodTramite.Value);
            //    nuevoTramite.CODTAREA_INICIAL = Convert.ToInt32(respCodTarea.Value);

            //    dbSIM.Entry(nuevoTramite).State = System.Data.Entity.EntityState.Added;
            //    dbSIM.SaveChanges();

            //    int codTramite = Convert.ToInt32(respCodTramite.Value);

            //    documentoProyeccion.S_TRAMITES = respCodTramite.Value.ToString();
            //    //documentoProyeccion.D_FECHA_TRAMITE = dbSIM.TBTRAMITE.Where(t => t.CODTRAMITE == codTramite).Select(t => t.FECHAINI).FirstOrDefault();
            //    int codProcesoNuevoTramite = Convert.ToInt32(codProceso);
            //    documentoProyeccion.S_PROCESOS = dbSIM.TBPROCESO.Where(p => p.CODPROCESO == codProcesoNuevoTramite).Select(p => p.NOMBRE).FirstOrDefault();

            //    tramiteNuevo = nuevoTramite.CODTRAMITE.ToString();
            //}

            TramitesLibrary utilidad = new TramitesLibrary();
            var tramites = dbSIM.TRAMITES_PROYECCION.Where(tp => tp.ID_PROYECCION_DOC == idDocumento).ToList();
            var usuario = dbSIM.USUARIO_FUNCIONARIO.Where(uf => uf.CODFUNCIONARIO == documentoProyeccion.CODFUNCIONARIO).FirstOrDefault();

            DateTime fechaActual = DateTime.Now;

            int contTramites = 1;

            RADICADO_DOCUMENTO radicado = dbSIM.RADICADO_DOCUMENTO.Where(r => r.ID_RADICADODOC == documentoProyeccion.ID_RADICADODOC).FirstOrDefault();

            foreach (var tramiteDoc in tramites)
            {
                TBTRAMITE tramite = dbSIM.TBTRAMITE.Where(t => t.CODTRAMITE == tramiteDoc.CODTRAMITE).FirstOrDefault();
                TBRUTAPROCESO rutaProceso = dbSIM.TBRUTAPROCESO.Where(rp => rp.CODPROCESO == tramite.CODPROCESO).FirstOrDefault();
                TBTRAMITEDOCUMENTO ultimoDocumento = dbSIM.TBTRAMITEDOCUMENTO.Where(td => td.CODTRAMITE == tramiteDoc.CODTRAMITE).OrderByDescending(td => td.CODDOCUMENTO).FirstOrDefault();

                if (ultimoDocumento == null)
                    idCodDocumento = 1;
                else
                    idCodDocumento = Convert.ToInt32(ultimoDocumento.CODDOCUMENTO) + 1;

                rutaDocumento = rutaProceso.PATH + "\\" + Archivos.GetRutaDocumento(Convert.ToUInt64(tramite.CODTRAMITE), 100) + tramite.CODTRAMITE.ToString("0") + "-" + idCodDocumento.ToString() + ".pdf";

                if (!Directory.Exists(Path.GetDirectoryName(rutaDocumento)))
                    Directory.CreateDirectory(Path.GetDirectoryName(rutaDocumento));

                if (File.Exists(rutaDocumento))
                {
                    var documentoExistente = dbSIM.TBTRAMITEDOCUMENTO.Where(td => td.RUTA == rutaDocumento).FirstOrDefault();

                    if (documentoExistente == null)
                    {
                        var rutaDocumentoCopia = rutaProceso.PATH + "\\" + Archivos.GetRutaDocumento(Convert.ToUInt64(tramite.CODTRAMITE), 100) + tramite.CODTRAMITE.ToString("0") + "-" + idCodDocumento.ToString() + "_Copia.pdf";

                        File.Move(rutaDocumento, rutaDocumentoCopia);
                    }
                    else
                    {
                        throw new Exception("Trámite " + tramite.CODTRAMITE.ToString() + " intenta crear el documento con la ruta '" + rutaDocumento + "', la cual ya existte en el trámite " + documentoExistente.CODTRAMITE.ToString());
                    }
                }

                FileStream archivoRadicado = new FileStream(rutaDocumento, FileMode.CreateNew);

                archivoRadicado.Write(documentoBinario, 0, documentoBinario.Length);
                archivoRadicado.Close();

                TBTRAMITEDOCUMENTO documento = new TBTRAMITEDOCUMENTO();
                documento.CODDOCUMENTO = idCodDocumento;
                documento.CODTRAMITE = tramite.CODTRAMITE;
                documento.TIPODOCUMENTO = 1;
                documento.FECHACREACION = DateTime.Now;
                documento.CODFUNCIONARIO = documentoProyeccion.CODFUNCIONARIO;
                documento.ID_USUARIO = usuario.ID_USUARIO;
                documento.RUTA = rutaDocumento;
                documento.MAPAARCHIVO = "M";
                documento.MAPABD = "M";
                documento.PAGINAS = numPaginas;
                documento.CODSERIE = Convert.ToInt32(documentoProyeccion.CODSERIE);
                documento.CIFRADO = "0";

                dbSIM.Entry(documento).State = System.Data.Entity.EntityState.Added;
                dbSIM.SaveChanges();

                if (radicado != null && contTramites == 1)
                {
                    radicado.CODTRAMITE = tramite.CODTRAMITE;
                    radicado.CODDOCUMENTO = idCodDocumento;
                    radicado.ID_DOCUMENTO = documento.ID_DOCUMENTO;
                    dbSIM.Entry(radicado).State = System.Data.Entity.EntityState.Modified;
                    dbSIM.SaveChanges();

                    contTramites++;
                }

                TBTRAMITE_DOC documentoRelacionado = new TBTRAMITE_DOC();
                documentoRelacionado.CODTRAMITE = documento.CODTRAMITE;
                documentoRelacionado.CODDOCUMENTO = documento.CODDOCUMENTO;
                documentoRelacionado.ID_DOCUMENTO = documento.ID_DOCUMENTO;
                dbSIM.Entry(documentoRelacionado).State = System.Data.Entity.EntityState.Added;
                dbSIM.SaveChanges();

                TRAMITES_PROYECCION tramiteAsignado = dbSIM.TRAMITES_PROYECCION.Where(tp => tp.ID_TRAMITES_PROYECCION == tramiteDoc.ID_TRAMITES_PROYECCION).FirstOrDefault();
                tramiteAsignado.CODDOCUMENTO = idCodDocumento;
                tramiteAsignado.D_FECHA_GENERACION = fechaActual;
                tramiteAsignado.ID_DOCUMENTO = documento.ID_DOCUMENTO;
                dbSIM.Entry(tramiteAsignado).State = System.Data.Entity.EntityState.Modified;
                dbSIM.SaveChanges();

                if (radicado != null)
                {
                    // Consulta Datos de los Indices del Documento
                    var indiceSerieRadicado = dbSIM.TBINDICESERIE.Where(isd => isd.CODSERIE == documentoProyeccion.CODSERIE && isd.INDICE_RADICADO != null).ToList();

                    foreach (var indice in indiceSerieRadicado)
                    {
                        string valor = "";
                        if (indice.INDICE_RADICADO.Trim() == "R")
                        {
                            valor = radicado.S_RADICADO;
                        }
                        else
                        {
                            valor = radicado.D_RADICADO.ToString("dd/MM/yyyy");
                        }

                        indiceDocumento = new TBINDICEDOCUMENTO();
                        indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                        indiceDocumento.CODDOCUMENTO = idCodDocumento;
                        indiceDocumento.CODINDICE = indice.CODINDICE;
                        indiceDocumento.VALOR = valor;
                        indiceDocumento.ID_DOCUMENTO = documento.ID_DOCUMENTO;
                        dbSIM.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                        dbSIM.SaveChanges();
                    }
                }

                var indiceDocumentoRadicado = dbSIM.PROYECCION_DOC_INDICES.Where(pi => pi.ID_PROYECCION_DOC == idDocumento).ToList();

                foreach (var indice in indiceDocumentoRadicado)
                {
                    indiceDocumento = new TBINDICEDOCUMENTO();
                    indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                    indiceDocumento.CODDOCUMENTO = idCodDocumento;
                    indiceDocumento.CODINDICE = indice.CODINDICE;
                    indiceDocumento.VALOR = indice.S_VALOR;
                    indiceDocumento.ID_DOCUMENTO = documento.ID_DOCUMENTO;
                    dbSIM.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                    dbSIM.SaveChanges();
                }

                try
                {
                    utilidad.GenerarIndicesFullTextDocumento(Convert.ToInt32(tramite.CODTRAMITE), idCodDocumento);
                }
                catch (Exception error)
                {
                    Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "RegistrarDocumento [GenerarIndicesFullTextDocumento(" + Convert.ToInt32(tramite.CODTRAMITE).ToString() + ", " + idCodDocumento.ToString() + ")]" + Utilidades.LogErrores.ObtenerError(error));
                }
            }

            return tramiteNuevo;
        }

        private dynamic DocumentoRadicado(int id, int idRadicado)
        {
            TramitesLibrary utilidad = new TramitesLibrary();

            var documento = utilidad.DocumentoRadicado(id, true, idRadicado, false, "Firmado electrónicamente según decreto 491 de 2020");

            return documento;
        }

        [HttpGet, ActionName("ComentariosDocumento")]
        public dynamic GetComentariosDocumento(int id)
        {
            string comentariosDocumento = "";

            var comentarios = (from c in dbSIM.PROYECCION_DOC_COM
                               join f in dbSIM.TBFUNCIONARIO on c.CODFUNCIONARIO equals f.CODFUNCIONARIO
                               where c.ID_PROYECCION_DOC == id
                               orderby c.FECHA descending
                               select new
                               {
                                   FUNCIONARIO = f.NOMBRES + " " + f.APELLIDOS,
                                   FECHA = c.FECHA,
                                   COMENTARIO = c.S_COMENTARIO
                               }).ToList();

            foreach (var comentario in comentarios)
            {
                comentariosDocumento += "===============================================================================================\r\n";
                comentariosDocumento += "[" + comentario.FECHA.ToString("dd/MM/yyyy HH:mm") + "] " + comentario.FUNCIONARIO + "\r\n";
                comentariosDocumento += "===============================================================================================\r\n";
                comentariosDocumento += comentario.COMENTARIO + "\r\n" + "\r\n";
            }

            return comentariosDocumento;
        }

        [HttpGet, ActionName("CargarTramitesProcesos")]
        public void GetCargarTramitesProcesos()
        {
            var proyecciones = (from p in dbSIM.PROYECCION_DOC
                                    //where p.S_TRAMITES == null && p.S_PROCESOS == null
                                select p).ToList();

            foreach (var proyeccion in proyecciones)
            {
                string listaTareasTramites = string.Join(", ", proyeccion.TRAMITES_PROYECCION.Select<TRAMITES_PROYECCION, string>(tp => tp.CODTAREA_INICIAL.ToString()));

                if (listaTareasTramites != null && listaTareasTramites.Trim() != "")
                {
                    var codProcesos = dbSIM.Database.SqlQuery<int>("SELECT CODPROCESO FROM TRAMITES.TBTAREA WHERE CODTAREA IN (" + listaTareasTramites + ")").ToList();


                    string listaTramites = string.Join(", ", proyeccion.TRAMITES_PROYECCION.Select<TRAMITES_PROYECCION, string>(tp => tp.CODTRAMITE.ToString()));
                    string listaProcesos = string.Join(", ", dbSIM.TBPROCESO.Where(p => codProcesos.Contains((int)p.CODPROCESO)).Select<TBPROCESO, string>(p => p.NOMBRE).Distinct());

                    proyeccion.S_TRAMITES = listaTramites;
                    proyeccion.S_PROCESOS = listaProcesos;

                    dbSIM.Entry(proyeccion).State = System.Data.Entity.EntityState.Modified;
                    dbSIM.SaveChanges();
                }
            }
        }

        [HttpGet, ActionName("CargarFechaTramites")]
        public void GetCargarFechaTramites()
        {
            var proyecciones = (from p in dbSIM.PROYECCION_DOC
                                    //where p.S_TRAMITES == null && p.S_PROCESOS == null
                                select p).ToList();

            foreach (var proyeccion in proyecciones)
            {
                if (proyeccion.D_FECHA_TRAMITE == null && proyeccion.S_TRAMITES != null && proyeccion.S_TRAMITES.Trim() != "")
                {
                    Nullable<DateTime> fechaTramite = dbSIM.Database.SqlQuery<Nullable<DateTime>>("SELECT MIN(FECHAINI) FROM TRAMITES.TBTRAMITE WHERE CODTRAMITE IN (" + proyeccion.S_TRAMITES.Trim() + ")").FirstOrDefault();

                    if (fechaTramite != null)
                    {
                        proyeccion.D_FECHA_TRAMITE = fechaTramite;
                    }

                    dbSIM.Entry(proyeccion).State = System.Data.Entity.EntityState.Modified;
                    dbSIM.SaveChanges();
                }
            }
        }

        [Authorize]
        [HttpGet, ActionName("EliminarDocumento")]
        public string GetEliminarDocumento(int id)
        {
            var documentoProyeccion = dbSIM.PROYECCION_DOC.Where(pd => pd.ID_PROYECCION_DOC == id).FirstOrDefault();

            if (documentoProyeccion != null)
            {
                documentoProyeccion.S_FORMULARIO = "25";
                documentoProyeccion.D_FECHA_TRAMITE = DateTime.Now;

                dbSIM.Entry(documentoProyeccion).State = System.Data.Entity.EntityState.Modified;
                dbSIM.SaveChanges();

                return "OK";
            }
            else
            {
                return "ERROR:Documento Inválido.";
            }
        }
        /// <summary>
        /// Determina siquein esta firmando esta marcado como principal y asu vez se valida si posee una firma digital
        /// </summary>
        /// <param name="id">Identificador de la proyeccion</param>
        /// <param name="Funcionario">Codigo del funcionario que esta firmando</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet, ActionName("EsFirmaDigital")]
        public string GetEsFirmaDigital(int id)
        {
            string _Resp = "";
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            int idUsuario = 0;
            try
            {
                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                }

                if (idUsuario == 0)
                {
                    _Resp = "Error: El Usuario no se encuentra autenticado.";
                }

                int Funcionario = dbSIM.USUARIO_FUNCIONARIO.Where(uf => uf.ID_USUARIO == idUsuario).Select(uf => uf.CODFUNCIONARIO).FirstOrDefault();
                var FuncFirma = (from Firma in dbSIM.PROYECCION_DOC_FIRMAS
                                 join Fun in dbSIM.TBFUNCIONARIO on Firma.CODFUNCIONARIO equals Fun.CODFUNCIONARIO
                                 where Firma.ID_PROYECCION_DOC == id && Firma.CODFUNCIONARIO == Funcionario
                                 select new
                                 {
                                     Firma.S_TIPO,
                                     Fun.FIRMA_DIGITAL
                                 }).FirstOrDefault();
                if (FuncFirma.S_TIPO == "Digital" && FuncFirma.FIRMA_DIGITAL == "1") _Resp = "Ok";
                else if (FuncFirma.S_TIPO == "Digital" && FuncFirma.FIRMA_DIGITAL == "0") _Resp = "Error: El funcionario esta marcado como Principal pero no posee una firma digital!!";
            }
            catch (Exception error)
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), Utilidades.LogErrores.ObtenerError(error));

                _Resp = "Error: Ocurrio Error al consultar el funcionario que firma.";
            }
            return _Resp;
        }
    }
}
