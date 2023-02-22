namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.TIPO_DOCUMENTAL")]
    public partial class TIPO_DOCUMENTAL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TIPODOCUMENTAL { get; set; }

        [StringLength(250)]
        public string S_NOMBRE { get; set; }

        [StringLength(1000)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(50)]
        public string S_CODIGO { get; set; }

        public DateTime? D_CREACION { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }
    }
}
