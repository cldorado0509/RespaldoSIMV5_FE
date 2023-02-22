namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.FORMULARIO")]
    public partial class FORMULARIO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FORMULARIO()
        {
            ENC_SOLUCION = new HashSet<ENC_SOLUCION>();
            FORMULARIO_ENCUESTA = new HashSet<FORMULARIO_ENCUESTA>();
            FRM_RESIDUOS_FOTOGRAFIA = new HashSet<FRM_RESIDUOS_FOTOGRAFIA>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_FORMULARIO { get; set; }

        [StringLength(250)]
        public string S_NOMBRE { get; set; }

        [StringLength(200)]
        public string S_URL { get; set; }

        public decimal? ID_RECURSO { get; set; }

        [StringLength(10)]
        public string ID_ASUNTO { get; set; }

        [StringLength(60)]
        public string TBL_ITEM { get; set; }

        [StringLength(60)]
        public string TBL_ESTADOS { get; set; }

        [StringLength(30)]
        public string S_CAMPO_NOMBRE { get; set; }

        [StringLength(30)]
        public string S_CAMPO_ID_VISITA { get; set; }

        [StringLength(30)]
        public string S_CAMPO_ID_ITEM { get; set; }

        [StringLength(60)]
        public string TBL_FOTOS { get; set; }

        [StringLength(60)]
        public string TBL_GEO { get; set; }

        [StringLength(1)]
        public string S_CARDINALIDAD { get; set; }

        [StringLength(250)]
        public string S_URL_MAPA { get; set; }

        [StringLength(60)]
        public string SEQ_ITEM { get; set; }

        [StringLength(60)]
        public string SEQ_ESTADOS { get; set; }

        [StringLength(60)]
        public string S_CAMPO_AUX1 { get; set; }

        [StringLength(60)]
        public string S_VALOR_CAMPO_AUX1 { get; set; }

        [StringLength(60)]
        public string S_CAMPO_AUX2 { get; set; }

        [StringLength(60)]
        public string S_VALOR_CAMPO_AUX2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ENC_SOLUCION> ENC_SOLUCION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FORMULARIO_ENCUESTA> FORMULARIO_ENCUESTA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FRM_RESIDUOS_FOTOGRAFIA> FRM_RESIDUOS_FOTOGRAFIA { get; set; }
    }
}
