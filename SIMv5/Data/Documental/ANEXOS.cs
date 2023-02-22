namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.ANEXOS")]
    public partial class ANEXOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_ANEXO { get; set; }

        public int ID_DOCUMENTO { get; set; }

        public int ID_TIPOANEXO { get; set; }

        [StringLength(250)]
        public string S_NOMBRE { get; set; }

        [StringLength(100)]
        public string S_CODIGO { get; set; }

        [StringLength(250)]
        public string S_NOMBRESERVIDOR { get; set; }

        [StringLength(250)]
        public string S_RUTA { get; set; }

        public int ID_RADICADO { get; set; }
    }
}
