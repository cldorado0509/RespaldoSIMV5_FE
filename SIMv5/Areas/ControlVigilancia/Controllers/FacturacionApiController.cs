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
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SIM.Data.Seguridad;

namespace SIM.Areas.ControlVigilancia.Controllers
{
    public class FacturacionApiController : ApiController
    {
        public class RegistroFacturacion
        {
            public int CODTRAMITE { get; set; }
            public int CODDOCUMENTO { get; set; }
            public string ASUNTO { get; set; }
            public string NIT_SOLICITANTE { get; set; }
            public string DIRECCION { get; set; }
            public string TELEFONO { get; set; }
            public string CM { get; set; }
            public DateTime? FECHARADICADO { get; set; }
            public string RADICADO { get; set; }
            public string TIPO_APROVECHAMIENTO_FORESTAL { get; set; }
        }

        public partial class IndicesDocumento
        {
            public decimal? CODTRAMITE { get; set; }

            public decimal CODDOCUMENTO { get; set; }

            public int CODINDICE { get; set; }

            public string VALOR { get; set; }

            public string VALORID { get; set; }

            public DateTime? D_RADICADO { get; set; }
        }

        public struct DatosRespuesta
        {
            public string tipoRespuesta; // OK, Error
            public string detalleRespuesta;
        }

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
            public string seleccionados;
        }

        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        [Authorize(Roles = "VFACTURACION")]
        [HttpGet, ActionName("ConsultaPendientesFacturacion")]
        public datosConsulta GetConsultaPendientesFacturacion()
        {
            dynamic modelData;
            int idUsuario = 0;
            USUARIO usuario = null;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

                usuario = (from u in dbSIM.USUARIO
                            where u.ID_USUARIO == idUsuario
                            select u).FirstOrDefault();
            }
            else
            {
                return new datosConsulta() { numRegistros = 0, datos = null };
            }

            if (usuario == null || (usuario.ID_GRUPO != 2 && usuario.ID_GRUPO != 101))
            {
                return new datosConsulta() { numRegistros = 0, datos = null };
            }
            else
            {
                string documentoActual = "";
                RegistroFacturacion registroFacturacion = null;
                List<RegistroFacturacion> pendientesFacturacion = new List<RegistroFacturacion>();

                string sql = "SELECT ida.*, rd.D_RADICADO, rd.S_RADICADO " +
                        "FROM TRAMITES.TBINDICEDOCUMENTO id INNER JOIN " +
                        "   TRAMITES.TBINDICEDOCUMENTO idf ON id.CODDOCUMENTO = idf.CODDOCUMENTO AND id.CODTRAMITE = idf.CODTRAMITE AND id.CODINDICE = 3922 AND id.VALOR = 'S' AND idf.CODINDICE = 2740 AND NVL(idf.VALOR, '0') IN ('0', 'NA', 'N/A')  INNER JOIN " +
                        "   TRAMITES.TBINDICEDOCUMENTO idm ON id.CODDOCUMENTO = idm.CODDOCUMENTO AND id.CODTRAMITE = idm.CODTRAMITE AND id.CODINDICE = 3922 AND id.VALOR = 'S' AND idm.CODINDICE = 326 AND NVL(idm.VALOR, '0') " + (usuario.ID_GRUPO == 2 ? "NOT LIKE '%ENV%'" : "LIKE '%ENV%'") + " INNER JOIN " +
                        //"   TRAMITES.TBINDICEDOCUMENTO ide ON id.CODDOCUMENTO = ide.CODDOCUMENTO AND id.CODTRAMITE = ide.CODTRAMITE AND id.CODINDICE = 3922 AND id.VALOR = 'S' AND ide.CODINDICE = 2022 AND NVL(ide.VALOR, '0') = '" + (usuario.ID_GRUPO == 2 ? "00" : "10") + "' INNER JOIN " +
                        "   TRAMITES.TBINDICEDOCUMENTO ida ON id.CODDOCUMENTO = ida.CODDOCUMENTO AND id.CODTRAMITE = ida.CODTRAMITE AND ida.CODINDICE IN(320, 321, 323, 324, 325, 327, 381, 3922, 3940) LEFT OUTER JOIN" +
                        "   TRAMITES.RADICADO_DOCUMENTO rd ON id.CODTRAMITE = rd.CODTRAMITE AND id.CODDOCUMENTO = rd.CODDOCUMENTO " +
                        "ORDER BY ida.CODTRAMITE, ida.CODDOCUMENTO, ida.CODINDICE";

                var documentosFacturacion = dbSIM.Database.SqlQuery<IndicesDocumento>(sql).ToList();

                foreach (var registro in documentosFacturacion)
                {
                    string documentoRegistro = registro.CODTRAMITE.ToString() + "|" + registro.CODDOCUMENTO.ToString();

                    if (documentoRegistro != documentoActual)
                    {
                        if (registroFacturacion != null)
                        {
                            pendientesFacturacion.Add(registroFacturacion);
                        }

                        registroFacturacion = new RegistroFacturacion();
                        registroFacturacion.CODTRAMITE = Convert.ToInt32(registro.CODTRAMITE);
                        registroFacturacion.CODDOCUMENTO = Convert.ToInt32(registro.CODDOCUMENTO);
                        registroFacturacion.FECHARADICADO = registro.D_RADICADO;

                        documentoActual = registroFacturacion.CODTRAMITE.ToString() + "|" + registroFacturacion.CODDOCUMENTO.ToString();
                    }

                    switch (registro.CODINDICE)
                    {
                        /*case 320:
                            registroFacturacion.PARA = registro.VALOR;
                            break;*/
                        case 321:
                            registroFacturacion.ASUNTO = registro.VALOR;
                            break;
                        case 323:
                            registroFacturacion.NIT_SOLICITANTE = registro.VALOR;
                            break;
                        case 324:
                            registroFacturacion.DIRECCION = registro.VALOR;
                            break;
                        case 325:
                            registroFacturacion.TELEFONO = registro.VALOR;
                            break;
                        case 327:
                            registroFacturacion.CM = registro.VALOR;
                            break;
                        case 381:
                            registroFacturacion.RADICADO = registro.VALOR;
                            break;
                        case 3940:
                            registroFacturacion.TIPO_APROVECHAMIENTO_FORESTAL = registro.VALOR;
                            break;
                    }
                }

                if (registroFacturacion != null)
                {
                    pendientesFacturacion.Add(registroFacturacion);
                }


                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = pendientesFacturacion.Count();
                resultado.datos = pendientesFacturacion;

                return resultado;
            }
        }

        [Authorize(Roles = "VFACTURACION")]
        [HttpGet, ActionName("RegistrarFactura")]
        public void GetRegistrarFactura(decimal Tramite, decimal Documento, string Factura)
        {
            var indiceFacturaDocumento = dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODTRAMITE == Tramite && id.CODDOCUMENTO == Documento && id.CODINDICE == 2740).FirstOrDefault();

            if (indiceFacturaDocumento == null)
            {
                indiceFacturaDocumento = new TBINDICEDOCUMENTO() { CODTRAMITE = Tramite, CODDOCUMENTO = Documento, CODSERIE = 62, CODINDICE = 2740, VALOR = Factura, FECHAREGISTRO = DateTime.Now };

                dbSIM.Entry(indiceFacturaDocumento).State = EntityState.Added;
            }
            else
            {
                indiceFacturaDocumento.VALOR = Factura;

                dbSIM.Entry(indiceFacturaDocumento).State = EntityState.Modified;
            }

            dbSIM.SaveChanges();
        }
    }
}
