namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.QRY_INDICADORES_PML")]
    public partial class QRY_INDICADORES_PML
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_DECINDICADOR { get; set; }

        public int? ID_ESTADO { get; set; }

        public int? ID_INSTALACION { get; set; }

        public int? ID_TERCERO { get; set; }

        [StringLength(15)]
        public string FECHAINICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime D_REPORTE { get; set; }

        [StringLength(100)]
        public string S_RESPONSABLEEMAIL { get; set; }

        [StringLength(100)]
        public string S_RESPONSABLE { get; set; }

        [StringLength(2000)]
        public string S_OBSERVACION { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_DECINDICADORDETALLE { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_DECINDICADORVARIABLE { get; set; }

        [StringLength(100)]
        public string VARIABLE { get; set; }

        public int? ID_UNIDAD { get; set; }

        [StringLength(50)]
        public string UNIDAD { get; set; }

        public decimal? VALOR { get; set; }

        [StringLength(500)]
        public string DESCRIPCION { get; set; }

        public int? TELEFONO { get; set; }

        [StringLength(301)]
        public string EMPRESA { get; set; }

        public int? ID_DIVIPOLA { get; set; }

        [StringLength(100)]
        public string MUNICIPIO { get; set; }

        [StringLength(191)]
        public string DIRECCION { get; set; }
    }
}
