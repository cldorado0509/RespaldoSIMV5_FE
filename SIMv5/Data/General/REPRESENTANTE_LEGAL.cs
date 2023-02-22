namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.REPRESENTANTE_LEGAL")]
    public partial class REPRESENTANTE_LEGAL
    {
        [Key]
        public decimal ID_REPRESENTANTE { get; set; }

        public int ID_TERCERO_NATURAL { get; set; }

        public decimal ID_JURIDICO { get; set; }

        public DateTime D_INICIO { get; set; }

        [ForeignKey("ID_TERCERO_NATURAL")]
        public virtual TERCERO TERCERO { get; set; }
    }
}
