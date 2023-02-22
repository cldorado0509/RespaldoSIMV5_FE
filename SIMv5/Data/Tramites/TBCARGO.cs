namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBCARGO")]
    public partial class TBCARGO
    {
        [Key]
        public decimal CODCARGO { get; set; }

        [Required]
        [StringLength(120)]
        public string NOMBRE { get; set; }

        [StringLength(510)]
        public string DESCRIPCION { get; set; }

        [StringLength(1)]
        public string ELIMINADO { get; set; }

        [Required]
        [StringLength(1)]
        public string ACTIVO { get; set; }

        [StringLength(1)]
        public string RADICA_MEMORANDO { get; set; }

        [StringLength(1)]
        public string RECIBE_MEMORANDO { get; set; }

        [StringLength(1)]
        public string PUEDE_FIRMAR { get; set; }

        [StringLength(1)]
        public string RECIBE_COPIAMEMO { get; set; }

        [StringLength(120)]
        public string NOMBRE_ENCARGO { get; set; }
    }
}
