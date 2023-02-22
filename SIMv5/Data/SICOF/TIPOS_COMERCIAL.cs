namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.TIPOS_COMERCIAL")]
    public partial class TIPOS_COMERCIAL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TIPOS_COMERCIAL()
        {
            DET_COMERCIAL = new HashSet<DET_COMERCIAL>();
            MAE_COMERCIAL = new HashSet<MAE_COMERCIAL>();
            TIPOS_COMERCIAL1 = new HashSet<TIPOS_COMERCIAL>();
        }

        [Key]
        [StringLength(4)]
        public string DATO { get; set; }

        [Required]
        [StringLength(50)]
        public string DESCRIPCION { get; set; }

        [Required]
        [StringLength(1)]
        public string SIGNO { get; set; }

        [StringLength(4)]
        public string TIPO_REFERENCIA { get; set; }

        [Required]
        [StringLength(1)]
        public string BORRADO { get; set; }

        [Required]
        [StringLength(1)]
        public string DOCUMENTO_REFERENCIA { get; set; }

        [Required]
        [StringLength(1)]
        public string DOCUMENTO_ORIGINAL { get; set; }

        [StringLength(30)]
        public string USUARIO_EMPRESA { get; set; }

        [StringLength(50)]
        public string CODIGO_MEMPRESA { get; set; }

        public DateTime? FECHA_REGISTRO { get; set; }

        [StringLength(1)]
        public string CI_TERCERO_AC { get; set; }

        [StringLength(1)]
        public string TRUNCAR_NIT { get; set; }

        [StringLength(1)]
        public string NETGOLD { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DET_COMERCIAL> DET_COMERCIAL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MAE_COMERCIAL> MAE_COMERCIAL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TIPOS_COMERCIAL> TIPOS_COMERCIAL1 { get; set; }

        public virtual TIPOS_COMERCIAL TIPOS_COMERCIAL2 { get; set; }
    }
}
