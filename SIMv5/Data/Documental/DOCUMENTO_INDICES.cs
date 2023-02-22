namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.DOCUMENTO_INDICES")]
    public partial class DOCUMENTO_INDICES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_DOCUMENTOINDICE { get; set; }

        [StringLength(1000)]
        public string S_VALOR { get; set; }

        public decimal? N_VALOR { get; set; }

        public DateTime? D_VALOR { get; set; }

        public DateTime? D_CREACION { get; set; }

        public int ID_INDICE { get; set; }

        public int ID_DOCUMENTO { get; set; }
    }
}
