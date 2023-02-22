namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.QRY_REQUISITOS_TRAMITE")]
    public partial class QRY_REQUISITOS_TRAMITE
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_REQUISITO { get; set; }

        [StringLength(250)]
        public string REQUISITO { get; set; }

        public int? ID_TRAMITE { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int N_ORDEN { get; set; }

        [StringLength(50)]
        public string FORMATO { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(1)]
        public string OBLIGATORIO { get; set; }
    }
}
