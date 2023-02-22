namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.EXPEDIENTE")]
    public partial class EXPEDIENTE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_EXPEDIENTE { get; set; }

        [StringLength(250)]
        public string S_EXPEDIENTE { get; set; }

        public int ID_UNIDADDOCUMENTAL { get; set; }

        public int ID_RADICADO { get; set; }
    }
}
