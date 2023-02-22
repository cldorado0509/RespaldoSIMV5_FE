namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBTAREARESPONSABLE")]
    public partial class TBTAREARESPONSABLE
    {
        public int CODTAREA { get; set; }

        public int CODFUNCIONARIO { get; set; }

        [Key]
        public decimal CODTAREARESPONSABLE { get; set; }

        public decimal? CODTAREAPADRE { get; set; }
    }
}
