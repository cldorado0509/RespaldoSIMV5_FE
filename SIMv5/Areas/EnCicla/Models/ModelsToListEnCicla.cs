namespace SIM.Areas.EnCicla.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using SIM.Data;
    using SIM.Data.EnCicla;

    /// <summary>
    /// Extension de las entidades del modelo EnCicla, para retornar inforamcion customizada
    /// </summary>
    public partial class ModelsToListEnCicla
    {
        public static IEnumerable<ESTADO_EN> GetEstadosActivos()
        {
            EntitiesSIMOracle dbEnCicla = new EntitiesSIMOracle();
            return dbEnCicla.ESTADO_EN.Where(c => c.S_ACTIVO == "1").ToList();
        }
   }
}