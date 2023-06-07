namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.INFORMATIVO_TIPOCOM")]
    public partial class INFORMATIVO_TIPOCOM
    {
        [Key]
        public int COD_TIPOCOMUNICACION { get; set; }

        [StringLength(250)]
        public string S_TIPOCOMUNICACION { get; set; }
    }
}
