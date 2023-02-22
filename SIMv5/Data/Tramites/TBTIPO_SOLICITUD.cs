namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBTIPO_SOLICITUD")]
    public partial class TBTIPO_SOLICITUD
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBTIPO_SOLICITUD()
        {
            TBSOLICITUD = new HashSet<TBSOLICITUD>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_TIPO_SOLICITUD { get; set; }

        [Required]
        [StringLength(100)]
        public string NOMBRE { get; set; }

        [StringLength(100)]
        public string REGISTRO { get; set; }

        public decimal? DISPONIBLE { get; set; }

        [StringLength(254)]
        public string RUTA { get; set; }

        [StringLength(4000)]
        public string DESCRIPCION { get; set; }

        [StringLength(1)]
        public string ELIMINADO { get; set; }

        [Required]
        [StringLength(1)]
        public string REQUIERE_AUTO_INICIO { get; set; }

        [Required]
        [StringLength(1)]
        public string REQUIERE_RESOLUCION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBSOLICITUD> TBSOLICITUD { get; set; }
    }
}
