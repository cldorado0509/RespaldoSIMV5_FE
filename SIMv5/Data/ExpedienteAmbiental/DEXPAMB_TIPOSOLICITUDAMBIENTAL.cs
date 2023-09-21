namespace SIM.Data.ExpedienteAmbiental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EXPEDIENTEAMBIENTAL.DEXPAMB_TIPOSOLICITUDAMBIENTAL")]
    public partial class DEXPAMB_TIPOSOLICITUDAMBIENTAL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DEXPAMB_TIPOSOLICITUDAMBIENTAL()
        {
            TEXPAMB_PUNTOCONTROL = new HashSet<TEXPAMB_PUNTOCONTROL>();
        }

        public int ID { get; set; }

        public int COMPONENTEAMBIENTAL_ID { get; set; }

        [Required]
        [StringLength(254)]
        public string S_NOMBRE { get; set; }

        [StringLength(2000)]
        public string S_DESCRIPCION { get; set; }

        public byte B_REQUIEREAUTOINICIO { get; set; }

        public byte B_REQUIERERESOLUCION { get; set; }

        public byte B_HABILITADO { get; set; }

        public int? ID_MIGRACION { get; set; }

        public virtual DEXPAMB_COMPONENTEAMBIENTAL DEXPAMB_COMPONENTEAMBIENTAL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEXPAMB_PUNTOCONTROL> TEXPAMB_PUNTOCONTROL { get; set; }
    }
}
