namespace SIM.Data.ExpedienteAmbiental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EXPEDIENTEAMBIENTAL.TEXPAMB_EXPEDIENTEAMBIENTAL")]
    public partial class TEXPAMB_EXPEDIENTEAMBIENTAL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEXPAMB_EXPEDIENTEAMBIENTAL()
        {
            TEXPAMB_ABOGADOEXPEDIENTE = new HashSet<TEXPAMB_ABOGADOEXPEDIENTE>();
            TEXPAMB_FUNCIONARIOEXPEDIENTE = new HashSet<TEXPAMB_FUNCIONARIOEXPEDIENTE>();
            TEXPAMB_PUNTOCONTROL = new HashSet<TEXPAMB_PUNTOCONTROL>();
        }

        public int ID { get; set; }

        public int? CODIGO_PROYECTO { get; set; }

        [Required]
        [StringLength(254)]
        public string S_NOMBRE { get; set; }

        [Required]
        [StringLength(12)]
        public string S_CM { get; set; }

        [Required]
        [StringLength(2000)]
        public string S_DESCRIPCION { get; set; }

        public int CLASIFICACIONEXPEDIENTE_ID { get; set; }

        public int MUNICIPIO_ID { get; set; }

        [Required]
        [StringLength(500)]
        public string S_DIRECCION { get; set; }

        public DateTime D_REGISTRO { get; set; }

        public decimal ANULADO { get; set; }

        public decimal ARCHIVADO { get; set; }

        public virtual DEXPAMB_CLASIFICACIONEXPEDIENTE DEXPAMB_CLASIFICACIONEXPEDIENTE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEXPAMB_ABOGADOEXPEDIENTE> TEXPAMB_ABOGADOEXPEDIENTE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEXPAMB_FUNCIONARIOEXPEDIENTE> TEXPAMB_FUNCIONARIOEXPEDIENTE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEXPAMB_PUNTOCONTROL> TEXPAMB_PUNTOCONTROL { get; set; }
    }
}
