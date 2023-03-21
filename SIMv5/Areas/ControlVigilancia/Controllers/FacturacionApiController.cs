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

namespace SIM.Areas.ControlVigilancia.Controllers
{
    public class FacturacionApiController : ApiController
    {
        public class RegistroFacturacion
        {
            public int CODTRAMITE { get; set; }
            public int CODDOCUMENTO { get; set; }
            public string PARA { get; set; }
            public string ASUNTO { get; set; }
            public string NIT_SOLICITANTE { get; set; }
            public string DIRECCION { get; set; }
            public string TELEFONO { get; set; }
            public string CM { get; set; }
            public string RADICADO { get; set; }
            public string TIPO_APROVECHAMIENTO_FORESTAL { get; set; }
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

        [Authorize]
        [HttpGet, ActionName("ConsultaPendientesFacturacion")]
        public datosConsulta GetConsultaPendientesFacturacion()
        {
            string documentoActual = "";
            RegistroFacturacion registroFacturacion = null;
            List<RegistroFacturacion> pendientesFacturacion = new List<RegistroFacturacion>();

            string sql = "SELECT ida.* " +
                    "FROM TRAMITES.TBINDICEDOCUMENTO id INNER JOIN " +
                    "   TRAMITES.TBINDICEDOCUMENTO idf ON id.CODDOCUMENTO = idf.CODDOCUMENTO AND id.CODTRAMITE = idf.CODTRAMITE AND id.CODINDICE = 3922 AND id.VALOR = 'S' AND idf.CODINDICE = 2740 AND NVL(idf.VALOR, '0') IN ('0', 'NA', 'N/A') INNER JOIN " +
                    "   TRAMITES.TBINDICEDOCUMENTO ida ON id.CODDOCUMENTO = ida.CODDOCUMENTO AND id.CODTRAMITE = ida.CODTRAMITE AND ida.CODINDICE IN(320, 321, 323, 324, 325, 327, 381, 3922, 3940) " +
                    "ORDER BY ida.CODTRAMITE, ida.CODDOCUMENTO, ida.CODINDICE";

            var documentosFacturacion = dbSIM.Database.SqlQuery<TBINDICEDOCUMENTO>(sql).ToList();

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

                    documentoActual = registroFacturacion.CODTRAMITE.ToString() + "|" + registroFacturacion.CODDOCUMENTO.ToString();
                }

                switch (registro.CODINDICE)
                {
                    case 320:
                        registroFacturacion.PARA = registro.VALOR;
                        break;
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

        [Authorize]
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
