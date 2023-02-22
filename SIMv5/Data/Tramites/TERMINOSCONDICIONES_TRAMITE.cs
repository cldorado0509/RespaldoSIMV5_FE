namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TERMINOSCONDICIONES_TRAMITE")]
    public partial class TERMINOSCONDICIONES_TRAMITE
    {
        [Key]
        [Column(Order = 0)]
        public decimal ID_TRAMITE { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal ID_INSTALACION { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal ID_TERCERO { get; set; }

        public string TEXTO_TERMINOSCODICIONES { get; set; }

        public byte? ACEPTA_TYC { get; set; }

        public DateTime? FECHA_ACEPTA_TYC { get; set; }
    }
}
