namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.PROYECCION_DOC_COM")]
    public partial class PROYECCION_DOC_COM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PROYECCION_DOC_COM { get; set; }

        public int ID_PROYECCION_DOC { get; set; }

        public int CODFUNCIONARIO { get; set; }

        public DateTime FECHA { get; set; }

        [StringLength(2048)]
        public string S_COMENTARIO { get; set; }

        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        [ForeignKey("ID_PROYECCION_DOC")]
        public virtual PROYECCION_DOC PROYECCION_DOC { get; set; }
    }
}
