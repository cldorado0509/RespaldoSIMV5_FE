namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.TIPO_EXPEDIENTE")]
    public partial class TIPO_EXPEDIENTE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TIPOEXPEDIENTE { get; set; }

        [StringLength(50)]
        public string S_NOMBRE { get; set; }
    }
}
