namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.ANULACION_DOC")]
    public partial class ANULACION_DOC
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_ANULACION_DOC { get; set; }

        public int? ID_PROYECCION_DOC { get; set; }

        public int? ID_RADICADODOC { get; set; }

        public int ID_MOTIVO_ANULACION { get; set; }

        [Required]
        [StringLength(3)]
        public string S_FORMULARIO { get; set; }

        [StringLength(2000)]
        public string S_SOLICITUD { get; set; }

        [StringLength(2000)]
        public string S_JUSTIFICACION { get; set; }

        [StringLength(2000)]
        public string S_APROBACION { get; set; }

        [StringLength(2000)]
        public string S_ATU { get; set; }

        public DateTime D_FECHA_SOLICITUD { get; set; }

        public DateTime? D_FECHA_JUSTIFICACION { get; set; }

        public DateTime? D_FECHA_APROBACION { get; set; }

        public DateTime? D_FECHA_ATU { get; set; }

        public int CODFUNCIONARIO { get; set; }

        public int CODFUNCIONARIO_ACTUAL { get; set; }

        [StringLength(4000)]
        public string S_TRAMITES { get; set; }

        [StringLength(256)]
        public string S_PROCESOS { get; set; }

        public int? CODSERIE { get; set; }

        public int? CODTRAMITE_ANULACION { get; set; }

        public int? CODFUNCIONARIO_JUSTIFICACION { get; set; }

        public int? CODFUNCIONARIO_APROBACION { get; set; }

        public int? CODFUNCIONARIO_ATU { get; set; }

        public DateTime? D_FECHA_FINALIZACION { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ESTADO { get; set; }

        [ForeignKey("ID_MOTIVO_ANULACION")]
        public virtual MOTIVO_ANULACION MOTIVO_ANULACION { get; set; }
    }
}
