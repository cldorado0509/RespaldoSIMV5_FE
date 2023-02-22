using SIM.Data.General;
using SIM.Data.Poeca;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace SIM.Data
{
    public partial class EntitiesSIMOracle : DbContext
    {
        public EntitiesSIMOracle()
            : base("name=EntitiesSIMv5Oracle")
        {
            Database.SetInitializer<EntitiesSIMOracle>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //DGA
            modelBuilder.Entity<DGA>()
                .Property(e => e.S_ESSGA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DGA>()
                .Property(e => e.S_SGA)
                .IsUnicode(false);

            modelBuilder.Entity<DGA>()
                .Property(e => e.S_ESSGC)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DGA>()
                .Property(e => e.S_SGC)
                .IsUnicode(false);

            modelBuilder.Entity<DGA>()
                .Property(e => e.S_PRODUCCIONMASLIMPIA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DGA>()
                .Property(e => e.S_ESECOETIQUETADO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DGA>()
                .Property(e => e.S_ECOETIQUETADO)
                .IsUnicode(false);

            modelBuilder.Entity<DGA>()
                .Property(e => e.S_COMPARTEDGA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DGA>()
                .Property(e => e.S_COMPARTEEMPRESA)
                .IsUnicode(false);

            modelBuilder.Entity<DGA>()
                .Property(e => e.S_AGREMIACION)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DGA>()
                .Property(e => e.S_AGREMIACIONASESORIA)
                .IsUnicode(false);

            modelBuilder.Entity<DGA>()
                .Property(e => e.S_FILIAL)
                .IsUnicode(false);

            modelBuilder.Entity<DGA>()
                .Property(e => e.S_FUNCION)
                .IsUnicode(false);

            modelBuilder.Entity<DGA>()
                .Property(e => e.S_ORGANIGRAMA)
                .IsUnicode(false);

            modelBuilder.Entity<DGA>()
                .Property(e => e.S_SEGUIMIENTO)
                .IsUnicode(false);

            modelBuilder.Entity<DGA>()
                .HasMany(e => e.PERSONAL_DGA)
                .WithRequired(e => e.DGA)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DGA>()
                .HasMany(e => e.PERMISO_AMBIENTAL)
                .WithMany(e => e.DGA)
                .Map(m => m.ToTable("DGA_PERMISOAMBIENTAL", "GENERAL").MapLeftKey("ID_DGA").MapRightKey("ID_PERMISOAMBIENTAL"));

            modelBuilder.Entity<PERMISO_AMBIENTAL>()
                .Property(e => e.S_PERMISOAMBIENTAL)
                .IsUnicode(false);

            modelBuilder.Entity<PERMISO_AMBIENTAL>()
                .Property(e => e.S_DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<PERSONAL_DGA>()
                .Property(e => e.S_TIPOPERSONAL)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PERSONAL_DGA>()
                .Property(e => e.S_ESRESPONSABLE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PERSONAL_DGA>()
                .Property(e => e.S_OBSERVACION)
                .IsUnicode(false);

            modelBuilder.Entity<TIPO_PERSONAL_DGA>()
                .Property(e => e.S_NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<TIPO_PERSONAL_DGA>()
                .Property(e => e.S_DESCRIPCION)
                .IsUnicode(false);
            // GENERAL
            modelBuilder.Entity<TERCERO>()
                .HasOptional(e => e.JURIDICA)
                .WithRequired(e => e.TERCERO);

            modelBuilder.Entity<TERCERO>()
                .HasOptional(e => e.NATURAL)
                .WithRequired(e => e.TERCERO);

            modelBuilder.Entity<DIVIPOLA>()
                .Property(e => e.S_CODIGO)
                .IsUnicode(false);

            modelBuilder.Entity<DIVIPOLA>()
                .Property(e => e.S_NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<DIVIPOLA>()
                .Property(e => e.S_CODIGOEPM)
                .IsUnicode(false);

            modelBuilder.Entity<DIVIPOLA>()
                .HasMany(e => e.DIVIPOLA1)
                .WithOptional(e => e.DIVIPOLA2)
                .HasForeignKey(e => e.ID_DIVIPOLAPADRE);

            modelBuilder.Entity<ESTADO>()
                .Property(e => e.S_NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<ESTADO>()
                .Property(e => e.S_CODIGO)
                .IsUnicode(false);

            modelBuilder.Entity<ESTADO>()
                .Property(e => e.S_DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<INSTALACION>()
                .Property(e => e.S_NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<INSTALACION>()
                .Property(e => e.S_CODIGO)
                .IsUnicode(false);

            modelBuilder.Entity<INSTALACION>()
                .Property(e => e.S_SENTIDOVIAPPAL)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<INSTALACION>()
                .Property(e => e.S_SENTIDOVIASEC)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<INSTALACION>()
                .Property(e => e.S_ESPECIAL)
                .IsUnicode(false);

            modelBuilder.Entity<INSTALACION>()
                .Property(e => e.N_COORDX)
                .HasPrecision(15, 10);

            modelBuilder.Entity<INSTALACION>()
                .Property(e => e.N_COORDY)
                .HasPrecision(15, 10);

            modelBuilder.Entity<INSTALACION>()
                .Property(e => e.N_COORDZ)
                .HasPrecision(15, 10);

            modelBuilder.Entity<INSTALACION>()
                .Property(e => e.S_CEDULACATASTRAL)
                .IsUnicode(false);

            modelBuilder.Entity<INSTALACION>()
                .Property(e => e.S_MATRICULAINMOBILIARIA)
                .IsUnicode(false);

            modelBuilder.Entity<INSTALACION>()
                .Property(e => e.S_OBSERVACION)
                .IsUnicode(false);

            modelBuilder.Entity<INSTALACION>()
                .Property(e => e.S_TELEFONO)
                .IsUnicode(false);

            modelBuilder.Entity<INSTALACION>()
                .Property(e => e.S_COORDX)
                .IsUnicode(false);

            modelBuilder.Entity<INSTALACION>()
                .Property(e => e.S_COORDY)
                .IsUnicode(false);

            modelBuilder.Entity<LETRA_VIA>()
                .Property(e => e.S_NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<LETRA_VIA>()
                .HasMany(e => e.INSTALACION)
                .WithOptional(e => e.LETRA_VIA)
                .HasForeignKey(e => e.ID_LETRAVIAPPAL);

            modelBuilder.Entity<LETRA_VIA>()
                .HasMany(e => e.INSTALACION1)
                .WithOptional(e => e.LETRA_VIA1)
                .HasForeignKey(e => e.ID_LETRAVIASEC);

            modelBuilder.Entity<TIPO_INSTALACION>()
                .Property(e => e.S_NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<TIPO_INSTALACION>()
                .Property(e => e.S_CODIGO)
                .IsUnicode(false);

            modelBuilder.Entity<TIPO_INSTALACION>()
                .Property(e => e.S_DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<TIPO_INSTALACION>()
                .Property(e => e.S_PRINCIPAL)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TIPO_INSTALACION>()
                .HasMany(e => e.TIPO_INSTALACION1)
                .WithOptional(e => e.TIPO_INSTALACION2)
                .HasForeignKey(e => e.ID_TIPOINSTALACIONPADRE);

            modelBuilder.Entity<TIPO_VIA>()
                .Property(e => e.S_NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<TIPO_VIA>()
                .Property(e => e.S_ABREVIATURA)
                .IsUnicode(false);

            modelBuilder.Entity<TIPO_VIA>()
                .HasMany(e => e.INSTALACION)
                .WithOptional(e => e.TIPO_VIA)
                .HasForeignKey(e => e.ID_TIPOVIAPPAL);

            modelBuilder.Entity<TIPO_VIA>()
                .HasMany(e => e.INSTALACION1)
                .WithOptional(e => e.TIPO_VIA1)
                .HasForeignKey(e => e.ID_TIPOVIASEC);

            // POECA
            modelBuilder.Entity<DPOEAIR_EPISODIO>()
                .Property(e => e.S_OBSERVACIONES)
                .IsUnicode(false);

            modelBuilder.Entity<DPOEAIR_EPISODIO>()
                .HasRequired(e => e.DPOEAIR_PERIODO_IMPLEMENTACION)
                .WithMany(e => e.DPOEAIR_EPISODIO)
                .HasForeignKey(e => e.ID_PERIODO)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<DPOEAIR_ACCION>()
                .Property(e => e.S_NOMBRE_ACCION)
                .IsUnicode(false);

            modelBuilder.Entity<DPOEAIR_ACCION>()
                .Property(e => e.S_DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<DPOEAIR_ACCION>()
                .HasMany(e => e.TPOEAIR_MEDIDA_ACCION)
                .WithRequired(e => e.DPOEAIR_ACCION)
                .HasForeignKey(e => e.ID_ACCION)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<DPOEAIR_MEDIDA>()
                .Property(e => e.S_NOMBRE_MEDIDA)
                .IsUnicode(false);

            modelBuilder.Entity<DPOEAIR_MEDIDA>()
                .Property(e => e.S_DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<DPOEAIR_MEDIDA>()
                .HasMany(e => e.TPOEAIR_SECTOR_MEDIDA)
                .WithRequired(e => e.DPOEAIR_MEDIDA)
                .HasForeignKey(e => e.ID_MEDIDA)
                .WillCascadeOnDelete(true);

            //modelBuilder.Entity<DPOEAIR_SECTOR>()
            //    .HasMany(e => e.TPOEAIR_SECTOR_MEDIDA)
            //    .WithMany(e => e.DPOEAIR_SECTOR)
            //    .Map(configAction =>
            //    {
            //        configAction.MapLeftKey("ID_SECTOR");
            //        configAction.MapRightKey("ID_MEDIDA");
            //        configAction.ToTable("TPOEAIR_SECTOR_MEDIDA");
            //    });

            modelBuilder.Entity<DPOEAIR_NIVEL>()
                .Property(e => e.S_NOMBRE_NIVEL)
                .IsUnicode(false);

            modelBuilder.Entity<DPOEAIR_NIVEL>()
                .Property(e => e.S_DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<DPOEAIR_NIVEL>()
                .HasMany(e => e.TPOEAIR_ACCIONES_PLAN)
                .WithRequired(e => e.DPOEAIR_NIVEL)
                .HasForeignKey(e => e.ID_NIVEL)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PeriodoImplementacion>()
                .Property(e => e.NombrePeriodo)
                .IsUnicode(false);

            modelBuilder.Entity<PeriodoImplementacion>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<DPOEAIR_EPISODIO>()
                .HasMany(e => e.TPOEAIR_SEGUIMIENTO_GLOBAL)
                .WithRequired(e => e.DPOEAIR_EPISODIO)
                .HasForeignKey(e => e.ID_EPISODIO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DPOEAIR_EPISODIO>()
                .HasMany(e => e.TPOEAIR_SEGUIMIENTO_META)
                .WithRequired(e => e.DPOEAIR_EPISODIO)
                .HasForeignKey(e => e.ID_EPISODIO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DPOEAIR_PRODUCTO>()
                .Property(e => e.S_NOMBRE_PRODUCTO)
                .IsUnicode(false);

            modelBuilder.Entity<DPOEAIR_PRODUCTO>()
                .Property(e => e.S_DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<DPOEAIR_PRODUCTO>()
                .HasMany(e => e.TPOEAIR_ACCIONES_PLAN)
                .WithRequired(e => e.DPOEAIR_PRODUCTO)
                .HasForeignKey(e => e.ID_PRODUCTO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DPOEAIR_SECTOR>()
                .Property(e => e.S_NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<DPOEAIR_SECTOR>()
                .Property(e => e.S_DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<DPOEAIR_SECTOR>()
                .HasMany(e => e.TPOEAIR_SECTOR_MEDIDA)
                .WithRequired(e => e.DPOEAIR_SECTOR)
                .HasForeignKey(e => e.ID_SECTOR)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<TPOEAIR_ACCIONES_PLAN>()
                .Property(e => e.S_RESPONSABLE)
                .IsUnicode(false);

            modelBuilder.Entity<TPOEAIR_ACCIONES_PLAN>()
                .Property(e => e.S_RECURSOS)
                .IsUnicode(false);

            modelBuilder.Entity<TPOEAIR_ACCIONES_PLAN>()
                .Property(e => e.S_OBSERVACIONES)
                .IsUnicode(false);

            modelBuilder.Entity<TPOEAIR_ACCIONES_PLAN>()
                .HasMany(e => e.TPOEAIR_SEGUIMIENTO_META)
                .WithRequired(e => e.TPOEAIR_ACCIONES_PLAN)
                .HasForeignKey(e => e.ID_INFO_ACCION)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TPOEAIR_MEDIDA_ACCION>()
                .HasMany(e => e.TPOEAIR_ACCIONES_PLAN)
                .WithRequired(e => e.TPOEAIR_MEDIDA_ACCION)
                .HasForeignKey(e => e.ID_MEDIDA_ACCION)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<TPOEAIR_PERIODICIDAD>()
                .Property(e => e.S_NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<TPOEAIR_PERIODICIDAD>()
                .Property(e => e.S_DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<TPOEAIR_PERIODICIDAD>()
                .HasMany(e => e.TPOEAIR_ACCIONES_PLAN)
                .WithRequired(e => e.TPOEAIR_PERIODICIDAD)
                .HasForeignKey(e => e.ID_PERIODICIDAD)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TPOEAIR_PLAN>()
                .Property(e => e.S_RADICADO)
                .IsUnicode(false);

            modelBuilder.Entity<TPOEAIR_PLAN>()
                .Property(e => e.S_OBSERVACIONES)
                .IsUnicode(false);

            modelBuilder.Entity<TPOEAIR_PLAN>()
                .Property(e => e.S_REMITENTE)
                .IsUnicode(false);

            modelBuilder.Entity<TPOEAIR_PLAN>()
                .Property(e => e.S_CARGO_REMITENTE)
                .IsUnicode(false);

            modelBuilder.Entity<TPOEAIR_PLAN>()
                .Property(e => e.S_URL_ANEXO)
                .IsUnicode(false);

            modelBuilder.Entity<TPOEAIR_PLAN>()
                .HasMany(e => e.TPOEAIR_ACCIONES_PLAN)
                .WithRequired(e => e.TPOEAIR_PLAN)
                .HasForeignKey(e => e.ID_PLAN)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TPOEAIR_PLAN>()
                .HasMany(e => e.TPOEAIR_SEGUIMIENTO_GLOBAL)
                .WithRequired(e => e.TPOEAIR_PLAN)
                .HasForeignKey(e => e.ID_PLAN)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TPOEAIR_SECTOR_MEDIDA>()
                .HasMany(e => e.TPOEAIR_MEDIDA_ACCION)
                .WithRequired(e => e.TPOEAIR_SECTOR_MEDIDA)
                .HasForeignKey(e => e.ID_MEDIDA_SECTOR)
                .WillCascadeOnDelete(true);


            modelBuilder.Entity<TPOEAIR_SEGUIMIENTO_GLOBAL>()
            .Property(e => e.S_RADICADO)
            .IsUnicode(true);

            modelBuilder.Entity<TPOEAIR_SEGUIMIENTO_GLOBAL>()
                .Property(e => e.S_OBSERVACIONES)
                .IsUnicode(false);

            modelBuilder.Entity<TPOEAIR_SEGUIMIENTO_GLOBAL>()
                .Property(e => e.S_REMITENTE)
                .IsUnicode(false);

            modelBuilder.Entity<TPOEAIR_SEGUIMIENTO_GLOBAL>()
                .Property(e => e.S_CARGO_REMITENTE)
                .IsUnicode(false);

            modelBuilder.Entity<TPOEAIR_SEGUIMIENTO_GLOBAL>()
                .Property(e => e.S_URL_EVIDENCIA)
                .IsUnicode(false);

            modelBuilder.Entity<TPOEAIR_SEGUIMIENTO_META>()
                .Property(e => e.N_VALORACION_ECONOMICA)
                .HasPrecision(38, 0);

            modelBuilder.Entity<TPOEAIR_SEGUIMIENTO_META>()
                .Property(e => e.S_OBSERVACIONES)
                .IsUnicode(false);
        }
    }
}