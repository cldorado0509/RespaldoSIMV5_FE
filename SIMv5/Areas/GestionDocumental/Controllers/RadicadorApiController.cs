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
using SIM.Data.Documental;

namespace SIM.Areas.General.Controllers
{
    /// <summary>
    /// Controlador RadicadorApi: Operaciones para Generar Radicados e imprimir etiquetas. También suministra los datos de serie, subserie, unidad documental y el documento asociado al radicado.
    /// </summary>
    public partial class RadicadorApiController : ApiController
    {
        /// <summary>
        /// Estructura con los datos que se le pueden suministrar a la etiqueta al ser impresa.
        /// </summary>
        public struct DATOSETIQUETA
        {
            public string CB;
            public string Texto1;
            public string Texto2;
            public string Texto3;
            public string Texto4;
            public string Texto5;
            public string Texto6;
            public string Texto7;
            public string Texto8;
            public string Texto9;
            public string Texto10;
        }

        /// <summary>
        /// Estructura de configuración de la radicación de una unidad documental. Esta compuesta por la estructura que define el documento asociado (si es el caso), el texto que construye el identificador (si es el caso), la estructura del código de barras y los campos que componen la etiqueta.
        /// </summary>
        public struct CONFIGURACION
        {
            public DOCUMENTOASOCIADO DocumentoAsociado;
            //public bool EsIdentificadorTextoIdentificador;
            public Dictionary<string, CAMPO> textoIdentificador;
            public string formatoTextoIdentificador;
            public Dictionary<string, CAMPO> CB;
            public Dictionary<string, CAMPO> Etiqueta;
            public Dictionary<string, CAMPO> Texto;
        }

        /// <summary>
        /// Estructura de configuración de visualización y consulta del documento asociado a la radicación de la unidad documental.
        /// </summary>
        public struct DOCUMENTOASOCIADO
        {
            public string Nombre;
            public string Funcion;
            //public bool EsIdentificador;
            public int TipoConsulta; // 0: No tiene Documento Asociado, 1: Lookup, 2: PopupGrid, 3: Campos Digitados
            public string PlaceHolder;
            public string Titulo;
            public ArrayList Campos; // Utilizado únicamente cuando TipoConsulta es 3
            public string OrdenadoPor;
            public ArrayList ColumnasCombo; // Solo Aplica para PopupGrid ([Nombre Columna]%[Titulo]%[Ancho]%[Visible S/N], ...)
            //public string ColumnasVisualizar; // Solo Aplica para ComboBox Extendido ([Nombre Columna], ...)
            //public string ColumnasValor; // Solo Aplica para ComboBox Extendido ([Nombre Columna]&[Formato], ...)
        }

        /// <summary>
        /// Estructura de configuración de la visualización de los campos asociados al documento asociado
        /// </summary>
        public struct CAMPOTEXTO
        {
            public string Nombre;
            public string PlaceHolder;
            public string Titulo;
            public int CaracteresMax;
        }

        public struct COLUMNASCOMBO
        {
            public string Nombre;
            public string Titulo;
            public string Ancho;
            public string TipoDato;
            public bool Visible;
        }

        /// <summary>
        /// Estructura de configuración de los campos utilizados en otras estructuras. Se definen las características del campo para ser formateado y utilizado para construir otros campos.
        /// </summary>
        public struct CAMPO
        {
            public string Nombre;
            public int Longitud; // -1 si no tiene límite
            public string Tipo; // N Numero, S String
            public string Formato; // Solo Aplica para Numero
            public string Alineacion; // I Izquierda, D Derecha - Solo Aplica para String
            public string Columna;
            public Dictionary<string, CAMPO> SUBCAMPOS;
        }

        /// <summary>
        /// Estructura de determina el tipo de radicado que retornará la invocación del radicador.
        /// </summary>
        public struct datosRadicacion
        {
            public string tipoRetorno;
            public string formatoRetorno;
            public DATOSRADICADO datosRadicado;
        }

        /// <summary>
        /// Estructura con los datos necesarios para realizar la radicación.
        /// </summary>
        public struct DATOSRADICADO
        {
            public string CB;
            public int serie;
            public int subSerie;
            public int unidadDocumental;
            public int tipoExpediente;
            public string tipoEtiqueta;
            public int tipoAnexo;
            public string ubicacion;
            public string consecutivo;
            public int documentoAsociado;
            public DOCUMENTOASOCIADOS[] documentoAsociadoTextos;
        }

        /// <summary>
        /// Estructura con los datos suministrados en los documentos asociados cuando éstos son digitados por el usuario.
        /// </summary>
        public struct DOCUMENTOASOCIADOS
        {
            public string nombre;
            public string texto;
        }

        /// <summary>
        /// Estructura con los datos suministrados en los documentos asociados cuando éstos son digitados por el usuario.
        /// </summary>
        public struct datosDocumentosAsociados
        {
            public string nombre;
            public object valor;
        }

        /// <summary>
        /// Estructura que almacena el resultado de una consulta, entregando los datos y número de registros en total.
        /// </summary>
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        /// <summary>
        /// Estructura con la configuración para la construcción de los docuemntos asociados.
        /// </summary>
        public struct datosRespuestaDocAsociado
        {
            public string tipoRespuesta; // OK, Error
            public string detalleRespuesta;
            public CONFIGURACION configuracion;
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

        /// <summary>
        /// Variable de Contexto de Base de Datos.
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// Lista las Series disponibles que hayan sido configuradas para generar radicados.
        /// </summary>
        /// <returns>Registros de Series</returns>
        // tipoRadicador: 1: Unidades Documentales Simples, 2: Unidades Documentales Complejas
        [HttpGet, ActionName("Serie")]
        public datosConsulta GetSerie(int? tipoRadicador)
        {
            if (tipoRadicador == null || tipoRadicador == 1)
            {
                var model = (from serie in dbSIM.SERIE
                             join subSerie in dbSIM.SUBSERIE on serie.ID_SERIE equals subSerie.ID_SERIE
                             join unidadDocumental in dbSIM.UNIDAD_DOCUMENTAL on subSerie.ID_SUBSERIE equals unidadDocumental.ID_SUBSERIE
                             join radicadoUnidadDocumental in dbSIM.RADICADO_UNIDADDOCUMENTAL on unidadDocumental.ID_UNIDADDOCUMENTAL equals radicadoUnidadDocumental.ID_UNIDADDOCUMENTAL
                             where serie.D_FIN == null && unidadDocumental.S_TIPO == "S"
                             orderby serie.S_NOMBRE
                             select new
                             {
                                 serie.ID_SERIE,
                                 serie.S_NOMBRE
                             }).Distinct();

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = model.Count();
                resultado.datos = model.ToList();

                return resultado;
            }
            else
            {
                var model = (from serie in dbSIM.SERIE
                             join subSerie in dbSIM.SUBSERIE on serie.ID_SERIE equals subSerie.ID_SERIE
                             join unidadDocumental in dbSIM.UNIDAD_DOCUMENTAL on subSerie.ID_SUBSERIE equals unidadDocumental.ID_SUBSERIE
                             join radicadoUnidadDocumental in dbSIM.RADICADO_UNIDADDOCUMENTAL on unidadDocumental.ID_UNIDADDOCUMENTAL equals radicadoUnidadDocumental.ID_UNIDADDOCUMENTAL
                             where serie.D_FIN == null && unidadDocumental.S_TIPO == "C"
                             orderby serie.S_NOMBRE
                             select new
                             {
                                 serie.ID_SERIE,
                                 serie.S_NOMBRE
                             }).Distinct();

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = model.Count();
                resultado.datos = model.ToList();

                return resultado;
            }
        }

        /// <summary>
        /// Lista las Subseries disponibles que hayan sido configuradas para generar radicados y que dependan de una serie.
        /// </summary>
        /// <param name="idSerie">Código de la Serie</param>
        /// <returns>Registros de Subseries</returns>
        [HttpGet, ActionName("SubSerie")]
        public datosConsulta GetSubSerie(int? idSerie, int? tipoRadicador)
        {
            if (tipoRadicador == null || tipoRadicador == 1)
            {
                var model = (from subSerie in dbSIM.SUBSERIE
                             join unidadDocumental in dbSIM.UNIDAD_DOCUMENTAL on subSerie.ID_SUBSERIE equals unidadDocumental.ID_SUBSERIE
                             join radicadoUnidadDocumental in dbSIM.RADICADO_UNIDADDOCUMENTAL on unidadDocumental.ID_UNIDADDOCUMENTAL equals radicadoUnidadDocumental.ID_UNIDADDOCUMENTAL
                             where subSerie.D_FIN == null && (idSerie == null || subSerie.ID_SERIE == idSerie) && unidadDocumental.S_TIPO == "S"
                             orderby subSerie.S_NOMBRE
                             select new
                             {
                                 subSerie.ID_SERIE,
                                 subSerie.ID_SUBSERIE,
                                 subSerie.S_NOMBRE
                             }).Distinct();

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = model.Count();
                resultado.datos = model.ToList();

                return resultado;
            }
            else
            {
                var model = (from subSerie in dbSIM.SUBSERIE
                             join unidadDocumental in dbSIM.UNIDAD_DOCUMENTAL on subSerie.ID_SUBSERIE equals unidadDocumental.ID_SUBSERIE
                             join radicadoUnidadDocumental in dbSIM.RADICADO_UNIDADDOCUMENTAL on unidadDocumental.ID_UNIDADDOCUMENTAL equals radicadoUnidadDocumental.ID_UNIDADDOCUMENTAL
                             where subSerie.D_FIN == null && (idSerie == null || subSerie.ID_SERIE == idSerie) && unidadDocumental.S_TIPO == "C"
                             orderby subSerie.S_NOMBRE
                             select new
                             {
                                 subSerie.ID_SERIE,
                                 subSerie.ID_SUBSERIE,
                                 subSerie.S_NOMBRE
                             }).Distinct();

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = model.Count();
                resultado.datos = model.ToList();

                return resultado;
            }
        }

        /// <summary>
        /// Lista las Unidades Documentales disponibles que hayan sido configuradas para generar radicados y que dependan de una Subserie.
        /// </summary>
        /// <param name="idSerie">Código de la Serie</param>
        /// <param name="idSubSerie">Código de la Subserie</param>
        /// <returns>Registros de Subseries</returns>
        [HttpGet, ActionName("UnidadDocumental")]
        public datosConsulta GetUnidadDocumental(int? idSerie, int? idSubSerie, int? tipoRadicador)
        {
            if (tipoRadicador == null || tipoRadicador == 1)
            {
                var model = (from unidadDocumental in dbSIM.UNIDAD_DOCUMENTAL
                             join radicadoUnidadDocumental in dbSIM.RADICADO_UNIDADDOCUMENTAL on unidadDocumental.ID_UNIDADDOCUMENTAL equals radicadoUnidadDocumental.ID_UNIDADDOCUMENTAL
                             where unidadDocumental.D_FIN == null && (idSubSerie == null || unidadDocumental.SUBSERIE.ID_SUBSERIE == idSubSerie) && (idSerie == null || unidadDocumental.SUBSERIE.ID_SERIE == idSerie) && unidadDocumental.S_TIPO == "S"
                             orderby unidadDocumental.S_NOMBRE
                             select new
                             {
                                 unidadDocumental.SUBSERIE.ID_SERIE,
                                 unidadDocumental.ID_SUBSERIE,
                                 unidadDocumental.ID_UNIDADDOCUMENTAL,
                                 unidadDocumental.S_NOMBRE
                             }).Distinct();

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = model.Count();
                resultado.datos = model.ToList();

                return resultado;
            }
            else
            {
                var model = (from unidadDocumental in dbSIM.UNIDAD_DOCUMENTAL
                             join radicadoUnidadDocumental in dbSIM.RADICADO_UNIDADDOCUMENTAL on unidadDocumental.ID_UNIDADDOCUMENTAL equals radicadoUnidadDocumental.ID_UNIDADDOCUMENTAL
                             where unidadDocumental.D_FIN == null && (idSubSerie == null || unidadDocumental.SUBSERIE.ID_SUBSERIE == idSubSerie) && (idSerie == null || unidadDocumental.SUBSERIE.ID_SERIE == idSerie) && unidadDocumental.S_TIPO == "C"
                             orderby unidadDocumental.S_NOMBRE
                             select new
                             {
                                 unidadDocumental.SUBSERIE.ID_SERIE,
                                 unidadDocumental.ID_SUBSERIE,
                                 unidadDocumental.ID_UNIDADDOCUMENTAL,
                                 unidadDocumental.S_NOMBRE
                             }).Distinct();

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = model.Count();
                resultado.datos = model.ToList();

                return resultado;
            }
        }

        /// <summary>
        /// Lista los Tipos de Expediente disponibles
        /// </summary>
        /// <returns>Registros de Tipos de Expediente</returns>
        [HttpGet, ActionName("TipoExpediente")]
        public datosConsulta GetTipoExpediente()
        {
            var model = (from expediente in dbSIM.TIPO_EXPEDIENTE
                         orderby expediente.S_NOMBRE
                         select new
                         {
                             expediente.ID_TIPOEXPEDIENTE,
                             expediente.S_NOMBRE
                         }).Distinct();

            datosConsulta resultado = new datosConsulta();
            resultado.numRegistros = model.Count();
            resultado.datos = model.ToList();

            return resultado;
        }

        /*
        /// <summary>
        /// Obtiene la configuración para solicitar los datos del docuemnto asociado.
        /// </summary>
        /// <param name="idUnidadDocumental">Código de la Unidad Documental</param>
        /// <returns>Configuración de los docuemntos asociados</returns>
        [HttpGet, ActionName("DocumentoAsociado")]
        public datosRespuestaDocAsociado GetDocumentoAsociado(int idUnidadDocumental)
        {
            var model = (from tipoRadicado in dbSIM.RADICADO_UNIDADDOCUMENTAL
                         where tipoRadicado.TIPO_RADICADO.D_FIN == null && tipoRadicado.ID_UNIDADDOCUMENTAL == idUnidadDocumental
                         select new
                         {
                             tipoRadicado.TIPO_RADICADO.S_CONFIGURACION
                         }).FirstOrDefault();

            if (model == null)
            {
                return new datosRespuestaDocAsociado { tipoRespuesta = "Error", detalleRespuesta = "Unidad Documental SIN Tipo de Radicado relacionado" };
            }
            else
            {
                try
                {
                    CONFIGURACION configuracion = ObtenerConfiguracion(model.S_CONFIGURACION, 1);

                    return new datosRespuestaDocAsociado { tipoRespuesta = "OK", detalleRespuesta = "OK", configuracion = configuracion };
                }
                catch (Exception error)
                {
                    // TODO: Almacenar log del error generado
                    return new datosRespuestaDocAsociado { tipoRespuesta = "Error", detalleRespuesta = "Error cargando configuración del Tipo de Radicado" };
                }
            }
        }*/

        /// <summary>
        /// Obtiene la configuración para solicitar los datos del docuemnto asociado.
        /// </summary>
        /// <param name="idTipoExpediente">Código del Tipo de Expediente</param>
        /// <returns>Configuración de los documentos asociados</returns>
        [HttpGet, ActionName("DocumentoAsociado")]
        public datosRespuestaDocAsociado GetDocumentoAsociado(int idTipoExpediente)
        {
            var model = (from tipoRadicado in dbSIM.RADICADO_UNIDADDOCUMENTAL
                         where tipoRadicado.TIPO_RADICADO.D_FIN == null && tipoRadicado.ID_UNIDADDOCUMENTAL == idTipoExpediente
                         select new
                         {
                             tipoRadicado.TIPO_RADICADO.S_CONFIGURACION
                         }).FirstOrDefault();

            if (model == null)
            {
                return new datosRespuestaDocAsociado { tipoRespuesta = "Error", detalleRespuesta = "Tipo Expediente SIN Tipo de Radicado relacionado" };
            }
            else
            {
                try
                {
                    CONFIGURACION configuracion = ObtenerConfiguracion(model.S_CONFIGURACION, 1);

                    return new datosRespuestaDocAsociado { tipoRespuesta = "OK", detalleRespuesta = "OK", configuracion = configuracion };
                }
                catch (Exception error)
                {
                    // TODO: Almacenar log del error generado
                    return new datosRespuestaDocAsociado { tipoRespuesta = "Error", detalleRespuesta = "Error cargando configuración del Tipo de Radicado" };
                }
            }
        }

        /*
        [HttpGet, ActionName("RadicarAnexo")]
        public object GetRadicarAnexo(string codigoBarras, int tipoAnexo)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero;
            datosRespuesta resultado;
            int consecutivoAnexo;
            string codigoBarrasAnexo;
            int idRadicado;
            string consecutivoRadicado;
            string codigoAnexo;
            DateTime fechaCreacion = DateTime.Now;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            }
            else
            {
                // Error, el usuario logueado no tiene un tercero asociado y por lo tanto no podría registrarse el campo ID_TERCERO
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El usuario logueado no tiene un tercero asociado.", id = "" };
                return resultado;
            }

            codigoBarras = codigoBarras.Trim();
            var radicadoPadre = (from radicados in dbSIM.RADICADOS
                                 where radicados.S_ETIQUETA == codigoBarras && radicados.RADICADO_UNIDADDOCUMENTAL.UNIDAD_DOCUMENTAL.S_TIPO == "S"
                                 select new { radicados.ID_RADICADO, radicados.ID_RADICADOPADRE, radicados.S_IDENTIFICADOR, radicados.ID_RADICADOUNIDADDOCUMENTAL, radicados.ID_REGISTROREL }).FirstOrDefault();

            if (radicadoPadre == null)
            {
                // Error, el codigo de barras no existe
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El Código de Barras NO Existe o NO es una Unidad Documental Simple.", id = "" };
                return resultado;
            }
            else if (radicadoPadre.ID_RADICADOPADRE != null)
            {
                // Error, el codigo de barras no existe
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El Código de Barras corresponde a un Anexo. Por favor ingresar el código de barras de la Unidad Documental Simple.", id = "" };
                return resultado;
            }

            var radicadosAsociados = from radicados in dbSIM.RADICADOS
                                     where radicados.ID_RADICADOPADRE == radicadoPadre.ID_RADICADO
                                     select radicados.ID_RADICADO;

            var codigoTipoAnexo = (from tiposAnexo in dbSIM.TIPO_ANEXO
                                   where tiposAnexo.ID_TIPOANEXO == tipoAnexo
                                   select tiposAnexo.S_CODIGO).FirstOrDefault();

            consecutivoAnexo = (radicadosAsociados.Count() + 1);
            codigoBarrasAnexo = codigoBarras.Substring(0, codigoBarras.Length - 4) + Convert.ToInt32(codigoTipoAnexo).ToString("00") + consecutivoAnexo.ToString("00");

            RADICADOS radicado = new RADICADOS();
            radicado.ID_RADICADOPADRE = radicadoPadre.ID_RADICADO;
            radicado.D_CREACION = fechaCreacion;
            radicado.S_ETIQUETA = codigoBarrasAnexo;
            radicado.S_IDENTIFICADOR = radicadoPadre.S_IDENTIFICADOR;
            radicado.ID_TERCERO = idTercero;
            radicado.ID_RADICADOUNIDADDOCUMENTAL = radicadoPadre.ID_RADICADOUNIDADDOCUMENTAL;
            radicado.ID_REGISTROREL = radicadoPadre.ID_REGISTROREL;
            radicado.ID_TIPOANEXO = tipoAnexo;
            dbSIM.Entry(radicado).State = EntityState.Added;

            ANEXOS anexo = new ANEXOS();

            var documento = (from documentoRadicado in dbSIM.DOCUMENTOS
                            where documentoRadicado.ID_RADICADO == radicadoPadre.ID_RADICADO
                            select documentoRadicado.ID_DOCUMENTO).FirstOrDefault();

            if (documento == 0)
            {
                // Error, el registro del expediente no existe
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El Registro del Documento NO Existe.", id = "" };
                return resultado;
            }

            anexo.ID_DOCUMENTO = documento;
            anexo.ID_TIPOANEXO = tipoAnexo;
            anexo.ID_RADICADO = radicadoPadre.ID_RADICADO;
            dbSIM.Entry(anexo).State = EntityState.Added;

            dbSIM.SaveChanges();

            idRadicado = radicado.ID_RADICADO;
            consecutivoRadicado = radicado.S_IDENTIFICADOR;

            resultado = new datosRespuesta() { tipoRespuesta = "OK", detalleRespuesta = "" };

            resultado.id = idRadicado.ToString();

            return resultado;
        }*/

        private object RadicarAnexo(RADICADOS_ETIQUETAS radicadoPadre, int tipoAnexo, int idTercero, string ubicacion, string consecutivo)
        {
            datosRespuesta resultado;
            string consecutivoAnexo;
            string codigoBarrasAnexo;
            int idRadicado;
            string consecutivoRadicado;
            string codigoBarras;
            DateTime fechaCreacion = DateTime.Now;

            consecutivoAnexo = consecutivo;

            var radicadosAsociados = from radicados in dbSIM.RADICADOS_ETIQUETAS
                                     where radicados.ID_RADICADOPADRE == radicadoPadre.ID_RADICADO && radicados.S_TIPO == "A"
                                     select radicados.ID_RADICADO;

            codigoBarras = radicadoPadre.S_ETIQUETA;
            try
            {
                var codigoTipoAnexo = (from tiposAnexo in dbSIM.TIPO_ANEXO
                                       where tiposAnexo.ID_TIPOANEXO == tipoAnexo
                                       select tiposAnexo.S_CODIGO).FirstOrDefault();
            }
            catch (Exception error)
            {
                string s = error.Message;
            }

            //consecutivoAnexo = (radicadosAsociados.Count() + 1);
            codigoBarrasAnexo = codigoBarras + "02" + consecutivoAnexo.PadLeft(10, '0');

            var verificacionExistencia = (from radicadoCB in dbSIM.RADICADOS_ETIQUETAS
                                          where radicadoCB.S_ETIQUETA == codigoBarrasAnexo
                                          select radicadoCB).FirstOrDefault();

            if (verificacionExistencia != null)
            {
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "Etiqueta de Anexo Ya Existe.", id = "" };
                return resultado;
            }

            RADICADOS_ETIQUETAS radicado = new RADICADOS_ETIQUETAS();
            radicado.ID_RADICADOPADRE = radicadoPadre.ID_RADICADO;
            radicado.D_CREACION = fechaCreacion;
            radicado.S_ETIQUETA = codigoBarrasAnexo;
            radicado.S_IDENTIFICADOR = radicadoPadre.S_IDENTIFICADOR + "02" + consecutivoAnexo.PadLeft(10, '0');
            radicado.ID_TERCERO = idTercero;
            radicado.ID_RADICADOUNIDADDOCUMENTAL = radicadoPadre.ID_RADICADOUNIDADDOCUMENTAL;
            radicado.ID_REGISTROREL = radicadoPadre.ID_REGISTROREL;
            radicado.S_TIPO = "A";
            radicado.ID_TIPOANEXO = tipoAnexo;
            radicado.S_TEXTO = radicadoPadre.S_TEXTO;
            radicado.S_UBICACION = ubicacion.ToUpper().Trim();
            radicado.S_CONSECUTIVOTIPO = consecutivoAnexo;
            dbSIM.Entry(radicado).State = EntityState.Added;

            try {
                dbSIM.SaveChanges();
            }
            catch (Exception error)
            {
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "Error Registrando el Anexo." };

                return resultado;
            }

            idRadicado = radicado.ID_RADICADO;
            consecutivoRadicado = radicado.S_IDENTIFICADOR;

            resultado = new datosRespuesta() { tipoRespuesta = "OK", detalleRespuesta = "" };

            resultado.id = idRadicado.ToString();

            return resultado;
        }

        private object RadicarTomo(RADICADOS_ETIQUETAS radicadoPadre, int idTercero, string ubicacion, string consecutivo)
        {
            datosRespuesta resultado;
            string consecutivoTomo;
            string codigoBarrasTomo;
            int idRadicado;
            string consecutivoRadicado;
            string codigoBarras;
            DateTime fechaCreacion = DateTime.Now;

            consecutivoTomo = consecutivo;

            var radicadosAsociados = from radicados in dbSIM.RADICADOS_ETIQUETAS
                                     where radicados.ID_RADICADOPADRE == radicadoPadre.ID_RADICADO && radicados.S_TIPO == "T"
                                     select radicados.ID_RADICADO;

            codigoBarras = radicadoPadre.S_ETIQUETA;

            //consecutivoTomo = (radicadosAsociados.Count() + 1);
            codigoBarrasTomo = codigoBarras + "01" + consecutivoTomo.PadLeft(10, '0');

            var verificacionExistencia = (from radicadoCB in dbSIM.RADICADOS_ETIQUETAS
                                          where radicadoCB.S_ETIQUETA == codigoBarrasTomo
                                          select radicadoCB).FirstOrDefault();

            if (verificacionExistencia != null)
            {
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "Etiqueta de Tomo Ya Existe.", id = "" };
                return resultado;
            }

            RADICADOS_ETIQUETAS radicado = new RADICADOS_ETIQUETAS();
            radicado.ID_RADICADOPADRE = radicadoPadre.ID_RADICADO;
            radicado.D_CREACION = fechaCreacion;
            radicado.S_ETIQUETA = codigoBarrasTomo;
            radicado.S_IDENTIFICADOR = radicadoPadre.S_IDENTIFICADOR + "01" + consecutivoTomo.PadLeft(10, '0');
            radicado.ID_TERCERO = idTercero;
            radicado.ID_RADICADOUNIDADDOCUMENTAL = radicadoPadre.ID_RADICADOUNIDADDOCUMENTAL;
            radicado.ID_REGISTROREL = radicadoPadre.ID_REGISTROREL;
            radicado.S_TIPO = "T";
            radicado.S_TEXTO = radicadoPadre.S_TEXTO;
            radicado.S_UBICACION = ubicacion.ToUpper().Trim();
            //radicado.S_FOLIOS = folios;
            radicado.S_CONSECUTIVOTIPO = consecutivoTomo;
            dbSIM.Entry(radicado).State = EntityState.Added;

            try
            {
                dbSIM.SaveChanges();
            } catch (Exception error)
            {
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "Error Registrando el Tomo" };

                return resultado;
            }

            idRadicado = radicado.ID_RADICADO;
            consecutivoRadicado = radicado.S_IDENTIFICADOR;

            resultado = new datosRespuesta() { tipoRespuesta = "OK", detalleRespuesta = "" };

            resultado.id = idRadicado.ToString();

            return resultado;
        }

        /*[HttpGet, ActionName("RadicarTomo")]
        public object GetRadicarTomo(string codigoBarras)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero;
            datosRespuesta resultado;
            int consecutivoTomo;
            string codigoBarrasTomo;
            int idRadicado;
            string consecutivoRadicado;
            DateTime fechaCreacion = DateTime.Now;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            }
            else
            {
                // Error, el usuario logueado no tiene un tercero asociado y por lo tanto no podría registrarse el campo ID_TERCERO
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El usuario logueado no tiene un tercero asociado.", id = "" };
                return resultado;
            }

            codigoBarras = codigoBarras.Trim();
            var radicadoRelacionado = (from radicados in dbSIM.RADICADOS
                                 where radicados.S_ETIQUETA == codigoBarras && radicados.RADICADO_UNIDADDOCUMENTAL.UNIDAD_DOCUMENTAL.S_TIPO == "C"
                                 select new { radicados.ID_RADICADO, radicados.ID_RADICADOPADRE, radicados.S_IDENTIFICADOR, radicados.ID_RADICADOUNIDADDOCUMENTAL, radicados.ID_REGISTROREL, radicados.S_TEXTO }).FirstOrDefault();

            if (radicadoRelacionado == null)
            {
                // Error, el codigo de barras no existe
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El Código de Barras NO Existe o NO es una Unidad Documental Compleja.", id = "" };
                return resultado;
            }
            //else if (radicadoRelacionado.ID_RADICADOPADRE != null)
            //{
            //    resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El Código de Barras corresponde a un Tomo. Por favor ingresar el código de barras de la Unidad Documental Compleja.", id = "" };
            //    return resultado;
            //}

            var radicadosAsociados = from radicados in dbSIM.RADICADOS
                                     where radicados.S_IDENTIFICADOR == radicadoRelacionado.S_IDENTIFICADOR && radicados.S_TIPO == "T"
                                     select radicados.ID_RADICADO;

            consecutivoTomo = (radicadosAsociados.Count() + 1);
            // El código de barras termina con 01XX donde 01 es porque es Tomo y XX el consecutivo
            codigoBarrasTomo = codigoBarras.Substring(0, codigoBarras.Length - 4) + "01" + consecutivoTomo.ToString("00");

            RADICADOS radicado = new RADICADOS();
            //radicado.ID_RADICADOPADRE = radicadoRelacionado.ID_RADICADO;
            radicado.D_CREACION = fechaCreacion;
            radicado.S_ETIQUETA = codigoBarrasTomo;
            radicado.S_IDENTIFICADOR = radicadoRelacionado.S_IDENTIFICADOR;
            radicado.ID_TERCERO = idTercero;
            radicado.ID_RADICADOUNIDADDOCUMENTAL = radicadoRelacionado.ID_RADICADOUNIDADDOCUMENTAL;
            radicado.ID_REGISTROREL = radicadoRelacionado.ID_REGISTROREL;
            radicado.S_TEXTO = radicadoRelacionado.S_TEXTO;
            radicado.S_TIPO = "T";
            dbSIM.Entry(radicado).State = EntityState.Added;

            TOMOS tomo = new TOMOS();

            var expediente = (from expedienteRadicado in dbSIM.EXPEDIENTE
                              where expedienteRadicado.ID_RADICADO == radicadoRelacionado.ID_RADICADO
                              select expedienteRadicado.ID_EXPEDIENTE).FirstOrDefault();

            if (expediente == 0)
            {
                // Error, el registro del expediente no existe
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El Registro del Expediente NO Existe.", id = "" };
                return resultado;
            }

            tomo.ID_EXPEDIENTE = expediente;
            tomo.ID_RADICADO = radicadoRelacionado.ID_RADICADO;
            dbSIM.Entry(tomo).State = EntityState.Added;

            dbSIM.SaveChanges();

            idRadicado = radicado.ID_RADICADO;
            consecutivoRadicado = radicado.S_IDENTIFICADOR;

            resultado = new datosRespuesta() { tipoRespuesta = "OK", detalleRespuesta = "" };

            resultado.id = idRadicado.ToString();

            return resultado;
        }*/

        [HttpGet, ActionName("Radicar")]
        public object GetRadicar(int idUnidadDocumental)
        {
            return GetRadicar(idUnidadDocumental, "id");
        }

        // tipoRetorno: key, id (key: identity del radicado, id: identificador radicado)
        [HttpGet, ActionName("Radicar")]
        public object GetRadicar(int idUnidadDocumental, string tipoRetorno)
        {
            datosRadicacion radicacion = new datosRadicacion();

            radicacion.tipoRetorno = tipoRetorno;
            radicacion.formatoRetorno = "";
            radicacion.datosRadicado = new DATOSRADICADO();
            //radicacion.datosRadicado.serie = 0;
            //radicacion.datosRadicado.subSerie = 0;
            radicacion.datosRadicado.unidadDocumental = idUnidadDocumental;
            radicacion.datosRadicado.documentoAsociado = 0;
            radicacion.datosRadicado.documentoAsociadoTextos = null;


            return PostRadicar(radicacion);
        }

        // tipoRetorno: key, id, et (No aplica ya) (key: identity del radicado, id: identificador radicado, et: etiqueta (No aplica ya))
        // formatoRetorno: pdf, bmp, jpg, png (formato de la etiqueta de retorno, dado el caso que tipoRetorno sea et) (Ya no aplica porque no se devolverá etiqueta)
        //[HttpGet, ActionName("Radicar")]
        //public object GetRadicar(int idUnidadDocumental, string documentoAsociado, string tipoRetorno, string formatoRetorno)
        [HttpPost, ActionName("Radicar")]
        public object PostRadicar(datosRadicacion datosRadicacionUD)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero;
            int idRadicado;
            string consecutivoRadicado;
            string identificador = "";
            DateTime fechaCreacion;
            Dictionary<string, datosDocumentosAsociados> datosDocAsociados = null;
            RADICADOS_ETIQUETAS radicado;
            datosRespuesta resultado;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            }
            else
            {
                // Error, el usuario logueado no tiene un tercero asociado y por lo tanto no podría registrarse el campo ID_TERCERO
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El usuario logueado no tiene un tercero asociado.", id = "" };
                return resultado;
            }

            fechaCreacion = DateTime.Now;

            if (datosRadicacionUD.datosRadicado.CB != null && datosRadicacionUD.datosRadicado.CB.Trim() != "")
            {
                // Verificar que el CB no se haya generado aun
                var verificacionExistencia = (from radicadoCB in dbSIM.RADICADOS_ETIQUETAS
                                              where radicadoCB.S_ETIQUETA == datosRadicacionUD.datosRadicado.CB.Trim()
                                              //select new { radicadoCB.ID_RADICADO, radicadoCB.S_IDENTIFICADOR }).FirstOrDefault();
                                              select radicadoCB).FirstOrDefault();

                if (verificacionExistencia != null) // Ya existe el CB
                {
                    if (verificacionExistencia.ID_RADICADOPADRE != null)
                    {
                        verificacionExistencia = dbSIM.RADICADOS_ETIQUETAS.FirstOrDefault<RADICADOS_ETIQUETAS>(r => r.ID_RADICADO == verificacionExistencia.ID_RADICADOPADRE);

                        if (verificacionExistencia != null)
                        {
                            radicado = verificacionExistencia;
                            if (datosRadicacionUD.datosRadicado.tipoEtiqueta == "A")
                                return RadicarAnexo(radicado, datosRadicacionUD.datosRadicado.tipoAnexo, idTercero, datosRadicacionUD.datosRadicado.ubicacion, datosRadicacionUD.datosRadicado.consecutivo);
                            else if (datosRadicacionUD.datosRadicado.tipoEtiqueta == "T")
                                return RadicarTomo(radicado, idTercero, datosRadicacionUD.datosRadicado.ubicacion, datosRadicacionUD.datosRadicado.consecutivo);
                            else
                            {
                                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "Tipo de Etiqueta Inválida (Anexo, Tomo).", id = "" };
                                return resultado;
                            }
                        }
                        else
                        {
                            resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "Código de Barras Inválido, NO tiene un Expediente Asociado.", id = "" };
                            return resultado;
                        }
                    }
                    else
                    {
                        radicado = verificacionExistencia;
                        if (datosRadicacionUD.datosRadicado.tipoEtiqueta == "A")
                            return RadicarAnexo(radicado, datosRadicacionUD.datosRadicado.tipoAnexo, idTercero, datosRadicacionUD.datosRadicado.ubicacion, datosRadicacionUD.datosRadicado.consecutivo);
                        else if (datosRadicacionUD.datosRadicado.tipoEtiqueta == "T")
                            return RadicarTomo(radicado, idTercero, datosRadicacionUD.datosRadicado.ubicacion, datosRadicacionUD.datosRadicado.consecutivo);
                        else
                        {
                            resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "Tipo de Etiqueta Inválida (Anexo, Tomo).", id = "" };
                            return resultado;
                        }
                    }
                }
                else
                {
                    resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "Código de Barras Inválido, NO Existe.", id = "" };
                    return resultado;
                }
            }
            else
            {
                var modelRadicador = (from radicadoUD in dbSIM.RADICADO_UNIDADDOCUMENTAL
                                      join tipoExpediente in dbSIM.TIPO_EXPEDIENTE on radicadoUD.ID_UNIDADDOCUMENTAL equals tipoExpediente.ID_TIPOEXPEDIENTE
                                      //where radicadoUD.ID_UNIDADDOCUMENTAL == datosRadicacionUD.datosRadicado.unidadDocumental
                                      where radicadoUD.ID_UNIDADDOCUMENTAL == datosRadicacionUD.datosRadicado.tipoExpediente
                                      select new
                                      {
                                          radicadoUD.ID_RADICADOUNIDADDOCUMENTAL,
                                          radicadoUD.ID_TIPORADICADO,
                                          S_TIPORADICADO = radicadoUD.TIPO_RADICADO.S_NOMBRE,
                                          ID_SERIE = 0, //radicadoUD.UNIDAD_DOCUMENTAL.SUBSERIE.SERIE.ID_SERIE,
                                          S_SERIE = "", //radicadoUD.UNIDAD_DOCUMENTAL.SUBSERIE.SERIE.S_NOMBRE,
                                          ID_SUBSERIE = 0, //radicadoUD.UNIDAD_DOCUMENTAL.ID_SUBSERIE,
                                          S_SUBSERIE = "", //radicadoUD.UNIDAD_DOCUMENTAL.SUBSERIE.S_NOMBRE,
                                          ID_UNIDADDOCUMENTAL = 0, //radicadoUD.ID_UNIDADDOCUMENTAL,
                                          S_UNIDADDOCUMENTAL = "", //radicadoUD.UNIDAD_DOCUMENTAL.S_NOMBRE,
                                          radicadoUD.N_CONSECUTIVO,
                                          radicadoUD.TIPO_RADICADO.S_CONFIGURACION,
                                          S_TIPO = "", //radicadoUD.UNIDAD_DOCUMENTAL.S_TIPO,
                                          tipoExpediente.ID_TIPOEXPEDIENTE,
                                          S_TIPOEXPEDIENTE = tipoExpediente.S_NOMBRE
                                      }).FirstOrDefault();

                if (modelRadicador == null)
                {
                    // Error, la Unidad Documental no tiene un Tipo de Radicado configurado
                    resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "La Unidad Documental no tiene un Tipo de Radicado configurado.", id = "" };
                    return resultado;
                }
                else
                {
                    CONFIGURACION configuracion = ObtenerConfiguracion(modelRadicador.S_CONFIGURACION);
                    string CB = "";
                    string texto = "";
                    string valorCampo = "";

                    switch (configuracion.DocumentoAsociado.TipoConsulta) // 0: No tiene Documento Asociado, 1: Lookup, 2: PopupGrid, 3: Campos Digitados
                    {
                        case 1:
                        case 2:
                            if (datosRadicacionUD.datosRadicado.documentoAsociado != 0)
                            {
                                Type thisType = this.GetType();
                                MethodInfo theMethod = thisType.GetMethod("Datos" + configuracion.DocumentoAsociado.Funcion);
                                datosDocAsociados = (Dictionary<string, datosDocumentosAsociados>)theMethod.Invoke(this, new object[] { datosRadicacionUD.datosRadicado.documentoAsociado });
                                //identificador = datosRadicacionUD.datosRadicado.documentoAsociado.ToString();

                                foreach (KeyValuePair<string, CAMPO> entry in configuracion.textoIdentificador)
                                {
                                    CAMPO campo = entry.Value;

                                    if (datosDocAsociados[campo.Columna].valor != null && datosDocAsociados[campo.Columna].valor.ToString() != "")
                                    {
                                        identificador += Convert.ToInt64(datosDocAsociados[campo.Columna].valor).ToString(campo.Formato);
                                    }
                                    else
                                    {
                                        identificador += 0.ToString(campo.Formato);
                                    }
                                }
                            }
                            else
                            {
                                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "No se ha establecido un Documento Asociado al Radicado.", id = "" };
                                return resultado;
                            }
                            break;
                        case 3:
                            datosDocAsociados = new Dictionary<string, datosDocumentosAsociados>();

                            foreach (DOCUMENTOASOCIADOS doc in datosRadicacionUD.datosRadicado.documentoAsociadoTextos)
                            {
                                datosDocAsociados.Add(doc.nombre, new datosDocumentosAsociados() { nombre = doc.nombre, valor = doc.texto });
                            }

                            foreach (KeyValuePair<string, CAMPO> entry in configuracion.textoIdentificador)
                            {
                                CAMPO campo = entry.Value;

                                if (datosDocAsociados[campo.Columna].valor != null && datosDocAsociados[campo.Columna].valor.ToString() != "")
                                {
                                    identificador += Convert.ToInt64(datosDocAsociados[campo.Columna].valor).ToString(campo.Formato);
                                }
                                else
                                {
                                    identificador += 0.ToString(campo.Formato);
                                }
                            }

                            break;
                    }
                    //using (TransactionScope trans = new TransactionScope())
                    //{
                    foreach (KeyValuePair<string, CAMPO> entry in configuracion.CB)
                    {
                        CAMPO campo = entry.Value;

                        if (campo.Nombre.Length >= 3 && campo.Nombre.Substring(0, 3) == "CTE")
                        {
                            if (campo.Formato.Trim() != "")
                                valorCampo = Convert.ToInt32(campo.Columna).ToString(campo.Formato);
                            else
                                valorCampo = (campo.Alineacion == "D" ? campo.Columna.PadLeft(campo.Longitud) : campo.Columna.PadRight(campo.Longitud));
                        }
                        else
                        {
                            switch (campo.Columna)
                            {
                                case "ID_TIPORADICADO":
                                    if (campo.Formato.Trim() != "")
                                        valorCampo = modelRadicador.ID_TIPORADICADO.ToString(campo.Formato);
                                    else
                                        valorCampo = (campo.Alineacion == "D" ? modelRadicador.ID_TIPORADICADO.ToString().PadLeft(campo.Longitud) : modelRadicador.ID_TIPORADICADO.ToString().PadRight(campo.Longitud));
                                    break;
                                case "ID_SERIE":
                                    if (campo.Formato.Trim() != "")
                                        valorCampo = modelRadicador.ID_SERIE.ToString(campo.Formato);
                                    else
                                        valorCampo = (campo.Alineacion == "D" ? modelRadicador.ID_SERIE.ToString().PadLeft(campo.Longitud) : modelRadicador.ID_SERIE.ToString().PadRight(campo.Longitud));
                                    break;
                                case "ID_SUBSERIE":
                                    if (campo.Formato.Trim() != "")
                                        valorCampo = modelRadicador.ID_SUBSERIE.ToString(campo.Formato);
                                    else
                                        valorCampo = (campo.Alineacion == "D" ? modelRadicador.ID_SUBSERIE.ToString().PadLeft(campo.Longitud) : modelRadicador.ID_SUBSERIE.ToString().PadRight(campo.Longitud));
                                    break;
                                case "ID_UNIDADDOCUMENTAL":
                                    if (campo.Formato.Trim() != "")
                                        valorCampo = modelRadicador.ID_UNIDADDOCUMENTAL.ToString(campo.Formato);
                                    else
                                        valorCampo = (campo.Alineacion == "D" ? modelRadicador.ID_UNIDADDOCUMENTAL.ToString().PadLeft(campo.Longitud) : modelRadicador.ID_UNIDADDOCUMENTAL.ToString().PadRight(campo.Longitud));
                                    break;
                                case "ID_TIPOEXPEDIENTE":
                                    if (campo.Formato.Trim() != "")
                                        valorCampo = modelRadicador.ID_TIPOEXPEDIENTE.ToString(campo.Formato);
                                    else
                                        valorCampo = (campo.Alineacion == "D" ? modelRadicador.ID_TIPOEXPEDIENTE.ToString().PadLeft(campo.Longitud) : modelRadicador.ID_TIPOEXPEDIENTE.ToString().PadRight(campo.Longitud));
                                    break;
                                case "S_TIPOEXPEDIENTE":
                                    valorCampo = modelRadicador.S_TIPOEXPEDIENTE.Trim();
                                    break;
                                case "S_IDENTIFICADOR":
                                    if (configuracion.DocumentoAsociado.TipoConsulta == 0)
                                    {
                                        if (campo.Formato.Trim() != "")
                                            valorCampo = (modelRadicador.N_CONSECUTIVO == null ? 1 : (int)modelRadicador.N_CONSECUTIVO).ToString(campo.Formato);
                                        else
                                            valorCampo = (campo.Alineacion == "D" ? (modelRadicador.N_CONSECUTIVO == null ? 1 : (int)modelRadicador.N_CONSECUTIVO).ToString().PadLeft(campo.Longitud) : (modelRadicador.N_CONSECUTIVO == null ? 1 : (int)modelRadicador.N_CONSECUTIVO).ToString().PadRight(campo.Longitud));
                                    }
                                    else
                                    {
                                        if (campo.Formato.Trim() != "")
                                            valorCampo = Convert.ToInt64(identificador).ToString(campo.Formato);
                                        else
                                            valorCampo = (campo.Alineacion == "D" ? identificador.PadLeft(campo.Longitud) : identificador.PadRight(campo.Longitud));
                                    }
                                    break;
                                case "D_CREACION":
                                    valorCampo = fechaCreacion.ToString(campo.Formato);
                                    break;
                                /*case "TIPO_ANEXO_TOMO":
                                    //int tipoAnexoTomo = 0;
                                    //if (datosRadicacionUD.datosRadicado.tipoEtiqueta == "T")
                                    //    tipoAnexoTomo = 1;
                                    //else if (datosRadicacionUD.datosRadicado.tipoEtiqueta == "A")
                                    //    tipoAnexoTomo = 2;

                                    //if (campo.Formato.Trim() != "")
                                    //    valorCampo = tipoAnexoTomo.ToString(campo.Formato);
                                    //else
                                    //    valorCampo = (campo.Alineacion == "D" ? tipoAnexoTomo.ToString().PadLeft(campo.Longitud) : tipoAnexoTomo.ToString().PadRight(campo.Longitud));

                                    tipoAnexoTomo = true;
                                    formatoTipoAnexoTomo = campo.Formato;

                                    if (campo.Formato.Trim() != "")
                                            valorCampo = 0.ToString(campo.Formato);

                                    break;
                                case "CONSECUTIVO_ANEXO_TOMO": // TODO: Asignación de consecutivo cuando hay anexo. Por ahora no se utiliza, en el método RadicarAnexo se genera de forma fija en los dos ultimos caracteres del CB.
                                    if (campo.Formato.Trim() != "")
                                        valorCampo = 0.ToString(campo.Formato);

                                    consecutivoAnexoTomo = true;
                                    formatoConsecutivoAnexoTomo = campo.Formato;

                                    break;*/
                                default:
                                    if (campo.Formato.Trim() != "")
                                        valorCampo = Convert.ToDecimal("0" + datosDocAsociados[campo.Columna].valor).ToString(campo.Formato);
                                    else
                                        valorCampo = (campo.Alineacion == "D" ? datosDocAsociados[campo.Columna].valor.ToString().PadLeft(campo.Longitud) : datosDocAsociados[campo.Columna].valor.ToString().PadRight(campo.Longitud));
                                    break;
                            }
                        }
                        CB += valorCampo;
                    }

                    // DATOSTEXTO que almacena el texto a través del cual se podrá buscar el registro
                    foreach (KeyValuePair<string, CAMPO> entry in configuracion.Texto)
                    {
                        CAMPO campo = entry.Value;

                        if (campo.Nombre.Length >= 3 && campo.Nombre.Substring(0, 3) == "CTE")
                        {
                            if (campo.Formato != "")
                                valorCampo = Convert.ToInt32(campo.Columna).ToString(campo.Formato);
                            else
                                valorCampo = (campo.Alineacion == "D" ? campo.Columna.PadLeft(campo.Longitud) : campo.Columna.PadRight(campo.Longitud));
                        }
                        else
                        {
                            switch (campo.Columna)
                            {
                                case "ID_TIPORADICADO":
                                    if (campo.Formato.Trim() != "")
                                        valorCampo = modelRadicador.ID_TIPORADICADO.ToString(campo.Formato);
                                    else
                                        valorCampo = (campo.Alineacion == "D" ? modelRadicador.ID_TIPORADICADO.ToString().PadLeft(campo.Longitud) : modelRadicador.ID_TIPORADICADO.ToString().PadRight(campo.Longitud));
                                    break;
                                case "ID_SERIE":
                                    if (campo.Formato.Trim() != "")
                                        valorCampo = modelRadicador.ID_SERIE.ToString(campo.Formato);
                                    else
                                        valorCampo = (campo.Alineacion == "D" ? modelRadicador.ID_SERIE.ToString().PadLeft(campo.Longitud) : modelRadicador.ID_SERIE.ToString().PadRight(campo.Longitud));
                                    break;
                                case "ID_SUBSERIE":
                                    if (campo.Formato.Trim() != "")
                                        valorCampo = modelRadicador.ID_SUBSERIE.ToString(campo.Formato);
                                    else
                                        valorCampo = (campo.Alineacion == "D" ? modelRadicador.ID_SUBSERIE.ToString().PadLeft(campo.Longitud) : modelRadicador.ID_SUBSERIE.ToString().PadRight(campo.Longitud));
                                    break;
                                case "ID_UNIDADDOCUMENTAL":
                                    if (campo.Formato.Trim() != "")
                                        valorCampo = modelRadicador.ID_UNIDADDOCUMENTAL.ToString(campo.Formato);
                                    else
                                        valorCampo = (campo.Alineacion == "D" ? modelRadicador.ID_UNIDADDOCUMENTAL.ToString().PadLeft(campo.Longitud) : modelRadicador.ID_UNIDADDOCUMENTAL.ToString().PadRight(campo.Longitud));
                                    break;
                                case "ID_TIPOEXPEDIENTE":
                                    if (campo.Formato.Trim() != "")
                                        valorCampo = modelRadicador.ID_TIPOEXPEDIENTE.ToString(campo.Formato);
                                    else
                                        valorCampo = (campo.Alineacion == "D" ? modelRadicador.ID_TIPOEXPEDIENTE.ToString().PadLeft(campo.Longitud) : modelRadicador.ID_TIPOEXPEDIENTE.ToString().PadRight(campo.Longitud));
                                    break;
                                case "S_TIPOEXPEDIENTE":
                                    valorCampo = modelRadicador.S_TIPOEXPEDIENTE.Trim();
                                    break;
                                case "S_IDENTIFICADOR":
                                    if (configuracion.DocumentoAsociado.TipoConsulta == 0)
                                    {
                                        if (campo.Formato.Trim() != "")
                                            valorCampo = (modelRadicador.N_CONSECUTIVO == null ? 1 : (int)modelRadicador.N_CONSECUTIVO).ToString(campo.Formato);
                                        else
                                            valorCampo = (campo.Alineacion == "D" ? (modelRadicador.N_CONSECUTIVO == null ? 1 : (int)modelRadicador.N_CONSECUTIVO).ToString().PadLeft(campo.Longitud) : (modelRadicador.N_CONSECUTIVO == null ? 1 : (int)modelRadicador.N_CONSECUTIVO).ToString().PadRight(campo.Longitud));
                                    }
                                    else
                                    {
                                        if (campo.Formato.Trim() != "")
                                            valorCampo = Convert.ToInt64(identificador).ToString(campo.Formato);
                                        else
                                            valorCampo = (campo.Alineacion == "D" ? identificador.PadLeft(campo.Longitud) : identificador.PadRight(campo.Longitud));
                                    }
                                    break;
                                case "D_CREACION":
                                    valorCampo = fechaCreacion.ToString(campo.Formato);
                                    break;
                                /*case "TIPO_ANEXO_TOMO":
                                    //int tipoAnexoTomo = 0;
                                    if (datosRadicacionUD.datosRadicado.tipoEtiqueta == "T")
                                        tipoAnexoTomo = 1;
                                    else if (datosRadicacionUD.datosRadicado.tipoEtiqueta == "A")
                                        tipoAnexoTomo = 2;

                                    if (campo.Formato.Trim() != "")
                                        valorCampo = tipoAnexoTomo.ToString(campo.Formato);
                                    else
                                        valorCampo = (campo.Alineacion == "D" ? tipoAnexoTomo.ToString().PadLeft(campo.Longitud) : tipoAnexoTomo.ToString().PadRight(campo.Longitud));

                                    break;
                                case "CONSECUTIVO_ANEXO_TOMO": // TODO: Asignación de consecutivo cuando hay anexo. Por ahora no se utiliza, en el método RadicarAnexo se genera de forma fija en los dos ultimos caracteres del CB.
                                    int consAnexoTomo = 0;

                                    if (campo.Formato.Trim() != "")
                                        valorCampo = consAnexoTomo.ToString(campo.Formato);
                                    else
                                        valorCampo = (campo.Alineacion == "D" ? consAnexoTomo.ToString().PadLeft(campo.Longitud) : consAnexoTomo.ToString().PadRight(campo.Longitud));
                                    break;*/
                                default:
                                    if (campo.Formato.Trim() != "")
                                        valorCampo = Convert.ToDecimal("0" + datosDocAsociados[campo.Columna].valor).ToString(campo.Formato);
                                    else
                                        valorCampo = (campo.Alineacion == "D" ? datosDocAsociados[campo.Columna].valor.ToString().PadLeft(campo.Longitud) : datosDocAsociados[campo.Columna].valor.ToString().PadRight(campo.Longitud));
                                    break;
                            }
                        }
                        texto += valorCampo;
                    }

                    // Verificar que el CB no se haya generado aun
                    var verificacionExistencia = (from radicadoCB in dbSIM.RADICADOS_ETIQUETAS
                                                  where radicadoCB.S_ETIQUETA == CB
                                                  //select new { radicadoCB.ID_RADICADO, radicadoCB.S_IDENTIFICADOR }).FirstOrDefault();
                                                  select radicadoCB).FirstOrDefault();

                    if (verificacionExistencia != null) // Ya existe el CB
                    {
                        /*if (modelRadicador.S_TIPO == "S")
                        {
                            resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El radicado ya Existe. Desea reimprimirlo ?" };

                            if (datosRadicacionUD.tipoRetorno == "key")
                            {
                                resultado.id = verificacionExistencia.ID_RADICADO.ToString();
                            }
                            else //if (datosRadicacionUD.tipoRetorno == "id")
                            {
                                resultado.id = verificacionExistencia.S_IDENTIFICADOR;
                            }

                            return resultado;
                        } else*/
                            radicado = verificacionExistencia;
                    }
                    else
                    {
                        radicado = new RADICADOS_ETIQUETAS();
                        radicado.D_CREACION = fechaCreacion;
                        radicado.S_ETIQUETA = CB;
                        if (configuracion.textoIdentificador.Count > 0)
                            radicado.S_IDENTIFICADOR = identificador;
                        else
                            radicado.S_IDENTIFICADOR = ((int)(modelRadicador.N_CONSECUTIVO)).ToString(configuracion.formatoTextoIdentificador);
                        radicado.S_TEXTO = (texto.Length > 250 ? texto.Substring(0, 250) : texto);
                        radicado.ID_TERCERO = idTercero;
                        radicado.ID_RADICADOUNIDADDOCUMENTAL = modelRadicador.ID_RADICADOUNIDADDOCUMENTAL;
                        if (datosRadicacionUD.datosRadicado.documentoAsociado > 0)
                            radicado.ID_REGISTROREL = datosRadicacionUD.datosRadicado.documentoAsociado;
                        radicado.S_UBICACION = datosRadicacionUD.datosRadicado.ubicacion.Trim().ToUpper();
                        //radicado.S_FOLIOS = datosRadicacionUD.datosRadicado.folios;

                        try
                        {
                            dbSIM.Entry(radicado).State = EntityState.Added;
                            dbSIM.SaveChanges();
                        }
                        catch (Exception error)
                        {
                            string s = error.Message;
                        }

                        //if (modelRadicador.S_TIPO == "C")
                        //{
                            EXPEDIENTE expediente = new EXPEDIENTE();

                            //expediente.ID_UNIDADDOCUMENTAL = modelRadicador.ID_UNIDADDOCUMENTAL;
                            expediente.ID_UNIDADDOCUMENTAL = modelRadicador.ID_TIPOEXPEDIENTE;
                            expediente.ID_RADICADO = radicado.ID_RADICADO;

                            dbSIM.Entry(expediente).State = EntityState.Added;
                            dbSIM.SaveChanges();
                        //}

                        idRadicado = radicado.ID_RADICADO;
                        consecutivoRadicado = radicado.S_IDENTIFICADOR;

                        if (configuracion.textoIdentificador.Count == 0)
                        {
                            var radicadoUD = (from radicadoUnidadDocumental in dbSIM.RADICADO_UNIDADDOCUMENTAL
                                              //where radicadoUnidadDocumental.ID_UNIDADDOCUMENTAL == datosRadicacionUD.datosRadicado.unidadDocumental && radicadoUnidadDocumental.D_FIN == null
                                              where radicadoUnidadDocumental.ID_UNIDADDOCUMENTAL == datosRadicacionUD.datosRadicado.tipoExpediente && radicadoUnidadDocumental.D_FIN == null
                                              select radicadoUnidadDocumental).FirstOrDefault();

                            if (radicadoUD != null)
                            {
                                radicadoUD.N_CONSECUTIVO++;
                                dbSIM.Entry(radicadoUD).State = EntityState.Modified;
                                dbSIM.SaveChanges();
                            }
                        }

                        //trans.Complete();
                        //}

                        if (modelRadicador.S_TIPO == "S")
                        {
                            resultado = new datosRespuesta() { tipoRespuesta = "OK", detalleRespuesta = "" };

                            if (datosRadicacionUD.tipoRetorno == "key")
                            {
                                resultado.id = idRadicado.ToString();
                            }
                            else //if (datosRadicacionUD.tipoRetorno == "id")
                            {
                                resultado.id = consecutivoRadicado;
                            }

                            return resultado;
                        }
                    }

                    if (datosRadicacionUD.datosRadicado.tipoEtiqueta == "A")
                        return RadicarAnexo(radicado, datosRadicacionUD.datosRadicado.tipoAnexo, idTercero, datosRadicacionUD.datosRadicado.ubicacion, datosRadicacionUD.datosRadicado.consecutivo);
                    else if (datosRadicacionUD.datosRadicado.tipoEtiqueta == "T")
                        return RadicarTomo(radicado, idTercero, datosRadicacionUD.datosRadicado.ubicacion, datosRadicacionUD.datosRadicado.consecutivo);
                    else
                    {
                        resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "Tipo de Etiqueta Inválida (Anexo, Tomo).", id = "" };
                        return resultado;
                    }
                }
            }
        }

        [HttpGet, ActionName("RecuperarRadicado")]
        public object GetRecuperarRadicado(int idUnidadDocumental, string tipoRetorno, string radicado)
        {
            datosRespuesta resultado;
            int longitudIdentificador = 0;

            var identificadorBase = (from radicados in dbSIM.RADICADOS_ETIQUETAS
                                        where radicados.RADICADO_UNIDADDOCUMENTAL.ID_UNIDADDOCUMENTAL == idUnidadDocumental
                                        select radicados.S_IDENTIFICADOR).FirstOrDefault();

            if (identificadorBase != null)
            {
                longitudIdentificador = identificadorBase.Length;
            }

            if (longitudIdentificador == 0)
            {
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "Radicado NO Existe.", id = "" };
                return resultado;
            }
            else
            {
                radicado = radicado.PadLeft(longitudIdentificador, '0');

                var modelRadicador = (from radicados in dbSIM.RADICADOS_ETIQUETAS
                                        where radicados.RADICADO_UNIDADDOCUMENTAL.ID_UNIDADDOCUMENTAL == idUnidadDocumental && radicados.S_IDENTIFICADOR == radicado
                                        select new {
                                            radicados.ID_RADICADO,
                                            radicados.S_IDENTIFICADOR
                                        }).FirstOrDefault();

                if (modelRadicador == null)
                {
                    // Error, la Unidad Documental no tiene un Tipo de Radicado configurado
                    resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "Radicado NO encontrado.", id = "" };
                    return resultado;
                }
                else
                {
                    resultado = new datosRespuesta() { tipoRespuesta = "OK", detalleRespuesta = "" };

                    if (tipoRetorno == "key")
                    {
                        resultado.id = modelRadicador.ID_RADICADO.ToString();
                    }
                    else
                    {
                        resultado.id = modelRadicador.S_IDENTIFICADOR;
                    }

                    return resultado;
                }
            }
        }

        // tipoId: key-Identity de la tabla radicados, id-S_IDENTIFICADOR
        [HttpGet, ActionName("GenerarEtiqueta")]
        public HttpResponseMessage GenerarEtiqueta(string idRadicado, int idUnidadDocumental, string formatoRetorno)
        {
            return GenerarEtiqueta(idRadicado, idUnidadDocumental, "id", formatoRetorno);
        }

        // tipoId: key-Identity de la tabla radicados, id-S_IDENTIFICADOR
        [HttpGet, ActionName("GenerarEtiqueta")]
        public HttpResponseMessage GenerarEtiqueta(int idRadicado, string formatoRetorno)
        {
            return GenerarEtiqueta(idRadicado.ToString(), 0, "key", formatoRetorno);
        }

        // tipoId: key-Identity de la tabla radicados, id-S_IDENTIFICADOR
        [HttpGet, ActionName("GenerarEtiqueta")]
        public HttpResponseMessage GenerarEtiqueta(string idRadicado, int idUnidadDocumental, string tipoId, string formatoRetorno)
        {
            string CB = "";
            string etiqueta = "";
            string valorCampo = "";
            Dictionary<string, datosDocumentosAsociados> datosDocAsociados = null;
            int idRadicadoKey;

            dynamic modelRadicador;

            if (tipoId == "key")
            {
                idRadicadoKey = Convert.ToInt32(idRadicado);

                modelRadicador = (from radicado in dbSIM.RADICADOS_ETIQUETAS
                                  join tipoExpediente in dbSIM.TIPO_EXPEDIENTE on radicado.RADICADO_UNIDADDOCUMENTAL.ID_UNIDADDOCUMENTAL equals tipoExpediente.ID_TIPOEXPEDIENTE
                                  where radicado.ID_RADICADO == idRadicadoKey
                                  select new
                                  {
                                      radicado.ID_RADICADOUNIDADDOCUMENTAL,
                                      radicado.RADICADO_UNIDADDOCUMENTAL.ID_TIPORADICADO,
                                      S_TIPORADICADO = radicado.RADICADO_UNIDADDOCUMENTAL.TIPO_RADICADO.S_NOMBRE,
                                      radicado.D_CREACION,
                                      ID_SERIE = 0, //radicado.RADICADO_UNIDADDOCUMENTAL.UNIDAD_DOCUMENTAL.SUBSERIE.ID_SERIE,
                                      S_SERIE = "", //radicado.RADICADO_UNIDADDOCUMENTAL.UNIDAD_DOCUMENTAL.SUBSERIE.SERIE.S_NOMBRE,
                                      ID_SUBSERIE = 0, //radicado.RADICADO_UNIDADDOCUMENTAL.UNIDAD_DOCUMENTAL.ID_SUBSERIE,
                                      S_SUBSERIE = "", //radicado.RADICADO_UNIDADDOCUMENTAL.UNIDAD_DOCUMENTAL.SUBSERIE.S_NOMBRE,
                                      ID_UNIDADDOCUMENTAL = 0, //radicado.RADICADO_UNIDADDOCUMENTAL.ID_UNIDADDOCUMENTAL,
                                      S_UNIDADDOCUMENTAL = "", //radicado.RADICADO_UNIDADDOCUMENTAL.UNIDAD_DOCUMENTAL.S_NOMBRE,
                                      radicado.S_IDENTIFICADOR,
                                      radicado.S_ETIQUETA,
                                      radicado.RADICADO_UNIDADDOCUMENTAL.TIPO_RADICADO.S_CONFIGURACION,
                                      radicado.ID_RADICADOPADRE,
                                      radicado.ID_REGISTROREL,
                                      radicado.S_TIPO,
                                      radicado.S_CONSECUTIVOTIPO,
                                      radicado.ID_TIPOANEXO,
                                      tipoExpediente.ID_TIPOEXPEDIENTE,
                                      S_TIPOEXPEDIENTE = tipoExpediente.S_NOMBRE,
                                      radicado.S_FOLIOS
                                  }).FirstOrDefault();
            }
            else
            {
                modelRadicador = (from radicado in dbSIM.RADICADOS_ETIQUETAS
                                  join tipoExpediente in dbSIM.TIPO_EXPEDIENTE on radicado.RADICADO_UNIDADDOCUMENTAL.ID_UNIDADDOCUMENTAL equals tipoExpediente.ID_TIPOEXPEDIENTE
                                  where radicado.S_IDENTIFICADOR == idRadicado && radicado.RADICADO_UNIDADDOCUMENTAL.ID_UNIDADDOCUMENTAL == idUnidadDocumental && radicado.ID_RADICADOPADRE == null
                                  select new
                                  {
                                      radicado.ID_RADICADOUNIDADDOCUMENTAL,
                                      radicado.RADICADO_UNIDADDOCUMENTAL.ID_TIPORADICADO,
                                      S_TIPORADICADO = radicado.RADICADO_UNIDADDOCUMENTAL.TIPO_RADICADO.S_NOMBRE,
                                      radicado.D_CREACION,
                                      ID_SERIE = 0, //radicado.RADICADO_UNIDADDOCUMENTAL.UNIDAD_DOCUMENTAL.SUBSERIE.ID_SERIE,
                                      S_SERIE = "", //radicado.RADICADO_UNIDADDOCUMENTAL.UNIDAD_DOCUMENTAL.SUBSERIE.SERIE.S_NOMBRE,
                                      ID_SUBSERIE = 0, //radicado.RADICADO_UNIDADDOCUMENTAL.UNIDAD_DOCUMENTAL.ID_SUBSERIE,
                                      S_SUBSERIE = "", //radicado.RADICADO_UNIDADDOCUMENTAL.UNIDAD_DOCUMENTAL.SUBSERIE.S_NOMBRE,
                                      ID_UNIDADDOCUMENTAL = 0, //radicado.RADICADO_UNIDADDOCUMENTAL.ID_UNIDADDOCUMENTAL,
                                      S_UNIDADDOCUMENTAL = "", //radicado.RADICADO_UNIDADDOCUMENTAL.UNIDAD_DOCUMENTAL.S_NOMBRE,
                                      radicado.S_IDENTIFICADOR,
                                      radicado.S_ETIQUETA,
                                      radicado.RADICADO_UNIDADDOCUMENTAL.TIPO_RADICADO.S_CONFIGURACION,
                                      radicado.ID_RADICADOPADRE,
                                      radicado.ID_REGISTROREL,
                                      radicado.S_TIPO,
                                      radicado.S_CONSECUTIVOTIPO,
                                      radicado.ID_TIPOANEXO,
                                      tipoExpediente.ID_TIPOEXPEDIENTE,
                                      S_TIPOEXPEDIENTE = tipoExpediente.S_NOMBRE
                                  }).FirstOrDefault();
            }

            CONFIGURACION configuracion = ObtenerConfiguracion(modelRadicador.S_CONFIGURACION);

            switch (configuracion.DocumentoAsociado.TipoConsulta)
            {
                case 1:
                case 2:
                    Type thisType = this.GetType();
                    MethodInfo theMethod = thisType.GetMethod("Datos" + configuracion.DocumentoAsociado.Funcion);
                    datosDocAsociados = (Dictionary<string, datosDocumentosAsociados>)theMethod.Invoke(this, new object[] { Convert.ToInt32(modelRadicador.ID_REGISTROREL) });
                    break;
            }

            DATOSETIQUETA datosEtiqueta = new DATOSETIQUETA();

            foreach (KeyValuePair<string, CAMPO> entry in configuracion.Etiqueta)
            {
                CAMPO campo = entry.Value;

                if (campo.Columna != null && campo.Columna.Trim() != "")
                {
                    switch (campo.Columna)
                    {
                        case "CTE":
                            valorCampo = (campo.Alineacion == "D" ? campo.Formato.PadLeft(campo.Longitud) : campo.Formato.PadRight(campo.Longitud));
                            break;
                        case "ID_TIPORADICADO":
                            if (campo.Formato.Trim() != "")
                                valorCampo = modelRadicador.ID_TIPORADICADO.ToString(campo.Formato);
                            else
                                valorCampo = (campo.Alineacion == "D" ? modelRadicador.ID_TIPORADICADO.ToString().PadLeft(campo.Longitud) : modelRadicador.ID_TIPORADICADO.ToString().PadRight(campo.Longitud));
                            break;
                        case "S_TIPORADICADO":
                            valorCampo = (campo.Alineacion == "D" ? modelRadicador.S_TIPORADICADO.PadLeft(campo.Longitud) : modelRadicador.S_TIPORADICADO.PadRight(campo.Longitud));
                            break;
                        case "ID_SERIE":
                            if (campo.Formato.Trim() != "")
                                valorCampo = modelRadicador.ID_SERIE.ToString(campo.Formato);
                            else
                                valorCampo = (campo.Alineacion == "D" ? modelRadicador.ID_SERIE.ToString().PadLeft(campo.Longitud) : modelRadicador.ID_SERIE.ToString().PadRight(campo.Longitud));
                            break;
                        case "S_SERIE":
                            valorCampo = (campo.Alineacion == "D" ? modelRadicador.S_SERIE.PadLeft(campo.Longitud) : modelRadicador.S_SERIE.PadRight(campo.Longitud));
                            break;
                        case "ID_SUBSERIE":
                            if (campo.Formato.Trim() != "")
                                valorCampo = modelRadicador.ID_SUBSERIE.ToString(campo.Formato);
                            else
                                valorCampo = (campo.Alineacion == "D" ? modelRadicador.ID_SUBSERIE.ToString().PadLeft(campo.Longitud) : modelRadicador.ID_SUBSERIE.ToString().PadRight(campo.Longitud));
                            break;
                        case "S_SUBSERIE":
                            valorCampo = (campo.Alineacion == "D" ? modelRadicador.S_SUBSERIE.PadLeft(campo.Longitud) : modelRadicador.S_SUBSERIE.PadRight(campo.Longitud));
                            break;
                        case "ID_UNIDADDOCUMENTAL":
                            if (campo.Formato.Trim() != "")
                                valorCampo = modelRadicador.ID_UNIDADDOCUMENTAL.ToString(campo.Formato);
                            else
                                valorCampo = (campo.Alineacion == "D" ? modelRadicador.ID_UNIDADDOCUMENTAL.ToString().PadLeft(campo.Longitud) : modelRadicador.ID_UNIDADDOCUMENTAL.ToString().PadRight(campo.Longitud));
                            break;
                        case "ID_TIPOEXPEDIENTE":
                            if (campo.Formato.Trim() != "")
                                valorCampo = modelRadicador.ID_TIPOEXPEDIENTE.ToString(campo.Formato);
                            else
                                valorCampo = (campo.Alineacion == "D" ? modelRadicador.ID_TIPOEXPEDIENTE.ToString().PadLeft(campo.Longitud) : modelRadicador.ID_TIPOEXPEDIENTE.ToString().PadRight(campo.Longitud));
                            break;
                        case "S_TIPOEXPEDIENTE":
                            valorCampo = modelRadicador.S_TIPOEXPEDIENTE.Trim();
                            break;
                        case "S_FOLIOS":
                            valorCampo = modelRadicador.S_FOLIOS == null ? "" : "FOLIOS " + modelRadicador.S_FOLIOS.Trim();
                            break;
                        case "S_UNIDADDOCUMENTAL":
                            valorCampo = (campo.Alineacion == "D" ? modelRadicador.S_UNIDADDOCUMENTAL.PadLeft(campo.Longitud) : modelRadicador.S_UNIDADDOCUMENTAL.PadRight(campo.Longitud));
                            break;
                        case "S_IDENTIFICADOR":
                            if (campo.Formato.Trim() != "")
                                valorCampo = Convert.ToDecimal(modelRadicador.S_IDENTIFICADOR).ToString(campo.Formato);
                            else
                                valorCampo = (campo.Alineacion == "D" ? modelRadicador.S_IDENTIFICADOR.PadLeft(campo.Longitud) : modelRadicador.S_IDENTIFICADOR.PadRight(campo.Longitud));
                            break;
                        case "D_CREACION":
                            valorCampo = ((DateTime)modelRadicador.D_CREACION).ToString(campo.Formato);
                            break;
                        case "S_ETIQUETA":
                            valorCampo = (campo.Alineacion == "D" ? modelRadicador.S_ETIQUETA.PadLeft(campo.Longitud) : modelRadicador.S_ETIQUETA.PadRight(campo.Longitud));
                            break;
                        case "TEXTO_IDENTIFICADOR":
                            //if (configuracion.EsIdentificadorTextoIdentificador)
                            valorCampo = (campo.Alineacion == "D" ? modelRadicador.S_IDENTIFICADOR.PadLeft(campo.Longitud) : modelRadicador.S_IDENTIFICADOR.PadRight(campo.Longitud));
                            /*else
                            {
                                valorCampo = "";
                                foreach (KeyValuePair<string, CAMPO> entryTexto in configuracion.textoIdentificador)
                                {
                                    CAMPO campoTexto = entryTexto.Value;

                                    if (campoTexto.Nombre.Length >= 3 && campoTexto.Nombre.Substring(0, 3) == "CTE")
                                    {
                                        if (campoTexto.Formato.Trim() != "")
                                            valorCampo += Convert.ToInt32(campoTexto.Columna).ToString(campoTexto.Formato);
                                        else
                                            valorCampo += (campoTexto.Alineacion == "D" ? campoTexto.Columna.PadLeft(campoTexto.Longitud) : campoTexto.Columna.PadRight(campoTexto.Longitud));
                                    }
                                    else
                                    {
                                        switch (campoTexto.Columna)
                                        {
                                            case "ID_TIPORADICADO":
                                                if (campoTexto.Formato.Trim() != "")
                                                    valorCampo += modelRadicador.ID_TIPORADICADO.ToString(campoTexto.Formato);
                                                else
                                                    valorCampo += (campoTexto.Alineacion == "D" ? modelRadicador.ID_TIPORADICADO.ToString().PadLeft(campoTexto.Longitud) : modelRadicador.ID_TIPORADICADO.ToString().PadRight(campoTexto.Longitud));
                                                break;
                                            case "ID_SERIE":
                                                if (campoTexto.Formato.Trim() != "")
                                                    valorCampo += modelRadicador.ID_SERIE.ToString(campoTexto.Formato);
                                                else
                                                    valorCampo += (campoTexto.Alineacion == "D" ? modelRadicador.ID_SERIE.ToString().PadLeft(campoTexto.Longitud) : modelRadicador.ID_SERIE.ToString().PadRight(campoTexto.Longitud));
                                                break;
                                            case "ID_SUBSERIE":
                                                if (campoTexto.Formato.Trim() != "")
                                                    valorCampo += modelRadicador.ID_SUBSERIE.ToString(campoTexto.Formato);
                                                else
                                                    valorCampo += (campoTexto.Alineacion == "D" ? modelRadicador.ID_SUBSERIE.ToString().PadLeft(campoTexto.Longitud) : modelRadicador.ID_SUBSERIE.ToString().PadRight(campoTexto.Longitud));
                                                break;
                                            case "ID_UNIDADDOCUMENTAL":
                                                if (campoTexto.Formato.Trim() != "")
                                                    valorCampo += modelRadicador.ID_UNIDADDOCUMENTAL.ToString(campoTexto.Formato);
                                                else
                                                    valorCampo += (campoTexto.Alineacion == "D" ? modelRadicador.ID_UNIDADDOCUMENTAL.ToString().PadLeft(campoTexto.Longitud) : modelRadicador.ID_UNIDADDOCUMENTAL.ToString().PadRight(campoTexto.Longitud));
                                                break;
                                            case "D_CREACION":
                                                valorCampo += ((DateTime)modelRadicador.D_CREACION).ToString(campoTexto.Formato);
                                                break;
                                            default:
                                                if (campoTexto.Formato.Trim() != "")
                                                    valorCampo += Convert.ToDecimal("0" + datosDocAsociados[campoTexto.Columna].valor).ToString(campoTexto.Formato);
                                                else
                                                    valorCampo += (campoTexto.Alineacion == "D" ? datosDocAsociados[campoTexto.Columna].valor.ToString().PadLeft(campoTexto.Longitud) : datosDocAsociados[campoTexto.Columna].valor.ToString().PadRight(campoTexto.Longitud));
                                                break;
                                        }
                                    }
                                }
                            }*/
                            break;
                        case "CONSECUTIVO_ANEXO_TOMO":
                            if (modelRadicador.S_TIPO != null)
                            {
                                /*if (modelRadicador.S_TIPO == "T")
                                {
                                    valorCampo = "TOMO " + modelRadicador.N_CONSECUTIVOTIPO.ToString(); //modelRadicador.S_ETIQUETA.Substring(modelRadicador.S_ETIQUETA.Length - 2, 2);
                                }
                                else
                                {
                                    valorCampo = "ANEXO " + modelRadicador.N_CONSECUTIVOTIPO.ToString(); // modelRadicador.S_ETIQUETA.Substring(modelRadicador.S_ETIQUETA.Length - 2, 2);
                                }*/

                                valorCampo = modelRadicador.S_TIPO + " " + modelRadicador.S_CONSECUTIVOTIPO;
                            }
                            else
                            {
                                valorCampo = "";
                            }

                            break;
                        case "TIPO_ANEXO":
                            if (modelRadicador.S_TIPO != null && modelRadicador.S_TIPO == "A" && modelRadicador.ID_TIPOANEXO != null && modelRadicador.ID_TIPOANEXO > 0)
                            {
                                int idTipoAnexo = Convert.ToInt32(modelRadicador.ID_TIPOANEXO);
                                var tipoAnexo = (from tiposAnexo in dbSIM.TIPO_ANEXO
                                                 where tiposAnexo.ID_TIPOANEXO == idTipoAnexo
                                                 select tiposAnexo.S_NOMBRE).FirstOrDefault<string>();

                                valorCampo = tipoAnexo;
                            }
                            else
                            {
                                valorCampo = "";
                            }

                            break;
                        default:
                            if (campo.Formato.Trim() != "")
                                valorCampo = Convert.ToDecimal("0" + datosDocAsociados[campo.Columna].valor).ToString(campo.Formato);
                            else
                                valorCampo = (campo.Alineacion == "D" ? datosDocAsociados[campo.Columna].valor.ToString().PadLeft(campo.Longitud) : datosDocAsociados[campo.Columna].valor.ToString().PadRight(campo.Longitud));
                            break;
                    }
                }
                else
                {
                    valorCampo = "";

                    if (campo.SUBCAMPOS != null && campo.SUBCAMPOS.Count > 0)
                    {
                        foreach (KeyValuePair<string, CAMPO> entrySubCampo in campo.SUBCAMPOS)
                        {
                            CAMPO subCampo = entrySubCampo.Value;

                            switch (subCampo.Columna)
                            {
                                case "CTE":
                                    valorCampo += (subCampo.Alineacion == "D" ? subCampo.Formato.PadLeft(subCampo.Longitud) : subCampo.Formato.PadRight(subCampo.Longitud));
                                    break;
                                case "ID_TIPORADICADO":
                                    if (subCampo.Formato.Trim() != "")
                                        valorCampo += modelRadicador.ID_TIPORADICADO.ToString(subCampo.Formato);
                                    else
                                        valorCampo += (subCampo.Alineacion == "D" ? modelRadicador.ID_TIPORADICADO.ToString().PadLeft(subCampo.Longitud) : modelRadicador.ID_TIPORADICADO.ToString().PadRight(subCampo.Longitud));
                                    break;
                                case "S_TIPORADICADO":
                                    valorCampo += (subCampo.Alineacion == "D" ? modelRadicador.S_TIPORADICADO.PadLeft(subCampo.Longitud) : modelRadicador.S_TIPORADICADO.PadRight(subCampo.Longitud));
                                    break;
                                case "ID_SERIE":
                                    if (subCampo.Formato.Trim() != "")
                                        valorCampo += modelRadicador.ID_SERIE.ToString(subCampo.Formato);
                                    else
                                        valorCampo += (subCampo.Alineacion == "D" ? modelRadicador.ID_SERIE.ToString().PadLeft(subCampo.Longitud) : modelRadicador.ID_SERIE.ToString().PadRight(subCampo.Longitud));
                                    break;
                                case "S_SERIE":
                                    valorCampo += (subCampo.Alineacion == "D" ? modelRadicador.S_SERIE.PadLeft(subCampo.Longitud) : modelRadicador.S_SERIE.PadRight(subCampo.Longitud));
                                    break;
                                case "ID_SUBSERIE":
                                    if (subCampo.Formato.Trim() != "")
                                        valorCampo += modelRadicador.ID_SUBSERIE.ToString(subCampo.Formato);
                                    else
                                        valorCampo += (subCampo.Alineacion == "D" ? modelRadicador.ID_SUBSERIE.ToString().PadLeft(subCampo.Longitud) : modelRadicador.ID_SUBSERIE.ToString().PadRight(subCampo.Longitud));
                                    break;
                                case "S_SUBSERIE":
                                    valorCampo += (subCampo.Alineacion == "D" ? modelRadicador.S_SUBSERIE.PadLeft(subCampo.Longitud) : modelRadicador.S_SUBSERIE.PadRight(subCampo.Longitud));
                                    break;
                                case "ID_UNIDADDOCUMENTAL":
                                    if (subCampo.Formato.Trim() != "")
                                        valorCampo += modelRadicador.ID_UNIDADDOCUMENTAL.ToString(subCampo.Formato);
                                    else
                                        valorCampo += (subCampo.Alineacion == "D" ? modelRadicador.ID_UNIDADDOCUMENTAL.ToString().PadLeft(subCampo.Longitud) : modelRadicador.ID_UNIDADDOCUMENTAL.ToString().PadRight(subCampo.Longitud));
                                    break;
                                case "S_UNIDADDOCUMENTAL":
                                    valorCampo += (subCampo.Alineacion == "D" ? modelRadicador.S_UNIDADDOCUMENTAL.PadLeft(subCampo.Longitud) : modelRadicador.S_UNIDADDOCUMENTAL.PadRight(subCampo.Longitud));
                                    break;
                                case "ID_TIPOEXPEDIENTE":
                                    if (campo.Formato.Trim() != "")
                                        valorCampo = modelRadicador.ID_TIPOEXPEDIENTE.ToString(campo.Formato);
                                    else
                                        valorCampo = (campo.Alineacion == "D" ? modelRadicador.ID_TIPOEXPEDIENTE.ToString().PadLeft(campo.Longitud) : modelRadicador.ID_TIPOEXPEDIENTE.ToString().PadRight(campo.Longitud));
                                    break;
                                case "S_TIPOEXPEDIENTE":
                                    valorCampo = modelRadicador.S_TIPOEXPEDIENTE.Trim();
                                    break;
                                case "S_IDENTIFICADOR":
                                    if (subCampo.Formato.Trim() != "")
                                        valorCampo += Convert.ToDecimal(modelRadicador.S_IDENTIFICADOR).ToString(subCampo.Formato);
                                    else
                                        valorCampo += (subCampo.Alineacion == "D" ? modelRadicador.S_IDENTIFICADOR.PadLeft(subCampo.Longitud) : modelRadicador.S_IDENTIFICADOR.PadRight(subCampo.Longitud));
                                    break;
                                case "D_CREACION":
                                    valorCampo += ((DateTime)modelRadicador.D_CREACION).ToString(subCampo.Formato);
                                    break;
                                case "S_ETIQUETA":
                                    valorCampo += (subCampo.Alineacion == "D" ? modelRadicador.S_ETIQUETA.PadLeft(subCampo.Longitud) : modelRadicador.S_ETIQUETA.PadRight(subCampo.Longitud));
                                    break;
                                case "TEXTO_IDENTIFICADOR":
                                    valorCampo += (subCampo.Alineacion == "D" ? modelRadicador.S_IDENTIFICADOR.PadLeft(subCampo.Longitud) : modelRadicador.S_IDENTIFICADOR.PadRight(subCampo.Longitud));
                                    break;
                                case "CONSECUTIVO_ANEXO_TOMO":
                                    if (modelRadicador.ID_RADICADOPADRE != null)
                                    {
                                        if (modelRadicador.ID_REGISTROREL != null)
                                        {
                                            valorCampo += "TOMO " + modelRadicador.S_ETIQUETA.Substring(modelRadicador.S_ETIQUETA.Length - 10, 10).TrimStart('0');
                                        }
                                        else
                                        {
                                            valorCampo += "ANEXO " + modelRadicador.S_ETIQUETA.Substring(modelRadicador.S_ETIQUETA.Length - 10, 10).TrimStart('0');
                                        }
                                    }
                                    else
                                    {
                                        valorCampo += "";
                                    }

                                    break;
                                default:
                                    if (subCampo.Formato.Trim() != "")
                                        valorCampo += Convert.ToDecimal("0" + datosDocAsociados[subCampo.Columna].valor).ToString(subCampo.Formato);
                                    else
                                        valorCampo += (subCampo.Alineacion == "D" ? datosDocAsociados[subCampo.Columna].valor.ToString().PadLeft(subCampo.Longitud) : datosDocAsociados[subCampo.Columna].valor.ToString().PadRight(subCampo.Longitud));
                                    break;
                            }
                        }
                    }
                }

                switch (campo.Nombre)
                {
                    case "CB":
                        datosEtiqueta.CB = valorCampo;
                        break;
                    case "Texto1":
                        datosEtiqueta.Texto1 = valorCampo.Trim().ToUpper();
                        break;
                    case "Texto2":
                        datosEtiqueta.Texto2 = valorCampo.Trim().ToUpper();
                        break;
                    case "Texto3":
                        datosEtiqueta.Texto3 = valorCampo.Trim().ToUpper();
                        break;
                    case "Texto4":
                        datosEtiqueta.Texto4 = valorCampo.Trim().ToUpper();
                        break;
                    case "Texto5":
                        datosEtiqueta.Texto5 = valorCampo.Trim().ToUpper();
                        break;
                    case "Texto6":
                        datosEtiqueta.Texto6 = valorCampo.Trim().ToUpper();
                        break;
                    case "Texto7":
                        datosEtiqueta.Texto7 = valorCampo.Trim().ToUpper();
                        break;
                    case "Texto8":
                        datosEtiqueta.Texto8 = valorCampo.Trim().ToUpper();
                        break;
                    case "Texto9":
                        datosEtiqueta.Texto9 = valorCampo.Trim().ToUpper();
                        break;
                    case "Texto10":
                        datosEtiqueta.Texto10 = valorCampo.Trim().ToUpper();
                        break;
                }
            }

            datosEtiqueta.CB = modelRadicador.S_ETIQUETA;

            return GenerarEtiqueta(datosEtiqueta, formatoRetorno);
        }

        // GET api/<controller>
        [Authorize(Roles = "VTERCERO")]
        public HttpResponseMessage GenerarEtiqueta(DATOSETIQUETA datosEtiqueta, string formato)
        {
            var report = new CBReport();
            report.CB = datosEtiqueta.CB;
            report.Texto1 = datosEtiqueta.Texto1;
            report.Texto2 = datosEtiqueta.Texto2;
            report.Texto3 = datosEtiqueta.Texto3;
            report.Texto4 = datosEtiqueta.Texto4;
            report.Texto5 = datosEtiqueta.Texto5;
            report.Texto8 = datosEtiqueta.Texto8;
            report.Texto9 = datosEtiqueta.Texto9;
            report.Texto10 = datosEtiqueta.Texto10;

            var stream = new MemoryStream();

            switch (formato)
            {
                case "pdf":
                    report.ExportToPdf(stream);
                    break;
                default:
                    report.ExportToImage(stream, (formato == "png" ? ImageFormat.Png : (formato == "tiff" ? ImageFormat.Tiff : (formato == "jpeg" ? ImageFormat.Jpeg : (formato == "gif" ? ImageFormat.Gif : ImageFormat.Png)))));
                    break;
            }
            report.Dispose();

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            //response.Content = new StreamContent(stream);
            response.Content = new ByteArrayContent(stream.ToArray());
            //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue((formato == "pdf" ? "application/pdf" : (formato == "png" ? "image/png" : (formato == "tiff" ? "image/tiff" : (formato == "jpeg" ? "image/jpeg" : (formato == "gif" ? "image/gif" : "image/png"))))));

            return response;
        }

        // parte: 0 Todo, 1 Documento Asociado, 2 CB, 3 Datos Etiqueta, 4 Texto
        private CONFIGURACION ObtenerConfiguracion(string datosConfiguracion)
        {
            return ObtenerConfiguracion(datosConfiguracion, 0);
        }

        // parte: 0 Todo, 1 Documento Asociado, 2 CB, 3 Datos Etiqueta
        private CONFIGURACION ObtenerConfiguracion(string datosConfiguracion, int parte)
        {
            string configuracionColumnas;
            CONFIGURACION configuracion = new CONFIGURACION();
            CAMPO campo;
            CAMPO subCampo;
            COLUMNASCOMBO configuracionColumna;

            var xml = XDocument.Parse(datosConfiguracion);

            //configuracion.EsIdentificadorTextoIdentificador = (xml.Root.Element("TI").Attribute("EsIdentificador").Value.ToUpper() == "S" ? true : false);

            if (parte == 0 || parte == 1)
            {
                configuracion.DocumentoAsociado = new DOCUMENTOASOCIADO();
                configuracion.DocumentoAsociado.Nombre = xml.Root.Element("DA").Attribute("Nombre").Value;
                if (configuracion.DocumentoAsociado.Nombre.Trim() != "")
                {
                    configuracion.DocumentoAsociado.Funcion = xml.Root.Element("DA").Attribute("Funcion").Value;
                    //configuracion.DocumentoAsociado.EsIdentificador = (xml.Root.Element("DA").Attribute("EsIdentificador").Value.ToUpper() == "S" ? true : false);
                    configuracion.DocumentoAsociado.TipoConsulta = Convert.ToInt32(xml.Root.Element("DA").Attribute("TipoConsulta").Value);
                    configuracion.DocumentoAsociado.PlaceHolder = xml.Root.Element("DA").Attribute("PlaceHolder").Value;
                    configuracion.DocumentoAsociado.Titulo = xml.Root.Element("DA").Attribute("Titulo").Value;
                    configuracion.DocumentoAsociado.OrdenadoPor = xml.Root.Element("DA").Attribute("OrdenadoPor").Value;
                    configuracionColumnas = xml.Root.Element("DA").Attribute("ColCombo").Value;
                    if (configuracionColumnas.Trim() != "")
                    {
                        configuracion.DocumentoAsociado.ColumnasCombo = new ArrayList();
                        foreach (string configCol in configuracionColumnas.Split(','))
                        {
                            string[] datosConfig = configCol.Split('%');
                            configuracionColumna = new COLUMNASCOMBO();
                            configuracionColumna.Nombre = datosConfig[0];
                            configuracionColumna.Titulo = datosConfig[1];
                            configuracionColumna.Ancho = datosConfig[2];
                            configuracionColumna.TipoDato = datosConfig[3].ToUpper();
                            configuracionColumna.Visible = datosConfig[4].ToUpper() == "S" ? true : false;

                            configuracion.DocumentoAsociado.ColumnasCombo.Add(configuracionColumna);
                        }
                    }

                    if (configuracion.DocumentoAsociado.TipoConsulta == 3)
                    {
                        string[] titulos = configuracion.DocumentoAsociado.Titulo.Split('|');
                        string[] placeHolders = configuracion.DocumentoAsociado.PlaceHolder.Split('|');
                        string[] caracteresMax = xml.Root.Element("DA").Attribute("CaracteresMax").Value.Split('|');
                        string[] nombres = xml.Root.Element("DA").Attribute("NombresCampos").Value.Split('|');

                        configuracion.DocumentoAsociado.Campos = new ArrayList();

                        for (int i = 0; i < titulos.Length; i++)
                        {
                            configuracion.DocumentoAsociado.Campos.Add(new CAMPOTEXTO() { Titulo = titulos[i], PlaceHolder = placeHolders[i], CaracteresMax = Convert.ToInt32(caracteresMax[i]), Nombre = nombres[i] });
                        }
                    }
                }

                //lcobjConfiguracion.TipoDoc.ColumnasVisualizar = xml.Root.Element("TIPODOC").Attribute("ColVisualizar").Value;
                //lcobjConfiguracion.TipoDoc.ColumnasValor = xml.Root.Element("TIPODOC").Attribute("ColValor").Value;

                configuracion.formatoTextoIdentificador = (xml.Root.Element("TI").Attribute("Formato") == null ? "" : xml.Root.Element("TI").Attribute("Formato").Value);

                var queryTextoIdentificador = from c in xml.Root.Element("TI").Descendants("C")
                                              select new CAMPO
                                              {
                                                  Nombre = c.Attribute("Nombre").Value,
                                                  Longitud = Convert.ToInt32(c.Attribute("Long").Value),
                                                  Tipo = c.Attribute("Tipo").Value,
                                                  Formato = (c.Attribute("Formato") == null ? "" : c.Attribute("Formato").Value),
                                                  Alineacion = (c.Attribute("Alineacion") == null ? "I" : c.Attribute("Alineacion").Value),
                                                  Columna = (c.Attribute("Columna").Value == null ? "" : c.Attribute("Columna").Value),
                                              };

                configuracion.textoIdentificador = new Dictionary<string, CAMPO>();

                foreach (CAMPO lcobjCampoConfig in queryTextoIdentificador)
                {
                    campo = new CAMPO();

                    campo.Nombre = lcobjCampoConfig.Nombre;
                    campo.Longitud = lcobjCampoConfig.Longitud;
                    campo.Tipo = lcobjCampoConfig.Tipo; // N Numero, S String
                    campo.Formato = lcobjCampoConfig.Formato; // N Numero, S String
                    campo.Alineacion = lcobjCampoConfig.Alineacion; // I Izquierda, D Derecha - Solo Aplica para String
                    campo.Columna = lcobjCampoConfig.Columna;

                    configuracion.textoIdentificador.Add(lcobjCampoConfig.Nombre, campo);
                }
            }

            if (parte == 0 || parte == 2)
            {
                var queryCB = from c in xml.Root.Element("CB").Descendants("C")
                              select new CAMPO
                              {
                                  Nombre = c.Attribute("Nombre").Value,
                                  Longitud = Convert.ToInt32(c.Attribute("Long").Value),
                                  Tipo = c.Attribute("Tipo").Value,
                                  Formato = (c.Attribute("Formato") == null ? "" : c.Attribute("Formato").Value),
                                  Alineacion = (c.Attribute("Alineacion") == null ? "I" : c.Attribute("Alineacion").Value),
                                  Columna = (c.Attribute("Columna").Value == null ? "" : c.Attribute("Columna").Value),
                              };

                configuracion.CB = new Dictionary<string, CAMPO>();

                foreach (CAMPO lcobjCampoConfig in queryCB)
                {
                    campo = new CAMPO();

                    campo.Nombre = lcobjCampoConfig.Nombre;
                    campo.Longitud = lcobjCampoConfig.Longitud;
                    campo.Tipo = lcobjCampoConfig.Tipo; // N Numero, S String
                    campo.Formato = lcobjCampoConfig.Formato; // N Numero, S String
                    campo.Alineacion = lcobjCampoConfig.Alineacion; // I Izquierda, D Derecha - Solo Aplica para String
                    campo.Columna = lcobjCampoConfig.Columna;

                    configuracion.CB.Add(lcobjCampoConfig.Nombre, campo);
                }
            }

            if (parte == 0 || parte == 3)
            {
                var queryEtiqueta = from c in xml.Root.Element("DE").Descendants("C")
                                    select new CAMPO
                                    {
                                        Nombre = c.Attribute("Nombre").Value,
                                        Longitud = Convert.ToInt32(c.Attribute("Long").Value),
                                        Tipo = c.Attribute("Tipo").Value,
                                        Formato = (c.Attribute("Formato") == null ? "" : c.Attribute("Formato").Value),
                                        Alineacion = (c.Attribute("Alineacion") == null ? "I" : c.Attribute("Alineacion").Value),
                                        Columna = (c.Attribute("Columna").Value == null ? "" : c.Attribute("Columna").Value),
                                    };

                configuracion.Etiqueta = new Dictionary<string, CAMPO>();

                foreach (CAMPO lcobjCampoConfig in queryEtiqueta)
                {
                    campo = new CAMPO();

                    campo.Nombre = lcobjCampoConfig.Nombre;
                    campo.Longitud = lcobjCampoConfig.Longitud;
                    campo.Tipo = lcobjCampoConfig.Tipo; // N Numero, S String
                    campo.Formato = lcobjCampoConfig.Formato; // N Numero, S String
                    campo.Alineacion = lcobjCampoConfig.Alineacion; // I Izquierda, D Derecha - Solo Aplica para String

                    if (lcobjCampoConfig.Columna != null && lcobjCampoConfig.Columna.Trim() != "")
                        campo.Columna = lcobjCampoConfig.Columna;
                    else
                    {
                        var querySubCamposEtiqueta = from c in xml.Root.Element("DE").Descendants("C").Where(n => (string)n.Attribute("Nombre") == lcobjCampoConfig.Nombre).Descendants()
                                                      select new CAMPO
                                                      {
                                                          Nombre = c.Attribute("Nombre").Value,
                                                          Longitud = Convert.ToInt32(c.Attribute("Long").Value),
                                                          Tipo = c.Attribute("Tipo").Value,
                                                          Formato = (c.Attribute("Formato") == null ? "" : c.Attribute("Formato").Value),
                                                          Alineacion = (c.Attribute("Alineacion") == null ? "I" : c.Attribute("Alineacion").Value),
                                                          Columna = (c.Attribute("Columna").Value == null ? "" : c.Attribute("Columna").Value),
                                                      };

                        if (querySubCamposEtiqueta.Count() == 0)
                        {
                            campo.Columna = lcobjCampoConfig.Columna;
                        }
                        else
                        {
                            campo.SUBCAMPOS = new Dictionary<string, CAMPO>();

                            foreach (CAMPO lcobjSubCampoConfig in querySubCamposEtiqueta)
                            {
                                subCampo = new CAMPO();

                                subCampo.Nombre = lcobjSubCampoConfig.Nombre;
                                subCampo.Longitud = lcobjSubCampoConfig.Longitud;
                                subCampo.Tipo = lcobjSubCampoConfig.Tipo; // N Numero, S String
                                subCampo.Formato = lcobjSubCampoConfig.Formato; // N Numero, S String
                                subCampo.Alineacion = lcobjSubCampoConfig.Alineacion; // I Izquierda, D Derecha - Solo Aplica para String
                                subCampo.Columna = lcobjSubCampoConfig.Columna;

                                campo.SUBCAMPOS.Add(subCampo.Nombre, subCampo);
                            }
                        }
                    }

                    configuracion.Etiqueta.Add(lcobjCampoConfig.Nombre, campo);
                }
            }

            if (parte == 0 || parte == 4)
            {
                var queryTexto = from c in xml.Root.Element("DT").Descendants("C")
                                    select new CAMPO
                                    {
                                        Nombre = c.Attribute("Nombre").Value,
                                        Longitud = Convert.ToInt32(c.Attribute("Long").Value),
                                        Tipo = c.Attribute("Tipo").Value,
                                        Formato = (c.Attribute("Formato") == null ? "" : c.Attribute("Formato").Value),
                                        Alineacion = (c.Attribute("Alineacion") == null ? "I" : c.Attribute("Alineacion").Value),
                                        Columna = (c.Attribute("Columna").Value == null ? "" : c.Attribute("Columna").Value),
                                    };

                configuracion.Texto = new Dictionary<string, CAMPO>();

                foreach (CAMPO lcobjCampoConfig in queryTexto)
                {
                    campo = new CAMPO();

                    campo.Nombre = lcobjCampoConfig.Nombre;
                    campo.Longitud = lcobjCampoConfig.Longitud;
                    campo.Tipo = lcobjCampoConfig.Tipo; // N Numero, S String
                    campo.Formato = lcobjCampoConfig.Formato; // N Numero, S String
                    campo.Alineacion = lcobjCampoConfig.Alineacion; // I Izquierda, D Derecha - Solo Aplica para String
                    campo.Columna = lcobjCampoConfig.Columna;

                    configuracion.Texto.Add(lcobjCampoConfig.Nombre, campo);
                }
            }

            return configuracion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idSerie">Código de la Serie</param>
        /// <param name="idSubSerie">Código de la Subserie</param>
        /// <returns>Registros de Subseries</returns>
        [HttpGet, ActionName("RegistrosRelacionadosRadicado")]
        public datosConsulta GetRegistrosRelacionadosRadicado(int idUnidadDocumental, string idRadicado)
        {
            var modelfilter = (from radicado in dbSIM.RADICADOS_ETIQUETAS
                         join tipoAnexo in dbSIM.TIPO_ANEXO on radicado.ID_TIPOANEXO equals tipoAnexo.ID_TIPOANEXO into lj
                         from radicadoTipoAnexo in lj.DefaultIfEmpty()
                         where radicado.RADICADO_UNIDADDOCUMENTAL.ID_UNIDADDOCUMENTAL == idUnidadDocumental && radicado.S_IDENTIFICADOR.Contains(idRadicado)
                         orderby radicado.S_ETIQUETA
                         select new
                         {
                             radicado.ID_RADICADO,
                             radicado.S_IDENTIFICADOR,
                             radicado.S_ETIQUETA,
                             S_TIPO = radicado.ID_RADICADOPADRE != null ? (radicado.ID_REGISTROREL == null ? "ANEXO" : "TOMO") : (radicado.ID_REGISTROREL == null ? "RADICADO" : "INVENTARIO"),
                             radicado.ID_TIPOANEXO,
                             S_TIPOANEXO = radicadoTipoAnexo.S_NOMBRE,
                             radicado.ID_RADICADOPADRE
                         });

            var model = from radicadosSel in modelfilter.ToList()
                        where radicadosSel.S_IDENTIFICADOR.Trim().TrimStart('0') == idRadicado.Trim().TrimStart('0')
                        orderby radicadosSel.S_ETIQUETA
                        select new
                        {
                            radicadosSel.ID_RADICADO,
                            radicadosSel.S_IDENTIFICADOR,
                            radicadosSel.S_ETIQUETA,
                            S_CONSECUTIVO = (radicadosSel.ID_RADICADOPADRE == null ? "" : radicadosSel.S_ETIQUETA.Substring(radicadosSel.S_ETIQUETA.Length-10, 10)),
                            radicadosSel.S_TIPO,
                            radicadosSel.S_TIPOANEXO
                        };

            datosConsulta resultado = new datosConsulta();
            resultado.numRegistros = model.Count();
            resultado.datos = model.ToList();

            return resultado;
        }


        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idSerie">Código de la Serie</param>
        /// <param name="idSubSerie">Código de la Subserie</param>
        /// <returns></returns>
        [HttpGet, ActionName("RegistrosRelacionadosExpediente")]
        public datosConsulta GetRegistrosRelacionadosExpediente(int idUnidadDocumental, int idRelacionado)
        {
            var modelfilter = (from radicado in dbSIM.RADICADOS_ETIQUETAS
                               join tipoAnexo in dbSIM.TIPO_ANEXO on radicado.ID_TIPOANEXO equals tipoAnexo.ID_TIPOANEXO into lj
                               from radicadoTipoAnexo in lj.DefaultIfEmpty()
                               where radicado.RADICADO_UNIDADDOCUMENTAL.ID_UNIDADDOCUMENTAL == idUnidadDocumental && radicado.ID_REGISTROREL == idRelacionado && radicado.S_TIPO != null
                               orderby radicado.S_ETIQUETA
                               select new
                               {
                                   radicado.ID_RADICADO,
                                   radicado.S_IDENTIFICADOR,
                                   radicado.S_ETIQUETA,
                                   S_TIPO = (radicado.S_TIPO == "A" ? "ANEXO" : (radicado.S_TIPO == "T" ? "TOMO" : "")),
                                   radicado.ID_TIPOANEXO,
                                   S_TIPOANEXO = radicadoTipoAnexo.S_NOMBRE,
                                   radicado.ID_RADICADOPADRE,
                                   S_CONSECUTIVO = radicado.S_ETIQUETA.Substring(radicado.S_ETIQUETA.Length - 2, 2),
                               });

            datosConsulta resultado = new datosConsulta();
            resultado.numRegistros = modelfilter.Count();
            resultado.datos = modelfilter.ToList();

            return resultado;
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idTipoExpediente">Identificador del Tipo de Expediente</param>
        /// <param name="idRelacionado">Id del Registro Relacionado de acuerdo al Tipo de Expediente</param>
        /// <returns></returns>
        [HttpGet, ActionName("RegistrosRelacionadosExpediente")]
        public datosConsulta GetRegistrosRelacionadosExpediente(int idTipoExpediente, int idRelacionado)
        {
            var modelfilter = (from radicado in dbSIM.RADICADOS_ETIQUETAS
                               join tipoAnexo in dbSIM.TIPO_ANEXO on radicado.ID_TIPOANEXO equals tipoAnexo.ID_TIPOANEXO into lj
                               from radicadoTipoAnexo in lj.DefaultIfEmpty()
                               where radicado.RADICADO_UNIDADDOCUMENTAL.ID_UNIDADDOCUMENTAL == idTipoExpediente && radicado.ID_REGISTROREL == idRelacionado && radicado.S_TIPO != null
                               orderby radicado.S_ETIQUETA
                               select new
                               {
                                   radicado.ID_RADICADO,
                                   radicado.S_IDENTIFICADOR,
                                   radicado.S_ETIQUETA,
                                   S_TIPO = (radicado.S_TIPO == "A" ? "ANEXO" : (radicado.S_TIPO == "T" ? "TOMO" : "")),
                                   radicado.ID_TIPOANEXO,
                                   S_TIPOANEXO = radicadoTipoAnexo.S_NOMBRE,
                                   radicado.ID_RADICADOPADRE,
                                   S_CONSECUTIVO = radicado.S_ETIQUETA.Substring(radicado.S_ETIQUETA.Length - 10, 10),
                               });

            datosConsulta resultado = new datosConsulta();
            resultado.numRegistros = modelfilter.Count();
            resultado.datos = modelfilter.ToList();

            return resultado;
        }

        /// <summary>
        /// De acuerdo a un código de Barras, obtiene los datos relacionados a la Serie, Subserie, Unidad Documental y demás datos relacionados
        /// </summary>
        /// <param name="CB">Código de Barras</param>
        /// <returns>Datos relacionados al Código de Barras</returns>
        [HttpGet, ActionName("DatosAsociadosCB")]
        public object GetDatosAsociadosCB(string CB)
        {
            return null;
            /*var model= (from radicado in dbSIM.RADICADOS
                        join 
                        join tipoAnexo in dbSIM.TIPO_ANEXO on radicado.ID_TIPOANEXO equals tipoAnexo.ID_TIPOANEXO into lj
                        from radicadoTipoAnexo in lj.DefaultIfEmpty()
                        where radicado.RADICADO_UNIDADDOCUMENTAL.ID_UNIDADDOCUMENTAL == idUnidadDocumental && radicado.S_IDENTIFICADOR.Contains(idRadicado)
                        orderby radicado.S_ETIQUETA
                        select new
                        {
                            radicado.ID_RADICADO,
                            radicado.S_IDENTIFICADOR,
                            radicado.S_ETIQUETA,
                            S_TIPO = radicado.ID_RADICADOPADRE != null ? (radicado.ID_REGISTROREL == null ? "ANEXO" : "TOMO") : (radicado.ID_REGISTROREL == null ? "RADICADO" : "INVENTARIO"),
                            radicado.ID_TIPOANEXO,
                            S_TIPOANEXO = radicadoTipoAnexo.S_NOMBRE,
                            radicado.ID_RADICADOPADRE
                        });


            datosConsulta resultado = new datosConsulta();
            resultado.numRegistros = model.Count();
            resultado.datos = model.ToList();

            return resultado;*/
        }

        [HttpGet, ActionName("Foliar")]
        public object GetFoliar(string CBTomo, string CBDocumento, int folioInicial, int folioFinal)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero;
            datosRespuesta resultado;
            DateTime fechaCreacion = DateTime.Now;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            }
            else
            {
                // Error, el usuario logueado no tiene un tercero asociado y por lo tanto no podría registrarse el campo ID_TERCERO
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El usuario logueado no tiene un tercero asociado.", id = "" };
                return resultado;
            }

            CBTomo = CBTomo.Trim();
            CBDocumento = CBDocumento.Trim();

            var tomo = (from tomos in dbSIM.TOMOS
                        join radicados in dbSIM.RADICADOS_ETIQUETAS on tomos.ID_RADICADO equals radicados.ID_RADICADO
                        where radicados.S_ETIQUETA == CBTomo
                        select new { tomos.ID_TOMO }).FirstOrDefault();

            if (tomo == null)
            {
                // Error, el codigo de barras no existe
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El Código de Barras NO Existe o NO pertenece a un Tomo.", id = "" };
                return resultado;
            }

            var documento = (from documentos in dbSIM.DOCUMENTOS
                        join radicados in dbSIM.RADICADOS_ETIQUETAS on documentos.ID_RADICADO equals radicados.ID_RADICADO
                        where radicados.S_ETIQUETA == CBDocumento
                        select new { documentos.ID_DOCUMENTO }).FirstOrDefault();

            if (documento == null)
            {
                // Error, el codigo de barras no existe
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El Código de Barras NO Existe o NO pertenece a un Documento.", id = "" };
                return resultado;
            }

            var folioDocumento = (from documentosTomo in dbSIM.DOCUMENTOS_TOMO
                                  where documentosTomo.DOCUMENTOS.RADICADOS.S_ETIQUETA == CBDocumento
                                  select new { documentosTomo.ID_DOCUMENTOTOMO }).FirstOrDefault();

            if (folioDocumento != null && folioDocumento.ID_DOCUMENTOTOMO > 0)
            {
                // Error, el documento ya fue foliado
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El Documento ya ha sido foliado.", id = "" };
                return resultado;
            }

            DOCUMENTOS_TOMO foliado = new DOCUMENTOS_TOMO();
            foliado.ID_DOCUMENTO = documento.ID_DOCUMENTO;
            foliado.ID_TOMO = tomo.ID_TOMO;
            foliado.ID_TERCERO = idTercero;
            foliado.D_CREACION = fechaCreacion;
            foliado.N_FOLIOINICIAL = folioInicial;
            foliado.N_FOLIOFINAL = folioFinal;
            dbSIM.Entry(foliado).State = EntityState.Added;

            try
            {
                dbSIM.SaveChanges();
                resultado = new datosRespuesta() { tipoRespuesta = "OK", detalleRespuesta = "", id = foliado.ID_DOCUMENTOTOMO.ToString() };
            } catch (Exception error)
            {
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "Error registrando el foliado del Documento.", id = null };
            }

            return resultado;
        }
    }
}