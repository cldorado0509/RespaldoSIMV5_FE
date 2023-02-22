namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("TRAMITES.EXP_DOCUMENTOSEXPEDIENTE")]
    public class EXP_DOCUMENTOSEXPEDIENTE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_DOCUMENTOSEXPEDIENTE { get; set; }
        public decimal ID_TOMO { get; set; }
        public decimal ID_DOCUMENTO { get; set; }
        public decimal? N_FOLIOINI { get; set; }
        public decimal? N_FOLIOFIN { get; set; }
        public DateTime D_FECHA { get; set; }
        public int N_ORDEN { get; set; }
        public decimal? ID_FUNCASOCIA { get; set; }
        public decimal N_IMAGENES { get; set; }
    }
}