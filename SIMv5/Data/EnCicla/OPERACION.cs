namespace SIM.Data.EnCicla
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ENCICLA.OPERACION")]
    public partial class OPERACION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OPERACION()
        {
            HISTORICO = new HashSet<HISTORICO>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_OPERACION { get; set; }

        public int? ID_ESTACION { get; set; }

        public int? ID_ESTACIONORIGEN { get; set; }

        public int ID_BICICLETA { get; set; }

        public DateTime D_INICIO { get; set; }

        public int? ID_USUARIO { get; set; }

        public int? ID_RESPONSABLE { get; set; }

        public int? ID_RESPONSABLEORIGEN { get; set; }

        public int? ID_ESTADO { get; set; }

        [StringLength(200)]
        public string S_OBSERVACION { get; set; }

        [ForeignKey("ID_BICICLETA")]
        public virtual BICICLETA BICICLETA { get; set; }

        [ForeignKey("ID_ESTACION")]
        public virtual ESTACION ESTACION { get; set; }

        [ForeignKey("ID_ESTACIONORIGEN")]
        public virtual ESTACION ESTACIONORIGEN { get; set; }

        [ForeignKey("ID_ESTADO")]
        public virtual ESTADO_EN ESTADO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HISTORICO> HISTORICO { get; set; }

        [ForeignKey("ID_USUARIO")]
        public virtual TERCERO_ROL TERCERO_ROL { get; set; }

        [ForeignKey("ID_RESPONSABLE")]
        public virtual TERCERO_ROL TERCERO_ROL1 { get; set; }

        [ForeignKey("ID_RESPONSABLEORIGEN")]
        public virtual TERCERO_ROL TERCERO_ROL2 { get; set; }
    }
}
