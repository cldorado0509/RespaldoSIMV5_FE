namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Web;

    [Table("TRAMITES.EXP_EXPEDIENTES")]
    public partial class EXP_EXPEDIENTES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_EXPEDIENTE { get; set; }
        public decimal ID_UNIDADDOC { get; set; }
        public string S_NOMBRE { get; set; }
        public string S_CODIGO { get; set; }
        public string S_DESCRIPCION { get; set; }
        public DateTime D_FECHACREACION { get; set; }
        public decimal ID_FUNCCREACION { get; set; }
        [StringLength(1)]
        public string S_ESTADO { get; set; }
    }
}