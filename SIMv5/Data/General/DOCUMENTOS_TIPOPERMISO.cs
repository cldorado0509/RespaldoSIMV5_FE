namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.DOCUMENTOS_TIPOPERMISO")]
    public partial class DOCUMENTOS_TIPOPERMISO
    {
        [Key]
        public decimal ID_DOC { get; set; }

        public decimal ID_TIPOPERMISO { get; set; }

        [Required]
        [StringLength(255)]
        public string S_NOMBREDOC { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ACTIVO { get; set; }
    }
}
