using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIM.Data.Tramites
{
    /// <summary>
    /// 
    /// </summary>
    [Table("TRAMITES.LOG_TEMPORALES")]
    public class LOG_TEMPORALES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }
        public DateTime FECHAEVENTO { get; set; }
        public decimal CODFUNCIONARIO { get; set; }
        public decimal CODTRAMITE { get; set; }
        public string NOMBREARCHIVO { get; set; }
        public decimal? ID_DOCUMENTOTEMP { get; set; }
        public string EVENTO { get; set; }
    }
}