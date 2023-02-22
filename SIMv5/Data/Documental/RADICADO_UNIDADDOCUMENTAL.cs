namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.RADICADO_UNIDADDOCUMENTAL")]
    public partial class RADICADO_UNIDADDOCUMENTAL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RADICADO_UNIDADDOCUMENTAL()
        {
            RADICADOS = new HashSet<RADICADOS_ETIQUETAS>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_RADICADOUNIDADDOCUMENTAL { get; set; }

        public int ID_TIPORADICADO { get; set; }

        public int ID_UNIDADDOCUMENTAL { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        public int? N_CONSECUTIVO { get; set; }

        [ForeignKey("ID_TIPORADICADO")]
        public virtual TIPO_RADICADO TIPO_RADICADO { get; set; }

        [ForeignKey("ID_UNIDADDOCUMENTAL")]
        public virtual UNIDAD_DOCUMENTAL UNIDAD_DOCUMENTAL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RADICADOS_ETIQUETAS> RADICADOS { get; set; }
    }
}
