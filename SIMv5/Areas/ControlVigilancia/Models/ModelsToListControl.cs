namespace SIM.Areas.ControlVigilancia.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using DevExpress.Web.ASPxTreeList;
    using System.Web.Mvc;
    using System.Web;
    using DevExpress.Web.Mvc;
    using SIM.Data;
    using SIM.Data.Control;

    public class ModelsToListControl
    {

        public static IEnumerable<TIPO_VISITA> GetTiposVisitas()
        {
            EntitiesSIMOracle db = new EntitiesSIMOracle();
           // SIM.Areas.ControlVigilancia.Models.EntitiesCont db = new SIM.Areas.ControlVigilancia.Models.EntitiesCont();
            return db.TIPO_VISITA.OrderBy(p => p.S_NOMBRE).ToList();
        }

        public static IEnumerable GetInstalacionTipos()
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            return dbSIM.MAESTRO_TIPO.OrderBy(t => t.NOMBRE_TIPO).ToList();
        }
    }
}