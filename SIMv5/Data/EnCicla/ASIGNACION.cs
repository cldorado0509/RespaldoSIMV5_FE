namespace SIM.Data.EnCicla
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ENCICLA.ASIGNACION")]
    public partial class ASIGNACION
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ASIGNACION { get; set; }

        public int ID_ESTRATEGIA { get; set; }

        public int ID_BICICLETA { get; set; }

        public DateTime D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        [ForeignKey("ID_BICICLETA")]
        public virtual BICICLETA BICICLETA { get; set; }

        [ForeignKey("ID_ESTRATEGIA")]
        public virtual ESTRATEGIA ESTRATEGIA { get; set; }
    }
}
