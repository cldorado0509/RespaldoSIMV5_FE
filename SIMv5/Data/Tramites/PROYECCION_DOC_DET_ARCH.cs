namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.PROYECCION_DOC_DET_ARCH")]
    public partial class PROYECCION_DOC_DET_ARCH
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PROYECCION_DOC_DET_ARCH { get; set; }

        [Required]
        [StringLength(2000)]
        public string S_RUTA_ARCHIVO { get; set; }

        public DateTime D_FECHA_CARGA { get; set; }

        public int ID_PROYECCION_DOC_ARCHIVOS { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        [ForeignKey("ID_PROYECCION_DOC_ARCHIVOS")]
        public virtual PROYECCION_DOC_ARCHIVOS PROYECCION_DOC_ARCHIVOS { get; set; }
    }
}
