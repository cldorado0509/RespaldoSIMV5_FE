namespace SIM.Data.EnCicla
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ENCICLA.ESTRATEGIA_ZONA")]
    public partial class ESTRATEGIA_ZONA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ESTRATEGIAZONA { get; set; }

        public int ID_ESTRATEGIA { get; set; }

        public int ID_ZONA { get; set; }

        [StringLength(200)]
        public string S_OBSERVACION { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        [ForeignKey("ID_ESTRATEGIA")]
        public virtual ESTRATEGIA ESTRATEGIA { get; set; }

        [ForeignKey("ID_ZONA")]
        public virtual ZONA_EN ZONA { get; set; }
    }
}
