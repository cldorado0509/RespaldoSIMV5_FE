namespace SIM.Data.Poeca
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("POECA.DPOEAIR_MEDIDA")]
    public partial class DPOEAIR_MEDIDA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DPOEAIR_MEDIDA()
        {
            TPOEAIR_SECTOR_MEDIDA = new HashSet<TPOEAIR_SECTOR_MEDIDA>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(511)]
        [Display(Name = "Medida")]
        [DataType(DataType.MultilineText)]
        public string S_NOMBRE_MEDIDA { get; set; }

        [StringLength(511)]
        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        public string S_DESCRIPCION { get; set; }

        //[Display(Name = "¿Es obligatoria? (Acuerdo 04)")]
        //public char S_ES_OBLIGATORIA { get; set; }

        [Display(Name = "¿Es obligatoria? (Establecida por Acuerdo Metropolitano)")]
        public string S_ES_OBLIGATORIA { get; set; }

        //[NotMapped]
        //public bool ES_OBLIGATORIA
        //{
        //    get { return S_ES_OBLIGATORIA == "1"; }
        //    set { S_ES_OBLIGATORIA = value ? "1" : "0"; } // Si value es verdadero, se pone 1 en la BD. Se pone 0 en caso contratio
        //}

        [Display(Name = "Responsable")]
        public int? ID_RESPONSABLE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TPOEAIR_SECTOR_MEDIDA> TPOEAIR_SECTOR_MEDIDA { get; set; }
    }
}
