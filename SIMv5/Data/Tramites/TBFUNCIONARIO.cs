namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBFUNCIONARIO")]
    public partial class TBFUNCIONARIO
    {
        [Key]
        public decimal CODFUNCIONARIO { get; set; }

        [Required]
        [StringLength(120)]
        public string NOMBRES { get; set; }

        public decimal CODCARGO { get; set; }

        [StringLength(10)]
        public string OFICINA { get; set; }

        [StringLength(8)]
        public string EXTENSION { get; set; }

        [StringLength(200)]
        public string EMAIL { get; set; }

        [StringLength(100)]
        public string LOGIN { get; set; }

        [StringLength(100)]
        public string PASSWORD { get; set; }

        public decimal? CODGRUPO { get; set; }

        public decimal? TIPO { get; set; }

        public int? CODGRUPOT { get; set; }

        [StringLength(1)]
        public string ACTIVO { get; set; }

        [StringLength(50)]
        public string APELLIDOS { get; set; }

        [StringLength(20)]
        public string CEDULA { get; set; }

        [Required]
        [StringLength(1)]
        public string MOSTRAR_TAREAS_GRUPO { get; set; }
        public string FIRMA_DIGITAL { get; set; }
        public string USUARIO_FIRMA { get; set; }
        public DateTime? FECHAFIN_FIRMA { get; set; }

        public virtual ICollection<DOCUMENTO_TEMPORAL> DOCUMENTO_TEMPORAL { get; set; }

        public virtual ICollection<PRESTAMO_DETALLE_TRAMITES> PRESTAMO_DETALLE { get; set; }

        public virtual ICollection<PRESTAMOS_TRAMITES> PRESTAMOS { get; set; }
    }
}
