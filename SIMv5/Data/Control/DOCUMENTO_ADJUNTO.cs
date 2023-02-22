namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.DOCUMENTO_ADJUNTO")]
    public partial class DOCUMENTO_ADJUNTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_DOCUMENTOADJUNTO { get; set; }

        public int? ID_DOCUMENTO { get; set; }

        public int? ID_FORMULARIO { get; set; }

        public decimal? ID_ESTADO { get; set; }
    }
}
