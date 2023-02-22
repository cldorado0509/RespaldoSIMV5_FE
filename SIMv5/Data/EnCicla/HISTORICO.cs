namespace SIM.Data.EnCicla
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ENCICLA.HISTORICO")]
    public partial class HISTORICO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_HISTORICO { get; set; }

        public int ID_OPERACION { get; set; }

        public int? ID_ESTACION { get; set; }

        public int? ID_ESTACIONORIGEN { get; set; }

        public int ID_BICICLETA { get; set; }

        public int ID_ESTRATEGIA { get; set; }

        public DateTime D_INICIO { get; set; }

        public DateTime D_FIN { get; set; }

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
        public virtual ESTACION ESTACION1 { get; set; }

        [ForeignKey("ID_ESTADO")]
        public virtual ESTADO_EN ESTADO { get; set; }

        [ForeignKey("ID_ESTRATEGIA")]
        public virtual ESTRATEGIA ESTRATEGIA { get; set; }

        [ForeignKey("ID_OPERACION")]
        public virtual OPERACION OPERACION { get; set; }

        [ForeignKey("ID_USUARIO")]
        public virtual TERCERO_ROL TERCERO_ROL { get; set; }

        [ForeignKey("ID_RESPONSABLE")]
        public virtual TERCERO_ROL TERCERO_ROL1 { get; set; }

        [ForeignKey("ID_RESPONSABLEORIGEN")]
        public virtual TERCERO_ROL TERCERO_ROL2 { get; set; }
    }
}
