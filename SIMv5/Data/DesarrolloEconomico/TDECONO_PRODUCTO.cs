namespace SIM.Data.DesarrolloEconomico
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DLLO_ECONOMICO.TDECONO_PRODUCTO")]
    public partial class TDECONO_PRODUCTO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }

        public decimal DDECONO_TERCERODEID { get; set; }

        public decimal DDECONO_UNIDAD_MEDIDAID { get; set; }

        [Required]
        [StringLength(200)]
        public string S_NOMBRE { get; set; }

        [Required]
        [StringLength(4000)]
        public string S_DESCRIPCION { get; set; }

        public decimal N_VALOR_UNIDAD { get; set; }

        [StringLength(200)]
        public string S_URL_IMAGEN { get; set; }

        [Required]
        [StringLength(1)]
        public string B_ACTIVO { get; set; }

        //public virtual DDECONO_TERCERODE DDECONO_TERCERODE { get; set; }

        [ForeignKey("DDECONO_UNIDAD_MEDIDAID")]
        public virtual DDECONO_UNIDAD_MEDIDA DDECONO_UNIDAD_MEDIDA { get; set; }
    }
}
