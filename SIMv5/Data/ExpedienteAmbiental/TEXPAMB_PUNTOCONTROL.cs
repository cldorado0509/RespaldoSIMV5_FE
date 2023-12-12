namespace SIM.Data.ExpedienteAmbiental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EXPEDIENTEAMBIENTAL.TEXPAMB_PUNTOCONTROL")]
    public partial class TEXPAMB_PUNTOCONTROL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEXPAMB_PUNTOCONTROL()
        {
            TEXPAMB_ANOTACIONESPUNTO = new HashSet<TEXPAMB_ANOTACIONESPUNTO>();
            TEXPAMB_ESTADOPUNTOCONTROL = new HashSet<TEXPAMB_ESTADOPUNTOCONTROL>();
        }

        public int ID { get; set; }

        public int EXPEDIENTEAMBIENTAL_ID { get; set; }

        public int? EXPEDIENTEDOCUMENTAL_ID { get; set; }

        public int TIPOSOLICITUDAMBIENTAL_ID { get; set; }

        [Required]
        [StringLength(254)]
        public string S_NOMBRE { get; set; }

        [Required]
        [StringLength(60)]
        public string S_CONEXO { get; set; }

        [StringLength(2000)]
        public string S_OBSERVACION { get; set; }

        public DateTime? D_ORIGEN { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_REGISTRO { get; set; }

        public decimal? CODIGOSOLICITUD_ID { get; set; }

        public virtual DEXPAMB_TIPOSOLICITUDAMBIENTAL DEXPAMB_TIPOSOLICITUDAMBIENTAL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEXPAMB_ANOTACIONESPUNTO> TEXPAMB_ANOTACIONESPUNTO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEXPAMB_ESTADOPUNTOCONTROL> TEXPAMB_ESTADOPUNTOCONTROL { get; set; }

        public virtual TEXPAMB_EXPEDIENTEAMBIENTAL TEXPAMB_EXPEDIENTEAMBIENTAL { get; set; }
    }
}
