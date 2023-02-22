namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.PROYECCION_DOC_FIRMAS")]
    public partial class PROYECCION_DOC_FIRMAS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PROYECCION_DOC_FIRMAS { get; set; }

        public int ID_PROYECCION_DOC { get; set; }

        public int CODFUNCIONARIO { get; set; }

        public DateTime? D_FECHA_FIRMA { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ESTADO { get; set; }

        public int N_ORDEN { get; set; }

        public string S_TIPO { get; set; }
        public int? CODCARGO { get; set; }

        public string S_TIPOFIRMA { get; set; }

        public string TMP_PASS { get; set; }

        [StringLength(1)]
        public string S_REVISA { get; set; }

        [StringLength(1)]
        public string S_APRUEBA { get; set; }

        [ForeignKey("ID_PROYECCION_DOC")]
        public virtual PROYECCION_DOC PROYECCION_DOC { get; set; }
    }
}
