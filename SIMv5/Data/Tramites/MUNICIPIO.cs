namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.MUNICIPIO")]
    public partial class MUNICIPIO
    {
        [Key]
        [StringLength(5)]
        public string ID_MUNICIPIO { get; set; }

        [StringLength(200)]
        public string S_MUNICIPIO { get; set; }

        [StringLength(2000)]
        public string S_OBSERVACION { get; set; }
    }
}
