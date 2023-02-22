namespace SIM.Data.Poeca
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("POECA.TPOEAIR_ACCIONES_PLAN")]
    public partial class TPOEAIR_ACCIONES_PLAN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TPOEAIR_ACCIONES_PLAN()
        {
            TPOEAIR_SEGUIMIENTO_META = new HashSet<TPOEAIR_SEGUIMIENTO_META>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ID_PLAN { get; set; }

        public int ID_TERCEROS { get; set; }

        [Required]
        [Display(Name = "Acción")]
        public int ID_MEDIDA_ACCION { get; set; }

        [Display(Name = "Nivel")]
        public int ID_NIVEL { get; set; }

        [Display(Name = "Producto")]
        public int ID_PRODUCTO { get; set; }

        [Display(Name = "Periodicidad")]
        public int ID_PERIODICIDAD { get; set; }

        [Required]
        [Display(Name = "Meta propuesta")]
        public int N_META_PROPUESTA { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Cargo del Responsable")]
        public string S_RESPONSABLE { get; set; }

        [StringLength(100)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Recursos no monetarios")]
        public string S_RECURSOS { get; set; }

        [Display(Name = "Valoración económica")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public double? N_VALORACION_ECONOMICA { get; set; }

        public DateTime? D_FECHA_CREACION { get; set; }


        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Observaciones")]
        public string S_OBSERVACIONES { get; set; }

        public int? ID_RESPONSABLE { get; set; }

        public virtual DPOEAIR_NIVEL DPOEAIR_NIVEL { get; set; }

        public virtual DPOEAIR_PRODUCTO DPOEAIR_PRODUCTO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TPOEAIR_SEGUIMIENTO_META> TPOEAIR_SEGUIMIENTO_META { get; set; }

        public virtual TPOEAIR_MEDIDA_ACCION TPOEAIR_MEDIDA_ACCION { get; set; }

        public virtual TPOEAIR_PERIODICIDAD TPOEAIR_PERIODICIDAD { get; set; }

        public virtual TPOEAIR_PLAN TPOEAIR_PLAN { get; set; }
    }
}
