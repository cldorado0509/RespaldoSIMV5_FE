namespace SIM.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web;
    using SIM.Data.Facturacion;

    public partial class EntitiesSIMOracle : DbContext
    {
        public DbSet<CONCEPTO_TIPOFACTURA> CONCEPTO_TIPOFACTURA { get; set; }
        public DbSet<CUENTA_TIPOFACTURA> CUENTA_TIPOFACTURA { get; set; }
        public DbSet<DETALLE_FACTURA> DETALLE_FACTURA { get; set; }
        //public DbSet<QRY_INFORMESFACTURA> QRY_INFORMESFACTURA { get; }
        //public DbSet<QRY_RESOLUCIONFACTURA> QRY_RESOLUCIONFACTURA { get; }
        public DbSet<TIPO_FACTURA> TIPO_FACTURA { get; set; }
        public DbSet<FACTURA> FACTURA { get; set; }

    }
}