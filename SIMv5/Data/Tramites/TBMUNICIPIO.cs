namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBMUNICIPIO")]
    public partial class TBMUNICIPIO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBMUNICIPIO()
        {
            TBPROYECTO = new HashSet<TBPROYECTO>();
            TBSOLICITUD = new HashSet<TBSOLICITUD>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_MUNICIPIO { get; set; }

        [StringLength(4)]
        public string CODIGO_AREA { get; set; }

        [Required]
        [StringLength(20)]
        public string CODIGO_DANE { get; set; }

        public decimal? COORX { get; set; }

        public decimal? COORY { get; set; }

        public decimal? COORZ { get; set; }

        [Required]
        [StringLength(100)]
        public string NOMBRE { get; set; }

        public int? RATA_CRECIMIENTO { get; set; }

        public decimal CODIGO_DEPARTAMENTO { get; set; }

        public int? ID_DIVIPOLA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBPROYECTO> TBPROYECTO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBSOLICITUD> TBSOLICITUD { get; set; }
    }
}
