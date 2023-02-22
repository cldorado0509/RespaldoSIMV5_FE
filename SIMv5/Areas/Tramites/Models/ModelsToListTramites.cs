namespace SIM.Areas.Tramites.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using SIM.Data;

    public class ModelsToListTramites
    {
        public static dynamic GetTiposTramite()
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            //EntitiesTramitesOracle dbSIM = new EntitiesTramitesOracle();

            var tiposTramites = from tipoTramite in dbSIM.TBTARIFAS_TRAMITE
                                group tipoTramite by tipoTramite.NOMBRE into tipoTramiteGroup
                                orderby tipoTramiteGroup.Key
                                select new {
                                    CODIGO_TRAMITE = tipoTramiteGroup.Max(f => f.CODIGO_TRAMITE),
                                    NOMBRE = tipoTramiteGroup.Key,
                                    TECNICOS = tipoTramiteGroup.Max(f => f.TECNICOS),
                                    UNIDAD = tipoTramiteGroup.Max(f => f.S_UNIDAD)
                                };

            /*var tiposTramites = from tipoTramite in dbSIM.TBTARIFAS_TRAMITE
                                orderby tipoTramite.NOMBRE
                                select new { tipoTramite.CODIGO_TRAMITE, tipoTramite.NOMBRE };*/

            return tiposTramites;
        }
    }
}
