namespace SIM.Areas.ParqueAguas.Controllers
{
    using DevExpress.Data.ODataLinq.Helpers;
    using SIM.Areas.ControlVigilancia.Models;
    using SIM.Areas.ParqueAguas.Models;
    using SIM.Data;
    using SIM.Data.ParqueAguas;
    using SIM.Models;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Security.Claims;
    using System.Web.Mvc;

    public class ParqueAguasController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();

        // GET: ParqueAguas/ParqueAguas
        public ActionResult ConfirmarVisitantes()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetReserva(int nroReserva)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            try
            {
                var reserva = this.dbSIM.TPARESE_RESERVAs.Find(nroReserva);
                var objReserva = new Reserva();
                
                if (reserva == null)
                {
                    objReserva.NumeroComprobante ="";
                    objReserva.Observaciones = "Reserva no encontrada!";
                }
                else
                {
                    if (reserva.B_POS == "1")
                    {
                        objReserva.NumeroComprobante = "-2";
                        objReserva.Observaciones = "Reserva validada anteriormente!";
                    }
                    else
                    {
                        objReserva.Id = nroReserva;
                        objReserva.Fecha = reserva.D_RESERVA;
                        objReserva.NroVisitantes = reserva.N_NROVISITANTES;
                        objReserva.Observaciones = reserva.S_OBSERVACIONES;
                        objReserva.NumeroComprobante = reserva.S_NRO_COMPROBANTE;
                        objReserva.Pos = reserva.B_POS;
                    }
                }
                return this.Json(objReserva);
            }
            catch(Exception exp)
            {
                return this.Json(exp.Message);
            }
          
        }

        [HttpPost]
        public JsonResult UpdateReserva(int nroReserva,int cantidad)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            try
            {
                var reserva = this.dbSIM.TPARESE_RESERVAs.Find(nroReserva);
                var objReserva = new Reserva();
 
                if (reserva == null)
                {
                    objReserva.NumeroComprobante = "";
                    objReserva.Observaciones = "Reserva no encontrada!";
                }
                else
                {
                    var estadoReserva = this.dbSIM.TPARESE_ESTADO_RESERVAs.Where(f => f.TPARESE_RESERVAID == nroReserva && f.ESTADO_ID == 1).FirstOrDefault();
                    if (estadoReserva != null && estadoReserva.ID > 0)
                    {
                        objReserva.NumeroComprobante = "-2";
                        objReserva.Observaciones = "La reserva ya fue verificada previamente!";
                    }
                    else
                    {

                        List<TPARESE_RESERVA> reservas = this.dbSIM.TPARESE_RESERVAs.ToList();
                        var cuposDisponibles = 5000 - (reservas.Where(f => f.D_RESERVA == reserva.D_RESERVA).Sum(c => c.N_NROVISITANTES) + (cantidad - (reserva.N_NROVISITANTES)));
                        if (cuposDisponibles > 0)
                        {
                            reserva.N_NROVISITANTES = cantidad;
                            reserva.B_POS = "1";
                           
                            objReserva.Id = nroReserva;
                            objReserva.Fecha = reserva.D_RESERVA;
                            objReserva.NroVisitantes = reserva.N_NROVISITANTES;
                            objReserva.Observaciones = reserva.S_OBSERVACIONES;
                            objReserva.NumeroComprobante = reserva.S_NRO_COMPROBANTE;
                            objReserva.Pos = reserva.B_POS;

                            TPARESE_ESTADO_RESERVA tPARESE_ESTADO_RESERVA = new TPARESE_ESTADO_RESERVA
                            {
                                ESTADO_ID = 1,
                                TPARESE_RESERVAID = nroReserva,
                                D_ESTADO = DateTime.Now,
                            };

                            this.dbSIM.TPARESE_ESTADO_RESERVAs.Add(tPARESE_ESTADO_RESERVA);
                            this.dbSIM.SaveChanges();

                        }
                        else
                        {
                            objReserva.Id = nroReserva;
                            objReserva.NumeroComprobante = "-1";
                            objReserva.NroVisitantes = reserva.N_NROVISITANTES;
                            objReserva.Observaciones = "No hay aforo disponible para modicar la cantidad de visitantes!";
                        }
                    }
                }

                return this.Json(objReserva);
            }
            catch (Exception exp)
            {
                return this.Json(exp.Message);

            }

        }

        public ActionResult ReservasMasivas()
        {

            List<SelectListItem> list = ObtenerTiposDocumentos();
          
            ReservaMasiva reservaMasiva = new ReservaMasiva
            {
                Apellidos = "",
                Nombres = "",
                TiposDocumentos = list,
                EMail = "",
                NumeroDocumento = 0,
                Direccion = "",
                Telefono = "",
                Comprobante = "",
                TipoDocumentoId = 0,
            };
            return View(reservaMasiva);
        }

        private List<SelectListItem> ObtenerTiposDocumentos()
        {

            List<SelectListItem> list = new List<SelectListItem>();
            list.Insert(0, new SelectListItem { Text = "Cédula de Ciudadanía", Value = "1" });
            list.Insert(1, new SelectListItem { Text = "Tarjeta de Identidad", Value = "2" });
            list.Insert(2, new SelectListItem { Text = "Registro Civil", Value = "3" });
            list.Insert(3, new SelectListItem { Text = "Cédula de Extranjería", Value = "4" });
            list.Insert(4, new SelectListItem { Text = "Cédula de Extranjería", Value = "5" });
            list.Insert(5, new SelectListItem { Text = "Pasaporte", Value = "6" });
            return list;
        }

        [HttpPost]
        public ActionResult ReservasMasivas(ReservaMasiva  reservaMasiva)
        {
            List<SelectListItem> list =  ObtenerTiposDocumentos();
            try
            {
                if (ModelState.IsValid)
                {
                    int aforo = 0;
                    var fechaDisponible = this.dbSIM.TPARESE_CALENDARIOs.Where(f => f.D_INHABIL.Year == reservaMasiva.Fecha.Year && f.D_INHABIL.Month == reservaMasiva.Fecha.Month && f.D_INHABIL.Day == reservaMasiva.Fecha.Day).FirstOrDefault();

                    var reservas = this.dbSIM.TPARESE_RESERVAs.Where(f => f.D_RESERVA == reservaMasiva.Fecha && f.B_CANCELADA == "0").ToList();
                    if (fechaDisponible == null) aforo = 5000 - reservas.Sum(s => s.N_NROVISITANTES);


                    if (reservaMasiva.NumeroPersonas > aforo)
                    {
                        reservaMasiva.Comprobante = "No hay el aforo disponible para este día";
                        reservaMasiva.TiposDocumentos = list;
                        return View(reservaMasiva);
                    }

                    TPARESE_RESERVA tPARESE_RESERVA = new TPARESE_RESERVA
                    {
                        D_RESERVA = reservaMasiva.Fecha,
                        B_CANCELADA = "0",
                        B_POS = "0",
                        N_NROVISITANTES = reservaMasiva.NumeroPersonas,
                        S_OBSERVACIONES = $"{reservaMasiva.NumeroDocumento}|{reservaMasiva.Nombres.ToUpper()} {reservaMasiva.Apellidos.ToUpper()}|Contacto: {reservaMasiva.Telefono}",
                        N_VALOR = 0,
                        S_NRO_COMPROBANTE = ".",
                    };
                    this.dbSIM.TPARESE_RESERVAs.Add(tPARESE_RESERVA);
                    this.dbSIM.SaveChanges();

                    tPARESE_RESERVA = this.dbSIM.TPARESE_RESERVAs.Find(tPARESE_RESERVA.ID);
                    tPARESE_RESERVA.S_NRO_COMPROBANTE = $"{tPARESE_RESERVA.ID}".PadLeft(10, '0');
                    this.dbSIM.SaveChanges();


                    TPARESE_VISITANTE tPARESE_VISITANTE = this.dbSIM.TPARESE_VISITANTEs.Where(f => f.N_NUMERO_DOCUMENTO == reservaMasiva.NumeroDocumento && f.TIPO_DOCUMENTO_ID == reservaMasiva.TipoDocumentoId).FirstOrDefault();
                    if (tPARESE_VISITANTE != null)
                    {
                        tPARESE_VISITANTE.S_NOMBRES = reservaMasiva.Nombres.ToUpper();
                        tPARESE_VISITANTE.S_APELLIDOS = reservaMasiva.Apellidos.ToUpper();
                        tPARESE_VISITANTE.TIPO_DOCUMENTO_ID = reservaMasiva.TipoDocumentoId;
                        tPARESE_VISITANTE.N_NUMERO_DOCUMENTO = reservaMasiva.NumeroDocumento;
                        tPARESE_VISITANTE.N_EDAD = 0;
                        tPARESE_VISITANTE.S_DIRECCION = reservaMasiva.Direccion;
                        tPARESE_VISITANTE.S_EMAIL = reservaMasiva.EMail;
                        tPARESE_VISITANTE.S_TELEFONO = reservaMasiva.Telefono;
                    }
                    else
                    {
                        tPARESE_VISITANTE = new TPARESE_VISITANTE();
                        tPARESE_VISITANTE.S_NOMBRES = reservaMasiva.Nombres.ToUpper();
                        tPARESE_VISITANTE.S_APELLIDOS = reservaMasiva.Apellidos.ToUpper();
                        tPARESE_VISITANTE.TIPO_DOCUMENTO_ID = reservaMasiva.TipoDocumentoId;
                        tPARESE_VISITANTE.N_NUMERO_DOCUMENTO = reservaMasiva.NumeroDocumento;
                        tPARESE_VISITANTE.N_EDAD = 0;
                        tPARESE_VISITANTE.S_DIRECCION = reservaMasiva.Direccion;
                        tPARESE_VISITANTE.S_EMAIL = reservaMasiva.EMail;
                        tPARESE_VISITANTE.S_TELEFONO = reservaMasiva.Telefono;
                        this.dbSIM.TPARESE_VISITANTEs.Add(tPARESE_VISITANTE);
                    }
                    this.dbSIM.SaveChanges();

                    TPARESE_VISITANTE_RESERVA tPARESE_VISITANTE_RESERVA = new TPARESE_VISITANTE_RESERVA
                    {
                        RESERVA_ID = tPARESE_RESERVA.ID,
                        VISITANTE_ID = tPARESE_VISITANTE.ID,
                        CATEGORIA_ID = 1,
                        B_RESPONSABLE = "1",
                    };
                    this.dbSIM.PARESE_VISITANTE_RESERVAs.Add(tPARESE_VISITANTE_RESERVA);
                    this.dbSIM.SaveChanges();

                    reservaMasiva.Comprobante = $"Reserva almacenada correctamente con número de comprobante : {tPARESE_RESERVA.S_NRO_COMPROBANTE}";
                    reservaMasiva.Apellidos = "";
                    reservaMasiva.Nombres = "";
                    reservaMasiva.NumeroDocumento = 0;
                    reservaMasiva.NumeroPersonas = 0;
                    reservaMasiva.Telefono = "";
                    reservaMasiva.Direccion = "";
                    reservaMasiva.EMail = "";
                    reservaMasiva.Fecha = DateTime.Now;
                    reservaMasiva.TiposDocumentos = list;
                    
                    return RedirectToAction("ReservasMasivas", "ParqueAguas");

                }
                else
                {
                    reservaMasiva.TiposDocumentos = list;
                    return View(reservaMasiva);
                }
            }
            catch (Exception exp)
            {
                reservaMasiva.TiposDocumentos = list;
                return View(reservaMasiva);
            }
        }
       

        [HttpPost]
        public JsonResult ObtenerAforoDisponible(DateTime fecha)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            AforoDisponible aforoDisponible = new AforoDisponible();
            CultureInfo culture = new CultureInfo("es-CO"); // Español
            try
            {

                int aforo = 0;

                var fechaDisponible = this.dbSIM.TPARESE_CALENDARIOs.Where(f => f.D_INHABIL.Year == fecha.Year && f.D_INHABIL.Month == fecha.Month && f.D_INHABIL.Day == fecha.Day).FirstOrDefault();
               
                var reservas = this.dbSIM.TPARESE_RESERVAs.Where(f => f.D_RESERVA == fecha && f.B_CANCELADA == "0").ToList();
                if (fechaDisponible == null) aforo = 5000 - reservas.Sum(s => s.N_NROVISITANTES);

                aforoDisponible.Aforo = aforo;
                aforoDisponible.Fecha = fecha;
                aforoDisponible.Mensaje = $"Para el {fecha.ToString("dddd, dd MMMM yyyy",culture)} hay {aforo} cupos disponibles";
                aforoDisponible.Result = true;


                return this.Json(aforoDisponible);
            }
            catch (Exception exp)
            {
                return this.Json(aforoDisponible);
            }
        }

        public ActionResult AdminReservas()
        {
            
            return View();

        }
    }
}