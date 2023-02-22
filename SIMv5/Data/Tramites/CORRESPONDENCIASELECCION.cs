namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.CORRESPONDENCIASELECCION")]
    public partial class CORRESPONDENCIASELECCION
    {
        [Key, Column(Order = 0)]
        public decimal CODTRAMITE { get; set; }
        [Key, Column(Order = 1)]
        public decimal CODDOCUMENTO { get; set; }

        [Required]
        [StringLength(100)]
        public string S_TIPOSERVICIO { get; set; }
    }
}
