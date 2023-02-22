namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.FOTOGRAFIA")]
    public partial class FOTOGRAFIA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FOTOGRAFIA()
        {
            FOTOGRAFIA_VISITA = new HashSet<FOTOGRAFIA_VISITA>();
            FRM_RESIDUOS_FOTOGRAFIA = new HashSet<FRM_RESIDUOS_FOTOGRAFIA>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_FOTOGRAFIA { get; set; }

        [StringLength(120)]
        public string S_ARCHIVO { get; set; }

        [StringLength(120)]
        public string S_RUTA { get; set; }

        [StringLength(120)]
        public string S_HASH { get; set; }

        public DateTime? D_CREACION { get; set; }

        [StringLength(50)]
        public string S_USUARIO { get; set; }

        public decimal? GPS_LATITUD { get; set; }

        public decimal? GPS_LONGITUD { get; set; }

        [StringLength(50)]
        public string S_ETIQUETA { get; set; }

        public byte? N_ESTADO { get; set; }

        [StringLength(1000)]
        public string PALABRA_CLAVE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FOTOGRAFIA_VISITA> FOTOGRAFIA_VISITA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FRM_RESIDUOS_FOTOGRAFIA> FRM_RESIDUOS_FOTOGRAFIA { get; set; }
    }
}
