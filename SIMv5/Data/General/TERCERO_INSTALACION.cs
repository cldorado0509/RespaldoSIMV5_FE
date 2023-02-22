namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.TERCERO_INSTALACION")]
    public partial class TERCERO_INSTALACION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TERCERO_INSTALACION()
        {
            TERCERO_INSTALACION_PROYECTO = new HashSet<TERCERO_INSTALACION_PROYECTO>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_INSTALACION { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TERCERO { get; set; }

        public int? ID_TIPOINSTALACION { get; set; }

        public int? ID_ACTIVIDADECONOMICA { get; set; }

        public decimal? CODIGO_PROYECTO { get; set; }

        public int? ID_ESTADO { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        public int? N_USUARIO { get; set; }

        public DateTime? D_REGISTRO { get; set; }

        /*[Required]*/
        [StringLength(1)]
        public string S_ACTUAL { get; set; }

        [ForeignKey("ID_INSTALACION")]
        public virtual INSTALACION INSTALACION { get; set; }

        [ForeignKey("ID_ACTIVIDADECONOMICA")]
        public virtual ACTIVIDAD_ECONOMICA ACTIVIDAD_ECONOMICA { get; set; }

        [ForeignKey("ID_TERCERO")]
        public virtual TERCERO TERCERO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TERCERO_INSTALACION_PROYECTO> TERCERO_INSTALACION_PROYECTO { get; set; }
    }
}
