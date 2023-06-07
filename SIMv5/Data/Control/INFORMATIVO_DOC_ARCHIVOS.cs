namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.INFORMATIVO_DOC_ARCHIVOS")]
    public partial class INFORMATIVO_DOC_ARCHIVOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_INFORMATIVO_DOC_ARCHIVOS { get; set; }

        public int ID_INFORMATIVO_DOC { get; set; }

        [Required]
        [StringLength(128)]
        public string S_DESCRIPCION { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        public int? N_ORDEN { get; set; }

        [ForeignKey("ID_INFORMATIVO_DOC")]
        public virtual INFORMATIVO_DOC INFORMATIVO_DOC { get; set; }
    }
}
