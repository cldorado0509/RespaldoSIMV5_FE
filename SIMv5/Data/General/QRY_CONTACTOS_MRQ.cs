namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.QRY_CONTACTOS_MRQ")]
    public partial class QRY_CONTACTOS_MRQ
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TERCERO { get; set; }

        public int? ID_TIPODOCUMENTO { get; set; }

        public long? N_DOCUMENTON { get; set; }

        public byte? N_DIGITOVER { get; set; }

        public int? N_TELEFONO { get; set; }

        [StringLength(100)]
        public string S_WEB { get; set; }

        [StringLength(100)]
        public string S_CORREO { get; set; }

        public long? N_CELULAR { get; set; }

        [StringLength(403)]
        public string NOMBRE_COMPLETO { get; set; }

        [StringLength(100)]
        public string S_NOMBRE1 { get; set; }

        [StringLength(100)]
        public string S_NOMBRE2 { get; set; }

        [StringLength(100)]
        public string S_APELLIDO1 { get; set; }

        [StringLength(100)]
        public string S_APELLIDO2 { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal ID_EMPRESA { get; set; }

        public decimal? NIT_EMPRESA { get; set; }

        public decimal? DIGITO_VER { get; set; }

        [StringLength(200)]
        public string EMPRESA { get; set; }
    }
}
