namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.TIPO_RADICADO")]
    public partial class TIPO_RADICADO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TIPORADICADO { get; set; }

        [StringLength(100)]
        public string S_NOMBRE { get; set; }

        [StringLength(250)]
        public string S_FORMATO { get; set; }

        [StringLength(250)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(1)]
        public string S_REINICIOANUAL { get; set; }

        [StringLength(4000)]
        public string S_CONFIGURACION { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }
    }
}
