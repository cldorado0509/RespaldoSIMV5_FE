namespace SIM.Data.DesarrolloEconomico
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DLLO_ECONOMICO.VDECONO_PRODUCTO")]
    public partial class VDECONO_PRODUCTO
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal ID_TERCERO { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal ID_UNIDAD { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(200)]
        public string S_NOMBRE { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(4000)]
        public string S_DESCRIPCION { get; set; }

        [Key]
        [Column(Order = 5)]
        public decimal N_VALOR_UNIDAD { get; set; }

        [StringLength(200)]
        public string S_URL_IMAGEN { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(200)]
        public string UNIDAD { get; set; }
    }
}
