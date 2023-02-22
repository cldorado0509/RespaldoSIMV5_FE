namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.FORMULARIO_ENCUESTA")]
    public partial class FORMULARIO_ENCUESTA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_FORMULARIOENCUESTA { get; set; }

        public int ID_FORMULARIO { get; set; }

        public int ID_ENCUESTA { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        public int? N_ORDEN { get; set; }

        [ForeignKey("ID_ENCUESTA")]
        public virtual ENC_ENCUESTA ENC_ENCUESTA { get; set; }

        [ForeignKey("ID_FORMULARIO")]
        public virtual FORMULARIO FORMULARIO { get; set; }
    }
}
