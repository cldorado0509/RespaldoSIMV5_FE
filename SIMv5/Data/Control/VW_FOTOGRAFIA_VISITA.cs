namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.VW_FOTOGRAFIA_VISITA")]
    public partial class VW_FOTOGRAFIA_VISITA
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_FOTOGRAFIA { get; set; }

        [StringLength(120)]
        public string S_ARCHIVO { get; set; }

        [StringLength(120)]
        public string S_RUTA { get; set; }

        [StringLength(120)]
        public string S_HASH { get; set; }

        public DateTime? D_CREACION { get; set; }

        [StringLength(50)]
        public string S_USUARIO { get; set; }

        public decimal? GPS_LATITUD { get; set; }

        public decimal? GPS_LONGITUD { get; set; }

        [StringLength(50)]
        public string S_ETIQUETA { get; set; }

        public byte? N_ESTADO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_VISITA { get; set; }

        [StringLength(178)]
        public string URL { get; set; }
    }
}
