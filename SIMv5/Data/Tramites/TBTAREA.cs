namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBTAREA")]
    public partial class TBTAREA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBTAREA()
        {
            DOCUMENTO_TEMPORAL = new HashSet<DOCUMENTO_TEMPORAL>();
            TBTRAMITETAREA = new HashSet<TBTRAMITETAREA>();
        }

        [Key]
        public decimal CODTAREA { get; set; }

        public decimal? FKCODTAREA { get; set; }

        public decimal CODPROCESO { get; set; }

        [StringLength(500)]
        public string NOMBRE { get; set; }

        [StringLength(2000)]
        public string DECRIPCION { get; set; }

        public DateTime? FECHAINI { get; set; }

        public DateTime? FECHAFIN { get; set; }

        [Column(TypeName = "float")]
        public decimal? TIEMPO { get; set; }

        public decimal? CODUNIDAD { get; set; }

        public DateTime? FECHALIMITE { get; set; }

        public decimal? ORDEN { get; set; }

        [StringLength(100)]
        public string SCRIPT { get; set; }

        [StringLength(100)]
        public string SHELL { get; set; }

        public DateTime? UPSIZE_TS { get; set; }

        [StringLength(500)]
        public string REGLA { get; set; }

        public int INICIO { get; set; }

        public int FIN { get; set; }

        public int? EMAIL { get; set; }

        [StringLength(2000)]
        public string MENSAJE { get; set; }

        public byte? PANTALLA { get; set; }

        [StringLength(2000)]
        public string OBJETIVO { get; set; }

        [StringLength(4000)]
        public string RECIBE { get; set; }

        [StringLength(4000)]
        public string TERMINA { get; set; }

        [StringLength(1)]
        public string MULTIHILO { get; set; }

        public decimal? CODSUBPROCESO { get; set; }

        public decimal? CODGRUPOTAREA { get; set; }

        [StringLength(1)]
        public string DIGITALIZAR { get; set; }

        [StringLength(1)]
        public string FIRMAR { get; set; }

        public int? COD_EVENTO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTO_TEMPORAL> DOCUMENTO_TEMPORAL { get; set; }

        [ForeignKey("CODPROCESO")]
        public virtual TBPROCESO TBPROCESO { get; set; }

        [ForeignKey("CODSUBPROCESO")]
        public virtual TBPROCESO TBPROCESO1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBTRAMITETAREA> TBTRAMITETAREA { get; set; }
    }
}
