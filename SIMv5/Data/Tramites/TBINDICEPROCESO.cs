namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBINDICEPROCESO")]
    public partial class TBINDICEPROCESO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBINDICEPROCESO()
        {
            TBINDICETRAMITE = new HashSet<TBINDICETRAMITE>();
        }

        [Key]
        public decimal CODINDICE { get; set; }

        public decimal CODPROCESO { get; set; }

        [Required]
        [StringLength(1000)]
        public string INDICE { get; set; }

        public decimal? TIPO { get; set; }

        public decimal? LONGITUD { get; set; }

        public decimal? OBLIGA { get; set; }

        [StringLength(10)]
        public string PROPIEDADES { get; set; }

        [StringLength(510)]
        public string VALORDEFECTO { get; set; }

        public decimal? CODIGO_SUBSERIE { get; set; }

        [StringLength(1)]
        public string SEGUIMIENTO { get; set; }

        [StringLength(1)]
        public string UNICO { get; set; }

        [StringLength(1)]
        public string AUTO { get; set; }

        [StringLength(4000)]
        public string SCRIPT { get; set; }

        [StringLength(1)]
        public string IDENTIFICA_EXPEDIENTE { get; set; }

        public int? CODGRUPO_INDICE { get; set; }

        [StringLength(1)]
        public string MOSTRAR { get; set; }

        [StringLength(1)]
        public string CONJUNTO_VALORES { get; set; }

        public decimal? ORDEN { get; set; }

        [StringLength(20)]
        public string VALORMAXIMO { get; set; }

        [StringLength(20)]
        public string VALORMINIMO { get; set; }

        public decimal? CODSECUENCIA { get; set; }

        public decimal? ID_INDICEOBLIGA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBINDICETRAMITE> TBINDICETRAMITE { get; set; }
    }
}
