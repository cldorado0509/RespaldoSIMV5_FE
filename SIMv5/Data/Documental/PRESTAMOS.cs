namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.PRESTAMOS")]
    public partial class PRESTAMOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PRESTAMO { get; set; }

        public DateTime? D_PRESTAMO { get; set; }

        [StringLength(1000)]
        public string S_OBSERVACION { get; set; }

        public int? ID_TERCEROPRESTA { get; set; }

        public int? ID_TERCEROPRESTAMO { get; set; }
    }
}
