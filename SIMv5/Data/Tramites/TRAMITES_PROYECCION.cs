namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TRAMITES_PROYECCION")]
    public partial class TRAMITES_PROYECCION
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_TRAMITES_PROYECCION { get; set; }

        public int ID_PROYECCION_DOC { get; set; }

        public int CODTRAMITE { get; set; }

        public int? CODDOCUMENTO { get; set; }

        public DateTime? D_FECHA_GENERACION { get; set; }

        public int? CODTAREA_INICIAL { get; set; }

        [ForeignKey("ID_PROYECCION_DOC")]
        public virtual PROYECCION_DOC PROYECCION_DOC { get; set; }
    }
}
