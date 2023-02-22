using SIM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using datosConsulta_ = SIM.Areas.Tramites.Controllers.TramitesADAApiController.datosConsulta ;

namespace SIM.Areas.Tramites.Controllers
{
    public class RequisitosTramiteApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        //[HttpGet]
        //[ActionName("Requisito")]
        //public object GetTramites(int id)
        //{
        //    var tramite = dbSIM.FOTOGRAFIA.Where(f => f.ID_FOTOGRAFIA == id).FirstOrDefault();

        //    return tramite;
        //}

        [HttpGet, ActionName("Requisitos")]
        public datosConsulta_ GetRequisitoTramite(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;


            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords))
            {
                datosConsulta_ resultado = new datosConsulta_();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                switch (tipoData)
                {
                    case "f": // full
                        {
                            var model = (from fotografia in dbSIM.FOTOGRAFIA
                                         select new
                                         {
                                             fotografia.ID_FOTOGRAFIA,
                                             fotografia.S_ARCHIVO,
                                             fotografia.S_RUTA,
                                             fotografia.S_HASH,
                                             fotografia.D_CREACION,
                                             fotografia.S_USUARIO,
                                             fotografia.GPS_LATITUD,
                                             fotografia.GPS_LONGITUD,
                                             fotografia.S_ETIQUETA,
                                             fotografia.N_ESTADO,
                                             fotografia.PALABRA_CLAVE
                                         });

                            modelData = model;

                        }
                        break;
                    case "r": // reduced
                        {
                            var model = (from fotografia in dbSIM.FOTOGRAFIA
                                         select new
                                         {
                                             fotografia.ID_FOTOGRAFIA,
                                             fotografia.S_ARCHIVO,
                                             fotografia.S_RUTA,
                                         });

                            modelData = model;
                        }
                        break;
                    default: // (l) lookup
                        {
                            var model = (from fotografia in dbSIM.FOTOGRAFIA
                                         select new
                                         {
                                             fotografia.ID_FOTOGRAFIA,
                                             fotografia.S_ARCHIVO,
                                         });

                            modelData = model;
                        }
                        break;
                }

                // Obtiene consulta linq dinámicamente de acuerdo a los filtros establecidos
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta_ resultado = new datosConsulta_();
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }


    }
}
