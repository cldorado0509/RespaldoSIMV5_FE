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
    using SIM.Data.FuentesFijas;

    public partial class EntitiesSIMOracle : DbContext
    {
        public virtual DbSet<TMOVPEM_MONITOREO> TMOVPEM_MONITOREO { get; set; }
    } 
}