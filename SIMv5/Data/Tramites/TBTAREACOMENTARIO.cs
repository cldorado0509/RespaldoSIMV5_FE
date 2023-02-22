namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBTAREACOMENTARIO")]
    public partial class TBTAREACOMENTARIO
    {
        public decimal CODTRAMITE { get; set; }

        public decimal CODTAREA { get; set; }

        [StringLength(1000)]
        public string COMENTARIO { get; set; }

        public DateTime? FECHA { get; set; }

        public decimal CODFUNCIONARIO { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CODCOMENTARIO { get; set; }

        [StringLength(1)]
        public string IMPORTANCIA { get; set; }
    }
}
