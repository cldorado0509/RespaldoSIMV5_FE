namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.PERIODOBALANCE_PARAMETRO")]
    public partial class PERIODOBALANCE_PARAMETRO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PBPARAMETRO { get; set; }

        public int ID_PERIODOBALANCE { get; set; }

        public int ID_PARAMETRO { get; set; }

        public decimal N_VALOR { get; set; }

        [StringLength(200)]
        public string S_DESCRIPCION { get; set; }

        [ForeignKey("ID_PARAMETRO")]
        public virtual PARAMETRO PARAMETRO { get; set; }

        [ForeignKey("ID_PERIODOBALANCE")]
        public virtual PERIODO_BALANCE PERIODO_BALANCE { get; set; }
    }
}
