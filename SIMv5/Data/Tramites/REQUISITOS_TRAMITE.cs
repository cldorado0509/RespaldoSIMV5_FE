namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.REQUISITOS_TRAMITE")]
    public partial class REQUISITOS_TRAMITE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_REQUISITOTRAMITE { get; set; }

        public int ID_TRAMITE { get; set; }

        public int ID_REQUISITO { get; set; }

        public int ID_TERCERO { get; set; }

        public int ID_INSTALACION { get; set; }

        public DateTime D_CARGA { get; set; }

        [Required]
        [StringLength(255)]
        public string RUTA_DOCUMENTO { get; set; }

        [StringLength(20)]
        public string EXTENSION { get; set; }
    }
}
