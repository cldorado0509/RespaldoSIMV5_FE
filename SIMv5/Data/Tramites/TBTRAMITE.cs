namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBTRAMITE")]
    public partial class TBTRAMITE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBTRAMITE()
        {
            DOCUMENTO_TEMPORAL = new HashSet<DOCUMENTO_TEMPORAL>();
            TBINDICEDOCUMENTO = new HashSet<TBINDICEDOCUMENTO>();
            TRAMITE_EXPEDIENTE_QUEJA = new HashSet<TRAMITE_EXPEDIENTE_QUEJA>();
            TBTRAMITEDOCUMENTO = new HashSet<TBTRAMITEDOCUMENTO>();
            TBTRAMITETAREA = new HashSet<TBTRAMITETAREA>();
            TRAMITE_EXPEDIENTE_AMBIENTAL = new HashSet<TRAMITE_EXPEDIENTE_AMBIENTAL>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal CODTRAMITE { get; set; }

        public decimal CODPROCESO { get; set; }

        [StringLength(200)]
        public string CLIENTE { get; set; }

        [StringLength(100)]
        public string CEDULA { get; set; }

        [StringLength(510)]
        public string DIRECCION { get; set; }

        [StringLength(100)]
        public string TELEFONO { get; set; }

        [StringLength(200)]
        public string MAIL { get; set; }

        [StringLength(2000)]
        public string COMENTARIOS { get; set; }

        public DateTime? FECHAINI { get; set; }

        public DateTime? FECHAFIN { get; set; }

        public DateTime? FECHALIMITE { get; set; }

        public decimal? PRIORIDAD { get; set; }

        public decimal? ESTADO { get; set; }

        [StringLength(100)]
        public string CARPETA { get; set; }

        [StringLength(200)]
        public string CLAVE { get; set; }

        [StringLength(500)]
        public string MENSAJE { get; set; }

        public decimal? TIEMPO_ACUMULADO { get; set; }

        public decimal? DIAS_ACUMULADOS { get; set; }

        [StringLength(4000)]
        public string HORAS_ACUMULADAS { get; set; }

        public decimal? MINUTOS_ACUMULADOS { get; set; }

        public decimal? DIAS_ACUMULADOS_HABILES { get; set; }

        public decimal? HORAS_ACUMULADAS_HABILES { get; set; }

        public decimal? MINUTOS_ACUMULADOS_HABILES { get; set; }

        [StringLength(30)]
        public string NUMERO_VITAL { get; set; }

        public decimal? CODTRAMITE_ANTERIOR { get; set; }

        public int? AGRUPADOR { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTO_TEMPORAL> DOCUMENTO_TEMPORAL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBINDICEDOCUMENTO> TBINDICEDOCUMENTO { get; set; }

        [ForeignKey("CODPROCESO")]
        public virtual TBPROCESO TBPROCESO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRAMITE_EXPEDIENTE_QUEJA> TRAMITE_EXPEDIENTE_QUEJA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBTRAMITEDOCUMENTO> TBTRAMITEDOCUMENTO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBTRAMITETAREA> TBTRAMITETAREA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRAMITE_EXPEDIENTE_AMBIENTAL> TRAMITE_EXPEDIENTE_AMBIENTAL { get; set; }
    }
}
