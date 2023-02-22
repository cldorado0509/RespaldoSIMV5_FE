namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.QRYTERCERO")]
    public partial class QRYTERCERO
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TERCERO { get; set; }

        [StringLength(10)]
        public string TIPO_DOCUMENTO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long NRO_DOCUMENTO { get; set; }

        [StringLength(200)]
        public string RAZON_SOCIAL { get; set; }

        public DateTime? FECHA_REGISTRO { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(200)]
        public string CIIU { get; set; }

        [StringLength(50)]
        public string CODIGO_CIIU { get; set; }

        public int? TELEFONO { get; set; }

        [StringLength(100)]
        public string CORREO_ELECTRONICO { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(1)]
        public string S_AUTORIZANOTIFIELECTRO { get; set; }
    }
}
