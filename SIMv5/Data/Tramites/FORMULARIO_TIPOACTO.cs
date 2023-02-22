namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.FORMULARIO_TIPOACTO")]
    public partial class FORMULARIO_TIPOACTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_FORMULARIOTIPOACTO { get; set; }

        public int ID_TIPOACTO { get; set; }

        public int ID_FORMULARIO { get; set; }

        [ForeignKey("ID_TIPOACTO")]
        public virtual TIPO_ACTO TIPO_ACTO { get; set; }
    }
}
