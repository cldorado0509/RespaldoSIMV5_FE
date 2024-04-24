using SIM.Areas.EnCicla.Models;
using SIM.Data;
using SIM.Data.EnCicla;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
//using System.Data;

namespace SIM.Areas.EnCicla.Controllers
{
    public class PrestamoController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema EnCila
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        //
        // GET: /EnCicla/Prestamo/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Prestamo(int? idEstacion)
        {
            /*ViewBag.TituloEstrategia = "Al Trabajo en Cicla";
            return View(new DATOSPRESTAMO { idEstacion = 1 });*/

            string tituloEstrategia = "";
            string ipCliente;

            var restriccionIP = (from parametro in dbSIM.PARAMETRO_EN
                                 join tipoParametro in dbSIM.TIPO_PARAMETRO on parametro.ID_TIPOPARAMETRO equals tipoParametro.ID_TIPOPARAMETRO
                                 where tipoParametro.ID_TIPOPARAMETRO == 21
                                 select parametro).FirstOrDefault();

            ipCliente = ObtenerCliente_IP().Trim();
            ViewBag.IPCliente = ipCliente;

            if (restriccionIP == null || restriccionIP.S_VALOR.Trim() == "" || restriccionIP.S_VALOR.Trim() == ipCliente)
            {
                DATOSPRESTAMO datosPrestamo = new DATOSPRESTAMO();
                ESTACION datosEstacion = (from estacion in dbSIM.ESTACION
                                          where estacion.ID_ESTACION == idEstacion
                                          select estacion).FirstOrDefault();

                if (datosEstacion != null && datosEstacion.ESTRATEGIA != null)
                    tituloEstrategia = datosEstacion.ESTRATEGIA.S_DESCRIPCION;

                ViewBag.TituloEstrategia = tituloEstrategia;

                datosPrestamo.idEstacion = (int)(idEstacion == null ? 0 : idEstacion);
                return View(datosPrestamo);
            }
            else
            {
                return null;
            }
        }

        public ActionResult Devolucion(int? idEstacion)
        {
            string tituloEstrategia = "";

            var restriccionIP = (from parametro in dbSIM.PARAMETRO_EN
                                 join tipoParametro in dbSIM.TIPO_PARAMETRO on parametro.ID_TIPOPARAMETRO equals tipoParametro.ID_TIPOPARAMETRO
                                 where tipoParametro.ID_TIPOPARAMETRO == 21
                                 select parametro).FirstOrDefault();

            if (restriccionIP == null || restriccionIP.S_VALOR.Trim() == "" || restriccionIP.S_VALOR.Trim() == ObtenerCliente_IP().Trim())
            {
                DATOSPRESTAMO datosPrestamo = new DATOSPRESTAMO();
                ESTACION datosEstacion = (from estacion in dbSIM.ESTACION
                                          where estacion.ID_ESTACION == idEstacion
                                          select estacion).FirstOrDefault();

                if (datosEstacion != null && datosEstacion.ESTRATEGIA != null)
                    tituloEstrategia = datosEstacion.ESTRATEGIA.S_DESCRIPCION;

                ViewBag.TituloEstrategia = tituloEstrategia;

                datosPrestamo.idEstacion = (int)(idEstacion == null ? 0 : idEstacion);
                return View(datosPrestamo);
            }
            else
            {
                return null;
            }
        }

        public ActionResult PrestarBicicleta(DATOSPRESTAMO Datos)
        {
            bool validacion = true;
            DATOSUSUARIO terceroCedula = null;
            OPERACION operacionBicileta = null;
            DateTime fechaActual = DateTime.Now;
            string textoEmail;
            int estrategiaBicicleta = 0;

            // Valida que la cédula sea ingresada
            if (Datos.cedula == null || Datos.cedula.Trim() == "")
            {
                validacion = false;

                ViewBag.Tipo = "Error";
                ViewBag.Limpiar = "N";
                ViewBag.TextoError = "Documento Requerido";
                return PartialView("_VisualizarMensaje");
            }

            // Valida que el código de biciclata sea ingresado
            if (Datos.codigoBicicleta == null || Datos.codigoBicicleta.Trim() == "")
            {
                validacion = false;

                ViewBag.Tipo = "Error";
                ViewBag.Limpiar = "N";
                ViewBag.TextoError = "Código de Bicicleta Requerido";
                return PartialView("_VisualizarMensaje");
            }

            // Valida que la cédula corresponda a un usuario válido y no tenga prestadas bicicletas
            if (Datos.cedula.Trim() != "")
            {
                long? cedulaUsuario = null;

                try
                {
                    cedulaUsuario = Convert.ToInt64(Datos.cedula);
                }
                catch
                {
                    validacion = false;

                    ViewBag.Tipo = "Error";
                    ViewBag.Limpiar = "N";
                    ViewBag.TextoError = "Documento No Registrado";
                    return PartialView("_VisualizarMensaje");
                }

                terceroCedula = (from terceroRol in dbSIM.TERCERO_ROL
                                 join terceroEstado in dbSIM.TERCERO_ESTADO_EN on terceroRol.ID_TERCEROESTADO equals terceroEstado.ID_TERCEROESTADO
                                 join tercero in dbSIM.TERCERO on terceroRol.ID_TERCERO equals tercero.ID_TERCERO
                                 join terceroNatural in dbSIM.NATURAL on tercero.ID_TERCERO equals terceroNatural.ID_TERCERO
                                 where tercero.N_DOCUMENTON == cedulaUsuario
                                 select new DATOSUSUARIO { natural = terceroNatural, terceroRol = terceroRol, mensaje = terceroEstado.S_MENSAJE }).FirstOrDefault();


                if (terceroCedula == null)
                {
                    validacion = false;

                    ViewBag.Tipo = "Error";
                    ViewBag.Limpiar = "N";
                    ViewBag.TextoError = "Documento No Registrado";
                    return PartialView("_VisualizarMensaje");
                }
                else if (terceroCedula.terceroRol.ID_TERCEROESTADO > 1)
                {
                    validacion = false;

                    ViewBag.Tipo = "Error";
                    ViewBag.Limpiar = "S";
                    ViewBag.TextoError = terceroCedula.mensaje;
                    return PartialView("_VisualizarMensaje");
                }

                var prestamoUsuario = (from operacion in dbSIM.OPERACION
                                       where operacion.ID_USUARIO == terceroCedula.terceroRol.ID_TERCEROROL && operacion.ID_ESTACION == null
                                       select operacion).FirstOrDefault();

                if (prestamoUsuario != null)
                {
                    validacion = false;

                    ViewBag.Tipo = "Error";
                    ViewBag.Limpiar = "S";
                    ViewBag.TextoError = "El Usuario " + (terceroCedula.natural.S_NOMBRE1 + " " + terceroCedula.natural.S_NOMBRE2 + " " + terceroCedula.natural.S_APELLIDO1 + " " + terceroCedula.natural.S_APELLIDO2).Replace("  ", " ") + " actualmente tiene una bicicleta prestada. Debe realizar la devolución de la bicicleta para prestar otra.";
                    return PartialView("_VisualizarMensaje");
                }
            }

            // Valida que el código de bicicleta exista y que no esté prestada
            if (Datos.codigoBicicleta.Trim() != "")
            {
                operacionBicileta = (from bicicleta in dbSIM.BICICLETA
                                     join operacion in dbSIM.OPERACION on bicicleta.ID_BICICLETA equals operacion.ID_BICICLETA
                                     join estado in dbSIM.ESTADO_EN on operacion.ID_ESTADO equals estado.ID_ESTADO into JoinedOperacionEstado
                                     from estado in JoinedOperacionEstado.DefaultIfEmpty()
                                     where bicicleta.S_CODIGO == Datos.codigoBicicleta.ToUpper() && operacion.ID_ESTACION == Datos.idEstacion
                                     select operacion).FirstOrDefault();

                if (operacionBicileta == null)
                {
                    validacion = false;
                    ViewBag.Tipo = "Error";
                    ViewBag.Limpiar = "N";
                    ViewBag.TextoError = "Código de Bicicleta Inválido";

                    return PartialView("_VisualizarMensaje");
                }
                else if (operacionBicileta.ID_ESTADO != null && operacionBicileta.ESTADO.S_DISPONIBLE == "0")
                {
                    validacion = false;
                    ViewBag.Tipo = "Error";
                    ViewBag.Limpiar = "N";
                    ViewBag.TextoError = "Bicicleta Inhabilitada.<br/> " + operacionBicileta.ESTADO.S_DESCRIPCION + ".";

                    return PartialView("_VisualizarMensaje");
                }
            }

            if (validacion)
            {
                estrategiaBicicleta = operacionBicileta.ESTACION.ID_ESTRATEGIA;

                var historicoOperacion = new HISTORICO();
                historicoOperacion.ID_OPERACION = operacionBicileta.ID_OPERACION;
                historicoOperacion.ID_ESTACION = operacionBicileta.ID_ESTACION;
                historicoOperacion.ID_ESTACIONORIGEN = operacionBicileta.ID_ESTACIONORIGEN;
                historicoOperacion.ID_BICICLETA = operacionBicileta.ID_BICICLETA;
                historicoOperacion.ID_ESTRATEGIA = operacionBicileta.ESTACION.ID_ESTRATEGIA;
                historicoOperacion.D_INICIO = operacionBicileta.D_INICIO;
                historicoOperacion.D_FIN = fechaActual;
                historicoOperacion.ID_USUARIO = operacionBicileta.ID_USUARIO;
                historicoOperacion.ID_RESPONSABLE = operacionBicileta.ID_RESPONSABLE;
                historicoOperacion.ID_RESPONSABLEORIGEN = operacionBicileta.ID_RESPONSABLEORIGEN;
                historicoOperacion.ID_ESTADO = operacionBicileta.ID_ESTADO;
                historicoOperacion.S_OBSERVACION = operacionBicileta.S_OBSERVACION;

                dbSIM.Entry(historicoOperacion).State = EntityState.Added;
                dbSIM.SaveChanges();

                operacionBicileta.D_INICIO = fechaActual;
                operacionBicileta.ID_ESTADO = (Datos.reporteNovedad == null ? null : (int?)Convert.ToInt32(Datos.reporteNovedad));
                operacionBicileta.ID_ESTACION = null;
                operacionBicileta.ID_ESTACIONORIGEN = Datos.idEstacion;
                operacionBicileta.ID_USUARIO = terceroCedula.terceroRol.ID_TERCEROROL;
                operacionBicileta.S_OBSERVACION = Datos.observacionesNovedad;

                dbSIM.Entry(operacionBicileta).State = EntityState.Modified;
                dbSIM.SaveChanges();
            }

            ViewBag.Tipo = "Mensaje";
            ViewBag.Limpiar = "S";
            ViewBag.Texto = "El préstamo de la cicla " + Datos.codigoBicicleta.ToUpper() + " realizado a " + (terceroCedula.natural.S_NOMBRE1 + " " + terceroCedula.natural.S_NOMBRE2 + " " + terceroCedula.natural.S_APELLIDO1 + " " + terceroCedula.natural.S_APELLIDO2).Replace("  ", " ") + " se ha realizado con éxito (" + fechaActual.ToString("dd-MMM-yyyy HH:mm") + ")<br/><br/>¡Disfruta de tu recorrido en cicla!";
            textoEmail = "El préstamo de la cicla " + Datos.codigoBicicleta.ToUpper() + " realizado a '" + (terceroCedula.natural.S_NOMBRE1 + " " + terceroCedula.natural.S_NOMBRE2 + " " + terceroCedula.natural.S_APELLIDO1 + " " + terceroCedula.natural.S_APELLIDO2).Replace("  ", " ") + "' se ha realizado con éxito (" + fechaActual.ToString("dd-MMM-yyyy HH:mm") + ").";

            //if (Datos.cedula == "98640066")
            //{
            string correoUsuario = (terceroCedula.natural.TERCERO.S_CORREO == null ? "" : terceroCedula.natural.TERCERO.S_CORREO.Trim());
            string correoMonitor = "";

            var monitor = (from usuarioMonitor in dbSIM.TERCERO_ROL
                           join tercero in dbSIM.TERCERO on usuarioMonitor.ID_TERCERO equals tercero.ID_TERCERO
                           where usuarioMonitor.ID_ESTRATEGIA == estrategiaBicicleta && usuarioMonitor.ID_ROL == 1
                           select tercero).FirstOrDefault();

            if (monitor != null)
                correoMonitor = (monitor.S_CORREO == null ? "" : monitor.S_CORREO.Trim());

            try
            {
                SIM.Utilidades.EmailMK.EnviarEmail("metropol@metropol.gov.co", correoUsuario + ";" + correoMonitor, "Préstamo Bicicleta " + Datos.codigoBicicleta.ToUpper(), textoEmail, "172.16.0.5", false, "", "");
                //SIM.Utilidades.Email.EnviarEmail("rene.meneses@metropol.gov.co", "rene.meneses@metropol.gov.co", "Prestamo Bicicleta", ViewBag.Texto, "172.16.0.5", true, "rene.meneses@metropol.gov.co", "renemeneses1282");
            }
            catch { }
            //}

            return PartialView("_VisualizarMensaje");
        }

        public ActionResult DevolverBicicletaValidacion(DATOSPRESTAMO Datos)
        {
            DATOSUSUARIO terceroCedula = null;
            OPERACION operacionBicileta = null;
            DateTime fechaActual = DateTime.Now;

            // Valida que el código de bicicleta sea ingresado
            if (Datos.codigoBicicleta == null || Datos.codigoBicicleta.Trim() == "")
            {
                ViewBag.Tipo = "Error";
                ViewBag.Limpiar = "N";
                ViewBag.TextoError = "Código de Bicicleta Requerido";
                return PartialView("_VisualizarMensaje");
            }

            // Valida que la bicicleta exista y esté prestada
            if (Datos.codigoBicicleta.Trim() != "")
            {
                operacionBicileta = (from bicicleta in dbSIM.BICICLETA
                                     join operacion in dbSIM.OPERACION on bicicleta.ID_BICICLETA equals operacion.ID_BICICLETA
                                     where bicicleta.S_CODIGO == Datos.codigoBicicleta.ToUpper() && operacion.ID_ESTACION == null
                                     select operacion).FirstOrDefault();

                if (operacionBicileta == null)
                {
                    ViewBag.Tipo = "Error";
                    ViewBag.Limpiar = "N";
                    ViewBag.TextoError = "Código de Bicicleta Inválido";

                    return PartialView("_VisualizarMensaje");
                }

                terceroCedula = (from terceroRol in dbSIM.TERCERO_ROL
                                 join terceroNatural in dbSIM.NATURAL on terceroRol.ID_TERCERO equals terceroNatural.ID_TERCERO
                                 where terceroRol.ID_TERCEROROL == operacionBicileta.ID_USUARIO
                                 select new DATOSUSUARIO { natural = terceroNatural, terceroRol = terceroRol, mensaje = "" }).FirstOrDefault();
            }

            ViewBag.Tipo = "Mensaje";
            ViewBag.Limpiar = "S";
            ViewBag.Texto = "¿Desea realizar la devolución de la cicla " + Datos.codigoBicicleta.ToUpper() + " prestada por " + (terceroCedula.natural.S_NOMBRE1 + " " + terceroCedula.natural.S_NOMBRE2 + " " + terceroCedula.natural.S_APELLIDO1 + " " + terceroCedula.natural.S_APELLIDO2).Replace("  ", " ") + " el día " + operacionBicileta.D_INICIO.ToString("dd-MMM-yyyy HH:mm") + "?";
            return PartialView("_VisualizarConfirmacion");
        }

        public ActionResult DevolverBicicleta(DATOSPRESTAMO Datos)
        {
            bool validacion = true;
            DATOSUSUARIO terceroCedula = null;
            OPERACION operacionBicileta = null;
            DateTime fechaActual = DateTime.Now;
            string textoEmail = "";

            // Valida que el código de bicicleta sea ingresado
            if (Datos.codigoBicicleta == null || Datos.codigoBicicleta.Trim() == "")
            {
                validacion = false;

                ViewBag.Tipo = "Error";
                ViewBag.Limpiar = "N";
                ViewBag.TextoError = "Código de Bicicleta Requerido";
                return PartialView("_VisualizarMensaje");
            }

            // Valida que la bicicleta exista y esté prestada
            if (Datos.codigoBicicleta.Trim() != "")
            {
                operacionBicileta = (from bicicleta in dbSIM.BICICLETA
                                     join operacion in dbSIM.OPERACION on bicicleta.ID_BICICLETA equals operacion.ID_BICICLETA
                                     where bicicleta.S_CODIGO == Datos.codigoBicicleta.ToUpper() && operacion.ID_ESTACION == null
                                     select operacion).FirstOrDefault();

                if (operacionBicileta == null)
                {
                    validacion = false;
                    ViewBag.Tipo = "Error";
                    ViewBag.Limpiar = "N";
                    ViewBag.TextoError = "Código de Bicicleta Inválido";

                    return PartialView("_VisualizarMensaje");
                }

                terceroCedula = (from terceroRol in dbSIM.TERCERO_ROL
                                 join terceroNatural in dbSIM.NATURAL on terceroRol.ID_TERCERO equals terceroNatural.ID_TERCERO
                                 where terceroRol.ID_TERCEROROL == operacionBicileta.ID_USUARIO
                                 select new DATOSUSUARIO { natural = terceroNatural, terceroRol = terceroRol, mensaje = "" }).FirstOrDefault();
            }

            if (validacion)
            {
                var historicoOperacion = new HISTORICO();
                historicoOperacion.ID_OPERACION = operacionBicileta.ID_OPERACION;
                historicoOperacion.ID_ESTACION = operacionBicileta.ID_ESTACION;
                historicoOperacion.ID_ESTACIONORIGEN = operacionBicileta.ID_ESTACIONORIGEN;
                historicoOperacion.ID_BICICLETA = operacionBicileta.ID_BICICLETA;
                historicoOperacion.ID_ESTRATEGIA = dbSIM.ASIGNACION.Where(c => c.ID_BICICLETA == operacionBicileta.ID_BICICLETA).FirstOrDefault().ID_ESTRATEGIA;
                historicoOperacion.D_INICIO = operacionBicileta.D_INICIO;
                historicoOperacion.D_FIN = fechaActual;
                historicoOperacion.ID_USUARIO = operacionBicileta.ID_USUARIO;
                historicoOperacion.ID_RESPONSABLE = operacionBicileta.ID_RESPONSABLE;
                historicoOperacion.ID_RESPONSABLEORIGEN = operacionBicileta.ID_RESPONSABLEORIGEN;
                historicoOperacion.ID_ESTADO = operacionBicileta.ID_ESTADO;
                historicoOperacion.S_OBSERVACION = operacionBicileta.S_OBSERVACION;

                dbSIM.Entry(historicoOperacion).State = EntityState.Added;
                dbSIM.SaveChanges();

                operacionBicileta.D_INICIO = fechaActual;
                operacionBicileta.ID_ESTACION = Datos.idEstacion;
                operacionBicileta.ID_ESTADO = (Datos.reporteNovedad == null ? null : (int?)Convert.ToInt32(Datos.reporteNovedad));
                operacionBicileta.ID_USUARIO = null;
                operacionBicileta.S_OBSERVACION = Datos.observacionesNovedad;

                dbSIM.Entry(operacionBicileta).State = EntityState.Modified;
                dbSIM.SaveChanges();
            }

            ViewBag.Tipo = "Mensaje";
            ViewBag.Limpiar = "S";
            //ViewBag.Texto = "Bicicleta '" + Datos.codigoBicicleta.ToUpper() + "'<br>Devoluci&oacute;n Satisfactoria.<br>Usuario '" + (terceroCedula.natural.S_NOMBRE1 + " " + terceroCedula.natural.S_NOMBRE2 + " " + terceroCedula.natural.S_APELLIDO1 + " " + terceroCedula.natural.S_APELLIDO2).Replace("  ", " ") + "'.";
            ViewBag.Texto = "La devolución de la cicla " + Datos.codigoBicicleta.ToUpper() + " se ha realizado con éxito.<br/><br/>¡Te esperamos de nuevo, gracias por ir en cicla!";
            textoEmail = "La devolución de la cicla " + Datos.codigoBicicleta.ToUpper() + " se ha realizado con éxito. Usuario '" + (terceroCedula.natural.S_NOMBRE1 + " " + terceroCedula.natural.S_NOMBRE2 + " " + terceroCedula.natural.S_APELLIDO1 + " " + terceroCedula.natural.S_APELLIDO2).Replace("  ", " ") + "' (" + fechaActual.ToString("dd-MMM-yyyy HH:mm") + ").";

            if (Datos.reporteNovedad != null)
            {
                int idReporteNovedad = Convert.ToInt32(Datos.reporteNovedad);

                var novedad = (from estado in dbSIM.ESTADO_EN
                               where estado.ID_ESTADO == idReporteNovedad
                               select estado).FirstOrDefault();

                if (novedad != null && novedad.S_DESCRIPCION != null)
                    textoEmail += " Reporte: " + novedad.S_DESCRIPCION + ".";
            }

            //if (terceroCedula.natural.TERCERO.N_DOCUMENTON == 98640066)
            //{
            string correoUsuario = (terceroCedula.natural.TERCERO.S_CORREO == null ? "" : terceroCedula.natural.TERCERO.S_CORREO.Trim());
            string correoMonitor = "";

            var monitor = (from usuarioMonitor in dbSIM.TERCERO_ROL
                           join tercero in dbSIM.TERCERO on usuarioMonitor.ID_TERCERO equals tercero.ID_TERCERO
                           where usuarioMonitor.ID_ESTRATEGIA == operacionBicileta.ESTACION.ID_ESTRATEGIA && usuarioMonitor.ID_ROL == 1
                           select tercero).FirstOrDefault();

            if (monitor != null)
                correoMonitor = (monitor.S_CORREO == null ? "" : monitor.S_CORREO.Trim());

            try
            {
                SIM.Utilidades.EmailMK.EnviarEmail("metropol@metropol.gov.co", correoUsuario + ";" + correoMonitor, "Devolución Bicicleta " + Datos.codigoBicicleta.ToUpper() + " ", textoEmail, "172.16.0.5", false, "", "");
                //SIM.Utilidades.Email.EnviarEmail("rene.meneses@metropol.gov.co", "rene.meneses@metropol.gov.co", "Prestamo Bicicleta", ViewBag.Texto, "172.16.0.5", true, "rene.meneses@metropol.gov.co", "renemeneses1282");
            }
            catch { }
            //}

            return PartialView("_VisualizarMensaje");
        }

        protected string ObtenerCliente_IP()
        {
            string VisitorsIPAddr = string.Empty;
            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorsIPAddr = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (Request.UserHostAddress.Length != 0)
            {
                VisitorsIPAddr = Request.UserHostAddress;
            }

            return VisitorsIPAddr;
        }
    }
}