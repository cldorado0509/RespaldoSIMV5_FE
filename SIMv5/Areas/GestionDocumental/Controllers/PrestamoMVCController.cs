using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using SIM.Areas.GestionDocumental.Models;
using SIM.Areas.General.Models;
using System.Data;
using System.Globalization;
using SIM.Areas.Seguridad.Models;
using SIM.Data;

namespace SIM.Areas.GestionDocumental.Controllers
{
    public class PrestamoMVCController : Controller
    {
        /*EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        //
        // GET: /GestionDocumental/Prestamo/
        [Authorize(Roles = "VPRESTAMO")]
        public ActionResult Prestamo()
        {
            return View();
        }

        [Authorize(Roles = "VPRESTAMO")]
        public ActionResult Devolucion()
        {
            return View();
        }

        public ActionResult ConsultaPrestamos(int? tipoReporte)
        {
            ViewBag.TipoReporte = (tipoReporte == null ? 1 : tipoReporte);

            return View();
        }

        public ActionResult ConsultaPrestamosDatos(int? idTercero, string fechaInicial, string fechaFinal, int? rangoTiempos)
        {
            DateTime fechaInicialSel, fechaFinalSel;

            ViewBag.IdTercero = idTercero;
            ViewBag.FechaInicial = fechaInicial;
            ViewBag.FechaFinal = fechaFinal;
            ViewBag.RangoTiempos = rangoTiempos;

            if (idTercero != null)
            {
                decimal idTerceroSel = Convert.ToDecimal(idTercero);

                var resultadoConsulta = (from prestamos in dbSIM.PRESTAMOS
                                        join detallePrestamos in dbSIM.PRESTAMO_DETALLE on prestamos.ID_PRESTAMO equals detallePrestamos.ID_PRESTAMO
                                        join tipo in dbSIM.PRESTAMO_TIPO on detallePrestamos.ID_TIPOPRESTAMO equals tipo.ID_TIPOPRESTAMO
                                        //join tercero in dbGeneral.NATURAL on prestamos.ID_TERCEROPRESTAMO equals tercero.ID_TERCERO
                                        join identificadores in dbSIM.IDENTIFICADORES on detallePrestamos.ID_IDENTIFICADOR equals identificadores.ID_IDENTIFICADOR
                                        where prestamos.ID_TERCEROPRESTAMO == idTerceroSel && detallePrestamos.D_FECHADEVOLUCION == null
                                        select new
                                        {
                                            detallePrestamos.ID_PRESDETALLE,
                                            prestamos.ID_TERCEROPRESTAMO,
                                            //S_NOMBRETERCERO = tercero.S_NOMBRE1 + (tercero.S_NOMBRE2 != null && tercero.S_NOMBRE2.Trim() != "" ? " " + tercero.S_NOMBRE2 : "") + " " + tercero.S_APELLIDO1 + (tercero.S_APELLIDO2 != null && tercero.S_APELLIDO2.Trim() != "" ? " " + tercero.S_APELLIDO2 : ""),
                                            S_DESCPRESTAMO = identificadores.S_DESCRIPCION,
                                            S_TIPO = tipo.S_NOMBRE,
                                            prestamos.D_FECHAPRESTAMO,
                                            detallePrestamos.D_FECHAHASTA,
                                            detallePrestamos.D_FECHADEVOLUCION
                                        }).ToArray();

                var resultadoConsultaFinal = from prestamos in resultadoConsulta
                                             join tercero in dbSIM.NATURAL on prestamos.ID_TERCEROPRESTAMO equals tercero.ID_TERCERO
                                             select new
                                             {
                                                 prestamos.ID_PRESDETALLE,
                                                 S_NOMBRETERCERO = tercero.S_NOMBRE1 + (tercero.S_NOMBRE2 != null && tercero.S_NOMBRE2.Trim() != "" ? " " + tercero.S_NOMBRE2 : "") + " " + tercero.S_APELLIDO1 + (tercero.S_APELLIDO2 != null && tercero.S_APELLIDO2.Trim() != "" ? " " + tercero.S_APELLIDO2 : ""),
                                                 prestamos.S_DESCPRESTAMO,
                                                 prestamos.S_TIPO,
                                                 prestamos.D_FECHAPRESTAMO,
                                                 prestamos.D_FECHAHASTA,
                                                 prestamos.D_FECHADEVOLUCION
                                             };

                return PartialView("_gvwConsultaPrestamosPartial", resultadoConsultaFinal.ToList());
            }

            if (fechaInicial != null && fechaFinal != null)
            {
                fechaInicialSel = DateTime.ParseExact(fechaInicial, "yyyy/M/d", CultureInfo.InvariantCulture);
                fechaFinalSel = DateTime.ParseExact(fechaFinal, "yyyy/M/d", CultureInfo.InvariantCulture);

                fechaFinalSel = ((DateTime)fechaFinalSel).AddDays(1);
                var resultadoConsulta = (from prestamos in dbSIM.PRESTAMOS
                                        join detallePrestamos in dbSIM.PRESTAMO_DETALLE on prestamos.ID_PRESTAMO equals detallePrestamos.ID_PRESTAMO
                                        join tipo in dbSIM.PRESTAMO_TIPO on detallePrestamos.ID_TIPOPRESTAMO equals tipo.ID_TIPOPRESTAMO
                                        //join tercero in dbGeneral.NATURAL on prestamos.ID_TERCEROPRESTAMO equals tercero.ID_TERCERO
                                        join identificadores in dbSIM.IDENTIFICADORES on detallePrestamos.ID_IDENTIFICADOR equals identificadores.ID_IDENTIFICADOR
                                        where prestamos.D_FECHAPRESTAMO >= fechaInicialSel && prestamos.D_FECHAPRESTAMO < fechaFinalSel
                                        select new
                                        {
                                            detallePrestamos.ID_PRESDETALLE,
                                            prestamos.ID_TERCEROPRESTAMO,
                                            //S_NOMBRETERCERO = tercero.S_NOMBRE1 + (tercero.S_NOMBRE2 != null && tercero.S_NOMBRE2.Trim() != "" ? " " + tercero.S_NOMBRE2 : "") + " " + tercero.S_APELLIDO1 + (tercero.S_APELLIDO2 != null && tercero.S_APELLIDO2.Trim() != "" ? " " + tercero.S_APELLIDO2 : ""),
                                            S_DESCPRESTAMO = identificadores.S_DESCRIPCION,
                                            S_TIPO = tipo.S_NOMBRE,
                                            prestamos.D_FECHAPRESTAMO,
                                            detallePrestamos.D_FECHAHASTA,
                                            detallePrestamos.D_FECHADEVOLUCION
                                        }).ToArray();

                var resultadoConsultaFinal = from prestamos in resultadoConsulta
                                             join tercero in dbSIM.NATURAL on prestamos.ID_TERCEROPRESTAMO equals tercero.ID_TERCERO
                                             select new
                                             {
                                                 prestamos.ID_PRESDETALLE,
                                                 S_NOMBRETERCERO = tercero.S_NOMBRE1 + (tercero.S_NOMBRE2 != null && tercero.S_NOMBRE2.Trim() != "" ? " " + tercero.S_NOMBRE2 : "") + " " + tercero.S_APELLIDO1 + (tercero.S_APELLIDO2 != null && tercero.S_APELLIDO2.Trim() != "" ? " " + tercero.S_APELLIDO2 : ""),
                                                 prestamos.S_DESCPRESTAMO,
                                                 prestamos.S_TIPO,
                                                 prestamos.D_FECHAPRESTAMO,
                                                 prestamos.D_FECHAHASTA,
                                                 prestamos.D_FECHADEVOLUCION
                                             };

                return PartialView("_gvwConsultaPrestamosPartial", resultadoConsultaFinal.ToList());
            }

            if (rangoTiempos != null)
            {
                if (rangoTiempos < 10)
                    fechaInicialSel = DateTime.Today.AddYears((int)rangoTiempos * (-1));
                else
                    fechaInicialSel = DateTime.Today.AddMonths((int)rangoTiempos/30 * (-1));

                var resultadoConsulta = (from prestamos in dbSIM.PRESTAMOS
                                        join detallePrestamos in dbSIM.PRESTAMO_DETALLE on prestamos.ID_PRESTAMO equals detallePrestamos.ID_PRESTAMO
                                        join tipo in dbSIM.PRESTAMO_TIPO on detallePrestamos.ID_TIPOPRESTAMO equals tipo.ID_TIPOPRESTAMO
                                        //join tercero in dbGeneral.NATURAL on prestamos.ID_TERCEROPRESTAMO equals tercero.ID_TERCERO
                                        join identificadores in dbSIM.IDENTIFICADORES on detallePrestamos.ID_IDENTIFICADOR equals identificadores.ID_IDENTIFICADOR
                                        where prestamos.D_FECHAPRESTAMO <= fechaInicialSel && detallePrestamos.D_FECHADEVOLUCION == null
                                        select new
                                        {
                                            detallePrestamos.ID_PRESDETALLE,
                                            prestamos.ID_TERCEROPRESTAMO,
                                            //S_NOMBRETERCERO = tercero.S_NOMBRE1 + (tercero.S_NOMBRE2 != null && tercero.S_NOMBRE2.Trim() != "" ? " " + tercero.S_NOMBRE2 : "") + " " + tercero.S_APELLIDO1 + (tercero.S_APELLIDO2 != null && tercero.S_APELLIDO2.Trim() != "" ? " " + tercero.S_APELLIDO2 : ""),
                                            S_DESCPRESTAMO = identificadores.S_DESCRIPCION,
                                            S_TIPO = tipo.S_NOMBRE,
                                            prestamos.D_FECHAPRESTAMO,
                                            detallePrestamos.D_FECHAHASTA,
                                            detallePrestamos.D_FECHADEVOLUCION
                                        }).ToArray();

                var resultadoConsultaFinal = from prestamos in resultadoConsulta
                                             join tercero in dbSIM.NATURAL on prestamos.ID_TERCEROPRESTAMO equals tercero.ID_TERCERO
                                             select new
                                             {
                                                 prestamos.ID_PRESDETALLE,
                                                 S_NOMBRETERCERO = tercero.S_NOMBRE1 + (tercero.S_NOMBRE2 != null && tercero.S_NOMBRE2.Trim() != "" ? " " + tercero.S_NOMBRE2 : "") + " " + tercero.S_APELLIDO1 + (tercero.S_APELLIDO2 != null && tercero.S_APELLIDO2.Trim() != "" ? " " + tercero.S_APELLIDO2 : ""),
                                                 prestamos.S_DESCPRESTAMO,
                                                 prestamos.S_TIPO,
                                                 prestamos.D_FECHAPRESTAMO,
                                                 prestamos.D_FECHAHASTA,
                                                 prestamos.D_FECHADEVOLUCION
                                             };

                return PartialView("_gvwConsultaPrestamosPartial", resultadoConsultaFinal.ToList());
            }

            return null;
        }

        public string ValidarPrestamoDocumento(string documentoID)
        {
            PRESTAMO_DETALLE idPrestamoSeleccionado;

            documentoID = documentoID.Trim();

            if (documentoID.Substring(0, 2) == "20")
            {
                var qryDocumento = from identificadores in dbSIM.RADICADOS
                                   join tiposDocumento in dbSIM.PRESTAMO_TIPO on 1 equals tiposDocumento.ID_TIPOPRESTAMO
                                   where identificadores.S_ETIQUETA == documentoID
                                   select new { identificadores.ID_RADICADO, identificadores.S_ETIQUETA, S_DESCRIPCION = identificadores.S_ETIQUETA, tiposDocumento.ID_TIPOPRESTAMO, TIPODOCUMENTO = tiposDocumento.S_NOMBRE };

                var idSeleccionado = qryDocumento.FirstOrDefault();

                if (idSeleccionado != null)
                {
                    var qryPrestamoDocumento = from prestamoDetalle in dbSIM.PRESTAMO_DETALLE
                                               where prestamoDetalle.ID_IDENTIFICADOR == idSeleccionado.ID_RADICADO && prestamoDetalle.D_FECHADEVOLUCION == null && prestamoDetalle.ID_TIPOPRESTAMO == idSeleccionado.ID_TIPOPRESTAMO
                                               select prestamoDetalle;

                    idPrestamoSeleccionado = qryPrestamoDocumento.FirstOrDefault();

                    if (idPrestamoSeleccionado == null)
                    {
                        return "OK%Documento disponible para prestamo%" + (idSeleccionado.ID_RADICADO * (-1)).ToString() + "%" + documentoID + "%" + idSeleccionado.S_DESCRIPCION + "%" + idSeleccionado.TIPODOCUMENTO;
                    }
                    else
                    {
                        return "FAIL%El Documento ya se encuentra prestado.";
                    }
                }
                else
                {
                    return "FAIL%Documento NO existe.";
                }
            }
            else
            {
                var qryDocumento = from identificadores in dbSIM.IDENTIFICADORES
                                   join tiposDocumento in dbSIM.PRESTAMO_TIPO on identificadores.ID_TIPO equals tiposDocumento.ID_TIPOPRESTAMO
                                   where identificadores.S_IDENTIFICADOR == documentoID
                                   select new { identificadores.ID_IDENTIFICADOR, identificadores.S_IDENTIFICADOR, identificadores.S_DESCRIPCION, tiposDocumento.ID_TIPOPRESTAMO, TIPODOCUMENTO = tiposDocumento.S_NOMBRE };

                var idSeleccionado = qryDocumento.FirstOrDefault();

                if (idSeleccionado != null)
                {
                    var qryPrestamoDocumento = from prestamoDetalle in dbSIM.PRESTAMO_DETALLE
                                               where prestamoDetalle.ID_IDENTIFICADOR == idSeleccionado.ID_IDENTIFICADOR && prestamoDetalle.D_FECHADEVOLUCION == null && prestamoDetalle.ID_TIPOPRESTAMO == idSeleccionado.ID_TIPOPRESTAMO
                                               select prestamoDetalle;

                    idPrestamoSeleccionado = qryPrestamoDocumento.FirstOrDefault();

                    if (idPrestamoSeleccionado == null)
                    {
                        return "OK%Documento disponible para prestamo%" + idSeleccionado.ID_IDENTIFICADOR.ToString() + "%" + documentoID + "%" + idSeleccionado.S_DESCRIPCION + "%" + idSeleccionado.TIPODOCUMENTO;
                    }
                    else
                    {
                        return "FAIL%El Documento ya se encuentra prestado.";
                    }
                }
                else
                {
                    return "FAIL%Documento NO existe.";
                }
            }
        }

        public string PrestamoDocumentos(int idTercero, string documentosIDs, string observacion)
        {
            IDENTIFICADORES identificadorI;
            RADICADOS identificadorR;

            PRESTAMO_DETALLE modelPrestamosDetalle;
            decimal idPrestamo;
            int idDocumentoSel;
            string observacionDocumento;
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            PRESTAMOS modelPrestamos = new PRESTAMOS();
            modelPrestamos.ID_TERCEROPRESTAMO = idTercero;
            modelPrestamos.D_FECHAPRESTAMO = DateTime.Now;
            modelPrestamos.ID_FUNCPRESTA = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            modelPrestamos.S_OBSERVACION = observacion;

            dbSIM.Entry(modelPrestamos).State = EntityState.Added;
            dbSIM.SaveChanges();

            idPrestamo = modelPrestamos.ID_PRESTAMO;

            foreach (string idDocumento in documentosIDs.Split('^'))
            {
                try
                {
                    observacionDocumento = idDocumento.Split('|')[1];
                    idDocumentoSel = Convert.ToInt32(idDocumento.Split('|')[0]);

                    if (idDocumentoSel >= 0)
                    {
                        identificadorI = (from id in dbSIM.IDENTIFICADORES
                                          where id.ID_IDENTIFICADOR == idDocumentoSel
                                          select id).FirstOrDefault();

                        modelPrestamosDetalle = new PRESTAMO_DETALLE();
                        modelPrestamosDetalle.ID_PRESTAMO = idPrestamo;
                        modelPrestamosDetalle.ID_TIPOPRESTAMO = identificadorI.ID_TIPO;
                        modelPrestamosDetalle.ID_IDENTIFICADOR = Convert.ToDecimal(idDocumentoSel);
                        modelPrestamosDetalle.D_FECHAHASTA = DateTime.Today.AddDays(8);
                        modelPrestamosDetalle.S_DESCRIPCION = observacionDocumento;

                        dbSIM.Entry(modelPrestamosDetalle).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }
                    else
                    {
                        idDocumentoSel = idDocumentoSel * (-1);

                        identificadorR = (from id in dbSIM.RADICADOS
                                          where id.ID_RADICADO == idDocumentoSel
                                          select id).FirstOrDefault();

                        modelPrestamosDetalle = new PRESTAMO_DETALLE();
                        modelPrestamosDetalle.ID_PRESTAMO = idPrestamo;
                        modelPrestamosDetalle.ID_TIPOPRESTAMO = 1;
                        modelPrestamosDetalle.ID_IDENTIFICADOR = Convert.ToDecimal(idDocumentoSel);
                        modelPrestamosDetalle.D_FECHAHASTA = DateTime.Today.AddDays(8);
                        modelPrestamosDetalle.S_DESCRIPCION = observacionDocumento;

                        dbSIM.Entry(modelPrestamosDetalle).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception lcobjError)
                {
                    string s = lcobjError.Message;
                }
            }

            return "OK%Préstamo Realizado Satisfactoriamente.";
        }

        public string ValidarDevolucionDocumento(string documentoID)
        {
            PRESTAMO_DETALLE idPrestamoSeleccionado;

            documentoID = documentoID.Trim();

            if (documentoID.Substring(0, 2) == "20")
            {
                var qryDocumento = from identificadores in dbSIM.RADICADOS
                                   join tiposDocumento in dbSIM.PRESTAMO_TIPO on 1 equals tiposDocumento.ID_TIPOPRESTAMO
                                   where identificadores.S_ETIQUETA == documentoID
                                   select new { identificadores.ID_RADICADO, identificadores.S_ETIQUETA, S_DESCRIPCION = identificadores.S_ETIQUETA, tiposDocumento.ID_TIPOPRESTAMO, TIPODOCUMENTO = tiposDocumento.S_NOMBRE };

                var idSeleccionado = qryDocumento.FirstOrDefault();

                if (idSeleccionado != null)
                {
                    var qryPrestamoDocumento = from prestamoDetalle in dbSIM.PRESTAMO_DETALLE
                                               where prestamoDetalle.ID_IDENTIFICADOR == idSeleccionado.ID_RADICADO && prestamoDetalle.D_FECHADEVOLUCION == null && prestamoDetalle.ID_TIPOPRESTAMO == idSeleccionado.ID_TIPOPRESTAMO
                                               select prestamoDetalle;

                    idPrestamoSeleccionado = qryPrestamoDocumento.FirstOrDefault();

                    if (idPrestamoSeleccionado == null)
                    {
                        return "FAIL%El Documento NO se encuentra prestado.";
                    }
                    else
                    {
                        int idTerceroPrestamo = Convert.ToInt32(idPrestamoSeleccionado.PRESTAMOS.ID_TERCEROPRESTAMO);

                        var qryTerceroPrestamo = from tercero in dbSIM.NATURAL
                                                 where tercero.ID_TERCERO == idTerceroPrestamo
                                                 select new { S_NOMBRETERCERO = tercero.S_NOMBRE1 + (tercero.S_NOMBRE2 != null && tercero.S_NOMBRE2.Trim() != "" ? " " + tercero.S_NOMBRE2 : "") + " " + tercero.S_APELLIDO1 + (tercero.S_APELLIDO2 != null && tercero.S_APELLIDO2.Trim() != "" ? " " + tercero.S_APELLIDO2 : "") };

                        var terceroPrestamo = qryTerceroPrestamo.FirstOrDefault();

                        return "OK%Documento disponible para devolucion%" + (idSeleccionado.ID_RADICADO * (-1)).ToString() + "%" + documentoID + "%" + idSeleccionado.S_DESCRIPCION + "%" + ((DateTime)idPrestamoSeleccionado.D_FECHAHASTA).ToString("dd-MMM-yyyy") + "%" + (terceroPrestamo == null ? "" : terceroPrestamo.S_NOMBRETERCERO) + "%" + idSeleccionado.TIPODOCUMENTO + "%" + idPrestamoSeleccionado.S_DESCRIPCION;
                    }
                }
                else
                {
                    return "FAIL%Documento NO existe.";
                }
            }
            else
            {
                var qryDocumento = from identificadores in dbSIM.IDENTIFICADORES
                                   join tiposDocumento in dbSIM.PRESTAMO_TIPO on identificadores.ID_TIPO equals tiposDocumento.ID_TIPOPRESTAMO
                                   where identificadores.S_IDENTIFICADOR == documentoID
                                   select new { identificadores.ID_IDENTIFICADOR, identificadores.S_IDENTIFICADOR, identificadores.S_DESCRIPCION, tiposDocumento.ID_TIPOPRESTAMO, TIPODOCUMENTO = tiposDocumento.S_NOMBRE };

                var idSeleccionado = qryDocumento.FirstOrDefault();

                if (idSeleccionado != null)
                {
                    var qryPrestamoDocumento = from prestamoDetalle in dbSIM.PRESTAMO_DETALLE
                                               where prestamoDetalle.ID_IDENTIFICADOR == idSeleccionado.ID_IDENTIFICADOR && prestamoDetalle.D_FECHADEVOLUCION == null && prestamoDetalle.ID_TIPOPRESTAMO == idSeleccionado.ID_TIPOPRESTAMO
                                               select prestamoDetalle;

                    idPrestamoSeleccionado = qryPrestamoDocumento.FirstOrDefault();

                    if (idPrestamoSeleccionado == null)
                    {
                        return "FAIL%El Documento NO se encuentra prestado.";
                    }
                    else
                    {
                        int idTerceroPrestamo = Convert.ToInt32(idPrestamoSeleccionado.PRESTAMOS.ID_TERCEROPRESTAMO);

                        var qryTerceroPrestamo = from tercero in dbSIM.NATURAL
                                                 where tercero.ID_TERCERO == idTerceroPrestamo
                                                 select new { S_NOMBRETERCERO = tercero.S_NOMBRE1 + (tercero.S_NOMBRE2 != null && tercero.S_NOMBRE2.Trim() != "" ? " " + tercero.S_NOMBRE2 : "") + " " + tercero.S_APELLIDO1 + (tercero.S_APELLIDO2 != null && tercero.S_APELLIDO2.Trim() != "" ? " " + tercero.S_APELLIDO2 : "") };

                        var terceroPrestamo = qryTerceroPrestamo.FirstOrDefault();

                        return "OK%Documento disponible para devolucion%" + idSeleccionado.ID_IDENTIFICADOR.ToString() + "%" + documentoID + "%" + idSeleccionado.S_DESCRIPCION + "%" + ((DateTime)idPrestamoSeleccionado.D_FECHAHASTA).ToString("dd-MMM-yyyy") + "%" + (terceroPrestamo == null ? "" : terceroPrestamo.S_NOMBRETERCERO) + "%" + idSeleccionado.TIPODOCUMENTO + "%" + idPrestamoSeleccionado.S_DESCRIPCION;
                    }
                }
                else
                {
                    return "FAIL%Documento NO existe.";
                }
            }
        }

        public string DevolucionDocumentos(string documentosIDs)
        {
            IDENTIFICADORES identificadorI;
            string observacionDocumento;
            PRESTAMO_DETALLE modelPrestamosDetalle;
            int idDocumentoSel;

            foreach (string idDocumento in documentosIDs.Split('^'))
            {
                try
                {
                    System.Web.HttpContext context = System.Web.HttpContext.Current;

                    observacionDocumento = idDocumento.Split('|')[1];
                    idDocumentoSel = Convert.ToInt32(idDocumento.Split('|')[0]);

                    if (idDocumentoSel >= 0)
                    {
                        identificadorI = (from id in dbSIM.IDENTIFICADORES
                                          where id.ID_IDENTIFICADOR == idDocumentoSel
                                          select id).FirstOrDefault();

                        modelPrestamosDetalle = (from prestamoDetalle in dbSIM.PRESTAMO_DETALLE
                                                 where prestamoDetalle.ID_TIPOPRESTAMO == identificadorI.ID_TIPO && prestamoDetalle.ID_IDENTIFICADOR == idDocumentoSel && prestamoDetalle.D_FECHADEVOLUCION == null
                                                 select prestamoDetalle).FirstOrDefault();

                        modelPrestamosDetalle.ID_FUNCRECIBE = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
                        modelPrestamosDetalle.D_FECHADEVOLUCION = DateTime.Now;
                        modelPrestamosDetalle.S_OBSERDEVOLUCION = observacionDocumento;

                        dbSIM.Entry(modelPrestamosDetalle).State = EntityState.Modified;
                        dbSIM.SaveChanges();
                    }
                    else
                    {
                        idDocumentoSel = idDocumentoSel * (-1);

                        modelPrestamosDetalle = (from prestamoDetalle in dbSIM.PRESTAMO_DETALLE
                                                 where prestamoDetalle.ID_TIPOPRESTAMO == 1 && prestamoDetalle.ID_IDENTIFICADOR == idDocumentoSel && prestamoDetalle.D_FECHADEVOLUCION == null
                                                 select prestamoDetalle).FirstOrDefault();

                        modelPrestamosDetalle.ID_FUNCRECIBE = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
                        modelPrestamosDetalle.D_FECHADEVOLUCION = DateTime.Now;
                        modelPrestamosDetalle.S_OBSERDEVOLUCION = observacionDocumento;

                        dbSIM.Entry(modelPrestamosDetalle).State = EntityState.Modified;
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception lcobjError)
                {
                    string s = lcobjError.Message;
                }
            }

            return "OK%Devolución Realizada Satisfactoriamente.";
        }


        [ValidateInput(false)]
        public ActionResult gvwConsultaPrestamosPartial()
        {
            var model = new object[0];
            return PartialView("_gvwConsultaPrestamosPartial", model);
        }*/
    }
}