namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.LOG_DOCUMENTO")]
    public partial class LOG_DOCUMENTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_LOGDOCUMENTO { get; set; }

        public int? ID_TERCERO { get; set; }

        public int ID_DOCUMENTO { get; set; }

        public DateTime? D_ACCESO { get; set; }
    }
}
