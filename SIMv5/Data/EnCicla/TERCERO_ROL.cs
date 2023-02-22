namespace SIM.Data.EnCicla
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ENCICLA.TERCERO_ROL")]
    public partial class TERCERO_ROL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TERCERO_ROL()
        {
            HISTORICO = new HashSet<HISTORICO>();
            HISTORICO1 = new HashSet<HISTORICO>();
            HISTORICO2 = new HashSet<HISTORICO>();
            OPERACION = new HashSet<OPERACION>();
            OPERACION1 = new HashSet<OPERACION>();
            OPERACION2 = new HashSet<OPERACION>();
            TERCERO_ESTACION = new HashSet<TERCERO_ESTACION>();
            TERCERO_HISTORICO = new HashSet<TERCERO_HISTORICO>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TERCEROROL { get; set; }

        public int ID_TERCERO { get; set; }

        public int ID_ROL { get; set; }

        public int? ID_ESTRATEGIA { get; set; }

        [Required]
        [StringLength(1)]
        public string S_HABILITADO { get; set; }

        public DateTime D_INICIO { get; set; }

        public int ID_TERCEROESTADO { get; set; }

        [ForeignKey("ID_ESTRATEGIA")]
        public virtual ESTRATEGIA ESTRATEGIA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HISTORICO> HISTORICO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HISTORICO> HISTORICO1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HISTORICO> HISTORICO2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OPERACION> OPERACION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OPERACION> OPERACION1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OPERACION> OPERACION2 { get; set; }

        [ForeignKey("ID_ROL")]
        public virtual ROL_EN ROL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TERCERO_ESTACION> TERCERO_ESTACION { get; set; }

        [ForeignKey("ID_TERCEROESTADO")]
        public virtual TERCERO_ESTADO_EN TERCERO_ESTADO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TERCERO_HISTORICO> TERCERO_HISTORICO { get; set; }
    }
}
