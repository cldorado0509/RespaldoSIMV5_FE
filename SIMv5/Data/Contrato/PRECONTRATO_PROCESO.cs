namespace SIM.Data.Contrato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTRATO.PRECONTRATO_PROCESO")]
    public partial class PRECONTRATO_PROCESO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_PROCESO { get; set; }

        [Required]
        [StringLength(512)]
        public string S_NOMBRE { get; set; }

        [Required]
        [StringLength(2000)]
        public string S_OBJETO { get; set; }

        public DateTime D_REGISTRO { get; set; }

        public decimal? N_MODALIDAD { get; set; }

        public DateTime? D_INICIAPROPUESTAS { get; set; }

        public DateTime D_CIERREPROPUESTAS { get; set; }

        public decimal N_FUNCIONARIO_CONTRATO { get; set; }

        [StringLength(1)]
        public string B_SOBRESELLADO { get; set; }

        public DateTime? D_APERTURA { get; set; }

        [StringLength(1)]
        public string B_PROPECO { get; set; }

        public DateTime? D_PROPECONOMICA { get; set; }
        public DateTime? D_PASOSIM { get; set; }

    }
}
