using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace SIM.Areas.Models
{
    public partial class EntitiesSIMSPOracle : DbContext
    {
        public EntitiesSIMSPOracle()
            : base("name=EntitiesSIMSPOracle")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    }
}