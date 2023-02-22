using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using System.Web;
using DevExpress.Web.Mvc;
using SIM.Areas.ControlVigilancia.Models;
using System.Security.Claims;
using System.Data.Linq.SqlClient;
using Newtonsoft.Json;
using System.Data.OleDb;
using System.Data;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using DevExpress.BarCodes;
using System.Drawing.Imaging;
using SIM.Areas.Fotografia.Controllers;
using System.Data.Entity.Core.Objects;
using Excel;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using SIM.Utilidades;
using PdfSharp.Drawing;
using System.Text;
using PdfSharp.Pdf.Content;
//using Novacode;
using System.Web.Hosting;
using SIM.Areas.Tramites.Models;
using DevExpress.Pdf;
using DevExpress.Office.Utils;
using System.Globalization;
using Xceed.Words.NET;
using System.Net.Http.Headers;
using SIM.Data.Control;
using SIM.Data.General;
using SIM.Data.Tramites;
using SIM.Models;
using SIM.Areas.Models;

namespace SIM.Areas.General.Controllers
{
    public class DatosDocumentoRadicado
    {
        public int idVisita { get; set; }
        public int idRadicado  { get; set; }
        public List<INDICE> indicesDocumento { get; set; }
    }


    public class VisitasWebAPIController : ApiController
    {
        public class ItemsVisita
        {
            public int IDVISITA { get; set; }
            public string DESCRIPCION { get; set; }
        }

        public struct datosRespuesta
        {
            public string tipoRespuesta; // OK, Error
            public string detalleRespuesta;
            public string id;
        }

        public struct Informe
        {
            public int ID_INF { get; set; }
            public string CM { get; set; }
            public string ASUNTO { get; set; }
            public string QUEJA { get; set; }
            public string OBSERVACION { get; set; }
            public int ID_VISITA { get; set; }
            public int FUNCIONARIO { get; set; }
            public string URL { get; set; }
            public string URL2 { get; set; }
            public int ID_RADICADO { get; set; }
            public DateTime D_RADICADO { get; set; }
            public string S_RADICADO { get; set; }
    }

        public struct DatosConsultaInformes
        {
            public int numRegistros;
            public List<Informe> datos;
        }

        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema General y Seguridad
        /// </summary>
        EntitiesSIMOracle db = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();
        EntitiesTramitesOracle dbTramites = new EntitiesTramitesOracle();

        System.Web.HttpContext context = System.Web.HttpContext.Current;
        Int32 idUsuario;
        int codFuncionario;
        string Responsable;
        string Copias;
        string IdFiltroAcompanante;
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        private List<VW_VISITAS> VisitasFuncionario(int funcionario)
        {
            string sql =
                "SELECT DISTINCT to_char(vas.D_inicio,'DD/MM/YYYY') D_ASIGNACIONVISITA, " +
                "   v.ID_VISITA, " +
                "   v.S_ASUNTO," +
                "   vas.D_INICIO D_ASIGNACION, " +
                "   vac.D_INICIO D_ACEPTACION, " +
                "   vep.D_INICIO D_INICIAL, " +
                "   ver.D_INICIO D_INICIOREV, " +
                "   vte.D_INICIO D_FINAL, " +
                "   v.S_OBSERVACION, " +
                "   ev.ID_ESTADOVISITA, " +
                "   ev.S_NOMBRE AS ESTADO_VISITA, " +
                "   rv.S_RADICADO AS RADICADO_VISITA, " +
                "   v.ID_TIPOVISITA, " +
                "   tv.S_NOMBRE S_NOMBRE_TIPOVISITA, " +
                "   t.X, " +
                "   t.Y, " +
                "   cast(control.ROWCONCAT('SELECT ID_TRAMITE FROM TRAMITE_VISITA WHERE ID_VISITA=' || v.ID_VISITA) as varchar2(1000)) tramites, " +
                "   cast(control.ROWCONCAT('SELECT DISTINCT TT.CODFUNCIONARIO FROM TRAMITES.TBTRAMITETAREA TT WHERE TT.ESTADO = 0 AND FECHAFIN is null and copia=1 and codtramite in(' || nvl(control.ROWCONCAT('SELECT DISTINCT ID_TRAMITE FROM TRAMITE_VISITA WHERE ID_VISITA=' || V.ID_VISITA), 0) || ')') as varchar2(1000)) copias , " +
                "   cast(control.ROWCONCAT('SELECT DISTINCT F.NOMBRES || '' '' || F.APELLIDOS FROM TRAMITES.TBTRAMITETAREA TT, VW_FUNCIONARIO F WHERE TT.ESTADO = 0 AND TT.CODFUNCIONARIO = F.CODFUNCIONARIO and FECHAFIN is null and copia=1 and codtramite in(' || nvl(control.ROWCONCAT('SELECT DISTINCT ID_TRAMITE FROM TRAMITE_VISITA WHERE ID_VISITA=' || V.ID_VISITA), 0) || ')') as varchar2(3000)) NOMBREcopias , " +
                "   cast(control.ROWCONCAT('SELECT DISTINCT TT.CODFUNCIONARIO FROM TRAMITES.TBTRAMITETAREA TT WHERE TT.ESTADO = 0 AND FECHAFIN is null and copia=0 and codtramite in(' || nvl(control.ROWCONCAT('SELECT DISTINCT ID_TRAMITE FROM TRAMITE_VISITA WHERE ID_VISITA=' || V.ID_VISITA), 0) || ')') as varchar2(1000)) responsable , " +
                "   cast(control.ROWCONCAT('SELECT DISTINCT F.NOMBRES || '' '' || F.APELLIDOS FROM TRAMITES.TBTRAMITETAREA TT, VW_FUNCIONARIO F WHERE TT.ESTADO = 0 AND TT.CODFUNCIONARIO = F.CODFUNCIONARIO and FECHAFIN is null and copia=0 and codtramite in(' || nvl(control.ROWCONCAT('SELECT DISTINCT ID_TRAMITE FROM TRAMITE_VISITA WHERE ID_VISITA=' || V.ID_VISITA), 0) || ')') as varchar2(3000)) NOMBREresponsable, " +
                "   NVL(I.ID_INFORME, 0) ID_INFORME, " +
                "   TO_CHAR(V.ID_VISITA) AS STR_ID_VISITA, " +
                //"   rownum-1 R " +
                "   0 R " +
                "FROM CONTROL.VISITA v INNER JOIN " +
                "   SIG_AMVA.P_VISITA pv ON v.ID_VISITA = pv.ID CROSS JOIN " +
                "   TABLE(sdo_util.getvertices(PV.shape)) t LEFT OUTER JOIN " +
                "   CONTROL.VISITA_INFORME i ON v.ID_VISITA = i.ID_VISITA LEFT OUTER JOIN " +
                "   CONTROL.VISITAESTADO vas ON v.ID_VISITA = vas.ID_VISITA AND vas.ID_ESTADOVISITA = 1 LEFT OUTER JOIN " +
                "   CONTROL.VISITAESTADO vac ON v.ID_VISITA = vac.ID_VISITA AND vac.ID_ESTADOVISITA = 2 LEFT OUTER JOIN " +
                "   CONTROL.VISITAESTADO vep ON v.ID_VISITA = vep.ID_VISITA AND vac.ID_ESTADOVISITA = 3 LEFT OUTER JOIN " +
                "   CONTROL.VISITAESTADO ver ON v.ID_VISITA = vep.ID_VISITA AND vac.ID_ESTADOVISITA = 4 LEFT OUTER JOIN " +
                "   CONTROL.VISITAESTADO vte ON v.ID_VISITA = vep.ID_VISITA AND vac.ID_ESTADOVISITA = 5 LEFT OUTER JOIN " +
                "   CONTROL.VISITAESTADO vea ON v.ID_VISITA = vea.ID_VISITA AND vea.D_FIN IS NULL LEFT OUTER JOIN " +
                "   CONTROL.ESTADOVISITA ev ON vea.ID_ESTADOVISITA = ev.ID_ESTADOVISITA LEFT OUTER JOIN " +
                "   CONTROL.TRAMITE_VISITA ttv ON v.ID_VISITA = ttv.ID_VISITA INNER JOIN " +
                "   TRAMITES.TBTRAMITETAREA tt ON ttv.ID_TRAMITE = tt.CODTRAMITE AND tt.ESTADO = 0 AND tt.FECHAFIN IS NULL AND tt.COPIA = 0 LEFT OUTER JOIN " +
                "   CONTROL.RADICADOS_VISITA rv ON v.ID_VISITA = rv.ID_VISITA AND rv.ID_TIPO_RADICADO = 1 LEFT OUTER JOIN " +
                "   CONTROL.TIPO_VISITA tv ON v.ID_TIPOVISITA = tv.ID_TIPOVISITA " +
                "WHERE tt.CODFUNCIONARIO = " + funcionario.ToString();

            return db.Database.SqlQuery<VW_VISITAS>(sql).ToList();
        }

        public IHttpActionResult GetVisitas(string filtro, int take, int skip, int tipo)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
                Responsable = codFuncionario.ToString();
            }

            var consulta = VisitasFuncionario(codFuncionario);

            var model = from p in consulta
                        join it in db.INFORME_TECNICO on p.ID_VISITA equals it.ID_VISITA into vit
                        from it in vit.DefaultIfEmpty()
                        where p.RESPONSABLE.Equals(Responsable)
                        orderby p.ID_VISITA descending
                        select new { p.ID_VISITA, p.NOMBRERESPONSABLE, p.S_ASUNTO, p.D_ASIGNACION, p.TRAMITES, p.COPIAS, p.NOMBRECOPIAS, p.ESTADO_VISITA, p.RADICADO_VISITA, p.ID_ESTADOVISITA, p.S_OBSERVACION, p.X, p.Y, p.D_ACEPTACION, p.D_FINAL, p.D_INICIAL, p.D_INICIOREV, p.ID_TIPOVISITA, ESTADO_INFORME = (it == null ? 0 : it.ID_ESTADOINF) };
            /*           
            var model = from p in db.VW_VISITAS
                        join it in db.INFORME_TECNICO on p.ID_VISITA equals it.ID_VISITA into vit
                        from it in vit.DefaultIfEmpty()
                        where p.RESPONSABLE.Equals(Responsable)
                        orderby p.ID_VISITA descending
                        select new { p.ID_VISITA, p.NOMBRERESPONSABLE, p.S_ASUNTO, p.D_ASIGNACION, p.TRAMITES, p.COPIAS, p.NOMBRECOPIAS, p.ESTADO_VISITA, p.RADICADO_VISITA, p.ID_ESTADOVISITA, p.S_OBSERVACION, p.X, p.Y, p.D_ACEPTACION, p.D_FINAL, p.D_INICIAL, p.D_INICIOREV, p.ID_TIPOVISITA, ESTADO_INFORME = (it == null ? 0 : it.ID_ESTADOINF) };
            */
            datosConsulta resultado = new datosConsulta() { numRegistros = model.Count() };
            if (filtro != null)
            {

                string[] arrFiltros = filtro.Split(',');

                for (int contFiltro = 0; contFiltro < arrFiltros.Length; contFiltro += 4)
                {
                    long datos = 0;
                    string nombreFiltro = "";
                    switch (arrFiltros[contFiltro + 1])
                    {

                        case "contains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "TRAMITES")
                            {
                                model = model.Where(t => t.TRAMITES.ToLower().Contains(nombreFiltro.ToLower()));

                            }
                            else if (arrFiltros[contFiltro] == "S_ASUNTO")
                            {
                                model = model.Where(t => t.S_ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "ID_VISITA")
                            {

                                //model = model.Where(t => t.Str_ID_VISITA.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "NOMBRERESPONSABLE")
                            {
                                model = model.Where(t => t.NOMBRERESPONSABLE.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "notcontains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "TRAMITES")
                            {
                                model = model.Where(t => t.TRAMITES.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "S_ASUNTO")
                            {
                                model = model.Where(t => t.S_ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "NOMBRERESPONSABLE")
                            {
                                model = model.Where(t => t.NOMBRERESPONSABLE.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "startswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "TRAMITES")
                            {
                                model = model.Where(t => t.TRAMITES.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "S_ASUNTO")
                            {
                                model = model.Where(t => t.S_ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "NOMBRERESPONSABLE")
                            {
                                model = model.Where(t => t.NOMBRERESPONSABLE.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "endswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "TRAMITES")
                            {
                                model = model.Where(t => t.TRAMITES.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "S_ASUNTO")
                            {
                                model = model.Where(t => t.S_ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "NOMBRERESPONSABLE")
                            {
                                model = model.Where(t => t.NOMBRERESPONSABLE.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "=":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "ID_VISITA")
                            {
                                // model = model.Where(t => t.Str_ID_VISITA.Contains(nombreFiltro));
                            }
                            if (arrFiltros[contFiltro] == "TRAMITES")
                            {
                                model = model.Where(t => t.TRAMITES.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "S_ASUNTO")
                            {
                                model = model.Where(t => t.S_ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "NOMBRERESPONSABLE")
                            {
                                model = model.Where(t => t.NOMBRERESPONSABLE.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "<>":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.ID_VISITA != datos);
                            break;
                        case "<":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.ID_VISITA < datos);
                            break;
                        case "<=":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.ID_VISITA <= datos);
                            break;
                        case ">":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.ID_VISITA > datos);
                            break;
                        case ">=":

                            break;
                    }
                }
            }
            if (tipo == 0)
            {
                model = model.Where(t => t.ID_ESTADOVISITA != 5 || t.ESTADO_INFORME < 3);
            }
            else
            {
                model = model.Where(t => t.ID_ESTADOVISITA == 5);
            }

            model = model.Skip(skip).Take(take);
            resultado.datos = model.ToList();

            return Ok(resultado);
        }

        public IHttpActionResult GetTipoVisita()
        {
            var instalacion = from i in db.TIPO_VISITA
                              select new { i.ID_TIPOVISITA, i.S_NOMBRE };

            if (instalacion.ToList() == null)
            {
                return NotFound();
            }
            return Ok(instalacion.ToList());
        }


        [ActionName("GetVisitasFinalizadas")]
        public IEnumerable<dynamic> GetVisitasFinalizadas()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
                Responsable = codFuncionario.ToString();
            }

            var consulta = VisitasFuncionario(codFuncionario);

            var model = from p in consulta
                where p.ID_ESTADOVISITA == 5
                orderby p.ID_VISITA descending
                select new { p.ID_VISITA, p.NOMBRERESPONSABLE, p.S_ASUNTO, p.D_ASIGNACION, p.TRAMITES, p.COPIAS, p.NOMBRECOPIAS, p.ESTADO_VISITA };


            /*var model = from p in db.VW_VISITAS
                        where p.ID_ESTADOVISITA == 5 && p.RESPONSABLE.Equals(Responsable)
                        orderby p.ID_VISITA descending
                        select new { p.ID_VISITA, p.NOMBRERESPONSABLE, p.S_ASUNTO, p.D_ASIGNACION, p.TRAMITES, p.COPIAS, p.NOMBRECOPIAS, p.ESTADO_VISITA };*/

            return model.ToList();
        }

        public IHttpActionResult GetAcompantesXVisita(string filtro, int take, int skip)
        {

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
                Copias = codFuncionario.ToString();
            }

            var model = from p in db.VW_VISITAS
                        where Copias.Contains(p.RESPONSABLE)
                        orderby p.ID_VISITA descending
                        select new { p.ID_VISITA, p.NOMBRERESPONSABLE, p.TRAMITES, p.S_ASUNTO, p.D_ASIGNACION, p.STR_ID_VISITA, p.R };

            datosConsulta resultado = new datosConsulta() { numRegistros = model.Count() };
            if (filtro != null)
            {

                string[] arrFiltros = filtro.Split(',');

                for (int contFiltro = 0; contFiltro < arrFiltros.Length; contFiltro += 4)
                {
                    long datos = 0;
                    string nombreFiltro = "";
                    switch (arrFiltros[contFiltro + 1])
                    {

                        case "contains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "TRAMITES")
                            {
                                model = model.Where(t => t.TRAMITES.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "S_ASUNTO")
                            {
                                model = model.Where(t => t.S_ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "ID_VISITA")
                            {
                                model = model.Where(t => t.STR_ID_VISITA.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "NOMBRERESPONSABLE")
                            {
                                model = model.Where(t => t.NOMBRERESPONSABLE.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "notcontains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "TRAMITES")
                            {
                                model = model.Where(t => t.TRAMITES.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "S_ASUNTO")
                            {
                                model = model.Where(t => t.S_ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "NOMBRERESPONSABLE")
                            {
                                model = model.Where(t => t.NOMBRERESPONSABLE.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "startswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "TRAMITES")
                            {
                                model = model.Where(t => t.TRAMITES.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "S_ASUNTO")
                            {
                                model = model.Where(t => t.S_ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "NOMBRERESPONSABLE")
                            {
                                model = model.Where(t => t.NOMBRERESPONSABLE.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "endswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "TRAMITES")
                            {
                                model = model.Where(t => t.TRAMITES.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "S_ASUNTO")
                            {
                                model = model.Where(t => t.S_ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "NOMBRERESPONSABLE")
                            {
                                model = model.Where(t => t.NOMBRERESPONSABLE.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "=":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "ID_VISITA")
                            {
                                model = model.Where(t => t.STR_ID_VISITA.Contains(nombreFiltro));
                            }
                            if (arrFiltros[contFiltro] == "TRAMITES")
                            {
                                model = model.Where(t => t.TRAMITES.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "S_ASUNTO")
                            {
                                model = model.Where(t => t.S_ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "NOMBRERESPONSABLE")
                            {
                                model = model.Where(t => t.NOMBRERESPONSABLE.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "<>":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.ID_VISITA != datos);
                            break;
                        case "<":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.ID_VISITA < datos);
                            break;
                        case "<=":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.ID_VISITA <= datos);
                            break;
                        case ">":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.ID_VISITA > datos);
                            break;
                        case ">=":

                            break;
                    }
                }
            }


            model = model.Skip(skip).Take(take);
            resultado.datos = model.ToList();

            return Ok(resultado);

        }

        public IHttpActionResult GetDetalleTramitesVisitas(int id, int id_visita)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
            }

            var product = from p in db.VW_DETALLE_TRAMITE_V
                          where p.ID_TRAMITE == id && p.ID_VISITA == id_visita
                          orderby p.ID_TRAMITE descending
                          select new { p.ID_TRAMITE, p.FECHAINI, p.DIRECCION, p.MUNICIPIO, p.ASUNTO };

            return Ok(product);
        }




        public IHttpActionResult GetTramitesVisitas(string filtro)
        {

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
            }
            var model = from p in db.VW_TRAMITE_A_VISITAR
                        where p.CODFUNCIONARIO == codFuncionario
                        orderby p.CODTRAMITE descending
                        select new { p.CODTRAMITE, p.FECHAINICIOTRAMITE, p.DIRECCION, p.MUNICIPIO, p.X, p.Y, p.ASUNTO, p.STR_CODTRAMITE };



            if (filtro != null)
            {

                string[] arrFiltros = filtro.Split(',');

                for (int contFiltro = 0; contFiltro < arrFiltros.Length; contFiltro += 4)
                {
                    long datos = 0;
                    string nombreFiltro = "";
                    switch (arrFiltros[contFiltro + 1])
                    {

                        case "contains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODTRAMITE")
                            {
                                model = model.Where(t => t.STR_CODTRAMITE.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "ASUNTO")
                            {
                                model = model.Where(t => t.ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "DIRECCION")
                            {
                                model = model.Where(t => t.DIRECCION.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "MUNICIPIO")
                            {
                                model = model.Where(t => t.MUNICIPIO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "notcontains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODTRAMITE")
                            {
                                model = model.Where(t => t.STR_CODTRAMITE.Contains(nombreFiltro));
                            }
                            else if (arrFiltros[contFiltro] == "ASUNTO")
                            {
                                model = model.Where(t => t.ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "DIRECCION")
                            {
                                model = model.Where(t => t.DIRECCION.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "MUNICIPIO")
                            {
                                model = model.Where(t => t.MUNICIPIO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "startswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODTRAMITE")
                            {
                                model = model.Where(t => t.STR_CODTRAMITE.Contains(nombreFiltro));
                            }
                            else if (arrFiltros[contFiltro] == "ASUNTO")
                            {
                                model = model.Where(t => t.ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "DIRECCION")
                            {
                                model = model.Where(t => t.DIRECCION.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "MUNICIPIO")
                            {
                                model = model.Where(t => t.MUNICIPIO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "endswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODTRAMITE")
                            {
                                model = model.Where(t => t.STR_CODTRAMITE.ToLower().Contains(nombreFiltro));
                            }
                            else if (arrFiltros[contFiltro] == "ASUNTO")
                            {
                                model = model.Where(t => t.ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "DIRECCION")
                            {
                                model = model.Where(t => t.DIRECCION.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "MUNICIPIO")
                            {
                                model = model.Where(t => t.MUNICIPIO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "=":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODTRAMITE")
                            {
                                model = model.Where(t => t.STR_CODTRAMITE.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "ASUNTO")
                            {
                                model = model.Where(t => t.ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "DIRECCION")
                            {
                                model = model.Where(t => t.DIRECCION.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "MUNICIPIO")
                            {
                                model = model.Where(t => t.MUNICIPIO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "<>":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.CODTRAMITE != datos);
                            break;
                        case "<":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.CODTRAMITE < datos);
                            break;
                        case "<=":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.CODTRAMITE <= datos);
                            break;
                        case ">":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.CODTRAMITE > datos);
                            break;
                        case ">=":

                            break;
                    }
                }
            }


            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);

        }




        public IHttpActionResult GetAcompanantes(string filtro)
        {
            var atiende = from t in db.VW_FUNCIONARIO
                          select new { t.CODFUNCIONARIO, t.NOMBRECOMPLETO, t.Str_CODFUNCIONARIO };

            if (filtro != null)
            {

                string[] arrFiltros = filtro.Split(',');

                for (int contFiltro = 0; contFiltro < arrFiltros.Length; contFiltro += 4)
                {
                    long datos = 0;
                    string nombreFiltro = "";
                    switch (arrFiltros[contFiltro + 1])
                    {

                        case "contains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().Contains(nombreFiltro.ToLower()));
                            break;
                        case "notcontains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => !t.NOMBRECOMPLETO.ToLower().Contains(nombreFiltro.ToLower()));
                            break;
                        case "startswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().StartsWith(nombreFiltro.ToLower()));

                            break;
                        case "endswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().EndsWith(nombreFiltro.ToLower()));

                            break;
                        case "=":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODFUNCIONARIO")
                            {
                                atiende = atiende.Where(t => t.Str_CODFUNCIONARIO.Contains(nombreFiltro));
                            }
                            else
                            {
                                atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "<>":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO != datos);
                            break;
                        case "<":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO < datos);
                            break;
                        case "<=":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO <= datos);
                            break;
                        case ">":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO > datos);
                            break;
                        case ">=":

                            break;
                    }
                }
            }
            else
            {
                //atiende = atiende.Where(t => t.CODFUNCIONARIO == -1);
            }

            if (atiende == null)
            {
                return NotFound();
            }
            return Ok(atiende);

        }



        public IHttpActionResult GetTramiteVisita_E(string id)
        {

            var product = from p in db.VW_TRAMITE_TODOS
                          where id.Contains(p.CODTRAMITE)
                          select new { p.CODTRAMITE, p.FECHAINICIOTRAMITE, p.DIRECCION, p.MUNICIPIO, p.X, p.Y, p.ASUNTO };

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        public IHttpActionResult GetDetalleTramiteVisita_E(int CODVISITA)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
            }

            var product = from p in db.VW_TRAMITE_A_VISITAR
                          where p.CODFUNCIONARIO == Convert.ToInt32(codFuncionario) && p.CODTRAMITE == CODVISITA
                          select new { p.CODTRAMITE, p.FECHAINICIOTRAMITE, p.DIRECCION, p.MUNICIPIO, p.X, p.Y, p.ASUNTO };

            if (product.ToList() == null)
            {
                return NotFound();
            }
            return Ok(product);
        }




        public IHttpActionResult GetTramitesVisitas_E(string filtro, int take, int skip)
        {

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
            }
            var model = from p in db.VW_TRAMITE_A_VISITAR
                        where p.CODFUNCIONARIO == codFuncionario
                        orderby p.CODTRAMITE descending
                        select new { p.CODTRAMITE, p.FECHAINICIOTRAMITE, p.DIRECCION, p.MUNICIPIO, p.X, p.Y, p.ASUNTO, p.STR_CODTRAMITE };

            datosConsulta resultado = new datosConsulta() { numRegistros = model.Count() };

            if (filtro != null)
            {

                string[] arrFiltros = filtro.Split(',');

                for (int contFiltro = 0; contFiltro < arrFiltros.Length; contFiltro += 4)
                {
                    long datos = 0;
                    string nombreFiltro = "";
                    switch (arrFiltros[contFiltro + 1])
                    {

                        case "contains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODTRAMITE")
                            {
                                model = model.Where(t => t.STR_CODTRAMITE.Contains(nombreFiltro));
                            }
                            else if (arrFiltros[contFiltro] == "ASUNTO")
                            {
                                model = model.Where(t => t.ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "DIRECCION")
                            {
                                model = model.Where(t => t.DIRECCION.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "MUNICIPIO")
                            {
                                model = model.Where(t => t.MUNICIPIO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "notcontains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODTRAMITE")
                            {
                                model = model.Where(t => t.STR_CODTRAMITE.Contains(nombreFiltro));
                            }
                            else if (arrFiltros[contFiltro] == "ASUNTO")
                            {
                                model = model.Where(t => t.ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "DIRECCION")
                            {
                                model = model.Where(t => t.DIRECCION.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "MUNICIPIO")
                            {
                                model = model.Where(t => t.MUNICIPIO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "startswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODTRAMITE")
                            {
                                model = model.Where(t => t.STR_CODTRAMITE.Contains(nombreFiltro));
                            }
                            else if (arrFiltros[contFiltro] == "ASUNTO")
                            {
                                model = model.Where(t => t.ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "DIRECCION")
                            {
                                model = model.Where(t => t.DIRECCION.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "MUNICIPIO")
                            {
                                model = model.Where(t => t.MUNICIPIO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "endswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODTRAMITE")
                            {
                                model = model.Where(t => t.STR_CODTRAMITE.Contains(nombreFiltro));
                            }
                            else if (arrFiltros[contFiltro] == "ASUNTO")
                            {
                                model = model.Where(t => t.ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "DIRECCION")
                            {
                                model = model.Where(t => t.DIRECCION.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "MUNICIPIO")
                            {
                                model = model.Where(t => t.MUNICIPIO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "=":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODTRAMITE")
                            {
                                model = model.Where(t => t.STR_CODTRAMITE.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "ASUNTO")
                            {
                                model = model.Where(t => t.ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "DIRECCION")
                            {
                                model = model.Where(t => t.DIRECCION.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "MUNICIPIO")
                            {
                                model = model.Where(t => t.MUNICIPIO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "<>":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.CODTRAMITE != datos);
                            break;
                        case "<":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.CODTRAMITE < datos);
                            break;
                        case "<=":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.CODTRAMITE <= datos);
                            break;
                        case ">":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.CODTRAMITE > datos);
                            break;
                        case ">=":

                            break;
                    }
                }
            }


            model = model.Skip(skip).Take(take);
            resultado.datos = model.ToList();

            return Ok(resultado);

        }


        public IHttpActionResult GetDetalleEncargados_E(string id, int take, int skip)
        {
            IdFiltroAcompanante = id;
            var model = from p in db.VW_FUNCIONARIO
                        where id.Contains(p.NOMBRECOMPLETO)
                        orderby p.CODFUNCIONARIO descending
                        select new { p.CODFUNCIONARIO, p.NOMBRECOMPLETO };
            datosConsulta resultado = new datosConsulta() { numRegistros = model.Count() };
            model = model.Skip(skip).Take(take);
            resultado.datos = model.ToList();

            return Ok(resultado);
        }




        public IHttpActionResult GetEncargados_E(string filtro, int take, int skip)
        {
            var atiende = from t in db.VW_FUNCIONARIO
                          where !IdFiltroAcompanante.Contains(t.Str_CODFUNCIONARIO)
                          orderby t.CODFUNCIONARIO descending
                          select new { t.CODFUNCIONARIO, t.NOMBRECOMPLETO, t.Str_CODFUNCIONARIO };


            if (filtro != null)
            {

                string[] arrFiltros = filtro.Split(',');

                for (int contFiltro = 0; contFiltro < arrFiltros.Length; contFiltro += 4)
                {
                    long datos = 0;
                    string nombreFiltro = "";
                    switch (arrFiltros[contFiltro + 1])
                    {

                        case "contains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().Contains(nombreFiltro.ToLower()));
                            break;
                        case "notcontains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => !t.NOMBRECOMPLETO.ToLower().Contains(nombreFiltro.ToLower()));
                            break;
                        case "startswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().StartsWith(nombreFiltro.ToLower()));

                            break;
                        case "endswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().EndsWith(nombreFiltro.ToLower()));

                            break;
                        case "=":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODFUNCIONARIO")
                            {
                                atiende = atiende.Where(t => t.Str_CODFUNCIONARIO.Contains(nombreFiltro));
                            }
                            else
                            {
                                atiende = atiende.Where(t => t.NOMBRECOMPLETO.Contains(nombreFiltro));
                            }
                            break;
                        case "<>":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO != datos);
                            break;
                        case "<":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO < datos);
                            break;
                        case "<=":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO <= datos);
                            break;
                        case ">":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO > datos);
                            break;
                        case ">=":

                            break;
                    }
                }
            }

            datosConsulta resultado = new datosConsulta() { numRegistros = atiende.Count() };
            atiende = atiende.Skip(skip).Take(take);
            resultado.datos = atiende.ToList();

            return Ok(resultado);
        }

        [HttpGet]
        public String testGet([FromUri]string p_Asunto, [FromUri]decimal p_cx, [FromUri]decimal p_cy, [FromUri]string p_TipoUbicacion, string p_IdsTramite,
            string p_Cometario, string p_IdsCopias, int p_IdTipoVisita, int p_IdVisita)
        {

            p_cx = Convert.ToDecimal(p_cx.ToString().Replace(".", ","));
            p_cy = Convert.ToDecimal(p_cy.ToString().Replace(".", ","));

            if (p_IdsTramite == "")
                p_IdsTramite = "0";

            if (p_IdsCopias == "")
                p_IdsCopias = "0";

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
            }

            //Guarda los datos de mis visitas
            dbControl.SP_NEW_VISITA(p_Asunto, p_cx, p_cy, p_TipoUbicacion, p_IdsTramite, codFuncionario, p_Cometario, p_IdsCopias, p_IdTipoVisita, p_IdVisita);

            return p_Asunto;
        }



        //REPARTO
        public IHttpActionResult GetGuardarTramitesGeocodificados(String Codtramites, Decimal CoordenadasX, Decimal CoordenadasY, String TipoUbicacion)
        {


            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
            }


            Decimal decCodigo = Decimal.Parse(Codtramites);
            //Decimal decCoorX = Decimal.Parse(CoordenadasX);
            // Decimal decCoorY = Decimal.Parse(CoordenadasY);
            dbControl.SP_NEW_P_TRAMITE2(decCodigo, CoordenadasX, CoordenadasY, codFuncionario, TipoUbicacion);

            return Ok();
        }





        public IHttpActionResult GetTramitesnoubicados(string filtro, int take, int skip)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
            }


            var model = from p in db.VW_REPARTO_NOGEO
                        where p.CODFUNCIONARIO.Equals(codFuncionario)
                        orderby p.CODTRAMITE descending
                        select new { p.CODTRAMITE, p.FECHAINICIOTRAMITE, p.DIRECCION, p.MUNICIPIO, p.ASUNTO, p.CODTAREA, p.ORDEN, p.STR_CODTRAMITE, p.TAREA };
            //datosConsulta resultado = new datosConsulta() { numRegistros = model.Count() };
            if (filtro != null)
            {
                string[] arrFiltros = filtro.Split(',');

                for (int contFiltro = 0; contFiltro < arrFiltros.Length; contFiltro += 4)
                {
                    long datos = 0;
                    string nombreFiltro = "";
                    switch (arrFiltros[contFiltro + 1])
                    {

                        case "contains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODTRAMITE")
                            {

                                model = model.Where(t => t.STR_CODTRAMITE.Contains(nombreFiltro));
                            }
                            else if (arrFiltros[contFiltro] == "ASUNTO")
                            {
                                model = model.Where(t => t.ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "DIRECCION")
                            {
                                model = model.Where(t => t.DIRECCION.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "MUNICIPIO")
                            {
                                model = model.Where(t => t.MUNICIPIO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "notcontains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODTRAMITE")
                            {
                                model = model.Where(t => t.STR_CODTRAMITE.Contains(nombreFiltro));
                            }
                            else if (arrFiltros[contFiltro] == "ASUNTO")
                            {
                                model = model.Where(t => t.ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "DIRECCION")
                            {
                                model = model.Where(t => t.DIRECCION.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "MUNICIPIO")
                            {
                                model = model.Where(t => t.MUNICIPIO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "startswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODTRAMITE")
                            {
                                model = model.Where(t => t.STR_CODTRAMITE.Contains(nombreFiltro));
                            }
                            else if (arrFiltros[contFiltro] == "ASUNTO")
                            {
                                model = model.Where(t => t.ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "DIRECCION")
                            {
                                model = model.Where(t => t.DIRECCION.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "MUNICIPIO")
                            {
                                model = model.Where(t => t.MUNICIPIO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "endswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODTRAMITE")
                            {
                                model = model.Where(t => t.STR_CODTRAMITE.Contains(nombreFiltro));
                            }
                            else if (arrFiltros[contFiltro] == "ASUNTO")
                            {
                                model = model.Where(t => t.ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "DIRECCION")
                            {
                                model = model.Where(t => t.DIRECCION.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "MUNICIPIO")
                            {
                                model = model.Where(t => t.MUNICIPIO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "=":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODTRAMITE")
                                if (arrFiltros[contFiltro] == "CODTRAMITE")
                                {
                                    model = model.Where(t => t.STR_CODTRAMITE.Contains(nombreFiltro));
                                }
                            break;
                        case "<>":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.CODTRAMITE != datos);
                            break;
                        case "<":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.CODTRAMITE < datos);
                            break;
                        case "<=":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.CODTRAMITE <= datos);
                            break;
                        case ">":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.CODTRAMITE > datos);
                            break;
                        case ">=":

                            break;
                    }
                }
            }
            datosConsulta resultado = new datosConsulta() { numRegistros = model.Count() };
            model = model.Skip(skip).Take(take);
            resultado.datos = model.ToList();

            return Ok(resultado);
        }



        public IHttpActionResult GetTramitesubicados(string filtro, int take, int skip)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
            }

            var model = from p in db.VW_PAG_REPARTO_GEO
                        where p.CODFUNCIONARIO.Equals(codFuncionario)
                        orderby p.CODTRAMITE descending
                        select new { p.CODTRAMITE, p.FECHAINICIOTRAMITE, p.DIRECCION, p.MUNICIPIO, p.ASUNTO, p.CODTAREA, p.ORDEN, p.X, p.Y, p.Strcodtramite, p.R, p.TAREA };


            if (filtro != null)
            {
                string[] arrFiltros = filtro.Split(',');
                for (int contFiltro = 0; contFiltro < arrFiltros.Length; contFiltro += 4)
                {
                    long datos = 0;
                    string nombreFiltro = "";
                    switch (arrFiltros[contFiltro + 1])
                    {

                        case "contains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "Strcodtramite")
                            {
                                model = model.Where(t => t.Strcodtramite.Contains(nombreFiltro));
                            }
                            else if (arrFiltros[contFiltro] == "ASUNTO")
                            {
                                model = model.Where(t => t.ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "DIRECCION")
                            {
                                model = model.Where(t => t.DIRECCION.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "MUNICIPIO")
                            {
                                model = model.Where(t => t.MUNICIPIO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "notcontains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "Strcodtramite")
                            {
                                model = model.Where(t => t.Strcodtramite.Contains(nombreFiltro));
                            }
                            else if (arrFiltros[contFiltro] == "ASUNTO")
                            {
                                model = model.Where(t => t.ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "DIRECCION")
                            {
                                model = model.Where(t => t.DIRECCION.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "MUNICIPIO")
                            {
                                model = model.Where(t => t.MUNICIPIO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "startswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "Strcodtramite")
                            {
                                model = model.Where(t => t.Strcodtramite.Contains(nombreFiltro));
                            }
                            else if (arrFiltros[contFiltro] == "ASUNTO")
                            {
                                model = model.Where(t => t.ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "DIRECCION")
                            {
                                model = model.Where(t => t.DIRECCION.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "MUNICIPIO")
                            {
                                model = model.Where(t => t.MUNICIPIO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "endswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "Strcodtramite")
                            {
                                model = model.Where(t => t.Strcodtramite.Contains(nombreFiltro));
                            }
                            else if (arrFiltros[contFiltro] == "ASUNTO")
                            {
                                model = model.Where(t => t.ASUNTO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "DIRECCION")
                            {
                                model = model.Where(t => t.DIRECCION.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            else if (arrFiltros[contFiltro] == "MUNICIPIO")
                            {
                                model = model.Where(t => t.MUNICIPIO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "=":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODTRAMITE")
                                if (arrFiltros[contFiltro] == "CODTRAMITE")
                                {
                                    model = model.Where(t => t.Strcodtramite.Contains(nombreFiltro));
                                }
                            break;
                        case "<>":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.CODTRAMITE != datos);
                            break;
                        case "<":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.CODTRAMITE < datos);
                            break;
                        case "<=":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.CODTRAMITE <= datos);
                            break;
                        case ">":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            model = model.Where(t => t.CODTRAMITE > datos);
                            break;
                        case ">=":

                            break;
                    }
                }
            }
            datosConsulta resultado = new datosConsulta() { numRegistros = model.Count() };
            model = model.Skip(skip).Take(take);
            resultado.datos = model.ToList();

            return Ok(resultado);
        }


        public IHttpActionResult GetRepartoResponsable(string filtro)
        {

            var atiende = from t in db.VW_FUNCIONARIO
                          select new { t.CODFUNCIONARIO, t.NOMBRECOMPLETO, t.Str_CODFUNCIONARIO };

            if (filtro != null)
            {

                string[] arrFiltros = filtro.Split(',');

                for (int contFiltro = 0; contFiltro < arrFiltros.Length; contFiltro += 4)
                {
                    long datos = 0;
                    string nombreFiltro = "";
                    switch (arrFiltros[contFiltro + 1])
                    {

                        case "contains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().Contains(nombreFiltro.ToLower()));
                            break;
                        case "notcontains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => !t.NOMBRECOMPLETO.ToLower().Contains(nombreFiltro.ToLower()));
                            break;
                        case "startswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().StartsWith(nombreFiltro.ToLower()));

                            break;
                        case "endswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().EndsWith(nombreFiltro.ToLower()));

                            break;
                        case "=":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODFUNCIONARIO")
                            {
                                atiende = atiende.Where(t => t.Str_CODFUNCIONARIO.Contains(nombreFiltro));
                            }
                            else
                            {
                                atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "<>":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO != datos);
                            break;
                        case "<":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO < datos);
                            break;
                        case "<=":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO <= datos);
                            break;
                        case ">":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO > datos);
                            break;
                        case ">=":

                            break;
                    }
                }
            }
            else
            {
                atiende = atiende.Where(t => t.CODFUNCIONARIO == -1);
            }

            if (atiende == null)
            {
                return NotFound();
            }
            return Ok(atiende);
        }

        public IHttpActionResult GetRepartoCopias(string filtro)
        {

            var atiende = from t in db.VW_FUNCIONARIO
                          select new { t.CODFUNCIONARIO, t.NOMBRECOMPLETO, t.Str_CODFUNCIONARIO };

            if (filtro != null)
            {

                string[] arrFiltros = filtro.Split(',');

                for (int contFiltro = 0; contFiltro < arrFiltros.Length; contFiltro += 4)
                {
                    long datos = 0;
                    string nombreFiltro = "";
                    switch (arrFiltros[contFiltro + 1])
                    {

                        case "contains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().Contains(nombreFiltro.ToLower()));
                            break;
                        case "notcontains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => !t.NOMBRECOMPLETO.ToLower().Contains(nombreFiltro.ToLower()));
                            break;
                        case "startswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().StartsWith(nombreFiltro.ToLower()));

                            break;
                        case "endswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().EndsWith(nombreFiltro.ToLower()));

                            break;
                        case "=":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODFUNCIONARIO")
                            {
                                atiende = atiende.Where(t => t.Str_CODFUNCIONARIO.Contains(nombreFiltro));
                            }
                            else
                            {
                                atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "<>":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO != datos);
                            break;
                        case "<":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO < datos);
                            break;
                        case "<=":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO <= datos);
                            break;
                        case ">":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO > datos);
                            break;
                        case ">=":

                            break;
                    }
                }
            }
            else
            {
                atiende = atiende.Where(t => t.CODFUNCIONARIO == -1);
            }

            if (atiende == null)
            {
                return NotFound();
            }
            return Ok(atiende);
        }

        public IHttpActionResult GetTramitesGeoFiltro(string tramites)
        {
            if (tramites == null)
            {
                tramites = "";
            }

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
                Responsable = codFuncionario.ToString();
            }

            var model = from p in db.VW_REPARTO_GEO
                        where tramites.Contains(p.Strcodtramite)
                        orderby p.CODTRAMITE descending
                        select new { p.CODTRAMITE, p.FECHAINICIOTRAMITE, p.DIRECCION, p.MUNICIPIO, p.ASUNTO, p.CODTAREA, p.ORDEN, p.X, p.Y, p.TAREA };


            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }



        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        public string GetVisitasComoAcompanate(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }


        ////////////////////////////////////////////////////
        //realizar visitas

        public IHttpActionResult GetRealizarVisitasAtiende(string filtro)
        {


            var atiende = from t in db.VW_TERCERO
                          //where t.IDDOCUMENTO == 2

                          select new { t.ID, t.DOCUMENTO, t.NOMBRE, t.DOCUMENTOINT };
            if (filtro != null)
            {
                string[] arrFiltros = filtro.Split(',');
                // atiende = atiende.Where(t => t.DOCUMENTO == "8110454213");
                for (int contFiltro = 0; contFiltro < arrFiltros.Length; contFiltro += 4)
                {
                    long datos = 0;
                    string nombreFiltro = "";
                    switch (arrFiltros[contFiltro + 1])
                    {

                        case "contains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRE.ToLower().Contains(nombreFiltro.ToLower()));
                            break;
                        case "notcontains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => !t.NOMBRE.ToLower().Contains(nombreFiltro.ToLower()));
                            break;
                        case "startswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRE.ToLower().StartsWith(nombreFiltro.ToLower()));

                            break;
                        case "endswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRE.ToLower().EndsWith(nombreFiltro.ToLower()));

                            break;
                        case "=":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "DOCUMENTO")
                            {
                                atiende = atiende.Where(t => t.DOCUMENTO.Contains(nombreFiltro));
                            }
                            else
                            {
                                atiende = atiende.Where(t => t.NOMBRE.Contains(nombreFiltro));
                            }
                            break;
                        case "<>":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.DOCUMENTOINT != datos);
                            break;
                        case "<":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.DOCUMENTOINT < datos);
                            break;
                        case "<=":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.DOCUMENTOINT <= datos);
                            break;
                        case ">":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.DOCUMENTOINT > datos);
                            break;
                        case ">=":

                            break;
                    }
                }
            }
            else
            {
                atiende = atiende.Where(t => t.DOCUMENTOINT == -1);
            }

            if (atiende == null)
            {
                return NotFound();
            }
            return Ok(atiende);

        }
        public IHttpActionResult GetResponsableTramite()
        {

            var funcionario = from f in db.VW_FUNCIONARIO

                              select new { f.CODFUNCIONARIO, f.NOMBRES, f.APELLIDOS, f.CEDULA };

            if (funcionario.ToList() == null)
            {
                return NotFound();
            }
            return Ok(funcionario.ToList());
        }
        public IHttpActionResult GetRealizarVisitasAtiendeVisita(string filtro)
        {

            var atiende = from t in db.VW_TERCERO
                          where t.IDDOCUMENTO == 1

                          select new { t.ID, t.DOCUMENTO, t.NOMBRE, t.DOCUMENTOINT };
            if (filtro != null)
            {
                string[] arrFiltros = filtro.Split(',');
                // atiende = atiende.Where(t => t.DOCUMENTO == "8110454213");
                for (int contFiltro = 0; contFiltro < arrFiltros.Length; contFiltro += 4)
                {
                    long datos = 0;
                    string nombreFiltro = "";
                    switch (arrFiltros[contFiltro + 1])
                    {

                        case "contains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRE.ToLower().Contains(nombreFiltro.ToLower()));
                            break;
                        case "notcontains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => !t.NOMBRE.ToLower().Contains(nombreFiltro.ToLower()));
                            break;
                        case "startswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRE.ToLower().StartsWith(nombreFiltro.ToLower()));

                            break;
                        case "endswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRE.ToLower().EndsWith(nombreFiltro.ToLower()));

                            break;
                        case "=":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "DOCUMENTO")
                            {
                                atiende = atiende.Where(t => t.DOCUMENTO.Contains(nombreFiltro));
                            }
                            else
                            {
                                atiende = atiende.Where(t => t.NOMBRE.Contains(nombreFiltro));
                            }
                            break;
                        case "<>":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.DOCUMENTOINT != datos);
                            break;
                        case "<":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.DOCUMENTOINT < datos);
                            break;
                        case "<=":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.DOCUMENTOINT <= datos);
                            break;
                        case ">":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.DOCUMENTOINT > datos);
                            break;
                        case ">=":

                            break;
                    }
                }
            }
            else
            {
                atiende = atiende.Where(t => t.DOCUMENTOINT == -1);
            }

            if (atiende.ToList() == null)
            {
                return NotFound();
            }
            return Ok(atiende.ToList());
        }
        public IHttpActionResult GetConsultarInstalacion(int id)
        {
            var instalacion = from i in db.VW_INSTALACION
                              where i.ID_TERCERO == id
                              select new { i.ID_INSTALACION, i.S_NOMBRE };

            if (instalacion.ToList() == null)
            {
                return NotFound();
            }
            return Ok(instalacion.ToList());
        }

        public IHttpActionResult GetConsultarConsultarRolVisita()
        {
            var rol = from r in db.ROL_VISITA

                      select new { r.ID_ROL, r.S_NOMBRE };

            if (rol.ToList() == null)
            {
                return NotFound();
            }
            return Ok(rol.ToList());
        }


        [HttpGet]
        public decimal guardarRealizarVisita([FromUri]string p_Asunto, [FromUri]decimal p_cx, [FromUri]decimal p_cy, [FromUri] int p_IdVisita
            , int idTercero, int idInstalacion, string atiende)
        {

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
            }

            decimal res;
            try
            {
                dbControl.SP_GUARDAR_VISITA(p_Asunto, p_cx, p_cy, "ss", idTercero, p_IdVisita, idTercero, idInstalacion, atiende);
                res = 1;
            }
            catch
            {
                res = 0;
            }
            return res;
        }

        [HttpGet]
        public decimal guardarIniciarVisita([FromUri] int p_IdVisita)
        {

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
            }

            decimal res;
            try
            {
                dbControl.SP_INIT_VISITA(codFuncionario, p_IdVisita);
                res = 1;
            }
            catch
            {
                res = 0;
            }
            return res;
        }

        [HttpGet]
        public decimal CerrarVisita([FromUri] int p_IdVisita, [FromUri] String radicado)
        {

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
            }

            decimal res;
            try
            {
                dbControl.SP_CERRAR_VISITA(codFuncionario, p_IdVisita, radicado);
                res = 1;
            }
            catch
            {
                res = 0;
            }
            return res;
        }
        [HttpGet]
        public decimal FinalizarVisita([FromUri] int p_IdVisita, [FromUri] String radicado, [FromUri] String pdf, [FromUri] String comentario, [FromUri] int responsable
             , String copias)
        {

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
            }

            decimal res;
            try
            {
                dbControl.SP_FIN_VISITA(codFuncionario, radicado, pdf, p_IdVisita, comentario, codFuncionario, copias);
                res = 1;
            }
            catch
            {
                res = 0;
            }
            return res;
        }


        [HttpGet]
        public decimal EliminarFotografia([FromUri] int p_idFotografia)
        {

            Decimal res;
            try
            {
                dbControl.SP_ELIMINAR_FOTO_VISITA(p_idFotografia);
                res = 1;

            }
            catch
            {
                res = 0;
            }
            return res;
        }
        [HttpGet]
        public decimal AgregarEtiqueta([FromUri] int p_idFotografia, String etiqueta, String palabra)
        {

            Decimal res;
            try
            {
                var etiquetaBD = (from f in db.FOTOGRAFIA
                                  where f.ID_FOTOGRAFIA == p_idFotografia
                                  select f).FirstOrDefault();
                etiquetaBD.S_ETIQUETA = etiqueta;
                etiquetaBD.PALABRA_CLAVE = palabra;
                db.SaveChanges();
                res = 1;
            }
            catch
            {
                res = 0;
            }
            return res;
        }
        public IHttpActionResult GETConsultarDatosRealizarVisita(int id)
        {
            var visitas = from v in db.VW_REALIZAR_VISITA
                          where v.IDVISITA == id
                          select new { v.IDVISITA, v.IDINSTALACION, v.IDTERCERO, v.INSTALACION, v.NOMBRETERCERO, v.OBSEVACION, ASUNTO = v.ASUNTO.Replace("\r", "").Replace("\n", "") };

            if (visitas == null)
            {
                return NotFound();
            }
            return Ok(visitas.ToList());
        }

        public IHttpActionResult GETConsultarAtiendeRealizarVisita(int id)
        {
            //left join
            var q = from v in db.TERCERO_VISITA
                    join od in db.TERCERO on v.ID_TERCERO equals od.ID_TERCERO
                    where v.ID_VISITA == id
                    select new { od.ID_TERCERO, od.N_DOCUMENTO, od.S_RSOCIAL, v.ID_ROL, od.S_CORREO };


            if (q == null)
            {
                return NotFound();
            }
            return Ok(q);
        }


        public IHttpActionResult GetConsultarFotografias(int id)
        {
            var fotografias = from f in db.VW_FOTOGRAFIA_VISITA
                              where f.ID_VISITA == id
                              select new { f.S_ETIQUETA, f.URL, f.ID_FOTOGRAFIA, f.S_ARCHIVO };

            if (fotografias == null)
            {
                return NotFound();
            }
            return Ok(fotografias);
        }

        public IHttpActionResult GetGuardarFotografias(int idVisita, String idFotos)
        {
            string[] arrFotos = idFotos.Split(',');
            FOTOGRAFIA_VISITA foto = new FOTOGRAFIA_VISITA();
            for (int i = 0; i < arrFotos.Length; i++)
            {



                foto.ID_FOTOGRAFIA = Convert.ToInt32(arrFotos[i]);
                foto.ID_VISITA = idVisita;
                db.FOTOGRAFIA_VISITA.Add(foto);
                db.SaveChanges();
            }

            return Ok();

        }

        public IHttpActionResult GetGuardarObservacion(int idVisita, String observacion)
        {
            var observ = (from v in db.VISITA
                          where v.ID_VISITA == idVisita
                          select v).FirstOrDefault();
            observ.S_OBSERVACION = observacion;
            db.SaveChanges();

            return Ok();

        }

        public IHttpActionResult GetConsultarObservacion(int id)
        {
            var observacion = from v in db.VISITA
                              where v.ID_VISITA == id
                              select new { v.S_OBSERVACION };

            if (observacion == null)
            {
                return NotFound();
            }
            return Ok(observacion);
        }
        public IHttpActionResult GetConsultarDatosRealizarVisitaIni(int id)
        {
            var realizarVisita = from v in db.VW_VISITAS
                                 where v.ID_VISITA == id
                                 select new { S_ASUNTO = v.S_ASUNTO.Replace("\r", "").Replace("\n", ""), v.RADICADO_VISITA, v.S_OBSERVACION, v.RESPONSABLE, v.COPIAS, v.D_ASIGNACIONVISITA };

            if (realizarVisita == null)
            {
                return NotFound();
            }
            return Ok(realizarVisita);
        }

        public IHttpActionResult GetConsultarRepresentanteLegal(int id)
        {
            var realizarVisita = from r in db.VW_REPRESENTANTE_LEGAL
                                 where r.ID_JURIDICO == id
                                 select new { r.NOMBRE, r.DOCUMENTO, r.ID_JURIDICO, r.FECHA_ACTUALIZACION };

            if (realizarVisita == null)
            {
                return NotFound();
            }
            return Ok(realizarVisita);
        }
        public IHttpActionResult GetConsultarFotografiasAguas(int id)
        {

            return Ok("");
        }


        public IHttpActionResult GetConsultarInfVisita()
        {


            var infVisita = from t in db.VW_TRAMITE_INFORME

                            select new { t.CODTRAMITE, t.COPIA, t.ASUNTO, t.ESTADO, t.FECHAINICIOTRAMITE, t.CODFUNCIONARIO, t.RADICADO_VISITA };

            return Ok(infVisita);

        }



        public IHttpActionResult GetGuardarFotografiasResiduos(String idFotos)
        {
            string[] arrFotos = idFotos.Split(',');

            FRM_RESIDUOS_FOTOGRAFIA foto = new FRM_RESIDUOS_FOTOGRAFIA();
            for (int i = 0; i < arrFotos.Length; i++)
            {

                foto.ID_FOTOGRAFIA = Convert.ToInt32(arrFotos[i]);
                foto.ID_RECIDUOS = Convert.ToInt32(1);
                db.FRM_RESIDUOS_FOTOGRAFIA.Add(foto);
                db.SaveChanges();
            }
            return Ok();
        }

        [HttpGet]
        public decimal EliminarFotografiaResiduos([FromUri] int p_idFotografia)
        {

            Decimal res;
            try
            {
                dbControl.SP_ELIMINAR_FOTO_RESIDUOS(p_idFotografia);
                res = 1;
            }
            catch
            {
                res = 0;
            }
            return res;
        }

        public IHttpActionResult GetConsultarFotografiasResiduos(int id)
        {


            return NotFound();
        }



        [HttpGet]
        public decimal EliminarFotografiaFlora([FromUri] int p_idFotografia)
        {

            Decimal res;
            try
            {
                dbControl.SP_ELIMINAR_FOTO_FLORA(p_idFotografia);
                res = 1;
            }
            catch
            {
                res = 0;
            }
            return res;
        }

        //public IHttpActionResult GetConsultarFotografiasFlora(int id)
        //{
        //    var fotografias = from f in db.VW_FOTOGRAFIA_FLORA
        //                      where f.ID_FLORA == id
        //                      select new { f.S_ETIQUETA, f.URL, f.ID_FOTOGRAFIA, f.S_ARCHIVO };

        //    if (fotografias == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(fotografias);
        //}

        public decimal GetGuardarFotografiasForm(int captacionestado, String idFotos, String tabla, String idForm)
        {
            //string[] arrayFoto = idFotos.Split(',');
            Decimal res;
            try
            {
                dbControl.SP_SET_FOTOS_FORM(captacionestado, tabla, idFotos, idForm);
                res = 1;
            }
            catch
            {
                res = 0;
            }
            return res;

        }

        public IHttpActionResult GetTramitesSau(string filtro)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
            }

            //var model = from p in db.VW_TRAMITE_A_VISITAR
            //            where p.CODFUNCIONARIO.Equals(codFuncionario)
            //            orderby p.CODTRAMITE descending
            //            select new { p.CODTRAMITE, p.FECHAINICIOTRAMITE, p.DIRECCION, p.MUNICIPIO, p.ASUNTO, p.CODTAREA, p.ORDEN };

            //return Ok(model);
            String sql = " select p.CODTRAMITE, p.FECHAINICIOTRAMITE, p.DIRECCION, p.MUNICIPIO, p.ASUNTO, p.CODTAREA, p.ORDEN,p.X,p.Y from control.VW_TRAMITE_A_VISITAR p";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }

        public IHttpActionResult GetImportarArbol([FromUri]string ruta)
        {
            UploadFileController va = new UploadFileController();



            //          string strConnectionString = "";
            //          string[] arrExtencion = ruta.Split('.');
            //          String extension = arrExtencion[1];
            //            switch (extension)
            //              {
            //                  case "xls": //Excel 97-03
            //                      strConnectionString = "Provider = Microsoft.Jet.OLEDB.12.0; Data Source ="
            //                          + ruta + "; Extended Properties = 'Excel 8.0;HDR=YES'";

            //                      break;
            //                  case "xlsx": //Excel 07

            //                      strConnectionString = "Provider =Microsoft.ACE.OLEDB.12.0; Data Source =" + ruta + "; Extended Properties = 'Excel 8.0;HDR=YES'";

            //                      break;
            //              }




            //          OleDbConnection cnCSV = new OleDbConnection(strConnectionString);


            //          cnCSV.Open();


            //DataSet ds = new DataSet();
            //DataTable sdt = cnCSV.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            /////string defaultSheet = ExcelSheets.Rows[0]["TABLE_NAME"].ToString();



            //OleDbCommand cmdSelect = new OleDbCommand(@"SELECT * FROM [" + sdt.Rows[0]["TABLE_NAME"].ToString() + "]", cnCSV);
            //          OleDbDataAdapter daCSV = new OleDbDataAdapter(cmdSelect);
            //          // daCSV.SelectCommand = cmdSelect;
            //          DataSet dtCSV = new DataSet();
            //          daCSV.Fill(dtCSV);
            //          cnCSV.Close();
            //          daCSV = null;
            //          DataTable dt = dtCSV.Tables[0];
            FileStream stream = File.Open(ruta, FileMode.Open, FileAccess.Read);

            IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);

            excelReader.IsFirstRowAsColumnNames = true;

            DataSet result = excelReader.AsDataSet();
            return Ok(result);
        }


        public IHttpActionResult GetConsultarFotografiasForm(int id)
        {
            var fotografias = from f in db.VW_FOTOGRAFIA_VISITA
                              where f.ID_VISITA == id
                              select new { f.S_ETIQUETA, f.URL, f.ID_FOTOGRAFIA, f.S_ARCHIVO };

            if (fotografias == null)
            {
                return NotFound();
            }
            return Ok(fotografias);
        }

        [HttpGet, ActionName("ObtenerFotografia")]
        public HttpResponseMessage GetObtenerFotografia(int id)
        {
            var rutaFotografia = (from f in db.FOTOGRAFIA
                             where f.ID_FOTOGRAFIA == id
                             select f.S_RUTA + "\\" + f.S_ARCHIVO).FirstOrDefault();

            if (rutaFotografia != null && File.Exists(rutaFotografia))
            {
                byte[] imagenFinal = null;
                System.Drawing.Image image = System.Drawing.Image.FromFile(rutaFotografia);

                image = ExifRotate(image);

                ImageConverter imgCon = new ImageConverter();
                imagenFinal = (byte[])imgCon.ConvertTo(image, typeof(byte[]));

                var response = new HttpResponseMessage(HttpStatusCode.OK);
                //response.Content = new ByteArrayContent(File.ReadAllBytes(rutaFotografia));
                response.Content = new ByteArrayContent(imagenFinal);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/" + Path.GetExtension(rutaFotografia));

                return response;
            }
            else
            {
                return null;
            }
        }

        private static Image ExifRotate(Image img)
        {
            int exifOrientationID = 0x112; //274

            if (!img.PropertyIdList.Contains(exifOrientationID))
                return img;

            var prop = img.GetPropertyItem(exifOrientationID);
            int val = BitConverter.ToUInt16(prop.Value, 0);
            var rot = RotateFlipType.RotateNoneFlipNone;

            if (val == 3 || val == 4)
                rot = RotateFlipType.Rotate180FlipNone;
            else if (val == 5 || val == 6)
                rot = RotateFlipType.Rotate90FlipNone;
            else if (val == 7 || val == 8)
                rot = RotateFlipType.Rotate270FlipNone;

            if (val == 2 || val == 4 || val == 5 || val == 7)
                rot |= RotateFlipType.RotateNoneFlipX;

            if (rot != RotateFlipType.RotateNoneFlipNone)
                img.RotateFlip(rot);

            return img;
        }

        public IHttpActionResult GetFuncionarioTarea(string filtro, int codTarea)
        {

            var atiende = from t in db.VW_FUNCIONARIOTAREA
                          select new { t.CODFUNCIONARIO, t.NOMBRECOMPLETO, t.CODTAREA, t.Str_CODFUNCIONARIO };

            if (filtro != "0")
            {

                string[] arrFiltros = filtro.Split(',');

                for (int contFiltro = 0; contFiltro < arrFiltros.Length; contFiltro += 4)
                {
                    long datos = 0;
                    string nombreFiltro = "";
                    switch (arrFiltros[contFiltro + 1])
                    {

                        case "contains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().Contains(nombreFiltro.ToLower()));
                            break;
                        case "notcontains":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => !t.NOMBRECOMPLETO.ToLower().Contains(nombreFiltro.ToLower()));
                            break;
                        case "startswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().StartsWith(nombreFiltro.ToLower()));

                            break;
                        case "endswith":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().EndsWith(nombreFiltro.ToLower()));

                            break;
                        case "=":
                            nombreFiltro = arrFiltros[contFiltro + 2];
                            if (arrFiltros[contFiltro] == "CODFUNCIONARIO")
                            {
                                atiende = atiende.Where(t => t.Str_CODFUNCIONARIO.Contains(nombreFiltro));
                            }
                            else
                            {
                                atiende = atiende.Where(t => t.NOMBRECOMPLETO.ToLower().Contains(nombreFiltro.ToLower()));
                            }
                            break;
                        case "<>":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO != datos);
                            break;
                        case "<":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO < datos);
                            break;
                        case "<=":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO <= datos);
                            break;
                        case ">":
                            datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                            atiende = atiende.Where(t => t.CODFUNCIONARIO > datos);
                            break;
                        case ">=":

                            break;
                    }
                }
                atiende = atiende.Where(t => t.CODTAREA == codTarea);
            }
            else
            {
                atiende = atiende.Where(t => t.CODTAREA == codTarea);
            }

            if (atiende == null)
            {
                return NotFound();
            }
            return Ok(atiende);
        }

        private void DrawImage(XGraphics gfx, Stream imageEtiqueta, int x, int y, int width, int height)
        {
            XImage image = XImage.FromStream(imageEtiqueta);
            //gfx.DrawImage(image, x, y, width, height);
            gfx.DrawImage(image, new System.Drawing.Point(x, y));
        }

        [HttpGet, ActionName("RadicarInformeTecnico")]
        public object GetRadicarInformeTecnico(int idVisita)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idUsuario;
            DateTime fechaCreacion;
            datosRespuesta resultado;

            INFORME_TECNICO informeTecnico = (from it in db.INFORME_TECNICO
                                              where it.ID_VISITA == idVisita
                                              select it).FirstOrDefault();

            if (informeTecnico != null)
            {
                if (informeTecnico.ID_RADICADO == null)
                {
                    bool puedeRadicar = (new Radicador()).SePuedeGenerarRadicado(DateTime.Now);

                    if (puedeRadicar)
                    {

                        //if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
                        //{
                        idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                        //}
                        //else
                        //{
                        // Error, el usuario logueado no tiene un tercero asociado y por lo tanto no podría registrarse el campo ID_TERCERO
                        //    resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El usuario logueado no tiene un tercero asociado.", id = "" };
                        //    return resultado;
                        // }

                        fechaCreacion = DateTime.Now;
                        Radicador radicador = new Radicador();
                        DatosRadicado radicadoGenerado = radicador.GenerarRadicado(db, 62, idUsuario, fechaCreacion);

                        informeTecnico.ID_RADICADO = radicadoGenerado.IdRadicado;
                        db.Entry(informeTecnico).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        return radicadoGenerado;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    int idRadicadoAsignado = (int)informeTecnico.ID_RADICADO;

                    RADICADO_DOCUMENTO nuevoRadicado = db.RADICADO_DOCUMENTO.Where(r => r.ID_RADICADODOC == idRadicadoAsignado).FirstOrDefault();

                    if (nuevoRadicado == null)
                    {
                        Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Error Asignando Radicado a Informe Técnico. El Radicado Asignado No Existe ID_VISITA = " + idVisita.ToString() + " - ID_RADICADO = " + idRadicadoAsignado.ToString());
                        return null;
                    }
                    else
                    {
                        Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "El Radicado ya exite para el Informe Técnico. ID_VISITA = " + idVisita.ToString() + " - ID_RADICADO = " + idRadicadoAsignado.ToString());
                        return new DatosRadicado() { IdRadicado = idRadicadoAsignado, Radicado = null, Etiqueta = null, Fecha = nuevoRadicado.D_RADICADO };
                    }
                }
            }
            else
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Error Asignando Radicado a Informe Técnico. No se encontró Informe Técnico Asociado a la Visita. ID_VISITA = " + idVisita.ToString());
                return null;
            }
        }

        [HttpGet, ActionName("ValidarHorarioRadicacion")]
        public int GetValidarHorarioRadicacion()
        {
            bool puedeRadicar = (new Radicador()).SePuedeGenerarRadicado(DateTime.Now);

            return puedeRadicar ? 1 : -1;
        }

        [HttpGet, ActionName("ReGenerarDocumentoRadicado")] // &&&
        public void GetReGenerarDocumentoRadicado(int idVisita, int idCodDocumento, int local, int registrar, int avanzaTarea)
        {
            string rutaDocumento;
            int codFuncionario1;

            //var visitaEstado = db.VISITAESTADO.Where(visitaFunc => visitaFunc.ID_VISITA == idVisita && visitaFunc.ID_ESTADOVISITA == 2).FirstOrDefault();
            codFuncionario1 = -1;

            Cryptografia crypt = new Cryptografia();

            INFORME_TECNICO informe = db.INFORME_TECNICO.Where(i => i.ID_VISITA == idVisita).FirstOrDefault();
            int codFuncionario2 = -1;

            var documento = Utilidades.Data.GenerarDocumentoRadicado(idVisita, (int)informe.ID_RADICADO, true, codFuncionario1, codFuncionario2);
            var tramitesVisita = db.TRAMITE_VISITA.Where(t => t.ID_VISITA == idVisita);

            foreach (var tramiteVisita in tramitesVisita.ToList<TRAMITE_VISITA>())
            {
                if (registrar == 1)
                {
                    rutaDocumento = RegistrarDocumento(idVisita, (int)informe.ID_RADICADO, documento.numPaginas, tramiteVisita, null);
                }
                else
                {
                    TBTRAMITE tramite = db.TBTRAMITE.Where(t => t.CODTRAMITE == tramiteVisita.ID_TRAMITE).FirstOrDefault();
                    TBRUTAPROCESO rutaProceso = db.TBRUTAPROCESO.Where(rp => rp.CODPROCESO == tramite.CODPROCESO).FirstOrDefault();

                    if (local == 0)
                        rutaDocumento = rutaProceso.PATH + "\\" + Archivos.GetRutaDocumento(Convert.ToUInt64(tramite.CODTRAMITE), 100) + tramite.CODTRAMITE.ToString("0") + "-" + idCodDocumento.ToString() + ".pdf";
                    else
                        rutaDocumento = "D:\\Temp\\Archivos\\Documento.pdf";
                }

                if (!Directory.Exists(Path.GetDirectoryName(rutaDocumento)))
                    Directory.CreateDirectory(Path.GetDirectoryName(rutaDocumento));

                if (local == 0 || registrar == 1)
                    crypt.Encriptar((byte[])documento.documentoBinario, rutaDocumento, UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"), UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"));
                else
                    System.IO.File.WriteAllBytes(rutaDocumento, (byte[])documento.documentoBinario);

                if (avanzaTarea == 1)
                {
                    ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));
                    dbTramites.SP_AVANZA_TAREA(1, tramiteVisita.ID_TRAMITE, 0, 0, 0, "0", "", rtaResultado);
                }
            }
        }

        [HttpPost, ActionName("GenerarDocumentoRadicado")]
        //public void GetGenerarDocumentoRadicado(int idVisita, int idRadicado, INDICE[] indicesDocumento)
        public void GetGenerarDocumentoRadicado(DatosDocumentoRadicado datosDocumentoRadicado)
        {
            int codFuncionario1;

            var visitaEstado = db.VISITAESTADO.Where(visitaFunc => visitaFunc.ID_VISITA == datosDocumentoRadicado.idVisita && visitaFunc.ID_ESTADOVISITA == 2).FirstOrDefault();
            codFuncionario1 = (visitaEstado == null ? -1 : (int)visitaEstado.ID_TERCERO);
            int codFuncionario2 = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value)));

            Cryptografia crypt = new Cryptografia();

            var documento = Utilidades.Data.GenerarDocumentoRadicado(datosDocumentoRadicado.idVisita, datosDocumentoRadicado.idRadicado, true, codFuncionario1, codFuncionario2);
            var tramitesVisita = db.TRAMITE_VISITA.Where(t => t.ID_VISITA == datosDocumentoRadicado.idVisita);

            foreach (var tramiteVisita in tramitesVisita.ToList<TRAMITE_VISITA>())
            {
                string rutaDocumento = RegistrarDocumento(datosDocumentoRadicado.idVisita, datosDocumentoRadicado.idRadicado, documento.numPaginas, tramiteVisita, datosDocumentoRadicado.indicesDocumento);

                if (!Directory.Exists(Path.GetDirectoryName(rutaDocumento)))
                    Directory.CreateDirectory(Path.GetDirectoryName(rutaDocumento));

                crypt.Encriptar((byte[])documento.documentoBinario, rutaDocumento, UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"), UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"));

                //TODO: Para ponerlo cuando los flujos estén configurados correctamente
                ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));
                dbTramites.SP_AVANZA_TAREA(1, tramiteVisita.ID_TRAMITE, 0, 0, 0, "0", "", rtaResultado);
            }
        }

        [HttpGet, ActionName("ActualizarEstadoInformeTecnico")]
        public bool GetActualizarEstadoInformeTecnico(decimal idVisita, decimal codResponsable, decimal codTarea, decimal codTareaSig)
        {
            try
            {
                dbControl.SP_MODIFICAR_INFTECNICO(idVisita, codResponsable, (codTarea == codTareaSig ? 2 : 1));
            }
            catch (Exception e)
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), Utilidades.LogErrores.ObtenerError(e));
                return false;
            }
            return true;
        }

        [HttpGet, ActionName("IndicesDocumentoActuacion")]
        public dynamic GetIndicesDocumentoActuacion(int idVisita)
        {
            try
            {
                var indicesDocumentos = Utilidades.Data.ObtenerIndicesDocumento(idVisita);

                return (indicesDocumentos);
            }
            catch (Exception error)
            {
                return null;
            }
        }

        [HttpGet, ActionName("PruebaPDFImagen")]
        public void GetPruebaPDFImagen()
        {
            int count;

            PdfSharp.Pdf.PdfDocument inputDocument;
            var stream = new MemoryStream();
            int numPaginas = 0;

            // Convertir a PDF, Obtener Coordenas del tag {FIRMA} y eliminar el archivo
            PdfSharp.Pdf.PdfDocument outputDocument = new PdfSharp.Pdf.PdfDocument();

            inputDocument = PdfReader.Open(Archivos.ConvertirAPDF("C:\\Temp\\InformeTecnico.docx"), PdfDocumentOpenMode.Import);

            count = inputDocument.PageCount;
            for (int idx = 0; idx < count; idx++)
            {
                PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];

                outputDocument.AddPage(page);

                numPaginas++;
            }

            outputDocument.Save("C:\\Temp\\InformeTecnicoTemp.pdf");
            outputDocument.Close();
            PdfDocumentProcessor documento = new PdfDocumentProcessor();
            documento.LoadDocument("C:\\Temp\\InformeTecnicoTemp.pdf");
            PdfTextSearchResults result = documento.FindText("FIRMA1");
            //PdfTextSearchResults result2 = documento.FindText("FIRMA2");
            //PdfTextSearchResults result3 = documento.FindText("FIRMA3");
            //PdfTextSearchResults result4 = documento.FindText("FIRMA4");
            documento.CloseDocument();
            File.Delete("C:\\Temp\\InformeTecnicoTemp.pdf");

            // Eliminar el Texto {FIRMA}
            DocX document = DocX.Load("C:\\Temp\\InformeTecnico.docx");
            //document.ReplaceText("{FIRMA}", "");
            document.SaveAs("C:\\Temp\\InformeTecnicoTemp.docx");

            // Convertir a PDF sin el Texto {FIRMA} e insertar la imagen de la firma
            inputDocument = PdfReader.Open(Archivos.ConvertirAPDF("C:\\Temp\\InformeTecnicoTemp.docx"), PdfDocumentOpenMode.Import);
            outputDocument = new PdfSharp.Pdf.PdfDocument();

            count = inputDocument.PageCount;
            for (int idx = 0; idx < count; idx++)
            {
                PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];

                outputDocument.AddPage(page);

                numPaginas++;
            }
            outputDocument.Save("C:\\Temp\\InformeTecnicoTemp.pdf");
            outputDocument.Close();

            PdfSharp.Pdf.PdfDocument documentFirma = PdfReader.Open("C:\\Temp\\InformeTecnicoTemp.pdf", PdfDocumentOpenMode.Modify);
            // Get an XGraphics object for drawing
            var alturaPagina = documentFirma.Pages[0].Height.Value;
            XGraphics gfx = XGraphics.FromPdfPage(documentFirma.Pages[result.PageNumber-1]);
            DrawImage(gfx, Security.ObtenerFirmaElectronicaFuncionario(71763413), Convert.ToInt32(result.Rectangles[0].Left), Convert.ToInt32((alturaPagina - result.Rectangles[0].Top) - (130) / 2), Convert.ToInt32(400), Convert.ToInt32(130));
            //DrawImage(gfx, "C:\\Temp\\FirmaTransparente.png", Convert.ToInt32(result.Rectangles[0].Left ), Convert.ToInt32((alturaPagina-result.Rectangles[0].Top)  - (177 * 0.7) / 2), Convert.ToInt32(249 * 0.7), Convert.ToInt32(177 * 0.7));
            //DrawImage(gfx, "C:\\Temp\\FirmaTransparente.png", Convert.ToInt32(result2.Rectangles[0].Left ), Convert.ToInt32((alturaPagina - result2.Rectangles[0].Top)  - (177 * 0.7) / 2), Convert.ToInt32(249 * 0.7), Convert.ToInt32(177 * 0.7));
            //DrawImage(gfx, "C:\\Temp\\FirmaTransparente.png", Convert.ToInt32(result3.Rectangles[0].Left ), Convert.ToInt32((alturaPagina - result3.Rectangles[0].Top)  - (177 * 0.7) / 2), Convert.ToInt32(249 * 0.7), Convert.ToInt32(177 * 0.7));
            //DrawImage(gfx, "C:\\Temp\\FirmaTransparente.png", Convert.ToInt32(result4.Rectangles[0].Left ), Convert.ToInt32((alturaPagina - result4.Rectangles[0].Top)  - (177 * 0.7) / 2), Convert.ToInt32(249 * 0.7), Convert.ToInt32(177 * 0.7));
            documentFirma.Save("C:\\Temp\\InformeTecnicoTemp.pdf");
            documentFirma.Close();
        }

        void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y, int width, int height)
        {
            XImage image = XImage.FromFile(jpegSamplePath);
            gfx.DrawImage(image, x, y, width, height);
        }

        void DrawImage(XGraphics gfx, System.Drawing.Image imageFirma, int x, int y, int width, int height)
        {
            var stream = new System.IO.MemoryStream();
            imageFirma.Save(stream, ImageFormat.Jpeg);
            stream.Position = 0;

            XImage image = XImage.FromStream(stream);
            gfx.DrawImage(image, x, y, width, height);
        }

        [HttpGet, ActionName("GetDocXPrueba")]
        public void GetDocXPrueba()
        {
            var stream = new MemoryStream();

            DocX document = DocX.Load("C:\\Temp\\InformeTecnico.docx");

            /*document.ReplaceText("{INSTALACION}", consulta.NOMBRE_INSTALACION ?? "");
            document.ReplaceText("{EMPRESA}", consulta.EMPRESA ?? "");
            document.ReplaceText("{REPRESENTANTE_LEGAL}", consulta.REPRESENTANTE_LEGAL ?? "");
            document.ReplaceText("{NIT}", consulta.NRO_DOCUMENTO == null ? "" : consulta.NRO_DOCUMENTO.ToString());
            document.ReplaceText("{DIRECCION}", consulta.DIRECCION ?? "");
            document.ReplaceText("{TELEFONO}", consulta.TELEFONO_INSTALACION ?? "");
            document.ReplaceText("{MUNICIPIO}", consulta.MUNICIPIO ?? "");
            document.ReplaceText("{CM}", consulta.CM ?? "");
            document.ReplaceText("{QUEJA}", consulta.QUEJA ?? "");
            document.ReplaceText("{ANO}", consulta.ANO == null ? "" : consulta.ANO.ToString());
            document.ReplaceText("{ABOGADO}", consulta.ABOGADO ?? "");*/

            document.SaveAs("C:\\Temp\\InformeTecnico_Firma.docx");
        }

        private string RegistrarDocumento(int idVisita, int idRadicado, int numPaginas, TRAMITE_VISITA tramiteVisita, List<INDICE> indicesDocumento)
        {
            string rutaDocumento = "";
            int idCodDocumento;
            int? codFuncionarioTecnico1 = null;
            string funcionarioTecnico = null;

            VISITA visita = db.VISITA.Where(v => v.ID_VISITA == idVisita).FirstOrDefault();
            var terceroVisita = visita.TERCERO_VISITA.FirstOrDefault();
            int idTerceroVisita = 0;
            TERCERO tercero = null;
            if (terceroVisita != null)
            {
                idTerceroVisita = terceroVisita.ID_TERCERO;
                tercero = db.TERCERO.Where(t => t.ID_TERCERO == idTerceroVisita).FirstOrDefault();
            }
            var idInstalacionVisita = visita.INSTALACION_VISITA.FirstOrDefault().ID_INSTALACION;
            INSTALACION instalacion = db.INSTALACION.Where(i => i.ID_INSTALACION == idInstalacionVisita).FirstOrDefault();
            var idProyecto = db.TERCERO_INSTALACION.Where(i => i.ID_INSTALACION == instalacion.ID_INSTALACION).FirstOrDefault().CODIGO_PROYECTO;
            TBPROYECTO cm = db.TBPROYECTO.Where(p => p.CODIGO_PROYECTO == idProyecto).FirstOrDefault();
            //var tramiteVisita = db.TRAMITE_VISITA.Where(t => t.ID_VISITA == idVisita).FirstOrDefault();
            TBTRAMITE tramite = db.TBTRAMITE.Where(t => t.CODTRAMITE == tramiteVisita.ID_TRAMITE).FirstOrDefault();
            TBRUTAPROCESO rutaProceso = db.TBRUTAPROCESO.Where(rp => rp.CODPROCESO == tramite.CODPROCESO).FirstOrDefault();
            TBTRAMITEDOCUMENTO ultimoDocumento = db.TBTRAMITEDOCUMENTO.Where(td => td.CODTRAMITE == tramiteVisita.ID_TRAMITE).OrderByDescending(td => td.CODDOCUMENTO).FirstOrDefault();
            RADICADO_DOCUMENTO radicado = db.RADICADO_DOCUMENTO.Where(r => r.ID_RADICADODOC == idRadicado).FirstOrDefault();

            codFuncionarioTecnico1 = db.VISITAESTADO.Where(visitaFunc => visitaFunc.ID_VISITA == idVisita && visitaFunc.ID_ESTADOVISITA == 2).Select(f => f.ID_TERCERO).FirstOrDefault();
            if (codFuncionarioTecnico1 != null)
                funcionarioTecnico = db.VW_FUNCIONARIO.Where(f => f.CODFUNCIONARIO == codFuncionarioTecnico1).Select(f => f.NOMBRECOMPLETO).FirstOrDefault();

            if (ultimoDocumento == null)
                idCodDocumento = 1;
            else
                idCodDocumento = Convert.ToInt32(ultimoDocumento.CODDOCUMENTO) + 1;

            rutaDocumento = rutaProceso.PATH + "\\" + Archivos.GetRutaDocumento(Convert.ToUInt64(tramite.CODTRAMITE), 100) + tramite.CODTRAMITE.ToString("0") + "-" + idCodDocumento.ToString() + ".pdf";
            //rutaDocumento = @"C:\Temp\SIM\TRAMITES\CONTROL Y VIGILANCIA" + "\\" + Archivos.GetRutaDocumento(Convert.ToUInt64(tramite.CODTRAMITE), 100) + tramite.CODTRAMITE.ToString("0") + "-" + idCodDocumento.ToString() + ".pdf";
            idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));

            TBTRAMITEDOCUMENTO documento = new TBTRAMITEDOCUMENTO();
            documento.CODDOCUMENTO = idCodDocumento;
            documento.CODTRAMITE = tramite.CODTRAMITE;
            documento.TIPODOCUMENTO = 1;
            documento.FECHACREACION = DateTime.Now;
            documento.CODFUNCIONARIO = codFuncionario;
            documento.ID_USUARIO = idUsuario;
            documento.RUTA = rutaDocumento;
            documento.MAPAARCHIVO = "M";
            documento.MAPABD = "M";
            documento.PAGINAS = numPaginas;
            documento.CODSERIE = Convert.ToInt32(radicado.CODSERIE);
            documento.CIFRADO = "1";

            db.Entry(documento).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();

            INFORME_TECNICO informeTecnico = db.INFORME_TECNICO.Where(it => it.ID_VISITA == idVisita).FirstOrDefault();
            informeTecnico.URL2 = rutaDocumento;
            informeTecnico.ID_ESTADOINF = 3; // Aprobado
            db.Entry(informeTecnico).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            radicado.CODTRAMITE = tramite.CODTRAMITE;
            radicado.CODDOCUMENTO = idCodDocumento;
            db.Entry(radicado).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            // Consulta Datos de los Indices del Documento
            //string sql = "SELECT distinct VWR_DETALLEINFTECNICO.IDTRAMITE, VWR_DETALLEINFTECNICO.IDVISITA, VWR_DETALLEINFTECNICO.EMPRESA, VWR_DETALLEINFTECNICO.DIRECCION, VWR_DETALLEINFTECNICO.MUNICIPIO, VWR_DETALLEINFTECNICO.NOMBRE_INSTALACION, VWR_DETALLEINFTECNICO.TELEFONO_INSTALACION, VWR_DETALLEINFTECNICO.CM, VWR_DETALLEINFTECNICO.REPRESENTANTE_LEGAL, VWR_DETALLEINFTECNICO.NRO_DOCUMENTO, VWR_DETALLEINFTECNICO.ID_TERCERO, VWR_DETALLEINFTECNICO.ID_INSTALACION, VWR_DETALLEINFTECNICO.QUEJA, VWR_DETALLEINFTECNICO.ANO, VWR_DETALLEINFTECNICO.ABOGADO FROM control.VWR_DETALLEINFTECNICO WHERE VWR_DETALLEINFTECNICO.IDVISITA = " + idVisita.ToString();

            //DatosVisita consulta = ((DBContext)db).Database.SqlQuery<DatosVisita>(sql).FirstOrDefault();

            /*
            "{INSTALACION}", consulta.NOMBRE_INSTALACION ?? "");
            "{EMPRESA}", consulta.EMPRESA ?? "");
            "{REPRESENTANTE_LEGAL}", consulta.REPRESENTANTE_LEGAL ?? "");
            "{NIT}", consulta.NRO_DOCUMENTO == null ? "" : consulta.NRO_DOCUMENTO.ToString());
            "{DIRECCION}", consulta.DIRECCION ?? "");
            "{TELEFONO}", consulta.TELEFONO_INSTALACION ?? "");
            "{MUNICIPIO}", consulta.MUNICIPIO ?? "");
            "{CM}", consulta.CM ?? "");
            "{QUEJA}", consulta.QUEJA ?? "");
            "{ANO}", consulta.ANO == null ? "" : consulta.ANO.ToString());
            "{ABOGADO}", consulta.ABOGADO ?? "");
            "{TRAMITE}", consulta.IDTRAMITE.ToString());*/

            TBINDICEDOCUMENTO indiceDocumento;

            if (indicesDocumento == null || indicesDocumento.Count == 0)
            {
                // 320	PARA
                if (tercero != null)
                {
                    indiceDocumento = new TBINDICEDOCUMENTO();
                    indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                    indiceDocumento.CODDOCUMENTO = idCodDocumento;
                    indiceDocumento.CODINDICE = 320;
                    indiceDocumento.VALOR = tercero.S_RSOCIAL;
                    db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                }

                // 321	ASUNTO
                if (visita != null)
                {
                    indiceDocumento = new TBINDICEDOCUMENTO();
                    indiceDocumento.CODDOCUMENTO = idCodDocumento;
                    indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                    indiceDocumento.CODINDICE = 321;
                    indiceDocumento.VALOR = visita.S_ASUNTO;
                    db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                }

                // 322	REPRESENTANTE LEGAL
                /*indiceDocumento = new TBINDICEDOCUMENTO();
                indiceDocumento.CODDOCUMENTO = idCodDocumento;
                indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                indiceDocumento.CODINDICE = 322;
                indiceDocumento.VALOR = consulta.REPRESENTANTE_LEGAL ?? "";
                db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();*/

                // 323	NIT O CEDULA SOLICITANTE
                if (tercero != null)
                {
                    indiceDocumento = new TBINDICEDOCUMENTO();
                    indiceDocumento.CODDOCUMENTO = idCodDocumento;
                    indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                    indiceDocumento.CODINDICE = 323;
                    indiceDocumento.VALOR = tercero.N_DOCUMENTON.ToString();
                    db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                }

                // 324	DIRECCION
                indiceDocumento = new TBINDICEDOCUMENTO();
                indiceDocumento.CODDOCUMENTO = idCodDocumento;
                indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                indiceDocumento.CODINDICE = 324;
                indiceDocumento.VALOR = tramite.DIRECCION;
                db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

                // 325	TELEFONO
                indiceDocumento = new TBINDICEDOCUMENTO();
                indiceDocumento.CODDOCUMENTO = idCodDocumento;
                indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                indiceDocumento.CODINDICE = 325;
                indiceDocumento.VALOR = tramite.TELEFONO;
                db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

                // 326	MUNICIPIO
                /*indiceDocumento = new TBINDICEDOCUMENTO();
                indiceDocumento.CODDOCUMENTO = idCodDocumento;
                indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                indiceDocumento.CODINDICE = 326;
                indiceDocumento.VALOR = consulta.MUNICIPIO ?? "";
                db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();*/

                // 327	CM
                if (cm != null)
                {
                    indiceDocumento = new TBINDICEDOCUMENTO();
                    indiceDocumento.CODDOCUMENTO = idCodDocumento;
                    indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                    indiceDocumento.CODINDICE = 327;
                    indiceDocumento.VALOR = cm.CM;
                    db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                }

                // 328	AÑO QUEJA
                /*indiceDocumento = new TBINDICEDOCUMENTO();
                indiceDocumento.CODDOCUMENTO = idCodDocumento;
                indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                indiceDocumento.CODINDICE = 328;
                indiceDocumento.VALOR = consulta.ANO.ToString();
                db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();*/

                // 340	DE
                /*indiceDocumento = new TBINDICEDOCUMENTO();
                indiceDocumento.CODDOCUMENTO = idCodDocumento;
                indiceDocumento.CODINDICE = 340;
                indiceDocumento.VALOR = "";
                db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();*/

                // 341	PROYECTO
                /*indiceDocumento = new TBINDICEDOCUMENTO();
                indiceDocumento.CODDOCUMENTO = idCodDocumento;
                indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                indiceDocumento.CODINDICE = 341;
                indiceDocumento.VALOR = ??;
                db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();*/

                // 342	ABOGADO ASIGNADO
                /*if (consulta.ABOGADO != null)
                {
                    indiceDocumento = new TBINDICEDOCUMENTO();
                    indiceDocumento.CODDOCUMENTO = idCodDocumento;
                    indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                    indiceDocumento.CODINDICE = 342;
                    indiceDocumento.VALOR = consulta.ABOGADO;
                    db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                }*/

                // 360	QUEJA
                /*if (consulta.QUEJA != null)
                {
                    indiceDocumento = new TBINDICEDOCUMENTO();
                    indiceDocumento.CODDOCUMENTO = idCodDocumento;
                    indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                    indiceDocumento.CODINDICE = 360;
                    indiceDocumento.VALOR = consulta.QUEJA;
                    db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                }*/

                // 380	CENTRO DE COSTOS
                /*indiceDocumento = new TBINDICEDOCUMENTO();
                indiceDocumento.CODDOCUMENTO = idCodDocumento;
                indiceDocumento.CODINDICE = 380;
                indiceDocumento.VALOR = "";
                db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();*/

                // 381	RADICADO
                indiceDocumento = new TBINDICEDOCUMENTO();
                indiceDocumento.CODDOCUMENTO = idCodDocumento;
                indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                indiceDocumento.CODINDICE = 381;
                indiceDocumento.VALOR = radicado.S_RADICADO;
                db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

                // 382	FECHA
                indiceDocumento = new TBINDICEDOCUMENTO();
                indiceDocumento.CODDOCUMENTO = idCodDocumento;
                indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                indiceDocumento.CODINDICE = 382;
                indiceDocumento.VALOR = radicado.D_RADICADO.ToString("dd-MM-yyyy");
                db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

                // 400	ANEXOS
                /*indiceDocumento = new TBINDICEDOCUMENTO();
                indiceDocumento.CODDOCUMENTO = idCodDocumento;
                indiceDocumento.CODINDICE = 400;
                indiceDocumento.VALOR = "";
                db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();*/

                // 680	TÉCNICO
                indiceDocumento = new TBINDICEDOCUMENTO();
                indiceDocumento.CODDOCUMENTO = idCodDocumento;
                indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                indiceDocumento.CODINDICE = 680;
                indiceDocumento.VALOR = funcionarioTecnico ?? "";
                db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

                // 2022 ENTIDAD
                /*indiceDocumento = new TBINDICEDOCUMENTO();
                indiceDocumento.CODDOCUMENTO = idCodDocumento;
                indiceDocumento.CODINDICE = 2022;
                indiceDocumento.VALOR = "";
                db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();*/

                // 2381 TIPO INFORME
                /*indiceDocumento = new TBINDICEDOCUMENTO();
                indiceDocumento.CODDOCUMENTO = idCodDocumento;
                indiceDocumento.CODINDICE = 2381;
                indiceDocumento.VALOR = "";
                db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();*/
            }
            else
            {
                // 381	RADICADO
                indiceDocumento = new TBINDICEDOCUMENTO();
                indiceDocumento.CODDOCUMENTO = idCodDocumento;
                indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                indiceDocumento.CODINDICE = 381;
                indiceDocumento.VALOR = radicado.S_RADICADO;
                db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

                // 382	FECHA
                indiceDocumento = new TBINDICEDOCUMENTO();
                indiceDocumento.CODDOCUMENTO = idCodDocumento;
                indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                indiceDocumento.CODINDICE = 382;
                indiceDocumento.VALOR = radicado.D_RADICADO.ToString("dd-MM-yyyy");
                db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

                foreach (INDICE indice in indicesDocumento)
                {
                    if (indice.VALOR != null && indice.VALOR.Trim() != "")
                    {
                        indiceDocumento = new TBINDICEDOCUMENTO();
                        indiceDocumento.CODDOCUMENTO = idCodDocumento;
                        indiceDocumento.CODTRAMITE = Convert.ToInt32(tramite.CODTRAMITE);
                        indiceDocumento.CODINDICE = indice.CODINDICE;
                        indiceDocumento.VALOR = indice.VALOR;
                        db.Entry(indiceDocumento).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                    }
                }
            }

            return rutaDocumento;
        }

        public IHttpActionResult GetTercero(string filtro)
        {


            var atiende = from t in db.VW_TERCERO


                          select new { t.ID, t.DOCUMENTO, t.NOMBRE, t.DOCUMENTOINT };
            if (filtro != null)
            {
                string[] arrFiltros = filtro.Split(',');
                if (arrFiltros[2].Length > 4)
                {
                    // atiende = atiende.Where(t => t.DOCUMENTO == "8110454213");
                    for (int contFiltro = 0; contFiltro < arrFiltros.Length; contFiltro += 4)
                    {
                        long datos = 0;
                        string nombreFiltro = "";
                        switch (arrFiltros[contFiltro + 1])
                        {

                            case "contains":
                                nombreFiltro = arrFiltros[contFiltro + 2];
                                atiende = atiende.Where(t => t.NOMBRE.ToLower().Contains(nombreFiltro.ToLower()));
                                break;
                            case "notcontains":
                                nombreFiltro = arrFiltros[contFiltro + 2];
                                atiende = atiende.Where(t => !t.NOMBRE.ToLower().Contains(nombreFiltro.ToLower()));
                                break;
                            case "startswith":
                                nombreFiltro = arrFiltros[contFiltro + 2];
                                atiende = atiende.Where(t => t.NOMBRE.ToLower().StartsWith(nombreFiltro.ToLower()));

                                break;
                            case "endswith":
                                nombreFiltro = arrFiltros[contFiltro + 2];
                                atiende = atiende.Where(t => t.NOMBRE.ToLower().EndsWith(nombreFiltro.ToLower()));

                                break;
                            case "=":
                                nombreFiltro = arrFiltros[contFiltro + 2];
                                if (arrFiltros[contFiltro] == "DOCUMENTO")
                                {
                                    atiende = atiende.Where(t => t.DOCUMENTO.Contains(nombreFiltro));
                                }
                                else
                                {
                                    atiende = atiende.Where(t => t.NOMBRE.Contains(nombreFiltro));
                                }
                                break;
                            case "<>":
                                datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                                atiende = atiende.Where(t => t.DOCUMENTOINT != datos);
                                break;
                            case "<":
                                datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                                atiende = atiende.Where(t => t.DOCUMENTOINT < datos);
                                break;
                            case "<=":
                                datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                                atiende = atiende.Where(t => t.DOCUMENTOINT <= datos);
                                break;
                            case ">":
                                datos = Convert.ToInt64(arrFiltros[contFiltro + 2]);
                                atiende = atiende.Where(t => t.DOCUMENTOINT > datos);
                                break;
                            case ">=":

                                break;
                        }
                    }
                }
                else
                {
                    atiende = atiende.Where(t => t.DOCUMENTOINT == -1);
                }
            }
            else
            {
                atiende = atiende.Where(t => t.DOCUMENTOINT == -1);
            }

            if (atiende.ToList() == null)
            {
                return NotFound();
            }
            return Ok(atiende.ToList());
        }

        // idF: Formulario
        // idItemP: Item principal
        // i: items a fusionar
        // Retorno: OK, Lista de items de la misma visita
        [ActionName("FusionarItems")]
        public string GetFusionarItems(int idF, int idItemP, string i)
        {
            string respuesta = "";
            string sql;
            FORMULARIO formulario = db.FORMULARIO.Where(f => f.ID_FORMULARIO == idF).FirstOrDefault();

            // Verificación si hay varios items de la misma visita
            sql = "SELECT e." + formulario.S_CAMPO_ID_VISITA + " AS IDVISITA, i.NOMBRE AS DESCRIPCION " +
                "FROM " + formulario.TBL_ESTADOS + " e INNER JOIN " +
                "(" +
                "   SELECT ei." + formulario.S_CAMPO_ID_VISITA + " AS IDVISITA " +
                "   FROM " + formulario.TBL_ESTADOS + " ei " +
                "   WHERE ei." + formulario.S_CAMPO_ID_ITEM + " IN (" + i + ") " +
                "   GROUP BY ei." + formulario.S_CAMPO_ID_VISITA + " " +
                "   HAVING COUNT(0) > 1 " +
                ") vi ON e." + formulario.S_CAMPO_ID_VISITA + " = vi.IDVISITA INNER JOIN " +
                formulario.TBL_ITEM + " i ON e." + formulario.S_CAMPO_ID_ITEM + " = i." + formulario.S_CAMPO_ID_ITEM + " " +
                "WHERE e." + formulario.S_CAMPO_ID_ITEM + " IN (" + i + ")";

            /*sql = "SELECT e." + formulario.S_CAMPO_ID_VISITA + " AS IDVISITA, i.NOMBRE AS DESCRIPCION " +
                "FROM " + formulario.TBL_ESTADOS + " e INNER JOIN " +
                formulario.TBL_ITEM + " i ON e." + formulario.S_CAMPO_ID_ITEM + " = i." + formulario.S_CAMPO_ID_ITEM + " " +
                "WHERE e." + formulario.S_CAMPO_ID_ITEM + " IN (" + i + ") " +
                "GROUP BY e." + formulario.S_CAMPO_ID_VISITA + " , i.NOMBRE " +
                "HAVING COUNT(0) > 1";*/

            var itemsInconsistentes = db.Database.SqlQuery<ItemsVisita>(sql).ToList();

            foreach (var itemInconsistente in itemsInconsistentes)
            {
                if (respuesta == "")
                    respuesta = itemInconsistente.DESCRIPCION;
                else
                    respuesta += ", " + itemInconsistente.DESCRIPCION;
            }

            if (respuesta == "")
            {
                sql = "UPDATE " + formulario.TBL_ESTADOS + " " +
                    "SET " + formulario.S_CAMPO_ID_ITEM + " = " + idItemP.ToString() + " " +
                    "WHERE " + formulario.S_CAMPO_ID_ITEM + " IN (" + i + ")";

                db.Database.ExecuteSqlCommand(sql);

                sql = "DELETE FROM CONTROL.FORMULARIO_ITEM " +
                    "WHERE ID_FORMULARIO = " + idF.ToString() + " AND ID_ITEM IN (" + i + ") AND ID_ITEM <> " + idItemP.ToString();

                db.Database.ExecuteSqlCommand(sql);

                sql = "UPDATE " + formulario.TBL_ITEM + " " +
                    "SET " + formulario.S_CAMPO_ID_ITEM + " = " + formulario.S_CAMPO_ID_ITEM + "*(-1) " +
                    "WHERE " + formulario.S_CAMPO_ID_ITEM + " IN (" + i + ") AND " + formulario.S_CAMPO_ID_ITEM + " <> " + idItemP.ToString();

                db.Database.ExecuteSqlCommand(sql);

                return "OK";
            } else {
                return respuesta;
            }
        }

        // fi: Fecha Inicial
        // ff: Fecha Final
        // Retorno: Lista de Informes en el rango de fechas seleccionado
        [HttpGet]
        [ActionName("HistoricoInformes")]
        public DatosConsultaInformes GetHistoricoInformes(string fi, string ff)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
            }

            var datos = db.Database.SqlQuery<Informe>("SELECT DISTINCT it.ID_INF, NVL(it.ASUNTO, v.S_ASUNTO) AS ASUNTO, NVL(it.OBSERVACION, v.S_OBSERVACION) AS OBSERVACION, it.ID_ESTADOINF, it.ID_VISITA, it.FUNCIONARIO, it.URL, it.URL2, it.ID_RADICADO, rd.D_RADICADO, rd.S_RADICADO, dit.CM, dit.QUEJA FROM CONTROL.INFORME_TECNICO it INNER JOIN CONTROL.VISITA v ON it.ID_VISITA = v.ID_VISITA LEFT OUTER JOIN CONTROL.VWR_DETALLEINFTECNICO dit ON it.ID_VISITA = dit.IDVISITA LEFT OUTER JOIN TRAMITES.RADICADO_DOCUMENTO rd ON it.ID_RADICADO = rd.ID_RADICADODOC WHERE it.FUNCIONARIO = " + codFuncionario + " AND it.ID_ESTADOINF = 3 AND rd.D_RADICADO BETWEEN TO_DATE('" + fi + "', 'dd/mm/yyyy') AND TO_DATE('" + ff + "', 'dd/mm/yyyy') + 1 ORDER BY rd.D_RADICADO DESC");

            var detalleInformes = datos.ToList<Informe>();

            DatosConsultaInformes resultado = new DatosConsultaInformes();
            resultado.numRegistros = detalleInformes.Count();
            resultado.datos = detalleInformes;

            return resultado;
        }
    }
}