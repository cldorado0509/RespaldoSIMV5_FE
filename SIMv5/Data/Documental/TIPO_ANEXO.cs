namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.TIPO_ANEXO")]
    public partial class TIPO_ANEXO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TIPOANEXO { get; set; }

        [StringLength(250)]
        public string S_NOMBRE { get; set; }

        [StringLength(250)]
        public string S_DESCRIPCION { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        [StringLength(10)]
        public string S_CODIGO { get; set; }
    }
}
