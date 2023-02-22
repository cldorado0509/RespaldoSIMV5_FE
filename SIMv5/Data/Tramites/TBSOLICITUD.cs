namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBSOLICITUD")]
    public partial class TBSOLICITUD
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_SOLICITUD { get; set; }

        public int CODIGO_TIPO_SOLICITUD { get; set; }

        public decimal CODIGO_PROYECTO { get; set; }

        public int CODIGO_MUNICIPIO { get; set; }

        public int CODIGO_COMPONENTE { get; set; }

        [Required]
        [StringLength(60)]
        public string CONEXO { get; set; }

        [StringLength(1000)]
        public string OBSERVACION { get; set; }

        public DateTime? FECHA_SOLICITUD { get; set; }

        public DateTime? FECHA_ARCHIVO { get; set; }

        public DateTime? FECHA_INICIO { get; set; }

        public DateTime? FECHA_FINAL { get; set; }

        public DateTime? FECHA_ORIGEN { get; set; }

        [StringLength(1)]
        public string ULTIMO { get; set; }

        [StringLength(20)]
        public string RADICADO { get; set; }

        public decimal? TOMOS { get; set; }

        public decimal? FOLIOS { get; set; }

        public DateTime? FECHA_ABOGADO { get; set; }

        public decimal? CODIGO_ABOGADO { get; set; }

        [StringLength(254)]
        public string NOMBRE { get; set; }

        [StringLength(1)]
        public string ARCHIVADO { get; set; }

        [StringLength(1)]
        public string ANULADO { get; set; }

        [StringLength(100)]
        public string NOMBRE_USUARIO { get; set; }

        [StringLength(50)]
        public string NUMERO { get; set; }

        public int? CODIGO_VEREDA { get; set; }

        [Required]
        [StringLength(1)]
        public string CONTROL_VIGILANCIA { get; set; }

        public DateTime? D_REGISTRO { get; set; }

        public int? CONSECUTIVOVIGENCIA { get; set; }

        public short? VIGENCIA { get; set; }

        [ForeignKey("CODIGO_MUNICIPIO")]
        public virtual TBMUNICIPIO TBMUNICIPIO { get; set; }

        [ForeignKey("CODIGO_PROYECTO")]
        public virtual TBPROYECTO TBPROYECTO { get; set; }

        [ForeignKey("CODIGO_TIPO_SOLICITUD")]
        public virtual TBTIPO_SOLICITUD TBTIPO_SOLICITUD { get; set; }
    }
}
