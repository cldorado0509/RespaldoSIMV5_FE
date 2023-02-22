namespace SIM.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using SIM.Data.EnCicla;
    
    public partial class EntitiesSIMOracle : DbContext
    {
        public virtual DbSet<ASIGNACION> ASIGNACION { get; set; }
        public virtual DbSet<BICICLETA> BICICLETA { get; set; }
        public virtual DbSet<DATOS_CONSULTA> DATOS_CONSULTA { get; set; }
        public virtual DbSet<ESTACION> ESTACION { get; set; }
        public virtual DbSet<ESTADO_EN> ESTADO_EN { get; set; }
        public virtual DbSet<ESTRATEGIA> ESTRATEGIA { get; set; }
        public virtual DbSet<ESTRATEGIA_ZONA> ESTRATEGIA_ZONA { get; set; }
        public virtual DbSet<HISTORICO> HISTORICO { get; set; }
        public virtual DbSet<LOG_CONSULTA> LOG_CONSULTA { get; set; }
        public virtual DbSet<OPERACION> OPERACION { get; set; }
        public virtual DbSet<OPERADOR> OPERADOR { get; set; }
        public virtual DbSet<PARAMETRO_EN> PARAMETRO_EN { get; set; }
        public virtual DbSet<PARTICULAR> PARTICULAR { get; set; }
        public virtual DbSet<ROL_EN> ROL_EN { get; set; }
        public virtual DbSet<TERCERO_ESTACION> TERCERO_ESTACION { get; set; }
        public virtual DbSet<TERCERO_ESTADO_EN> TERCERO_ESTADO_EN { get; set; }
        public virtual DbSet<TERCERO_HISTORICO> TERCERO_HISTORICO { get; set; }
        public virtual DbSet<TERCERO_ROL> TERCERO_ROL { get; set; }
        public virtual DbSet<TIPO_ESTACION> TIPO_ESTACION { get; set; }
        public virtual DbSet<TIPO_PARAMETRO> TIPO_PARAMETRO { get; set; }
        public virtual DbSet<ZONA_EN> ZONA_EN { get; set; }
    }
}
