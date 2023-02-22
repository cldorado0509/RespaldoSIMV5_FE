namespace SIM.Data.DesarrolloEconomico
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DLLO_ECONOMICO.DDECONO_MUNICIPIO_TERCERO")]
    public partial class DDECONO_MUNICIPIO_TERCERO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }

        public decimal DDECONO_TERCERODEID { get; set; }

        public decimal DDECONO_MUNICIPIOID { get; set; }

        //public virtual DDECONO_MUNICIPIO DDECONO_MUNICIPIO { get; set; }

        //public virtual DDECONO_TERCERODE DDECONO_TERCERODE { get; set; }
    }
}
