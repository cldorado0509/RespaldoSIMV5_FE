namespace SIM.Data.Flora
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FLORA.INT_INTERVENCION")]
    public partial class INT_INTERVENCION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public INT_INTERVENCION()
        {
            INT_REGISTRO_FOTOGRAFICO = new HashSet<INT_REGISTRO_FOTOGRAFICO>();
            FLR_SINTOMA_DM = new HashSet<FLR_SINTOMA_DM>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_INTERVENCION { get; set; }

        public DateTime? D_FECHA { get; set; }

        public int? ID_TIPO_INTERVENCION { get; set; }

        public int? ID_ACTOR { get; set; }

        [StringLength(2000)]
        public string S_OBSERVACIONES { get; set; }

        [StringLength(100)]
        public string S_FICHA { get; set; }

        public int? ID_INDIVIDUO_DISPERSO { get; set; }

        public int? ID_OPERADOR { get; set; }

        public DateTime? D_ACTUALIZACION { get; set; }

        [StringLength(1)]
        public string L_PROPUESTO { get; set; }

        public int? ID_CONTRATO { get; set; }

        public int? ID_ESTADO_INT_CONTRATO { get; set; }

        public int? ID_USUARIO { get; set; }

        [StringLength(20)]
        public string ID_INTERVENTOR { get; set; }

        public DateTime? FECHA_INGRESO { get; set; }

        [ForeignKey("ID_TIPO_INTERVENCION")]
        public virtual FLR_INTERVENCION FLR_INTERVENCION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INT_REGISTRO_FOTOGRAFICO> INT_REGISTRO_FOTOGRAFICO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FLR_SINTOMA_DM> FLR_SINTOMA_DM { get; set; }
    }
}
