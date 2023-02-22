namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBPROYECTO")]
    public partial class TBPROYECTO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBPROYECTO()
        {
            TBSOLICITUD = new HashSet<TBSOLICITUD>();
            TRAMITE_EXPEDIENTE_AMBIENTAL = new HashSet<TRAMITE_EXPEDIENTE_AMBIENTAL>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public decimal CODIGO_PROYECTO { get; set; }

        public int? CODIGO_EMPRESA { get; set; }

        public int? CODIGO_MUNICIPIO { get; set; }

        [StringLength(20)]
        public string CODIGO_INDERENA { get; set; }

        [StringLength(20)]
        public string CODIGO_ANTIGUO { get; set; }

        [Required]
        [StringLength(12)]
        public string CM { get; set; }

        [Required]
        [StringLength(254)]
        public string NOMBRE { get; set; }

        [StringLength(500)]
        public string DIRECCION { get; set; }

        public DateTime? FECHA_REGISTRO { get; set; }

        [StringLength(100)]
        public string FUNCIONARIO { get; set; }

        public int? FOLIOS { get; set; }

        [StringLength(20)]
        public string TELEFONO { get; set; }

        public byte? ANULADO { get; set; }

        [StringLength(100)]
        public string UBICACION { get; set; }

        [StringLength(2000)]
        public string OBSERVACION { get; set; }

        [Column(TypeName = "float")]
        public decimal? X { get; set; }

        [Column(TypeName = "float")]
        public decimal? Y { get; set; }

        [Column(TypeName = "float")]
        public decimal? Z { get; set; }

        public decimal? TOMOS { get; set; }

        public int? CODIGO_ABOGADO { get; set; }

        public byte ARCHIVADO { get; set; }

        [StringLength(254)]
        public string OBJETO { get; set; }

        [StringLength(25)]
        public string CIIU { get; set; }

        [StringLength(1)]
        public string REVISADO { get; set; }

        [Column(TypeName = "float")]
        public decimal X_CORD { get; set; }

        [Column(TypeName = "float")]
        public decimal Y_CORD { get; set; }

        [Column(TypeName = "float")]
        public decimal Z_CORD { get; set; }

        public int? COD_FUNCIONARIO { get; set; }

        public int? ID_CLASIFICACION { get; set; }

        [StringLength(1)]
        public string ENTIDAD_PUBLICA { get; set; }

        [StringLength(1)]
        public string ORIGEN { get; set; }

        public DateTime? D_REGISTRO { get; set; }

        [ForeignKey("CODIGO_MUNICIPIO")]
        public virtual TBMUNICIPIO TBMUNICIPIO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBSOLICITUD> TBSOLICITUD { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRAMITE_EXPEDIENTE_AMBIENTAL> TRAMITE_EXPEDIENTE_AMBIENTAL { get; set; }
    }
}
