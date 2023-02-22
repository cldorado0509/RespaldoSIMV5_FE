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
    using SIM.Data.Documental;

    public partial class EntitiesSIMOracle : DbContext
    {
        public virtual DbSet<ANEXOS> ANEXOS { get; set; }
        public virtual DbSet<DOCUMENTO_INDICES> DOCUMENTO_INDICES { get; set; }
        public virtual DbSet<DOCUMENTOS> DOCUMENTOS { get; set; }
        public virtual DbSet<DOCUMENTOS_TOMO> DOCUMENTOS_TOMO { get; set; }
        public virtual DbSet<EXPEDIENTE> EXPEDIENTE { get; set; }
        public virtual DbSet<EXPEDIENTE_INDICES> EXPEDIENTE_INDICES { get; set; }
        public virtual DbSet<LOG_DOCUMENTO> LOG_DOCUMENTO { get; set; }
        public virtual DbSet<PERMISO_UNIDADDOCUMENTAL> PERMISO_UNIDADDOCUMENTAL { get; set; }
        public virtual DbSet<PRESTAMO_DETALLE> PRESTAMO_DETALLE { get; set; }
        public virtual DbSet<PRESTAMOS> PRESTAMOS { get; set; }
        public virtual DbSet<RADICADO_UNIDADDOCUMENTAL> RADICADO_UNIDADDOCUMENTAL { get; set; }
        public virtual DbSet<RADICADOS_ETIQUETAS> RADICADOS_ETIQUETAS { get; set; }
        public virtual DbSet<SERIE> SERIE { get; set; }
        public virtual DbSet<SUBSERIE> SUBSERIE { get; set; }
        public virtual DbSet<TIPO_ANEXO> TIPO_ANEXO { get; set; }
        public virtual DbSet<TIPO_DOCUMENTAL> TIPO_DOCUMENTAL { get; set; }
        public virtual DbSet<TIPO_EXPEDIENTE> TIPO_EXPEDIENTE { get; set; }
        public virtual DbSet<TIPO_PERMISO> TIPO_PERMISO { get; set; }
        public virtual DbSet<TIPO_RADICADO> TIPO_RADICADO { get; set; }
        public virtual DbSet<TIPODOCUMENTAL_DATO> TIPODOCUMENTAL_DATO { get; set; }
        public virtual DbSet<TOMOS> TOMOS { get; set; }
        public virtual DbSet<UNIDAD_DOCUMENTAL> UNIDAD_DOCUMENTAL { get; set; }
        public virtual DbSet<UNIDAD_TIPO> UNIDAD_TIPO { get; set; }
    } 
}