namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.PARTICIPANTE")]
    public partial class PARTICIPANTE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PARTICIPANTE { get; set; }

        public long N_PARTICIPANTE { get; set; }

        [Required]
        [StringLength(150)]
        public string S_NOMBRE { get; set; }

        [Required]
        [StringLength(150)]
        public string S_APELLIDO { get; set; }

        [StringLength(150)]
        public string S_EMPRESA { get; set; }

        [StringLength(150)]
        public string S_CORREOELECTRONICO { get; set; }

        [Required]
        [StringLength(150)]
        public string S_TELEFONO { get; set; }

        [StringLength(150)]
        public string S_SECTOR { get; set; }

        public int? ID_TERCERO { get; set; }

        [StringLength(100)]
        public string S_MUNICIPIO { get; set; }

        public virtual ICollection<EVENTO> EVENTOS { get; set; }
    }
}
