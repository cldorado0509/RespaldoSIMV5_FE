namespace SIM.Data.Poeca
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("POECA.DPOEAIR_PERIODO_IMPLEMENTACION")]
    public partial class PeriodoImplementacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PeriodoImplementacion()
        {
           
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name = "Nombre del Período")]
        [Column("S_NOMBRE_PERIODO_IM")]
        public string NombrePeriodo { get; set; }

        [StringLength(200)]
        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        [Column("S_DESCRIPCION")]
        public string Descripcion { get; set; }

        [Column("ID_RESPONSABLE")]
        public int? IdResponsable { get; set; }

        
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DPOEAIR_EPISODIO> DPOEAIR_EPISODIO { get; set; }
    }
}
