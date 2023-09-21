namespace SIM.Data.ExpedienteAmbiental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EXPEDIENTEAMBIENTAL.DEXPAMB_COMPONENTEAMBIENTAL")]
    public partial class DEXPAMB_COMPONENTEAMBIENTAL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DEXPAMB_COMPONENTEAMBIENTAL()
        {
            DEXPAMB_TIPOSOLICITUDAMBIENTAL = new HashSet<DEXPAMB_TIPOSOLICITUDAMBIENTAL>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(254)]
        public string S_NOMBRE { get; set; }

        [StringLength(2000)]
        public string S_DESCRIPCION { get; set; }

        [Required]
        [StringLength(1)]
        public string B_HABILITADO { get; set; }

        public int? COMPONENTE_PADRE_ID { get; set; }

        public int? ID_MIGRACION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DEXPAMB_TIPOSOLICITUDAMBIENTAL> DEXPAMB_TIPOSOLICITUDAMBIENTAL { get; set; }
    }
}
