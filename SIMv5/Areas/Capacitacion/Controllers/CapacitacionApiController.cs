using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Data;
using SIM.Data.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SIM.Areas.Capacitacion.Controllers
{
    [Authorize]
    public class CapacitacionApiController : ApiController
    {
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// Obtiene los procesos para cargar el grid de la ventana principal
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="group"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchExpr"></param>
        /// <param name="comparation"></param>
        /// <param name="tipoData"></param>
        /// <param name="noFilterNoRecords"></param>
        /// <param name="CodFuncionario"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ObtieneCapacitaciones")]
        public datosConsulta ObtieneCapacitaciones(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;
            datosConsulta resultado = new datosConsulta();
            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)) // || (!administrador && idTerceroUsuario == null))
            {
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var model = (from Eve in dbSIM.EVENTO
                             orderby Eve.D_EVENTO
                             select new
                             {
                                 Eve.ID_EVENTO,
                                 EVENTO = Eve.S_EVENTO,
                                 LUGAR = Eve.S_LUGAR,
                                 FECHA = Eve.D_EVENTO
                             });
                modelData = model;
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        /// <summary>
        /// Obtiene los procesos para cargar el grid de la ventana principal
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="group"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchExpr"></param>
        /// <param name="comparation"></param>
        /// <param name="tipoData"></param>
        /// <param name="noFilterNoRecords"></param>
        /// <param name="CodFuncionario"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ObtieneInscritos")]
        public datosConsulta ObtieneInscritos(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, int IdEvento)
        {
            dynamic modelData;
            datosConsulta resultado = new datosConsulta();
            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)) // || (!administrador && idTerceroUsuario == null))
            {
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var model = (from Par in dbSIM.EVENTOPARTICIPANTES
                             join Ins in dbSIM.PARTICIPANTE on Par.ID_PARTICIPANTE equals Ins.ID_PARTICIPANTE
                             where Par.ID_EVENTO == IdEvento
                             select new
                             {
                                 Ins.ID_PARTICIPANTE,
                                 DOCUMENTO = Ins.N_PARTICIPANTE,
                                 NOMBRE = Ins.S_NOMBRE,
                                 APELLIDOS = Ins.S_APELLIDO,
                                 MUNICIPIO = Ins.S_MUNICIPIO,
                                 SECTOR = Ins.S_SECTOR,
                                 EMPRESA = Ins.S_EMPRESA,
                                 TELEFONO = Ins.S_TELEFONO,
                                 CORREO = Ins.S_CORREOELECTRONICO
                             });
                modelData = model;
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();
                return resultado;
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("DetalleCapacitacion")]
        public JObject GetEvento(string IdEvento)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                decimal _IdEvento = -1;
                if (IdEvento != null && IdEvento != "") _IdEvento = decimal.Parse(IdEvento);

                var Evento = (from Eve in dbSIM.EVENTO
                              where Eve.ID_EVENTO == _IdEvento
                              select Eve).FirstOrDefault();
                var Asistentes = (from Par in dbSIM.EVENTOPARTICIPANTES
                                  where Par.ID_EVENTO == _IdEvento
                                  select Par).Count();
                DatosCapacitacion DatosEvento = new DatosCapacitacion();
                DatosEvento.IdEvento = Evento.ID_EVENTO;
                DatosEvento.Evento = Evento.S_EVENTO;
                DatosEvento.Lugar = Evento.S_LUGAR;
                DatosEvento.Fecha = Evento.D_EVENTO.Value.ToString("yyyy-MM-dd HH:mm:ss");
                DatosEvento.Duracion = Evento.N_DURACION;
                DatosEvento.Responsable = Evento.S_RESPONSABLE != null ? Evento.S_RESPONSABLE : "";
                DatosEvento.Capacidad = Evento.N_CAPACIDAD;
                DatosEvento.Contacto = Evento.S_CONTACTO != null ? Evento.S_CONTACTO : "";
                DatosEvento.CorreoContacto = Evento.S_CORREOCONTACTO != null ? Evento.S_CORREOCONTACTO : "";
                DatosEvento.Url = Evento.S_URL != null ? Evento.S_URL : "";
                DatosEvento.Inscritos = Asistentes ;
                return JObject.FromObject(DatosEvento, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        [System.Web.Http.HttpPost, System.Web.Http.ActionName("GuardaCapacitacion")]
        public object GuardaCapacitacion(DatosCapacitacion objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error Almacenando la capcitacion" };
            try
            {
                if (objData.IdEvento > 0)
                {
                    var Evento = (from Eve in dbSIM.EVENTO
                                   where Eve.ID_EVENTO == objData.IdEvento
                                  select Eve).FirstOrDefault();
                    if (Evento != null)
                    {
                        Evento.S_EVENTO = objData.Evento;
                        Evento.S_LUGAR = objData.Lugar;
                        Evento.D_EVENTO = DateTime.Parse(objData.Fecha);
                        Evento.N_CAPACIDAD = objData.Capacidad.Value;
                        Evento.N_DURACION = objData.Duracion.Value;
                        Evento.S_CONTACTO = objData.Contacto;
                        Evento.S_CORREOCONTACTO = objData.CorreoContacto;
                        Evento.S_RESPONSABLE = objData.Responsable;
                        Evento.S_URL = objData.Url;
                        dbSIM.SaveChanges();
                    }
                }
                else if (objData.IdEvento <= 0)
                {
                    EVENTO Evento = new EVENTO();
                    Evento.S_EVENTO = objData.Evento;
                    Evento.S_LUGAR = objData.Lugar;
                    Evento.D_EVENTO = DateTime.Parse(objData.Fecha);
                    Evento.N_CAPACIDAD = objData.Capacidad.Value;
                    Evento.N_DURACION = objData.Duracion.Value;
                    Evento.S_CONTACTO = objData.Contacto;
                    Evento.S_CORREOCONTACTO = objData.CorreoContacto;
                    Evento.S_RESPONSABLE = objData.Responsable;
                    Evento.S_URL = objData.Url;
                    dbSIM.EVENTO.Add(Evento);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el evento de capacitacion: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Capacitación almacenada correctamente" };
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("EliminaCapacitacion")]
        public object EliminaCapacitacion(int IdEvento)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error eliminando la capacitacion" };
            try
            {
                if (IdEvento > 0)
                {
                    var Asistentes = (from Par in dbSIM.EVENTOPARTICIPANTES
                                      where Par.ID_EVENTO == IdEvento
                                      select Par).Count();
                    if (Asistentes > 0) return new { resp = "Error", mensaje = "La cacpitación no se puede eliminar ya que posee inscritos" };
                    var Capacitacion = (from Eve in dbSIM.EVENTO
                                        where Eve.ID_EVENTO == IdEvento
                                        select Eve).FirstOrDefault();
                    dbSIM.EVENTO.Remove(Capacitacion);
                    dbSIM.SaveChanges();
                }
                else return new { resp = "Error", mensaje = "La capacitación no se puede elimiar ya que no se encontró en la base de datos" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando COD: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Proceso eliminado correctamente!!" };
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("QuitaInscrito")]
        public object QuitaInscrito(int IdEvento, int IdParticipante)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error eliminando la capacitacion" };
            try
            {
                if (IdEvento > 0 && IdParticipante > 0)
                {
                    var Inscrito = (from Ins in dbSIM.EVENTOPARTICIPANTES
                                        where Ins.ID_EVENTO == IdEvento && Ins.ID_PARTICIPANTE == IdParticipante
                                        select Ins).FirstOrDefault();
                    dbSIM.EVENTOPARTICIPANTES.Remove(Inscrito);
                    dbSIM.SaveChanges();
                }
                else return new { resp = "Error", mensaje = "No se han ingresado todos los datos para retirar al participante" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando COD: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Participante retirado correctamente!!" };
        }
    }

    public class DatosCapacitacion
    {
        public int IdEvento { get; set; }
        public string Evento { get; set; }
        public string Lugar { get; set; }
        public string Fecha { get; set; }
        public int? Duracion { get; set; }
        public string Responsable { get; set; }
        public int? Capacidad { get; set; }
        public string Contacto { get; set; }
        public string CorreoContacto { get; set; }
        public string Url { get; set; }
        public int Inscritos { get; set; }
    }
}
