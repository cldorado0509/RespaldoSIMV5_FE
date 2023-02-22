namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.TOMOS")]
    public partial class TOMOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_TOMO { get; set; }

        [StringLength(250)]
        public string S_NOMBRE { get; set; }

        public DateTime? D_CREACION { get; set; }

        public int ID_EXPEDIENTE { get; set; }

        public int ID_RADICADO { get; set; }

        public int? N_TOMO { get; set; }
    }
}
