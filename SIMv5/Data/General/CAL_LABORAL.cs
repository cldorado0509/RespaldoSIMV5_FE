namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.CAL_LABORAL")]
    public partial class CAL_LABORAL
    {
        [Key]
        public decimal ID_DIA { get; set; }

        public decimal? N_DIA { get; set; }

        public DateTime D_DIA { get; set; }

        public DateTime D_JORNADA1INICIO { get; set; }

        public DateTime D_JORNADA1FIN { get; set; }

        public DateTime D_JORNADA2INICIO { get; set; }

        public DateTime D_JORNADA2FIN { get; set; }

        [StringLength(250)]
        public string S_DESCRIPCION { get; set; }

        [Required]
        [StringLength(1)]
        public string S_LABORAL { get; set; }
    }
}
