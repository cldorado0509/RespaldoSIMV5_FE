namespace SIM.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using SIM.Data.SICOF;

    public partial class EntitiesSIMOracle : DbContext
    {
        public virtual DbSet<B_CAMBIO_INGRESOS> B_CAMBIO_INGRESOS { get; set; }
        public virtual DbSet<DET_COMERCIAL> DET_COMERCIAL { get; set; }
        public virtual DbSet<DET_COMERCIAL_DOCUMENTOS> DET_COMERCIAL_DOCUMENTOS { get; set; }
        public virtual DbSet<MAE_COMERCIAL> MAE_COMERCIAL { get; set; }
        public virtual DbSet<MAE_CONCEPTOS_COMERCIAL> MAE_CONCEPTOS_COMERCIAL { get; set; }
        public virtual DbSet<TIPOS_COMERCIAL> TIPOS_COMERCIAL { get; set; }
        public virtual DbSet<TMP_ASIENTO_CONTABLE> TMP_ASIENTO_CONTABLE { get; set; }
        public virtual DbSet<TMP_COMPROBANTE_EGRESO> TMP_COMPROBANTE_EGRESO { get; set; }
        public virtual DbSet<TMP_COMPROBANTE_INGRESO> TMP_COMPROBANTE_INGRESO { get; set; }
        public virtual DbSet<TMP_COMPROMISO> TMP_COMPROMISO { get; set; }
        public virtual DbSet<TMP_CUENTA_COBRAR> TMP_CUENTA_COBRAR { get; set; }
        public virtual DbSet<TMP_DET_ASIENTO_CONTABLE> TMP_DET_ASIENTO_CONTABLE { get; set; }
        public virtual DbSet<TMP_DISPONIBILIDAD> TMP_DISPONIBILIDAD { get; set; }
        public virtual DbSet<TMP_ORDEN_PAGO> TMP_ORDEN_PAGO { get; set; }
        public virtual DbSet<TMP_RECEPCION_PEDIDO> TMP_RECEPCION_PEDIDO { get; set; }
        public virtual DbSet<PBCATCOL> PBCATCOL { get; set; }
        public virtual DbSet<PBCATFMT> PBCATFMT { get; set; }
        public virtual DbSet<PBCATTBL> PBCATTBL { get; set; }
        public virtual DbSet<V_AFECTACION_INGRESO> V_AFECTACION_INGRESO { get; set; }
        public virtual DbSet<V_FACTURAS_SICOF> V_FACTURAS_SICOF { get; set; }
        public virtual DbSet<V_PARAMETROS_SISTEMA> V_PARAMETROS_SISTEMA { get; set; }
    }
}
