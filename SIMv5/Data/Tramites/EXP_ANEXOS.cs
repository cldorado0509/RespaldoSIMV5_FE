namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("TRAMITES.EXP_ANEXOS")]
    public class EXP_ANEXOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_ANEXO { get; set; }
        [ForeignKey("EXP_TIPOANEXOS")]
        public decimal ID_TIPOANEXO { get; set; }
        [ForeignKey("EXP_EXPEDIENTES")]
        public decimal ID_EXPEDIENTE { get; set; }
        public string S_UBICACION { get; set; }
        public string S_DESCRIPCION { get; set; }
        public DateTime D_FECHACREACION { get; set; }
        public decimal ID_DOCUMENTO { get; set; }
        public virtual ICollection<EXP_TIPOANEXO> EXP_TIPOANEXOS { get; set; }
        public virtual ICollection<EXP_EXPEDIENTES> EXP_EXPEDIENTES { get; set; }
    }
}