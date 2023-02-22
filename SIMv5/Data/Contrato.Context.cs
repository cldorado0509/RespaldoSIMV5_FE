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
    using SIM.Data.Contrato;

    public partial class EntitiesSIMOracle : DbContext
    {
        public virtual DbSet<CONTRATO> CONTRATO { get; set; }
        public virtual DbSet<CONTRATO_TERCERO> CONTRATO_TERCERO { get; set; }
        public virtual DbSet<PRECONTRATO_PROCESO> PRECONTRATO_PROCESO { get; set; }
        public virtual DbSet<PRECONTRATO_PROCPROPUESTAS> PRECONTRATO_PROCPROPUESTAS { get; set; }
        public virtual DbSet<TIPO_ASOCIACION> TIPO_ASOCIACION { get; set; }
        public virtual DbSet<TIPO_CONTRATISTA> TIPO_CONTRATISTA { get; set; }
        public virtual DbSet<TIPO_CONTRATO> TIPO_CONTRATO { get; set; }
        public virtual DbSet<TIPO_PROCESO> TIPO_PROCESO { get; set; }
    }
}