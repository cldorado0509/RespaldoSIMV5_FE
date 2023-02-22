namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("TRAMITES.EXP_INDICES")]
    public class EXP_INDICES
    {
        [Key, Column(Order = 0)]
        [ForeignKey("EXPEDIENTE")]
        public decimal ID_EXPEDIENTE { get; set; }
        [Key, Column(Order = 1)]
        [ForeignKey("INDICE")]
        public int CODINDICE { get; set; }
        public string VALOR_TXT { get; set; }
        public decimal? VALOR_NUM { get; set; }
        public DateTime? VALOR_FEC { get; set; }
        public DateTime FECHAREGISTRO { get; set; }
        public virtual EXP_EXPEDIENTES EXPEDIENTE { get; set; }
        public virtual TBINDICESERIE INDICE { get; set; }
    }
}