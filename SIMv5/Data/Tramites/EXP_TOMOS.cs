namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("TRAMITES.EXP_TOMOS")]
    public class EXP_TOMOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_TOMO { get; set; }
        public decimal ID_EXPEDIENTE { get; set; }
        public decimal N_TOMO { get; set; }
        public DateTime D_FECHACREACION { get; set; }
        public decimal ID_FUNCCREACION { get; set; }
        public string S_UBICACION { get; set; }
        public string S_ABIERTO { get; set; }
        public string S_HASH { get; set; }
        public int N_FOLIOS { get; set; }
        public decimal? ID_FUNCCIERRA { get; set; }
        public DateTime? D_FECHACIERRE { get; set; }
    }
}