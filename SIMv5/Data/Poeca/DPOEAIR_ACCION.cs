namespace SIM.Data.Poeca
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("POECA.DPOEAIR_ACCION")]
    public partial class DPOEAIR_ACCION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DPOEAIR_ACCION()
        {
            TPOEAIR_MEDIDA_ACCION = new HashSet<TPOEAIR_MEDIDA_ACCION>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(511)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Acción")]
        public string S_NOMBRE_ACCION { get; set; }

        [StringLength(511)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        public string S_DESCRIPCION { get; set; }

        public int? ID_RESPONSABLE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "Sectores y Medidas")]
        public virtual ICollection<TPOEAIR_MEDIDA_ACCION> TPOEAIR_MEDIDA_ACCION { get; set; }
    }
}
