namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.DEC_INDICADORVARIABLE")]
    public partial class DEC_INDICADORVARIABLE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DEC_INDICADORVARIABLE()
        {
            DEC_INDICADORDETALLE = new HashSet<DEC_INDICADORDETALLE>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_DECINDICADORVARIABLE { get; set; }

        [Required]
        [StringLength(100)]
        public string S_VARIABLE { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ACTIVA { get; set; }

        public decimal N_ORDEN { get; set; }

        [StringLength(20)]
        public string S_UNIDADES { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DEC_INDICADORDETALLE> DEC_INDICADORDETALLE { get; set; }
    }
}
