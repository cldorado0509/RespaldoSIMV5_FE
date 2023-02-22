namespace SIM.Data.Contrato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTRATO.PRECONTRATO_PROCPROPUESTAS")]
    public partial class PRECONTRATO_PROCPROPUESTAS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_PROPUESTA { get; set; }

        public decimal ID_PROCESO { get; set; }

        [Required]
        [StringLength(1000)]
        public string S_PROPONENTE { get; set; }

        [Required]
        [StringLength(512)]
        public string S_DIRECCION { get; set; }

        [Required]
        [StringLength(100)]
        public string S_TELEFONO { get; set; }

        [Required]
        [StringLength(255)]
        public string S_CORREO { get; set; }

        [Required]
        [StringLength(512)]
        public string S_RESPONSABLE { get; set; }

        [Required]
        [StringLength(512)]
        public string S_CORREORESP { get; set; }

        public byte[] ARCHIVOPROPUESTA { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ABIERTA_PPTA { get; set; }

        public decimal ID_RADICADO { get; set; }

        public byte[] ARCHICOECONOMICA { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ABIERTA_ECO { get; set; }

        public decimal? ID_RADECO { get; set; }

        public decimal CODTRAMITE { get; set; }

        public DateTime D_REGISTRO { get; set; }

        public string S_NIT { get; set; }
    }
}
