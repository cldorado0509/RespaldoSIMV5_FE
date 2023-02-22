namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBQUEJA")]
    public partial class TBQUEJA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBQUEJA()
        {
            TRAMITE_EXPEDIENTE_QUEJA = new HashSet<TRAMITE_EXPEDIENTE_QUEJA>();
        }

        [Key]
        public decimal CODIGO_QUEJA { get; set; }

        public decimal? CODIGO_AFECTACION { get; set; }

        public decimal? CODIGO_RECURSO { get; set; }

        public int? CODIGO_USUARIO { get; set; }

        public int? CODIGO_COMPONENTE { get; set; }

        public int? CODIGO_MUNICIPIO { get; set; }

        [StringLength(2000)]
        public string ASUNTO { get; set; }

        public DateTime? FECHA_RECEPCION { get; set; }

        [StringLength(100)]
        public string RECIBE { get; set; }

        [StringLength(100)]
        public string REMITIDO_A { get; set; }

        [StringLength(20)]
        public string RADICADO { get; set; }

        public DateTime? FECHA_TECNICO { get; set; }

        [StringLength(4000)]
        public string COMENTARIOS { get; set; }

        public DateTime? FECHA_ABOGADO { get; set; }

        [StringLength(500)]
        public string DIRECCION { get; set; }

        [StringLength(100)]
        public string INFRACTOR { get; set; }

        [StringLength(20)]
        public string TELEFONO_INFRACTOR { get; set; }

        [StringLength(500)]
        public string DIRECCION_INFRACTOR { get; set; }

        public DateTime? FECHA_ESTADO { get; set; }

        [StringLength(100)]
        public string FUNCIONARIO_ESTADO { get; set; }

        public int? CODIGO_TIPO_ESTADO { get; set; }

        public decimal? CODIGO_FORMA_QUEJA { get; set; }

        public decimal? CODIGO_TECNICO { get; set; }

        public decimal? CODIGO_ABOGADO { get; set; }

        public decimal? CODIGO_CATEGORIA { get; set; }

        public decimal? QUEJA { get; set; }

        [StringLength(4)]
        public string ANO { get; set; }

        [StringLength(20)]
        public string RADICADO_VINCULO { get; set; }

        public DateTime? FECHA_RADICADO_VINCULO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRAMITE_EXPEDIENTE_QUEJA> TRAMITE_EXPEDIENTE_QUEJA { get; set; }
    }
}
