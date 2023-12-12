using DocumentFormat.OpenXml.EMMA;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using SIM.Data.Documental;
using SIM.Data.General;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web.Http;

namespace SIM.Areas.General.Controllers
{
    public class PrestamoApiController : ApiController
    {
        public struct datosPrestamo
        {
            public int idTercero;
            public string documentosIDs;
            public string observacion;
        }

        public struct datosDevolucion
        {
            public string documentosIDs;
        }

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        /// <summary>
        /// Estructura con la configuración de la respuesta en las solicitudes.
        /// </summary>
        public struct datosRespuesta
        {
            public string tipoRespuesta; // OK, Error
            public string detalleRespuesta;
            public object radicado;
        }

        public class datosDevolucionTercero
        {
            public int IdPrestamo;
            public string terceroEntrega;
            public string terceroRecibe;
            public string emailEntrega;
            public string emailRecibe;
            public string observacionPrestamo;
            public string observacionDevolucion;
            public StringBuilder registrosDevolucion;
        }

        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso a la base de datos
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// Obtiene los datos básicos del radicado sin prestar con el código de barras suministrado.
        /// </summary>
        /// <param name="radicado">Código de barras del radicado que se desea obtener</param>
        /// <returns>Registro del radicado</returns>
        [HttpGet, ActionName("ConsultaRadicado")]
        public datosRespuesta GetConsultaRadicado(string radicado)
        {
            datosRespuesta resultado;

            var model = (from radicadoUD in dbSIM.RADICADOS_ETIQUETAS
                         where radicadoUD.S_ETIQUETA == radicado
                         select new
                         {
                             radicadoUD.ID_RADICADO,
                             radicadoUD.S_ETIQUETA,
                             S_TIPO = radicadoUD.RADICADO_UNIDADDOCUMENTAL.TIPO_RADICADO.S_NOMBRE
                         }).FirstOrDefault();

            if (model == null)
            {
                resultado = new datosRespuesta();
                resultado.tipoRespuesta = "Error";
                resultado.detalleRespuesta = "No Existe Documento asociado al Código de Barras ingresado.";

                return resultado;
            }

            var prestamo = (from prestamos in dbSIM.PRESTAMO_DETALLE
                            where prestamos.ID_RADICADO == model.ID_RADICADO && prestamos.D_DEVOLUCION == null
                            select new { prestamos.ID_PRESTAMODETALLE }).FirstOrDefault();

            if (prestamo != null)
            {
                resultado = new datosRespuesta();
                resultado.tipoRespuesta = "Error";
                resultado.detalleRespuesta = "El documento se encuentra prestado.";

                return resultado;
            }

            resultado = new datosRespuesta();
            resultado.tipoRespuesta = "OK";
            resultado.detalleRespuesta = "";
            resultado.radicado = model;

            return resultado;
        }

        /// <summary>
        /// Obtiene los datos básicos del radicado prestado con el código de barras suministrado.
        /// </summary>
        /// <param name="radicado">Código de barras del radicado que se desea obtener</param>
        /// <returns>Registro del radicado</returns>
        [HttpGet, ActionName("ConsultaRadicadoPrestado")]
        public datosRespuesta GetConsultaRadicadoPrestado(string radicado)
        {
            datosRespuesta resultado;

            var model = (from radicadoUD in dbSIM.RADICADOS_ETIQUETAS
                         where radicadoUD.S_ETIQUETA == radicado
                         select new
                         {
                             radicadoUD.ID_RADICADO,
                             radicadoUD.S_ETIQUETA,
                             S_TIPO = radicadoUD.RADICADO_UNIDADDOCUMENTAL.TIPO_RADICADO.S_NOMBRE
                         }).FirstOrDefault();

            if (model == null)
            {
                resultado = new datosRespuesta();
                resultado.tipoRespuesta = "Error";
                resultado.detalleRespuesta = "No Existe Documento asociado al Código de Barras ingresado.";

                return resultado;
            }

            var prestamo = (from prestamos in dbSIM.PRESTAMOS
                            join prestamoDetalle in dbSIM.PRESTAMO_DETALLE on prestamos.ID_PRESTAMO equals prestamoDetalle.ID_PRESTAMO
                            join tercero in dbSIM.TERCERO on prestamos.ID_TERCEROPRESTAMO equals tercero.ID_TERCERO
                            where prestamoDetalle.ID_RADICADO == model.ID_RADICADO && prestamoDetalle.D_DEVOLUCION == null
                            select new
                            {
                                prestamoDetalle.ID_PRESTAMODETALLE,
                                OBSERVACIONES_PRESTAMO = prestamoDetalle.S_DESCRIPCION,
                                prestamos.D_PRESTAMO,
                                S_TERCERO = tercero.S_RSOCIAL
                            }).FirstOrDefault();

            if (prestamo == null)
            {
                resultado = new datosRespuesta();
                resultado.tipoRespuesta = "Error";
                resultado.detalleRespuesta = "El documento NO se encuentra prestado.";

                return resultado;
            }

            resultado = new datosRespuesta();
            resultado.tipoRespuesta = "OK";
            resultado.detalleRespuesta = "";
            resultado.radicado = new
            {
                prestamo.ID_PRESTAMODETALLE,
                model.ID_RADICADO,
                model.S_ETIQUETA,
                model.S_TIPO,
                prestamo.OBSERVACIONES_PRESTAMO,
                prestamo.D_PRESTAMO,
                prestamo.S_TERCERO
            };

            return resultado;
        }

        /// <summary>
        /// Consulta la ubicación de un documento de acuerdo al Código de Barras suministrado
        /// </summary>
        /// <param name="CB">Código de barras del radicado que se desea obtener</param>
        /// <returns>Registro del radicado</returns>
        [HttpGet, ActionName("ConsultaUbicacionDocumento")]
        public datosRespuesta GetConsultaUbicacionDocumento(string CB)
        {
            datosRespuesta resultado;

            var model = (from etiquetaCB in dbSIM.RADICADOS_ETIQUETAS
                         where etiquetaCB.S_ETIQUETA == CB
                         select new
                         {
                             etiquetaCB.ID_RADICADO,
                             etiquetaCB.S_ETIQUETA,
                             S_TIPO = etiquetaCB.RADICADO_UNIDADDOCUMENTAL.TIPO_RADICADO.S_NOMBRE,
                             etiquetaCB.S_UBICACION
                         }).FirstOrDefault();

            if (model == null)
            {
                resultado = new datosRespuesta();
                resultado.tipoRespuesta = "Error";
                resultado.detalleRespuesta = "No Existe Documento asociado al Código de Barras ingresado.";

                return resultado;
            }

            resultado = new datosRespuesta();
            resultado.tipoRespuesta = "OK";
            resultado.detalleRespuesta = "";
            resultado.radicado = new
            {
                model.S_UBICACION
            };

            return resultado;
        }

        /// <summary>
        /// Reubica un documento de acuerdo al Código de Barras suministrado
        /// </summary>
        /// <param name="CB">Código de barras del radicado que se desea obtener</param>
        /// <returns>OK, Error</returns>
        [HttpGet, ActionName("ReUbicacionDocumento")]
        public datosRespuesta GetReUbicacionDocumento(string CB, string ubicacion)
        {
            datosRespuesta resultado;

            RADICADOS_ETIQUETAS etiquetaDocumento = dbSIM.RADICADOS_ETIQUETAS.Where(et => et.S_ETIQUETA == CB.Trim().ToUpper()).FirstOrDefault();

            if (etiquetaDocumento == null)
            {
                resultado = new datosRespuesta();
                resultado.tipoRespuesta = "Error";
                resultado.detalleRespuesta = "No Existe Documento asociado al Código de Barras ingresado.";

                return resultado;
            }
            else
            {
                etiquetaDocumento.S_UBICACION = ubicacion.Trim().ToUpper();

                dbSIM.Entry(etiquetaDocumento).State = EntityState.Modified;
                dbSIM.SaveChanges();

                resultado = new datosRespuesta();
                resultado.tipoRespuesta = "OK";
                resultado.detalleRespuesta = "Documento Reubicado Satisfactoriamente";
            }

            return resultado;
        }

        [HttpPost, ActionName("PrestamoDocumentos")]
        public datosRespuesta PostPrestamoDocumentos(datosPrestamo datosPrestamoTercero)
        {
            datosRespuesta resultado = new datosRespuesta();
            PRESTAMO_DETALLE modelPrestamosDetalle;
            int idPrestamo;
            int idDocumentoSel;
            string observacionDocumento;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            StringBuilder prestamoExpedientesAnexos = new StringBuilder();

            PRESTAMOS modelPrestamos = new PRESTAMOS();
            modelPrestamos.D_PRESTAMO = DateTime.Now;
            modelPrestamos.S_OBSERVACION = datosPrestamoTercero.observacion;
            modelPrestamos.ID_TERCEROPRESTA = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            modelPrestamos.ID_TERCEROPRESTAMO = datosPrestamoTercero.idTercero;

            dbSIM.Entry(modelPrestamos).State = EntityState.Added;
            dbSIM.SaveChanges();

            string terceroEntrega = "";
            string terceroRecibe = "";
            string emailEntrega = "";
            string emailEntregaFuncionario = null;
            string emailRecibe = "";
            string emailRecibeFuncionario = null;
            TERCERO tercero;

            emailEntregaFuncionario = dbSIM.Database.SqlQuery<string>(
                        "SELECT f.EMAIL " +
                        "FROM SEGURIDAD.PROPIETARIO p INNER JOIN " +
                        "  SEGURIDAD.USUARIO_FUNCIONARIO uf ON p.ID_USUARIO = uf.ID_USUARIO INNER JOIN " +
                        "  TRAMITES.TBFUNCIONARIO f ON uf.CODFUNCIONARIO = f.CODFUNCIONARIO " +
                        "WHERE p.ID_TERCERO = " + modelPrestamos.ID_TERCEROPRESTA.ToString()).FirstOrDefault();

            emailRecibeFuncionario = dbSIM.Database.SqlQuery<string>(
                        "SELECT f.EMAIL " +
                        "FROM SEGURIDAD.PROPIETARIO p INNER JOIN " +
                        "  SEGURIDAD.USUARIO_FUNCIONARIO uf ON p.ID_USUARIO = uf.ID_USUARIO INNER JOIN " +
                        "  TRAMITES.TBFUNCIONARIO f ON uf.CODFUNCIONARIO = f.CODFUNCIONARIO " +
                        "WHERE p.ID_TERCERO = " + modelPrestamos.ID_TERCEROPRESTAMO.ToString()).FirstOrDefault();

            tercero = dbSIM.TERCERO.Where(t => t.ID_TERCERO == modelPrestamos.ID_TERCEROPRESTA).FirstOrDefault();
            if (tercero != null)
            {
                terceroEntrega = tercero.S_RSOCIAL;

                emailEntrega = emailEntregaFuncionario ?? tercero.S_CORREO;
            }

            tercero = dbSIM.TERCERO.Where(t => t.ID_TERCERO == modelPrestamos.ID_TERCEROPRESTAMO).FirstOrDefault();
            if (tercero != null)
            {
                terceroRecibe = tercero.S_RSOCIAL;
                emailRecibe = emailRecibeFuncionario ?? tercero.S_CORREO;
            }


            idPrestamo = modelPrestamos.ID_PRESTAMO;

            DateTime fechaEntrega;
            int diasTotales;
            int diasNoLaborales;
            int diasPrestamo = Convert.ToInt32(SIM.Utilidades.Data.ObtenerValorParametro("PrestamosDias"));

            fechaEntrega = DateTime.Today.AddDays(diasPrestamo);

            diasTotales = (fechaEntrega - DateTime.Today).Days;
            diasNoLaborales = dbSIM.DIAS_NOLABORABLES.Where(dnl => dnl.D_DIA >= DateTime.Today && dnl.D_DIA <= fechaEntrega).Count();

            while (diasTotales - diasNoLaborales < diasPrestamo)
            {
                fechaEntrega = fechaEntrega.AddDays(diasPrestamo - (diasTotales - diasNoLaborales));

                diasTotales = (fechaEntrega - DateTime.Today).Days;
                diasNoLaborales = dbSIM.DIAS_NOLABORABLES.Where(dnl => dnl.D_DIA >= DateTime.Today && dnl.D_DIA <= fechaEntrega).Count();
            }

            foreach (string idDocumento in datosPrestamoTercero.documentosIDs.Split('^'))
            {
                try
                {
                    observacionDocumento = idDocumento.Split('|')[1];
                    idDocumentoSel = Convert.ToInt32(idDocumento.Split('|')[0]);

                    modelPrestamosDetalle = new PRESTAMO_DETALLE();
                    modelPrestamosDetalle.ID_PRESTAMO = idPrestamo;
                    modelPrestamosDetalle.ID_RADICADO = idDocumentoSel;
                    modelPrestamosDetalle.S_DESCRIPCION = observacionDocumento;
                    modelPrestamosDetalle.D_HASTA = fechaEntrega;

                    dbSIM.Entry(modelPrestamosDetalle).State = EntityState.Added;
                    dbSIM.SaveChanges();

                    var etiqueta = dbSIM.RADICADOS_ETIQUETAS.Where(et => et.ID_RADICADO == modelPrestamosDetalle.ID_RADICADO).FirstOrDefault();

                    if (etiqueta != null)
                        prestamoExpedientesAnexos.AppendLine("<tr><td>" + etiqueta.S_TIPO + etiqueta.S_CONSECUTIVOTIPO + "</td>" + "<td>" + etiqueta.S_TEXTO + "</td>" + "<td>" + observacionDocumento + "</td></tr>");
                }
                catch (Exception lcobjError)
                {
                    resultado.tipoRespuesta = "Error";
                    resultado.detalleRespuesta = lcobjError.Message;

                    return resultado;
                }
            }

            string emailFrom;
            string emailSMTPServer;
            string emailSMTPUser;
            string emailSMTPPwd;
            StringBuilder emailHtml;

            emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
            emailSMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
            emailSMTPUser = ConfigurationManager.AppSettings["SMTPUser"];
            emailSMTPPwd = ConfigurationManager.AppSettings["SMTPPwd"];

            try
            {
                emailHtml = new StringBuilder(File.ReadAllText(HostingEnvironment.MapPath("~/Content/plantillas/PlantillaCorreoPrestamoDevolucion.html")));
                emailHtml.Replace("[PrestamoDevolucion]", "el PR&Eacute;STAMO");
                emailHtml.Replace("[ExpedientesAnexos]", prestamoExpedientesAnexos.ToString());
                emailHtml.Replace("[Observaciones]", datosPrestamoTercero.observacion);
                emailHtml.Replace("[EntregadoPor]", terceroEntrega);
                emailHtml.Replace("[RecibidoPor]", terceroRecibe);
            }
            catch (Exception error)
            {
                return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "Error Generando Correo de Notificación de Préstamo" };
            }

            try
            {
                Utilidades.Email.EnviarEmail(emailFrom, emailEntrega + ";" + emailRecibe, "Notificación de Préstamo de Tomos y/o Anexos", emailHtml.ToString(), emailSMTPServer, true, emailSMTPUser, emailSMTPPwd);
            }
            catch (Exception error)
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), Utilidades.LogErrores.ObtenerError(error));
                return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "Error Enviando Correo de Notificación de Préstamo de Tomos y/o Anexos" };
            }

            resultado.tipoRespuesta = "OK";
            resultado.detalleRespuesta = "Préstamo Realizado Satisfactoriamente.";

            return resultado;
        }

        [HttpPost, ActionName("DevolucionDocumentos")]
        public datosRespuesta PostDevolucionDocumentos(datosDevolucion datosDevolucion)
        {
            /*public class datosDevolucion
        {
            public int IdPrestamo;
            string terceroEntrega;
            string terceroRecibe;
            string emailEntrega;
            string emailRecibe;
            StringBuilder registrosDevolucion;
        }*/

            datosRespuesta resultado = new datosRespuesta();
            int idDocumentoSel;
            string observacionDocumento;
            List<int> listaDocumentos = new List<int>();
            Dictionary<int, string> observacionesDocumentos = new Dictionary<int, string>();
            DateTime fechaActual = DateTime.Now;
            int idTercero;
            Dictionary<int, datosDevolucionTercero> datosDevoluciones = new Dictionary<int, datosDevolucionTercero>();
            datosDevolucionTercero documentoActual;

            System.Web.HttpContext context = System.Web.HttpContext.Current;

            idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            string terceroRecibe = "";
            string emailRecibe = "";
            string emailRecibeFuncionario = null;
            string emailEntregaFuncionario = null;
            TERCERO tercero;

            emailRecibeFuncionario = dbSIM.Database.SqlQuery<string>(
                        "SELECT f.EMAIL " +
                        "FROM SEGURIDAD.PROPIETARIO p INNER JOIN " +
                        "  SEGURIDAD.USUARIO_FUNCIONARIO uf ON p.ID_USUARIO = uf.ID_USUARIO INNER JOIN " +
                        "  TRAMITES.TBFUNCIONARIO f ON uf.CODFUNCIONARIO = f.CODFUNCIONARIO " +
                        "WHERE p.ID_TERCERO = " + idTercero.ToString()).FirstOrDefault();

            tercero = dbSIM.TERCERO.Where(t => t.ID_TERCERO == idTercero).FirstOrDefault();
            if (tercero != null)
            {
                terceroRecibe = tercero.S_RSOCIAL;
                emailRecibe = emailRecibeFuncionario ?? tercero.S_CORREO;
            }

            foreach (string idDocumento in datosDevolucion.documentosIDs.Split('^'))
            {
                observacionDocumento = idDocumento.Split('|')[1];
                idDocumentoSel = Convert.ToInt32(idDocumento.Split('|')[0]);

                listaDocumentos.Add(idDocumentoSel);
                observacionesDocumentos.Add(idDocumentoSel, observacionDocumento);
            }

            var documentosDevueltos = from documentos in dbSIM.PRESTAMO_DETALLE
                                      where listaDocumentos.Contains(documentos.ID_PRESTAMODETALLE)
                                      select documentos;

            try
            {
                foreach (PRESTAMO_DETALLE documentoDevuelto in documentosDevueltos.ToList())
                {
                    documentoDevuelto.ID_TERCERORECIBE = idTercero;
                    documentoDevuelto.D_DEVOLUCION = fechaActual;
                    documentoDevuelto.S_OBSERVACIONDEVOLUCION = observacionesDocumentos[documentoDevuelto.ID_PRESTAMODETALLE];
                    dbSIM.Entry(documentoDevuelto).State = EntityState.Modified;

                    var etiqueta = dbSIM.RADICADOS_ETIQUETAS.Where(et => et.ID_RADICADO == documentoDevuelto.ID_RADICADO).FirstOrDefault();
                    if (datosDevoluciones.ContainsKey(documentoDevuelto.ID_PRESTAMO))
                    {
                        documentoActual = datosDevoluciones[documentoDevuelto.ID_PRESTAMO];
                        documentoActual.registrosDevolucion.AppendLine("<tr><td>" + etiqueta.S_TIPO + "</td>" + "<td>" + etiqueta.S_TEXTO + "</td>" + "<td>" + documentoDevuelto.S_OBSERVACIONDEVOLUCION + "</td></tr>");
                    }
                    else
                    {
                        var prestamo = dbSIM.PRESTAMOS.Where(p => p.ID_PRESTAMO == documentoDevuelto.ID_PRESTAMO).FirstOrDefault();
                        var terceroPrestamo = dbSIM.TERCERO.Where(t => t.ID_TERCERO == prestamo.ID_TERCEROPRESTAMO).FirstOrDefault();

                        emailEntregaFuncionario = dbSIM.Database.SqlQuery<string>(
                            "SELECT f.EMAIL " +
                            "FROM SEGURIDAD.PROPIETARIO p INNER JOIN " +
                            "  SEGURIDAD.USUARIO_FUNCIONARIO uf ON p.ID_USUARIO = uf.ID_USUARIO INNER JOIN " +
                            "  TRAMITES.TBFUNCIONARIO f ON uf.CODFUNCIONARIO = f.CODFUNCIONARIO " +
                            "WHERE p.ID_TERCERO = " + prestamo.ID_TERCEROPRESTAMO.ToString()).FirstOrDefault();

                        datosDevolucionTercero nuevoRegistro = new datosDevolucionTercero();
                        nuevoRegistro.IdPrestamo = documentoDevuelto.ID_PRESTAMO;
                        nuevoRegistro.terceroEntrega = terceroPrestamo.S_RSOCIAL;
                        nuevoRegistro.terceroRecibe = terceroRecibe;
                        nuevoRegistro.emailEntrega = emailEntregaFuncionario ?? terceroPrestamo.S_CORREO;
                        nuevoRegistro.emailRecibe = emailRecibe;
                        nuevoRegistro.observacionPrestamo = prestamo.S_OBSERVACION;
                        nuevoRegistro.observacionDevolucion = documentoDevuelto.S_OBSERVACIONDEVOLUCION;
                        nuevoRegistro.registrosDevolucion = new StringBuilder();
                        nuevoRegistro.registrosDevolucion.AppendLine("<tr><td>" + etiqueta.S_TIPO + etiqueta.S_CONSECUTIVOTIPO + "</td>" + "<td>" + etiqueta.S_TEXTO + "</td>" + "<td>" + documentoDevuelto.S_OBSERVACIONDEVOLUCION + "</td></tr>");

                        datosDevoluciones.Add(documentoDevuelto.ID_PRESTAMO, nuevoRegistro);
                    }
                }
                dbSIM.SaveChanges();
            }
            catch (Exception lcobjError)
            {
                resultado.tipoRespuesta = "Error";
                resultado.detalleRespuesta = lcobjError.Message;

                return resultado;
            }

            foreach (KeyValuePair<int, datosDevolucionTercero> entry in datosDevoluciones)
            {
                string emailFrom;
                string emailSMTPServer;
                string emailSMTPUser;
                string emailSMTPPwd;
                StringBuilder emailHtml;

                emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
                emailSMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
                emailSMTPUser = ConfigurationManager.AppSettings["SMTPUser"];
                emailSMTPPwd = ConfigurationManager.AppSettings["SMTPPwd"];

                try
                {
                    emailHtml = new StringBuilder(File.ReadAllText(HostingEnvironment.MapPath("~/Content/plantillas/PlantillaCorreoPrestamoDevolucion.html")));
                    emailHtml.Replace("[PrestamoDevolucion]", "la DEVOLUCI&Oacute;N");
                    emailHtml.Replace("[ExpedientesAnexos]", entry.Value.registrosDevolucion.ToString());
                    emailHtml.Replace("[Observaciones]", entry.Value.observacionPrestamo + "<br>" + entry.Value.observacionDevolucion);
                    emailHtml.Replace("[EntregadoPor]", entry.Value.terceroEntrega);
                    emailHtml.Replace("[RecibidoPor]", terceroRecibe);

                    try
                    {
                        Utilidades.Email.EnviarEmail(emailFrom, entry.Value.emailEntrega + ";" + emailRecibe, "Notificación de Devolución de Tomos y/o Anexos", emailHtml.ToString(), emailSMTPServer, true, emailSMTPUser, emailSMTPPwd);
                    }
                    catch (Exception error)
                    {
                        Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), Utilidades.LogErrores.ObtenerError(error));
                        //return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "Error Enviando Correo de Notificación de Préstamo de Tomos y/o Anexos" };
                    }
                }
                catch (Exception error)
                {
                    Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Error enviando correo de devolución.\r\n" + Utilidades.LogErrores.ObtenerError(error));
                    //return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "Error Generando Correo de Notificación de Préstamo" };
                }
            }

            resultado.tipoRespuesta = "OK";
            resultado.detalleRespuesta = "Devolución Realizada Satisfactoriamente.";

            return resultado;
        }

        /// <summary>
        /// Consulta Documentos
        /// </summary>
        /// <param name="filter">lista de campos con valores de filtros en la consulta</param>
        /// <param name="sort">Campos de ordenamiento</param>
        /// <param name="group">Campos de agrupación</param>
        /// <param name="skip">Para paginar. Indica a partir de qué registro debe cargar</param>
        /// <param name="take">Para paginar. Indica cuantos registros debe cargar (tamaño de página)</param>
        /// <param name="searchValue">Valor de Búsqueda</param>
        /// <param name="searchExpr">Campo de Búsqueda</param>
        /// <param name="comparation">Tipo de comparación en la búsqueda</param>
        /// <param name="tipoData">f: Carga consulta con todos los campos, r: Consulta con campos reducidos, l: Consulta con campos para ComboBox (LookUp)</param>
        /// <param name="noFilterNoRecords">Si es verdadero y no hay filtros, no retorna registros. Si es falso y no hay filtros, retorna todos los datos de acuerdo a la paginación</param>
        /// <returns>Registros resultado de la consulta</returns>
        [HttpGet, ActionName("BusquedaDocumentos")]
        public datosConsulta GetBusquedaDocumentos(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, bool noFilterNoRecords)
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
                /*var model = (from radicado in dbSIM.RADICADOS_ETIQUETAS
                             join prestamo in dbSIM.PRESTAMO_DETALLE on radicado.ID_RADICADO equals prestamo.ID_RADICADO into lj1
                             from prestamoRadicado in lj1.DefaultIfEmpty()
                             join tipoAnexo in dbSIM.TIPO_ANEXO on radicado.ID_TIPOANEXO equals tipoAnexo.ID_TIPOANEXO into lj2
                             from prestamoTipoAnexo in lj2.DefaultIfEmpty()
                             where radicado.S_TIPO != null
                             orderby radicado.RADICADO_UNIDADDOCUMENTAL.UNIDAD_DOCUMENTAL.S_NOMBRE, radicado.S_TEXTO, radicado.S_TIPO
                             select new
                             {
                                 S_UNIDADDOCUMENTAL = radicado.RADICADO_UNIDADDOCUMENTAL.UNIDAD_DOCUMENTAL.S_NOMBRE,
                                 S_TIPO = radicado.S_TIPO + " " + radicado.N_CONSECUTIVOTIPO.ToString(),
                                 S_TIPOANEXO = prestamoTipoAnexo.S_NOMBRE,
                                 radicado.S_TEXTO,
                                 radicado.S_UBICACION,
                                 S_ESTADO = (prestamoRadicado == null ? "DISPONIBLE" : (prestamoRadicado.D_DEVOLUCION == null ? "PRESTADO" : "DISPONIBLE"))
                             });*/

                var model = (from radicado in dbSIM.RADICADOS_ETIQUETAS
                             join prestamoDetalle in dbSIM.PRESTAMO_DETALLE.Where(pd => pd.D_DEVOLUCION == null) on radicado.ID_RADICADO equals prestamoDetalle.ID_RADICADO into lj0
                             from prestamoDetalleRadicado in lj0.DefaultIfEmpty()
                             join prestamo in dbSIM.PRESTAMOS on prestamoDetalleRadicado.ID_PRESTAMO equals prestamo.ID_PRESTAMO into lj1
                             from prestamoRadicado in lj1.DefaultIfEmpty()
                             join tercero in dbSIM.NATURAL on prestamoRadicado.ID_TERCEROPRESTAMO equals tercero.ID_TERCERO into lj2
                             from prestamoTercero in lj2.DefaultIfEmpty()
                             join tipoAnexo in dbSIM.TIPO_ANEXO on radicado.ID_TIPOANEXO equals tipoAnexo.ID_TIPOANEXO into lj3
                             from prestamoTipoAnexo in lj3.DefaultIfEmpty()
                             where radicado.S_TIPO != null && (prestamoRadicado.D_PRESTAMO == null || prestamoDetalleRadicado.D_DEVOLUCION == null)
                             orderby radicado.RADICADO_UNIDADDOCUMENTAL.UNIDAD_DOCUMENTAL.S_NOMBRE, radicado.S_TEXTO, radicado.S_TIPO
                             select new
                             {
                                 //S_UNIDADDOCUMENTAL = radicado.RADICADO_UNIDADDOCUMENTAL.UNIDAD_DOCUMENTAL.S_NOMBRE,
                                 S_TIPOEXPEDIENTE = radicado.RADICADO_UNIDADDOCUMENTAL.TIPO_RADICADO.S_NOMBRE,
                                 S_TIPO = radicado.S_TIPO + " " + radicado.S_CONSECUTIVOTIPO,
                                 S_TIPOANEXO = prestamoTipoAnexo.S_NOMBRE,
                                 radicado.S_TEXTO,
                                 radicado.S_UBICACION,
                                 S_ESTADO = (prestamoDetalleRadicado == null ? "DISPONIBLE" : (prestamoDetalleRadicado.D_DEVOLUCION == null ? "PRESTADO" : "DISPONIBLE")),
                                 S_PRESTADOPOR = (prestamoDetalleRadicado.D_DEVOLUCION == null ? prestamoTercero.S_NOMBRE1 + " " + prestamoTercero.S_NOMBRE2 + " " + prestamoTercero.S_APELLIDO1 + " " + prestamoTercero.S_APELLIDO2 : "")
                             });

                modelData = model;

                // Obtiene consulta linq dinámicamente de acuerdo a los filtros establecidos
                //IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, "", sort, group);
                IQueryable<dynamic> modelFilteredFullText = SIM.Utilidades.Data.ObtenerConsultaDinamicaFullText(modelData, "S_TEXTO," + filter);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFilteredFullText.Count();
                resultado.datos = modelFilteredFullText.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        /// <summary>
        /// Consulta Documentos
        /// </summary>
        /// <param name="filter">lista de campos con valores de filtros en la consulta</param>
        /// <param name="sort">Campos de ordenamiento</param>
        /// <param name="group">Campos de agrupación</param>
        /// <param name="skip">Para paginar. Indica a partir de qué registro debe cargar</param>
        /// <param name="take">Para paginar. Indica cuantos registros debe cargar (tamaño de página)</param>
        /// <param name="searchValue">Valor de Búsqueda</param>
        /// <param name="searchExpr">Campo de Búsqueda</param>
        /// <param name="comparation">Tipo de comparación en la búsqueda</param>
        /// <param name="tipoData">f: Carga consulta con todos los campos, r: Consulta con campos reducidos, l: Consulta con campos para ComboBox (LookUp)</param>
        /// <param name="noFilterNoRecords">Si es verdadero y no hay filtros, no retorna registros. Si es falso y no hay filtros, retorna todos los datos de acuerdo a la paginación</param>
        /// <returns>Registros resultado de la consulta</returns>
        [Authorize(Roles = "VPRESTAMO")]
        [HttpGet, ActionName("ConsultaPrestamo")]
        public datosConsulta GetConsultaPrestamo(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, bool noFilterNoRecords, int? identificacion, string prestadoPor, int estadoPrestamo, int? diasPrestado, string fechaInicial, string fechaFinal, string texto)
        {
            dynamic modelData;
            DateTime? fechaInicialSel = null;
            DateTime? fechaFinalSel = null;

            if (fechaInicial != null && fechaInicial != "")
                fechaInicialSel = DateTime.ParseExact(fechaInicial, "yyyy/MM/dd", CultureInfo.InvariantCulture);

            if (fechaFinal != null && fechaFinal != "")
                fechaFinalSel = DateTime.ParseExact(fechaFinal, "yyyy/MM/dd", CultureInfo.InvariantCulture);

            if ((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)
            {
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var model = (from radicado in dbSIM.RADICADOS_ETIQUETAS
                             join prestamoDetalle in dbSIM.PRESTAMO_DETALLE on radicado.ID_RADICADO equals prestamoDetalle.ID_RADICADO
                             join prestamo in dbSIM.PRESTAMOS on prestamoDetalle.ID_PRESTAMO equals prestamo.ID_PRESTAMO
                             join tercero in dbSIM.NATURAL on prestamo.ID_TERCEROPRESTAMO equals tercero.ID_TERCERO into lj2
                             from prestamoTercero in lj2.DefaultIfEmpty()
                             join tipoAnexo in dbSIM.TIPO_ANEXO on radicado.ID_TIPOANEXO equals tipoAnexo.ID_TIPOANEXO into lj3
                             from prestamoTipoAnexo in lj3.DefaultIfEmpty()
                             where radicado.S_TIPO != null && (estadoPrestamo == 0 || (estadoPrestamo == 1 && prestamoDetalle.D_DEVOLUCION == null) || (estadoPrestamo == 2 && prestamoDetalle.D_DEVOLUCION != null)) && (diasPrestado == null || (prestamoDetalle.D_DEVOLUCION == null ? DbFunctions.DiffDays(DbFunctions.TruncateTime(prestamo.D_PRESTAMO), DateTime.Today) : DbFunctions.DiffDays(DbFunctions.TruncateTime(prestamo.D_PRESTAMO), DbFunctions.TruncateTime(prestamoDetalle.D_DEVOLUCION))) >= diasPrestado) && (identificacion == null || prestamoTercero.TERCERO.N_DOCUMENTON == identificacion) && (prestadoPor == null || (prestamoTercero.S_NOMBRE1 + " " + prestamoTercero.S_NOMBRE2 + " " + prestamoTercero.S_APELLIDO1 + " " + prestamoTercero.S_APELLIDO2).ToUpper().Contains(prestadoPor.ToUpper())) && (fechaInicial == null || DbFunctions.TruncateTime(prestamo.D_PRESTAMO) >= fechaInicialSel) && (fechaFinal == null || DbFunctions.TruncateTime(prestamo.D_PRESTAMO) <= fechaFinalSel)
                             orderby radicado.RADICADO_UNIDADDOCUMENTAL.UNIDAD_DOCUMENTAL.S_NOMBRE, radicado.S_TEXTO, radicado.S_TIPO
                             select new
                             {
                                 //S_UNIDADDOCUMENTAL = radicado.RADICADO_UNIDADDOCUMENTAL.UNIDAD_DOCUMENTAL.S_NOMBRE,
                                 S_TIPOEXPEDIENTE = radicado.RADICADO_UNIDADDOCUMENTAL.TIPO_RADICADO.S_NOMBRE,
                                 S_TIPO = radicado.S_TIPO + " " + radicado.S_CONSECUTIVOTIPO,
                                 S_TIPOANEXO = prestamoTipoAnexo.S_NOMBRE,
                                 radicado.S_TEXTO,
                                 S_ESTADO = (prestamoDetalle.D_DEVOLUCION == null ? "PRESTADO" : "DEVUELTO"),
                                 S_PRESTADOPOR = (prestamoTercero == null ? "[Sin Datos]" : prestamoTercero.S_NOMBRE1 + " " + prestamoTercero.S_NOMBRE2 + " " + prestamoTercero.S_APELLIDO1 + " " + prestamoTercero.S_APELLIDO2),
                                 prestamo.D_PRESTAMO,
                                 prestamoDetalle.D_DEVOLUCION,
                                 N_DIAS = (prestamoDetalle.D_DEVOLUCION == null ? DbFunctions.DiffDays(DbFunctions.TruncateTime(prestamo.D_PRESTAMO), DateTime.Today) : DbFunctions.DiffDays(DbFunctions.TruncateTime(prestamo.D_PRESTAMO), DbFunctions.TruncateTime(prestamoDetalle.D_DEVOLUCION)))
                             });

                modelData = model;

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, filter, sort, group);
                if (texto != null)
                {
                    modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamicaFullText(modelFiltered, "S_TEXTO," + texto);
                }

                datosConsulta resultado = new datosConsulta();

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

                return resultado;
            }
        }

        /// <summary>
        /// Envía correo notificando que la fecha de devolución está próxima
        /// </summary>
        [HttpGet, ActionName("NotificacionDevolucion")]
        public datosRespuesta GetNotificacionDevolucion()
        {
            datosRespuesta resultado;
            int numErrores = 0;

            StringBuilder prestamoExpedientesAnexos = new StringBuilder();

            int diasNotificacionDevolucion = Convert.ToInt32(SIM.Utilidades.Data.ObtenerValorParametro("PrestamosDiasNotificacionDevolucion"));
            DateTime fechaDevolucion = DateTime.Today.AddDays(diasNotificacionDevolucion);

            var prestamosPorVencer = from p in dbSIM.PRESTAMOS
                                            join t in dbSIM.TERCERO on p.ID_TERCEROPRESTA equals t.ID_TERCERO
                                            join pr in dbSIM.PROPIETARIO on p.ID_TERCEROPRESTA equals pr.ID_TERCERO
                                            join uf in dbSIM.USUARIO_FUNCIONARIO on pr.ID_USUARIO equals uf.ID_USUARIO
                                            join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                            join pd in dbSIM.PRESTAMO_DETALLE on p.ID_PRESTAMO equals pd.ID_PRESTAMO
                                            join re in dbSIM.RADICADOS_ETIQUETAS on pd.ID_RADICADO equals re.ID_RADICADO
                                        where pd.D_DEVOLUCION == null && pd.D_HASTA <= fechaDevolucion && pd.D_NOTIFICACION == null
                                        orderby new { p.ID_TERCEROPRESTA, pd.D_HASTA }
                                        select new {
                                            t.ID_TERCERO,
                                            pd.ID_PRESTAMODETALLE,
                                            pd.D_HASTA,
                                            re.S_TIPO,
                                            re.S_CONSECUTIVOTIPO,
                                            re.S_TEXTO,
                                            pd.S_DESCRIPCION,
                                            EMAIL1 = f.EMAIL,
                                            EMAIL2 = t.S_CORREO
                                        };

            int idTercero = -1;
            string email = "";
            string idsPrestamosDetalle = "";

            foreach (var prestamo in prestamosPorVencer)
            {
                if (prestamo.ID_TERCERO != idTercero)
                {
                    if (idTercero != -1) // Enviamos el correo al usuario con los documentos prestados de forma consolidada
                    {
                        var retorno = EnviarEmailNotificacionFechaDevolucion(email, prestamoExpedientesAnexos.ToString());

                        if (retorno.tipoRespuesta == "OK")
                        {
                            dbSIM.Database.ExecuteSqlCommand(
                                "UPDATE DOCUMENTAL.PRESTAMO_DETALLE " +
                                "SET D_NOTIFICACION = SYSDATE  " +
                                "WHERE ID_PRESTAMODETALLE IN (" + idsPrestamosDetalle + ")"
                            );
                        }
                        else
                        {
                            numErrores++;
                        }
                    }

                    prestamoExpedientesAnexos.Clear();
                    idTercero = prestamo.ID_TERCERO;
                    email = prestamo.EMAIL1 ?? prestamo.EMAIL2;
                    idsPrestamosDetalle = "";
                }

                if (idsPrestamosDetalle == "")
                    idsPrestamosDetalle = prestamo.ID_PRESTAMODETALLE.ToString();
                else
                    idsPrestamosDetalle += "," + prestamo.ID_PRESTAMODETALLE.ToString();

                prestamoExpedientesAnexos.AppendLine("<tr><td>" + ((DateTime)prestamo.D_HASTA).ToString("dd/MM/yyyy") + "</td><td>" + prestamo.S_TIPO + prestamo.S_CONSECUTIVOTIPO + "</td>" + "<td>" + prestamo.S_TEXTO + "</td>" + "<td>" + prestamo.S_DESCRIPCION + "</td></tr>");
            }

            if (idTercero != -1) // Enviamos el correo al usuario con los documentos prestados de forma consolidada
            {
                var retorno = EnviarEmailNotificacionFechaDevolucion(email, prestamoExpedientesAnexos.ToString());

                if (retorno.tipoRespuesta == "OK")
                {
                    dbSIM.Database.ExecuteSqlCommand(
                        "UPDATE DOCUMENTAL.PRESTAMO_DETALLE " +
                        "SET D_NOTIFICACION = SYSDATE  " +
                        "WHERE ID_PRESTAMODETALLE IN (" + idsPrestamosDetalle + ")"
                    );
                }
                else
                {
                    numErrores++;
                }
            }

            resultado = new datosRespuesta();

            if (numErrores > 0)
            {
                resultado.tipoRespuesta = "Error";
                resultado.detalleRespuesta = "Por lo menos una notificación de fecha de devolución no pudo enviarse satisfactoriamente.";
            }
            else
            {
                resultado.tipoRespuesta = "OK";
                resultado.detalleRespuesta = "Notificaciones de Fecha de Devolución enviadas satisfactoriamente.";
            }

            return resultado;
        }

        private datosRespuesta EnviarEmailNotificacionFechaDevolucion(string email, string documentos)
        {
            StringBuilder emailHtml;

            var emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
            var emailSMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
            var emailSMTPUser = ConfigurationManager.AppSettings["SMTPUser"];
            var emailSMTPPwd = ConfigurationManager.AppSettings["SMTPPwd"];

            try
            {
                emailHtml = new StringBuilder(File.ReadAllText(HostingEnvironment.MapPath("~/Content/plantillas/PlantillaCorreoPrestamoNotificacionDevolucion.html")));
                emailHtml.Replace("[ExpedientesAnexos]", documentos);

            }
            catch (Exception error)
            {
                return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "Error Generando Correo de Notificación de Préstamo" };
            }

            try
            {
                Utilidades.Email.EnviarEmail(emailFrom, email, "Notificación Fecha de Devolución de Tomos y/o Anexos", emailHtml.ToString(), emailSMTPServer, true, emailSMTPUser, emailSMTPPwd);
            }
            catch (Exception error)
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), Utilidades.LogErrores.ObtenerError(error));
                return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "Error Enviando Correo de Notificación de Fecha de Devolución de Tomos y/o Anexos" };
            }

            return new datosRespuesta { tipoRespuesta = "OK", detalleRespuesta = "" };
        }
    }
}