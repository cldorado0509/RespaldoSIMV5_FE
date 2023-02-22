namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.QRY_FUNCIONARIO_ALL")]
    public partial class QRY_FUNCIONARIO_ALL
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

        public decimal? CODGRUPO { get; set; }

        [StringLength(500)]
        public string DEPENDENCIA { get; set; }

        [StringLength(120)]
        public string CARGO { get; set; }

        [StringLength(1)]
        public string ACTIVO { get; set; }

        [StringLength(200)]
        public string EMAIL { get; set; }
        public string FIRMA_DIGITAL { get; set; }
    }
}
