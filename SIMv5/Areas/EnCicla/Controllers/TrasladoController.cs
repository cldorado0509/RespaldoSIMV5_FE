using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIM.Areas.EnCicla.Models;
using SIM.Data;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using System.Data.Entity;
using SIM.Data.EnCicla;

namespace SIM.Areas.EnCicla.Controllers
{
    public class TrasladoController : Controller
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema EnCila
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        //
        // GET: /EnCicla/Traslado/
        public ActionResult Index(int? idEstacion)
        {
            IEnumerable<ESTACION> estaciones;
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            if (idEstacion == null)
            {
                int idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

                // La lista de estaciones corresponde tanto origen como destino a las de la estrategia

                estaciones = from terceroRol in dbSIM.TERCERO_ROL
                                 join estacion in dbSIM.ESTACION on terceroRol.ID_ESTRATEGIA equals estacion.ID_ESTRATEGIA
                                 where terceroRol.ID_TERCERO == idTercero
                                 select estacion;

                ViewBag.IdTercero = idTercero;
            }
            else
            {
                var datosEstacion = (from estacion in dbSIM.ESTACION
                                     where estacion.ID_ESTACION == idEstacion
                                     select estacion).FirstOrDefault();

                estaciones = from estacion in dbSIM.ESTACION
                                 where estacion.ID_ESTRATEGIA == datosEstacion.ID_ESTRATEGIA
                                 select estacion;

                ViewBag.IdTercero = null;
            }

            return View(estaciones.ToList());
        }

        public ActionResult ConsultarBicicletasEstacion(int idEstacion)
        {
            var bicicletasEstacion = from bicicleta in dbSIM.BICICLETA
                                     join operacion in dbSIM.OPERACION on bicicleta.ID_BICICLETA equals operacion.ID_BICICLETA
                                     join estado in dbSIM.ESTADO_EN on operacion.ID_ESTADO equals estado.ID_ESTADO into JoinedOperacionEstado
                                     from estado in JoinedOperacionEstado.DefaultIfEmpty()
                                     where operacion.ID_ESTACION == idEstacion
                                     select new { bicicleta.ID_BICICLETA, bicicleta.S_CODIGO, operacion.ID_ESTADO, S_DISPONIBLE = (estado.S_DISPONIBLE == null ? "2" : estado.S_DISPONIBLE) };

            return PartialView("_ConsultarBicicletas", bicicletasEstacion.ToList());
        }

        public string TrasladarBicicletas(int idEstacionOrigen, int idEstacionDestino, string ids)
        {
            DateTime fechaActual = DateTime.Now;
            string[] codes = ids.Split(',');

            var estacionOrigen = (from estacion in dbSIM.ESTACION
                                  where estacion.ID_ESTACION == idEstacionOrigen
                                  select estacion).FirstOrDefault();

            foreach (string id in codes)
            {
                var operacionBicileta = (from bicicleta in dbSIM.BICICLETA
                                     join operacion in dbSIM.OPERACION on bicicleta.ID_BICICLETA equals operacion.ID_BICICLETA
                                     where bicicleta.S_CODIGO == id.ToUpper()
                                     select operacion).FirstOrDefault();

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
                dbSIM.Entry(historicoOperacion).State = EntityState.Detached;

                operacionBicileta.D_INICIO = fechaActual;
                operacionBicileta.ID_ESTACIONORIGEN = operacionBicileta.ID_ESTACION;
                operacionBicileta.ID_ESTACION = idEstacionDestino;
                if (estacionOrigen.ID_TIPOESTACION == 4) // Taller
                    operacionBicileta.ID_ESTADO = null;

                dbSIM.Entry(operacionBicileta).State = EntityState.Modified;
                dbSIM.SaveChanges();
                dbSIM.Entry(operacionBicileta).State = EntityState.Detached;
            }

            return "Bicicletas Trasladadas Satisfactoriamente.";
        }
    }
}