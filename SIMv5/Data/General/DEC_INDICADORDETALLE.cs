namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.DEC_INDICADORDETALLE")]
    public partial class DEC_INDICADORDETALLE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_DECINDICADORDETALLE { get; set; }

        public int ID_DECINDICADOR { get; set; }

        public int ID_DECINDICADORVARIABLE { get; set; }

        public int? ID_UNIDAD { get; set; }

        public decimal? N_VALOR { get; set; }

        [StringLength(500)]
        public string S_DESCRIPCION { get; set; }

        [ForeignKey("ID_DECINDICADOR")]
        public virtual DEC_INDICADOR DEC_INDICADOR { get; set; }

        [ForeignKey("ID_DECINDICADORVARIABLE")]
        public virtual DEC_INDICADORVARIABLE DEC_INDICADORVARIABLE { get; set; }
    }
}
