namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.DOCUMENTO")]
    public partial class DOCUMENTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_DOCUMENTO { get; set; }

        [StringLength(120)]
        public string S_ARCHIVO { get; set; }

        [StringLength(120)]
        public string S_RUTA { get; set; }

        [StringLength(120)]
        public string S_HASH { get; set; }

        public DateTime? D_CREACION { get; set; }

        [StringLength(50)]
        public string S_USUARIO { get; set; }

        public int? GPS_LATITUD { get; set; }

        public int? GPS_LONGITUD { get; set; }

        [StringLength(50)]
        public string S_ETIQUETA { get; set; }

        public byte? N_ESTADO { get; set; }
    }
}
