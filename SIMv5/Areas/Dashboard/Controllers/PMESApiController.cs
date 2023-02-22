using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Areas.GestionDocumental.Models;
using SIM.Data;
using System.IO;
using System.Net.Http.Headers;
using System.Data.Entity;
using System.Transactions;
using System.Xml.Linq;
using System.Drawing.Imaging;
using System.Data.Entity.SqlServer;
using System.Reflection;
using SIM.Utilidades;

namespace SIM.Areas.Dashboard.Controllers
{
    /// <summary>
    /// Controlador RadicadorApi: Operaciones para Generar Radicados e imprimir etiquetas. También suministra los datos de serie, subserie, unidad documental y el documento asociado al radicado.
    /// </summary>
    public partial class PMESApiController : ApiController
    {
        /// <summary>
        /// Estructura que almacena el resultado de una consulta, entregando los datos y número de registros en total.
        /// </summary>
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        public class Instalacion
        {
            public int ID_INSTALACION { get; set; }
            public string S_NOMBRE { get; set; }
        }

        public class Vigencia
        {
            public string VIGENCIA { get; set; }
            public int ID_ENCUESTA { get; set; }
        }

        /// <summary>
        /// Variable de Contexto de Base de Datos.
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        [HttpGet, ActionName("Instalaciones")]
        public datosConsulta GetInstalaciones(int e)
        {
            List<Instalacion> instalaciones;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero;

            idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            var sql = "SELECT DISTINCT ge.ID_INSTALACION, i.S_NOMBRE " +
                        "FROM CONTROL.ENC_SOLUCION s INNER JOIN " +
                        "CONTROL.FRM_GENERICO_ESTADO ge ON s.ID_ESTADO = ge.ID_ESTADO INNER JOIN " +
                        "GENERAL.TERCERO t ON ge.ID_TERCERO = t.ID_TERCERO INNER JOIN " +
                        "GENERAL.INSTALACION i ON ge.ID_INSTALACION = i.ID_INSTALACION " +
                        "WHERE ID_FORMULARIO = 14 AND s.ID_ENCUESTA IN (" + (e == 1 ? "822, 842, 843, 844" : "1728,1729,1744,1743,1742,1730,1684,1683,1702,1682") + ") AND ge.ID_TERCERO = " + idTercero.ToString();

            var model = dbSIM.Database.SqlQuery<Instalacion>(sql);

            /*var model = (from instalacion in dbSIM.INSTALACION
                         join terceroInst in dbSIM.TERCERO_INSTALACION on instalacion.ID_INSTALACION equals terceroInst.ID_INSTALACION
                         where terceroInst.ID_TERCERO == idTercero
                         orderby instalacion.S_NOMBRE
                         select new
                         {
                             instalacion.ID_INSTALACION,
                             instalacion.S_NOMBRE
                         });*/

            datosConsulta resultado = new datosConsulta();
            resultado.numRegistros = model.Count();
            instalaciones = model.ToList<Instalacion>();
            instalaciones.Insert(0, new Instalacion() { ID_INSTALACION = -1, S_NOMBRE = "(Todas)" });
            resultado.datos = instalaciones;

            return resultado;
        }

        [HttpGet, ActionName("Vigencias")]
        public datosConsulta GetVigencias(int? t)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero = 0;
            bool terceroUsuario = true;

            if (t != null)
            {
                var rolesUsuario = ((System.Security.Claims.ClaimsPrincipal)context.User).FindAll(CustomClaimTypes.IdRol).Where(r => r.Value == "401");

                if (rolesUsuario != null)
                {
                    idTercero = (int)t;
                    terceroUsuario = false;
                }
            }

            if (terceroUsuario)
                idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            /*var sql = "SELECT DISTINCT vs.VALOR AS VIGENCIA, CASE WHEN vs.ID_VIGENCIA = 701 THEN 1 ELSE 2 END AS ID_ENCUESTA " +
                        "FROM CONTROL.VIGENCIA_SOLUCION vs INNER JOIN " +
                        "CONTROL.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO = ge.ID_ESTADO INNER JOIN " +
                        "GENERAL.TERCERO t on t.ID_TERCERO = ge.ID_TERCERO INNER JOIN " +
                        "GENERAL.INSTALACION i on i.ID_INSTALACION = ge.ID_INSTALACION " +
                        "WHERE t.ID_TERCERO = " + idTercero.ToString() + " AND ge.ACTIVO = 0 AND vs.ID_VIGENCIA IN (701,1081) " +
                        "ORDER BY vs.VALOR";


            var model = dbSIM.Database.SqlQuery<Vigencia>(sql);*/

            var vigencias = (from pmes in dbSIM.VWM_PMES
                             where pmes.ID_TERCERO == idTercero
                             select new Vigencia
                             {
                                 VIGENCIA = pmes.VIGENCIA,
                                 ID_ENCUESTA = pmes.ID_ENCUESTA ?? 1
                             }).Distinct().OrderBy(v => v.VIGENCIA);
                            

            datosConsulta resultado = new datosConsulta();
            resultado.numRegistros = vigencias.Count();
            resultado.datos = vigencias.ToList<Vigencia>();

            return resultado;
        }
    }
}