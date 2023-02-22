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
using SIM.Areas.Models;
using System.IO;
using System.Net.Http.Headers;
using System.Data.Entity;
using System.Transactions;
using System.Xml.Linq;
using System.Drawing.Imaging;
using System.Data.Entity.SqlServer;
using System.Reflection;
using SIM.Utilidades;
using System.Security.Claims;

namespace SIM.Areas.Tramites.Controllers
{
    /// <summary>
    /// Controlador RadicadorApi: Operaciones para Generar Radicados e imprimir etiquetas. También suministra los datos de serie, subserie, unidad documental y el documento asociado al radicado.
    /// </summary>
    public partial class RadicadorUDApiController : ApiController
    {
        /// <summary>
        /// Estructura de determina el tipo de radicado que retornará la invocación del radicador.
        /// </summary>
        public struct datosRadicacion
        {
            public string tipoRetorno;
            public string formatoRetorno;
            public int unidadDocumental;
        }

        /// <summary>
        /// Estructura con la configuración para la construcción de los docuemntos asociados.
        /// </summary>
        public struct datosRespuesta
        {
            public string tipoRespuesta; // OK, Error
            public string detalleRespuesta;
            public string id;
        }

        [HttpGet, ActionName("Radicar")]
        public object GetRadicar(int idUnidadDocumental)
        {
            return GetRadicar(idUnidadDocumental, "id", null);
        }

        [HttpGet, ActionName("Radicar")]
        public object GetRadicar(int idUnidadDocumental, string tipoRetorno)
        {
            return GetRadicar(idUnidadDocumental, tipoRetorno, null);
        }

        // tipoRetorno: key, id (key: identity del radicado, id: identificador radicado)
        [HttpGet, ActionName("Radicar")]
        public object GetRadicar(int idUnidadDocumental, string tipoRetorno, string tipoRadicado)
        {
            datosRadicacion radicacion = new datosRadicacion();

            radicacion.tipoRetorno = tipoRetorno;
            radicacion.formatoRetorno = "";
            radicacion.unidadDocumental = idUnidadDocumental;

            return PostRadicar(radicacion, tipoRadicado);
        }

        [HttpGet, ActionName("RadicarFuncionario")]
        public object GetRadicarFuncionario(int idUnidadDocumental, string tipoRetorno, string claveFuncionario)
        {
            datosRadicacion radicacion = new datosRadicacion();

            radicacion.tipoRetorno = tipoRetorno;
            radicacion.formatoRetorno = "";
            radicacion.unidadDocumental = idUnidadDocumental;

            return PostRadicarFuncionario(radicacion, null, claveFuncionario);
        }

        // tipoRetorno: key, id, et (key: identity del radicado, id: identificador radicado, et: etiqueta)
        // formatoRetorno: pdf, bmp, jpg, png (formato de la etiqueta de retorno, dado el caso que tipoRetorno sea et
        [HttpPost, ActionName("Radicar")]
        public object PostRadicar(datosRadicacion datosRadicacion, string tipoRadicado)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idUsuario;
            DateTime fechaCreacion;
            datosRespuesta resultado;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                // Error, el usuario logueado no tiene un tercero asociado y por lo tanto no podría registrarse el campo ID_TERCERO
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El usuario logueado no tiene un tercero asociado.", id = "" };
                return resultado;
            }

            fechaCreacion = DateTime.Now;
            Radicador radicador = new Radicador();
            DatosRadicado radicadoGenerado = radicador.GenerarRadicado(datosRadicacion.unidadDocumental, idUsuario, fechaCreacion);

            return radicadoGenerado;
        }

        // tipoRetorno: key, id, et (key: identity del radicado, id: identificador radicado, et: etiqueta)
        // formatoRetorno: pdf, bmp, jpg, png (formato de la etiqueta de retorno, dado el caso que tipoRetorno sea et
        [HttpPost, ActionName("RadicarFuncionario")]
        public object PostRadicarFuncionario(datosRadicacion datosRadicacion, string tipoRadicado, string claveFuncionario)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idUsuario;
            DateTime fechaCreacion;
            datosRespuesta resultado;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                // Error, el usuario logueado no tiene un tercero asociado y por lo tanto no podría registrarse el campo ID_TERCERO
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El usuario logueado no tiene un tercero asociado.", id = "" };
                return resultado;
            }

            fechaCreacion = DateTime.Now;
            Radicador radicador = new Radicador();
            DatosRadicado radicadoGenerado = radicador.GenerarRadicado(datosRadicacion.unidadDocumental, idUsuario, fechaCreacion, claveFuncionario);

            return radicadoGenerado;
        }
    }
}