namespace SIM.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using SIM.Data.ExpedienteAmbiental;

    public partial class EntitiesSIMOracle : DbContext
    {
        public virtual DbSet<DEXPAMB_CLASIFICACIONEXPEDIENTE> DEXPAMB_CLASIFICACIONEXPEDIENTE { get; set; }
        public virtual DbSet<DEXPAMB_COMPONENTEAMBIENTAL> DEXPAMB_COMPONENTEAMBIENTAL { get; set; }
        public virtual DbSet<DEXPAMB_TIPOESTADOPUNTOCONTROL> DEXPAMB_TIPOESTADOPUNTOCONTROL { get; set; }
        public virtual DbSet<DEXPAMB_TIPOSOLICITUDAMBIENTAL> DEXPAMB_TIPOSOLICITUDAMBIENTAL { get; set; }
        public virtual DbSet<TEXPAMB_ABOGADOEXPEDIENTE> TEXPAMB_ABOGADOEXPEDIENTE { get; set; }
        public virtual DbSet<TEXPAMB_ANOTACIONESPUNTO> TEXPAMB_ANOTACIONESPUNTO { get; set; }
        public virtual DbSet<TEXPAMB_ESTADOPUNTOCONTROL> TEXPAMB_ESTADOPUNTOCONTROL { get; set; }
        public virtual DbSet<TEXPAMB_EXPEDIENTEAMBIENTAL> TEXPAMB_EXPEDIENTEAMBIENTAL { get; set; }
        public virtual DbSet<TEXPAMB_FUNCIONARIOEXPEDIENTE> TEXPAMB_FUNCIONARIOEXPEDIENTE { get; set; }
        public virtual DbSet<TEXPAMB_PUNTOCONTROL> TEXPAMB_PUNTOCONTROL { get; set; }
    }
}
