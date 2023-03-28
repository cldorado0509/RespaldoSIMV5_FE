namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.PROYECCION_DOC_INDICES")]
    public partial class PROYECCION_DOC_INDICES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PROYECCION_DOC_INDICES { get; set; }

        public int CODINDICE { get; set; }

        [StringLength(510)]
        public string S_VALOR { get; set; }

        public int? ID_PROYECCION_DOC { get; set; }

        public int? ID_VALOR { get; set; }

        [ForeignKey("ID_PROYECCION_DOC")]
        public virtual PROYECCION_DOC PROYECCION_DOC { get; set; }
    }
}
