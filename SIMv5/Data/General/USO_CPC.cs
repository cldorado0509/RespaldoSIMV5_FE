namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.USO_CPC")]
    public partial class USO_CPC
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_USOCPC { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CPC { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PERIODOBALANCE { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal N_CANTIDAD { get; set; }

        [StringLength(1)]
        public string S_TIPO { get; set; }

        [StringLength(200)]
        public string S_DESCRIPCION { get; set; }

        [ForeignKey("ID_CPC")]
        public virtual CPC CPC { get; set; }

        [ForeignKey("ID_PERIODOBALANCE")]
        public virtual PERIODO_BALANCE PERIODO_BALANCE { get; set; }
    }
}
