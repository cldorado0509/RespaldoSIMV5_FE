namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.DEC_INDICADOR")]
    public partial class DEC_INDICADOR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DEC_INDICADOR()
        {
            DEC_INDICADORDETALLE = new HashSet<DEC_INDICADORDETALLE>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_DECINDICADOR { get; set; }

        public int? ID_ESTADO { get; set; }

        public int? ID_INSTALACION { get; set; }

        public int? ID_TERCERO { get; set; }

        public DateTime D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        public DateTime D_REPORTE { get; set; }

        [StringLength(100)]
        public string S_RESPONSABLEEMAIL { get; set; }

        [StringLength(100)]
        public string S_RESPONSABLE { get; set; }

        [StringLength(2000)]
        public string S_OBSERVACION { get; set; }

        [ForeignKey("ID_INSTALACION")]
        public virtual INSTALACION INSTALACION { get; set; }

        [ForeignKey("ID_TERCERO")]
        public virtual TERCERO TERCERO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DEC_INDICADORDETALLE> DEC_INDICADORDETALLE { get; set; }
    }
}
