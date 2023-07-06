namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.PROYECCION_DOC_COPIAS")]
    public partial class PROYECCION_DOC_COPIAS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PROYECCION_DOC_COPIAS { get; set; }

        public int ID_PROYECCION_DOC { get; set; }

        public int CODFUNCIONARIO { get; set; }

        [ForeignKey("ID_PROYECCION_DOC")]
        public virtual PROYECCION_DOC PROYECCION_DOC { get; set; }
    }
}
