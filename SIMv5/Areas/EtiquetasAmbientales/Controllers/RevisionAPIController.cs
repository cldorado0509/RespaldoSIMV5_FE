namespace SIM.Areas.EtiquetasAmbientales.Controllers
{
    using System.Data.Entity;
    using System.Web.Http;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;
    using SIM.Data;
    using SIM.Areas.EtiquetasAmbientales.Models;
    using SIM.Data.FuentesFijas;

    [Route("api/[controller]", Name = "RevisionAPI")]
    public class RevisionAPIController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        #region Monitoreos
        /// <summary>
        /// Retorna el Listado de los Monitoreos
        /// </summary>
        /// <param name="filter">Criterio de Búsqueda dado por el usuario</param>
        /// <param name="sort"></param>
        /// <param name="group"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchExpr"></param>
        /// <param name="comparation"></param>
        /// <param name="tipoData"></param>
        /// <param name="noFilterNoRecords"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetMonitoreos")]
        public datosConsulta GetMonitoreos(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic model = null;
            dynamic modelData;
            datosConsulta resultado = new datosConsulta(); // 14 Septiembre de 2020
            DateTime fechaInicial = new DateTime(2020, 9, 1);

            model = dbSIM.TMOVPEM_MONITOREO.Where(f => f.D_MONITOREO >= fechaInicial).OrderBy(f => f.D_MONITOREO);

            modelData = model;
            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

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

        [HttpGet, ActionName("ObtenerMonitoreo")]
        public JObject ObtenerMonitoreo(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                var monitoreo = this.dbSIM.TMOVPEM_MONITOREO.Where(f => f.ID == _Id).FirstOrDefault();

                Monitoreo Monitoreo = new Monitoreo();
                Monitoreo.Id = monitoreo.ID;
                Monitoreo.Placa = monitoreo.S_PLACA;
                Monitoreo.CoordenadaX = monitoreo.N_COORDENADA_X;
                Monitoreo.CoordenadaY = monitoreo.N_COORDENADA_Y;
                Monitoreo.Empresa = monitoreo.S_EMPRESA;
                Monitoreo.Estado = monitoreo.S_ESTADO;
                Monitoreo.FechaMonitoreo = monitoreo.D_MONITOREO;
                Monitoreo.Foto1 = monitoreo.S_FOTO1;
                Monitoreo.Foto2 = monitoreo.S_FOTO2;
                Monitoreo.Foto3 = monitoreo.S_FOTO3;
                if (Monitoreo.IdEtiqueta != null) Monitoreo.IdEtiqueta = monitoreo.ID_ETIQUETA.Value;
                Monitoreo.Kilometraje = monitoreo.N_KILOMETRAJE;
                Monitoreo.Observaciones = monitoreo.S_OBSERVACIONES;
                Monitoreo.Usuario = monitoreo.S_USUARIO;
                Monitoreo.ValorMonitoreo = monitoreo.N_VALOR_MONITOREO;
                Monitoreo.RPMRalenti = monitoreo.N_RPM_RALENTI;
           
                return JObject.FromObject(Monitoreo, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpPost, ActionName("GuardarMonitoreo")]
        public object GuardarMonitoreo(Monitoreo objData)
        {
            if (!ModelState.IsValid) return null;
            try
            {
                decimal Id = -1;
                Id = objData.Id;
                if (Id > 0)
                {
                    var _Monitoreo = dbSIM.TMOVPEM_MONITOREO.Where(f => f.ID == Id).FirstOrDefault();
                    if (_Monitoreo != null)
                    {
                        _Monitoreo.D_MONITOREO = objData.FechaMonitoreo;
                        _Monitoreo.N_COORDENADA_X = objData.CoordenadaX;
                        _Monitoreo.N_COORDENADA_Y = objData.CoordenadaY;
                        _Monitoreo.N_KILOMETRAJE = objData.Kilometraje;
                        _Monitoreo.N_RPM_RALENTI = objData.RPMRalenti;
                        _Monitoreo.N_VALOR_MONITOREO = objData.ValorMonitoreo;
                        _Monitoreo.S_EMPRESA = objData.Empresa;
                        _Monitoreo.S_FOTO1 = objData.Foto1;
                        _Monitoreo.S_FOTO2 = objData.Foto2;
                        _Monitoreo.S_FOTO3 = objData.Foto3;
                        _Monitoreo.S_OBSERVACIONES = objData.Observaciones;
                        _Monitoreo.S_PLACA = objData.Placa;
                        _Monitoreo.S_USUARIO = objData.Usuario;
                    }
                }
                else
                {
                    TMOVPEM_MONITOREO _MonitoreoN = new TMOVPEM_MONITOREO
                    {
                        D_MONITOREO = objData.FechaMonitoreo,
                        N_COORDENADA_X = objData.CoordenadaX,
                        N_COORDENADA_Y = objData.CoordenadaY,
                        N_KILOMETRAJE = objData.Kilometraje,
                        N_RPM_RALENTI = objData.RPMRalenti,
                        N_VALOR_MONITOREO = objData.ValorMonitoreo,
                        S_EMPRESA = objData.Empresa,
                        S_FOTO1 = objData.Foto1,
                        S_FOTO2 = objData.Foto2,
                        S_FOTO3 = objData.Foto3,
                        S_OBSERVACIONES = objData.Observaciones,
                        S_PLACA = objData.Placa,
                        S_USUARIO = objData.Usuario,
                    };
                    dbSIM.TMOVPEM_MONITOREO.Add(_MonitoreoN);
                }
                dbSIM.SaveChanges();
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el Monitoreo: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Monitoreo almacenado correctamente" };
        }

        [HttpGet, ActionName("EliminarMonitoreo")]
        public object EliminarMonitoreo(string objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error eliminando el Monitoreo" };
            try
            {
                int Id = -1;
                if (objData != null && objData != "") Id = int.Parse(objData);
                if (Id > 0)
                {
                    var monitoreo = this.dbSIM.TMOVPEM_MONITOREO.Where(f => f.ID == Id).FirstOrDefault();
                    this.dbSIM.TMOVPEM_MONITOREO.Remove(monitoreo);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error  eliminando el Monitoreo: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Monitoreo eliminado correctamente!!" };
        }
        #endregion


        /// <summary>
        /// Estructura que almacena el resultado de una consulta, entregando los datos y número de registros en total.
        /// </summary>
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }
    }
}
