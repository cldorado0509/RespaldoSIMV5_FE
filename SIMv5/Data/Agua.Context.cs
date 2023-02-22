namespace SIM.Data
{

    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web;
    using SIM.Data.Agua;

    public partial class EntitiesSIMOracle : DbContext
    {

        public virtual DbSet<TSIMTASA_PRODUCTO> TSIMTASA_PRODUCTO { get; set; }
        public virtual DbSet<TSIMTASA_CUENCAS> TSIMTASA_CUENCAS { get; set; }
        public virtual DbSet<TSIMTASA_CUENCAS_TERCERO> TSIMTASA_CUENCAS_TERCERO { get; set; }
        public virtual DbSet<TSIMTASA_FACTOR_REGIONAL> TSIMTASA_FACTOR_REGIONAL { get; set; }
        public virtual DbSet<TSIMTASA_LIQUIDACIONES> TSIMTASA_LIQUIDACIONES { get; set; }
        public virtual DbSet<TSIMTASA_MATERIAS_PRIMA> TSIMTASA_MATERIAS_PRIMA { get; set; }
        public virtual DbSet<TSIMTASA_PARAMETROS_AMBIENTAL> TSIMTASA_PARAMETROS_AMBIENTAL { get; set; }
        public virtual DbSet<TSIMTASA_PRODUCCION> TSIMTASA_PRODUCCION { get; set; }
        public virtual DbSet<TSIMTASA_PRODUCTOS> TSIMTASA_PRODUCTOS { get; set; }
        public virtual DbSet<TSIMTASA_REPORTES> TSIMTASA_REPORTES { get; set; }
        public virtual DbSet<TSIMTASA_TARIFAS_MINIMAS> TSIMTASA_TARIFAS_MINIMAS { get; set; }
        public virtual DbSet<TSIMTASA_TIPO_CUENCAS> TSIMTASA_TIPO_CUENCAS { get; set; }
        public virtual DbSet<TSIMTASA_TRAMOS> TSIMTASA_TRAMOS { get; set; }
        public virtual DbSet<TSIMTASA_TURNOS> TSIMTASA_TURNOS { get; set; }
        public virtual DbSet<TSIMTASA_UNIDADES> TSIMTASA_UNIDADES { get; set; }
        public virtual DbSet<TSIMTASA_CONSUMOS> TSIMTASA_CONSUMOS { get; set; }
        public virtual DbSet<TSIMTASA_TIPO_EMPRESA> TSIMTASA_TIPO_EMPRESA { get; set; }
        public virtual DbSet<TSIMTASA_GENERALIDADES> TSIMTASA_GENERALIDADES { get; set; }
        public virtual DbSet<TSIMTASA_ESTADO_REPORTE> TSIMTASA_ESTADO_REPORTE { get; set; }
        public virtual DbSet<TSIMTASA_TIPO_REPORTE> TSIMTASA_TIPO_REPORTE { get; set; }
        public virtual DbSet<TSIMTASA_MESES> TSIMTASA_MESES { get; set; }
        public virtual DbSet<TSIMTASA_AGNOS_REPORTES> TSIMTASA_AGNOS_REPORTES { get; set; }
        public virtual DbSet<TSIMTASA_TIPO_DESCARGA> TSIMTASA_TIPO_DESCARGA { get; set; }
        public virtual DbSet<TSIMTASA_TIPO_AGUA_RESIDUAL> TSIMTASA_TIPO_AGUA_RESIDUAL { get; set; }
        public virtual DbSet<TSIMTASA_QUINQUENO> TSIMTASA_QUINQUENO { get; set; }
        public virtual DbSet<TSIMTASA_PERIODO> TSIMTASA_PERIODO { get; set; }
        public virtual DbSet<TSIMTASA_METAS_INDIVIDUALES> TSIMTASA_METAS_INDIVIDUALES { get; set; }
        public virtual DbSet<TSIMTASA_METAS_GRUPALES> TSIMTASA_METAS_GRUPALES { get; set; }
    }
}


