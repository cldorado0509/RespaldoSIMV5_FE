namespace SIM.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using SIM.Data.DesarrolloEconomico;

    public partial class EntitiesSIMOracle : DbContext
    {
        public virtual DbSet<DDECONO_CATEGORIA> DDECONO_CATEGORIA { get; set; }
        public virtual DbSet<DDECONO_CATEGORIA_TERCERO> DDECONO_CATEGORIA_TERCERO { get; set; }
        public virtual DbSet<DDECONO_MUNICIPIO> DDECONO_MUNICIPIO { get; set; }
        public virtual DbSet<DDECONO_MUNICIPIO_TERCERO> DDECONO_MUNICIPIO_TERCERO { get; set; }
        public virtual DbSet<DDECONO_TERCERODE> DDECONO_TERCERODE { get; set; }
        public virtual DbSet<DDECONO_UNIDAD_MEDIDA> DDECONO_UNIDAD_MEDIDA { get; set; }
        public virtual DbSet<TDECONO_PRODUCTO> TDECONO_PRODUCTO { get; set; }
        public virtual DbSet<VDECONO_PRODUCTO> VDECONO_PRODUCTO { get; set; }
        public virtual DbSet<VDECONO_TERCERODE> VDECONO_TERCERODE { get; set; }
    }
}
