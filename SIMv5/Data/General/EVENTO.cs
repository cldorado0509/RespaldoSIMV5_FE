namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.EVENTO")]
    public partial class EVENTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_EVENTO { get; set; }

        [Required]
        [StringLength(250)]
        public string S_EVENTO { get; set; }

        [Required]
        [StringLength(150)]
        public string S_LUGAR { get; set; }

        public DateTime? D_EVENTO { get; set; }

        public int? N_DURACION { get; set; }

        [StringLength(200)]
        public string S_RESPONSABLE { get; set; }

        public int? N_CAPACIDAD { get; set; }

        [StringLength(150)]
        public string S_CONTACTO { get; set; }

        [StringLength(100)]
        public string S_CORREOCONTACTO { get; set; }

        [StringLength(512)]
        public string S_URL { get; set; }

        public virtual ICollection<PARTICIPANTE> PARTICIPANTES { get; set; }
    }
}
