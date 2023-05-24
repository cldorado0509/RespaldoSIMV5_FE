
namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.QRY_ABOGADOS")]
    public class QRY_ABOGADOS
    {
        [Key]
        [Column(Order = 0)]
        public decimal CODFUNCIONARIO { get; set; }

        [StringLength(246)]
        public string NOMBRES { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal CODCARGO { get; set; }

        public int? CODGRUPOT { get; set; }

        [StringLength(1)]
        public string ACTIVO { get; set; }

    }
}