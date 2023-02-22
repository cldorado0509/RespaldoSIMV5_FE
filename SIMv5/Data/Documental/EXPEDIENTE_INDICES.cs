namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.EXPEDIENTE_INDICES")]
    public partial class EXPEDIENTE_INDICES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_EXPEDIENTEINDICE { get; set; }

        [StringLength(1000)]
        public string S_VALOR { get; set; }

        public decimal? N_VALOR { get; set; }

        public DateTime? D_VALOR { get; set; }

        public DateTime? D_CREACION { get; set; }

        public int ID_EXPEDIENTE { get; set; }

        public int ID_INDICE { get; set; }
    }
}
