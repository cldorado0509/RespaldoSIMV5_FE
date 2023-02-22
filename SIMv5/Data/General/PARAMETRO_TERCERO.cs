namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.PARAMETRO_TERCERO")]
    public partial class PARAMETRO_TERCERO
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PARAMETRO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TERCERO { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime D_REPORTE { get; set; }

        [Required]
        [StringLength(200)]
        public string S_VALOR { get; set; }

        [ForeignKey("ID_PARAMETRO")]
        public virtual PARAMETRO PARAMETRO { get; set; }

        [ForeignKey("ID_TERCERO")]
        public virtual TERCERO TERCERO { get; set; }
    }
}
