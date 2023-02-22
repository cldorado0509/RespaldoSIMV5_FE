namespace SIM.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using SIM.Data.Seguridad;

    public partial class EntitiesSIMOracle : DbContext
    {
        public virtual DbSet<APLICACION> APLICACION { get; set; }
        public virtual DbSet<CAMPO> CAMPO { get; set; }
        public virtual DbSet<CARGO> CARGO { get; set; }
        public virtual DbSet<CARGO_ROL> CARGO_ROL { get; set; }
        public virtual DbSet<CARGO_TERCERO> CARGO_TERCERO { get; set; }
        public virtual DbSet<CONTROL> CONTROL { get; set; }
        public virtual DbSet<DEPENDENCIA> DEPENDENCIA { get; set; }
        public virtual DbSet<FORMA> FORMA { get; set; }
        public virtual DbSet<FUNCIONARIO_DEPENDENCIA> FUNCIONARIO_DEPENDENCIA { get; set; }
        public virtual DbSet<GRUPO> GRUPO { get; set; }
        public virtual DbSet<GRUPO_CONTROL> GRUPO_CONTROL { get; set; }
        public virtual DbSet<GRUPO_FORMA> GRUPO_FORMA { get; set; }
        public virtual DbSet<GRUPO_PANEL> GRUPO_PANEL { get; set; }
        public virtual DbSet<GRUPO_ROL> GRUPO_ROL { get; set; }
        public virtual DbSet<LOG_AUDITORIA> LOG_AUDITORIA { get; set; }
        public virtual DbSet<LOG_TRANSACCIONES> LOG_TRANSACCIONES { get; set; }
        public virtual DbSet<MANUAL> MANUAL { get; set; }
        public virtual DbSet<MENSAJE> MENSAJE { get; set; }
        public virtual DbSet<MENU> MENU { get; set; }
        public virtual DbSet<MODULO> MODULO { get; set; }
        public virtual DbSet<NOTICIA> NOTICIA { get; set; }
        public virtual DbSet<PANEL> PANEL { get; set; }
        public virtual DbSet<PERMISO_OBJETO> PERMISO_OBJETO { get; set; }
        public virtual DbSet<PROPIETARIO> PROPIETARIO { get; set; }
        public virtual DbSet<ROL> ROL { get; set; }
        public virtual DbSet<ROL_FORMA> ROL_FORMA { get; set; }
        public virtual DbSet<ROL_NOTIFICACION> ROL_NOTIFICACION { get; set; }
        public virtual DbSet<ROL_SOLICITADO> ROL_SOLICITADO { get; set; }
        public virtual DbSet<TIPO_OBJETO> TIPO_OBJETO { get; set; }
        public virtual DbSet<USUARIO> USUARIO { get; set; }
        public virtual DbSet<USUARIO_CONTROLES> USUARIO_CONTROLES { get; set; }
        public virtual DbSet<USUARIO_FORMA> USUARIO_FORMA { get; set; }
        public virtual DbSet<USUARIO_FUNCIONARIO> USUARIO_FUNCIONARIO { get; set; }
        public virtual DbSet<USUARIO_INGRESO> USUARIO_INGRESO { get; set; }
        public virtual DbSet<USUARIO_LOGIN> USUARIO_LOGIN { get; set; }
        public virtual DbSet<USUARIO_PANEL> USUARIO_PANEL { get; set; }
        public virtual DbSet<USUARIO_ROL> USUARIO_ROL { get; set; }
        public virtual DbSet<USUARIO_SP> USUARIO_SP { get; set; }
        public virtual DbSet<USUARIOS_VITAL> USUARIOS_VITAL { get; set; }
    }
}
