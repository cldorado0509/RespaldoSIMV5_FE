namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.TIPODOCUMENTAL_DATO")]
    public partial class TIPODOCUMENTAL_DATO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TIPODOCUMENTALDATO { get; set; }

        public int ID_DOCUMENTO { get; set; }

        public int ID_UNIDADTIPO { get; set; }

        [StringLength(1000)]
        public string S_VALOR { get; set; }

        public decimal? N_VALOR { get; set; }

        public DateTime? D_VALOR { get; set; }
    }
}
