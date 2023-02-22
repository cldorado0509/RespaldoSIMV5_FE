namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.VW_FUNCIONARIO")]
    public partial class VW_FUNCIONARIO
    {
        [Key]
        [Column(Order = 0)]
        public decimal CODFUNCIONARIO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(120)]
        public string NOMBRES { get; set; }

        [StringLength(246)]
        public string NOMBRECOMPLETO { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal CODCARGO { get; set; }

        [StringLength(10)]
        public string OFICINA { get; set; }

        [StringLength(8)]
        public string EXTENSION { get; set; }

        [StringLength(200)]
        public string EMAIL { get; set; }

        [StringLength(100)]
        public string LOGIN { get; set; }

        public decimal? CODGRUPO { get; set; }

        public decimal? TIPO { get; set; }

        public int? CODGRUPOT { get; set; }

        [StringLength(1)]
        public string ACTIVO { get; set; }

        [StringLength(50)]
        public string APELLIDOS { get; set; }

        [StringLength(20)]
        public string CEDULA { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(1)]
        public string MOSTRAR_TAREAS_GRUPO { get; set; }

        [StringLength(40)]
        public string Str_CODFUNCIONARIO { get; set; }
    }
}
