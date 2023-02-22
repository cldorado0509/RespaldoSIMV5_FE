namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.ACTUACION_DOCUMENTO")]
    public partial class ACTUACION_DOCUMENTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_ACTUACIONDOCUMENTO { get; set; }

        public int ID_ACTUACION { get; set; }

        public int? ID_TIPOACTO { get; set; }

        public int CODTRAMITE { get; set; }

        public int CODDOCUMENTO { get; set; }

        [ForeignKey("ID_TIPOACTO")]
        public virtual TIPO_ACTO TIPO_ACTO { get; set; }
    }
}
