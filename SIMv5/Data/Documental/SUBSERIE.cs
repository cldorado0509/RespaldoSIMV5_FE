namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.SUBSERIE")]
    public partial class SUBSERIE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_SUBSERIE { get; set; }

        public int ID_SERIE { get; set; }

        [StringLength(250)]
        public string S_NOMBRE { get; set; }

        public DateTime? D_CREACION { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        [StringLength(100)]
        public string S_CODIGO { get; set; }

        [StringLength(1000)]
        public string S_DESCRIPCION { get; set; }

        public int? N_TIEMPOCENTRAL { get; set; }

        public int? N_TIEMPOHISTORICO { get; set; }

        public int? N_TIEMPOGESTION { get; set; }
    }
}
