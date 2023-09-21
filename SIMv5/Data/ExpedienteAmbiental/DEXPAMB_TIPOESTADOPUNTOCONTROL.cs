namespace SIM.Data.ExpedienteAmbiental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EXPEDIENTEAMBIENTAL.DEXPAMB_TIPOESTADOPUNTOCONTROL")]
    public partial class DEXPAMB_TIPOESTADOPUNTOCONTROL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DEXPAMB_TIPOESTADOPUNTOCONTROL()
        {
            TEXPAMB_ESTADOPUNTOCONTROL = new HashSet<TEXPAMB_ESTADOPUNTOCONTROL>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(254)]
        public string S_NOMBRE { get; set; }

        [StringLength(2000)]
        public string S_DESCRIPCION { get; set; }

        public byte B_HABILITADO { get; set; }

        public int? ID_MIGRACION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEXPAMB_ESTADOPUNTOCONTROL> TEXPAMB_ESTADOPUNTOCONTROL { get; set; }
    }
}
