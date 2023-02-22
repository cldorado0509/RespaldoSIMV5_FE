using ModernHttpClient;
using Newtonsoft.Json;
using SIM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;
using SIM.Areas.Models;
using System.Data.Entity;

namespace SIM.Areas.Pagos.Controllers
{
    public class PagosEnLineaApiController : ApiController
    {
        public struct datosRespuesta
        {
            public string respuesta; // OK, Error
            public string detalle;
        }

        public class datosPago
        {
            public decimal REFERENCIA { get; set; }
            public string DATA { get; set; }
            public string FIXED_HASH { get; set; }
        }

        public struct Concepto
        {
            public string codigoConceptoFacturacion { get; set; }
            public string codigoConceptoRecaudo { get; set; }
            public decimal valor { get; set; }
        }

        public struct ReciboReferencia
        {
            public int codigoTercero { get; set; }
            public string centroCostos { get; set; }
            public string referencia { get; set; }
            public string descripcion { get; set; }
            public string tipoFactura { get; set; }
            public List<Concepto> infoSicofList { get; set; }
        }

        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema General y Seguridad
        /// </summary>
        EntitiesSIMOracle dbSICOF = new EntitiesSIMOracle();

        // GET api/<controller>
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet]
        [ActionName("PagarReferencia")]
        public datosRespuesta GetPagarReferencia(int tipoDocumento, decimal referencia)
        {
            try
            {
                //datosPago datos = ((DBContext)dbSICOF).Database.SqlQuery<datosPago>("SELECT REFERENCIA, DATA, FIXED_HASH FROM RECAUDO.TB_RECAUDO WHERE DATA = '" + (tipoDocumento == 1 ? "SIM" : "SICOF") + "' AND REFERENCIA = '" + referencia.ToString() + "'").FirstOrDefault();
                datosPago datos = ((DbContext)dbSICOF).Database.SqlQuery<datosPago>("SELECT REFERENCIA, DATA, FIXED_HASH FROM RECAUDO.TB_RECAUDO WHERE DATA = '" + (tipoDocumento == 1 ? "SIM" : "SICOF") + "' AND REFERENCIA = '" + referencia.ToString() + "'").FirstOrDefault();

                if (datos == null)
                {
                    if (tipoDocumento == 2) // SICOF
                    {
                        var task = RegistrarFactura(referencia);
                        task.Wait();

                        //datos = ((DBContext)dbSICOF).Database.SqlQuery<datosPago>("SELECT REFERENCIA, DATA, FIXED_HASH FROM RECAUDO.TB_RECAUDO WHERE DATA = '" + (tipoDocumento == 1 ? "SIM" : "SICOF") + "' AND REFERENCIA = '" + referencia.ToString() + "'").FirstOrDefault();
                        datos = ((DbContext)dbSICOF).Database.SqlQuery<datosPago>("SELECT REFERENCIA, DATA, FIXED_HASH FROM RECAUDO.TB_RECAUDO WHERE DATA = '" + (tipoDocumento == 1 ? "SIM" : "SICOF") + "' AND REFERENCIA = '" + referencia.ToString() + "'").FirstOrDefault();

                        if (datos == null)
                        {
                            return new datosRespuesta { respuesta = "Error", detalle = "El Pago No Está Disponible." };
                        }
                        else
                        {
                            return new datosRespuesta { respuesta = "OK", detalle = "//pagosenlinea.metropol.gov.co:8080/payment/" + datos.FIXED_HASH + ";url=https:,,www.metropol.gov.co,area,Paginas,pagos-en-linea.aspx" };
                        }
                    }
                    else
                    {
                        return new datosRespuesta { respuesta = "Error", detalle = "El Pago No Está Disponible." };
                    }
                }
                else
                    return new datosRespuesta { respuesta = "OK", detalle = "//pagosenlinea.metropol.gov.co:8080/payment/" + datos.FIXED_HASH + ";url=https:,,www.metropol.gov.co,area,Paginas,pagos-en-linea.aspx" };
                // "//pagosenlinea.metropol.gov.co:8080/payment/" + data + ";url=https:,,www.metropol.gov.co,area,Paginas,pagos-en-linea.aspx"
            }
            catch (Exception error)
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Error (PagosEnLineaApiController -> GetPagarReferencia(" + tipoDocumento + ", " + referencia.ToString() + ")\r\n" + Utilidades.LogErrores.ObtenerError(error));
                return new datosRespuesta { respuesta = "Error", detalle = "El Pago No Está Disponible. Error Interno" };
            }
        }

        public async Task RegistrarFactura(decimal referencia)
        {
            string responJsonText;
            decimal valor;

            Uri requestUri = new Uri("https://pagosenlinea.metropol.gov.co:8080/api/pago/factura/deep-search?referencia=" + referencia.ToString());

            using (var clientWS = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response;

                try
                {
                    //var timeoutToken = new CancellationTokenSource(8000).Token;
                    //response = await clientWS.GetAsync(requestUri, timeoutToken);
                    response = await clientWS.GetAsync(requestUri).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        responJsonText = await response.Content.ReadAsStringAsync();

                        ReciboReferencia datosFactura = JsonConvert.DeserializeObject<ReciboReferencia>(responJsonText);

                        if (datosFactura.codigoTercero != 0)
                        {
                            valor = 0;

                            foreach (Concepto concepto in datosFactura.infoSicofList)
                            {
                                valor += concepto.valor;
                            }

                            requestUri = new Uri("https://pagosenlinea.metropol.gov.co:8080/api/pago/factura/registrar");

                            var content = new StringContent(responJsonText, Encoding.UTF8, "application/json");

                            response = await clientWS.PostAsync(requestUri, content);
                        }
                    }
                }
                catch (Exception error)
                {
                    return;
                }
            }
        }
    }
}
