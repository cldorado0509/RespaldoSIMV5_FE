namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBTRAMITEDOCUMENTO")]
    public partial class TBTRAMITEDOCUMENTO
    {
        public decimal CODDOCUMENTO { get; set; }
        public decimal CODTRAMITE { get; set; }

        public decimal TIPODOCUMENTO { get; set; }

        public DateTime? FECHACREACION { get; set; }

        public decimal? CODFUNCIONARIO { get; set; }

        [StringLength(254)]
        public string NOMBRE { get; set; }

        [StringLength(510)]
        public string RUTAORIGINAL { get; set; }

        [StringLength(510)]
        public string RUTA { get; set; }

        [StringLength(508)]
        public string RUTACOPIA { get; set; }

        public DateTime? FECHAMODIFICACION { get; set; }

        [StringLength(100)]
        public string MAPAARCHIVO { get; set; }

        [StringLength(100)]
        public string MAPABD { get; set; }

        [StringLength(100)]
        public string DESCRIPCION { get; set; }

        public decimal? BORRAR { get; set; }

        [StringLength(200)]
        public string CLAVE { get; set; }

        public decimal? CODARCHIVO { get; set; }

        public decimal? CODCARPETA { get; set; }

        public int? PAGINAS { get; set; }

        public decimal CODSERIE { get; set; }

        public DateTime? FECHA_RECIBE { get; set; }

        public int? COD_FUNCRECIBE { get; set; }

        [StringLength(100)]
        public string ACTIVIDAD { get; set; }

        public decimal? ID_USUARIO { get; set; }

        [StringLength(1)]
        public string CIFRADO { get; set; }

        [StringLength(1)]
        public string S_ADJUNTO { get; set; }

        [StringLength(4000)]
        public string S_DETALLEADJUNTO { get; set; }

        [StringLength(1)]
        public string S_ESTADO { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_DOCUMENTO { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBINDICEDOCUMENTO> TBINDICEDOCUMENTO { get; set; }

        [ForeignKey("CODSERIE")]
        public virtual TBSERIE TBSERIE { get; set; }

        [ForeignKey("CODTRAMITE")]
        public virtual TBTRAMITE TBTRAMITE { get; set; }
    }
}
