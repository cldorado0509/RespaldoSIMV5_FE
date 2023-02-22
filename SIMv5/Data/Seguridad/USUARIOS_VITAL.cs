namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.USUARIOS_VITAL")]
    public partial class USUARIOS_VITAL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal CODIGO_USUARIO { get; set; }

        [Required]
        [StringLength(20)]
        public string NUMERO_IDENTIFICACION { get; set; }

        [Required]
        [StringLength(20)]
        public string PERSONA_ID { get; set; }

        [StringLength(20)]
        public string PRIMER_NOMBRE { get; set; }

        [StringLength(20)]
        public string SEGUNDO_NOMBRE { get; set; }

        [StringLength(20)]
        public string PRIMER_APELLIDO { get; set; }

        [StringLength(20)]
        public string SEGUNDO_APELLIDO { get; set; }

        [StringLength(200)]
        public string CORREO_ELECTRONICO { get; set; }

        [StringLength(30)]
        public string TELEFONO { get; set; }

        [StringLength(20)]
        public string CELULAR { get; set; }

        [StringLength(80)]
        public string RAZON_SOCIAL { get; set; }

        [StringLength(1)]
        public string ESTADO { get; set; }

        public DateTime? FECHA { get; set; }

        public decimal CODFUNCIONARIO { get; set; }

        [StringLength(4000)]
        public string MENSAJE_RECHAZO { get; set; }

        [StringLength(1)]
        public string AUTORIZADO { get; set; }
    }
}
