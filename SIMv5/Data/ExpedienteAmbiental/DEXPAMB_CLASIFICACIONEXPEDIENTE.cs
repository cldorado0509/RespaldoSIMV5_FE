namespace SIM.Data.ExpedienteAmbiental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EXPEDIENTEAMBIENTAL.DEXPAMB_CLASIFICACIONEXPEDIENTE")]
    public partial class DEXPAMB_CLASIFICACIONEXPEDIENTE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DEXPAMB_CLASIFICACIONEXPEDIENTE()
        {
            TEXPAMB_EXPEDIENTEAMBIENTAL = new HashSet<TEXPAMB_EXPEDIENTEAMBIENTAL>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(254)]
        public string S_NOMBRE { get; set; }

        public byte B_HABILITADO { get; set; }

        public decimal? ID_MIGRACION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEXPAMB_EXPEDIENTEAMBIENTAL> TEXPAMB_EXPEDIENTEAMBIENTAL { get; set; }
    }
}
