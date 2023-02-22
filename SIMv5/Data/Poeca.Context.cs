namespace SIM.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using DevExpress.Utils.Extensions;
    using SIM.Data.Poeca;
    using SIM.Data.General;

    public partial class EntitiesSIMOracle : DbContext
    {
        public virtual DbSet<DPOEAIR_ACCION> DPOEAIR_ACCION { get; set; }
        public virtual DbSet<DPOEAIR_EPISODIO> DPOEAIR_EPISODIO { get; set; }
        public virtual DbSet<DPOEAIR_MEDIDA> DPOEAIR_MEDIDA { get; set; }
        public virtual DbSet<DPOEAIR_NIVEL> DPOEAIR_NIVEL { get; set; }
        public virtual DbSet<PeriodoImplementacion> PeriodoImplementacion { get; set; }
        public virtual DbSet<DPOEAIR_PRODUCTO> DPOEAIR_PRODUCTO { get; set; }
        public virtual DbSet<DPOEAIR_SECTOR> DPOEAIR_SECTOR { get; set; }
        public virtual DbSet<TPOEAIR_ACCIONES_PLAN> TPOEAIR_ACCIONES_PLAN { get; set; }
        public virtual DbSet<TPOEAIR_MEDIDA_ACCION> TPOEAIR_MEDIDA_ACCION { get; set; }
        public virtual DbSet<TPOEAIR_PERIODICIDAD> TPOEAIR_PERIODICIDAD { get; set; }
        public virtual DbSet<TPOEAIR_PLAN> TPOEAIR_PLAN { get; set; }
        public virtual DbSet<TPOEAIR_SEGUIMIENTO_GLOBAL> TPOEAIR_SEGUIMIENTO_GLOBAL { get; set; }
        public virtual DbSet<TPOEAIR_SECTOR_MEDIDA> TPOEAIR_SECTOR_MEDIDA { get; set; }
        public virtual DbSet<TPOEAIR_SEGUIMIENTO_META> TPOEAIR_SEGUIMIENTO_META { get; set; }
        public virtual DbSet<DPOEAIR_MUNICIPIOS_TERCEROS> DPOEAIR_MUNICIPIOS_TERCEROS { get; set; }

    }
}
