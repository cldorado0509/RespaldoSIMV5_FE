namespace SIM.Areas.Dynamics.Data
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
    using SIM.Areas.Dynamics.Data;

    /// <summary>
    /// 
    /// </summary>
    public class DynamicsContext : DbContext
    {
        //public virtual DbSet<AssetTable> AssetTable { get; set; }
        //public virtual DbSet<AssetBook> AssetBook { get; set; }
        //public virtual DbSet<HcmWorker> HcmWorker { get; set; }
        //public virtual DbSet<DirPersonName> DirPersonName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DynamicsContext()
    : base("name=DynamicsContext")
        {
            Database.SetInitializer<DynamicsContext>(null);
        }
    }
}


