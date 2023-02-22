namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.TERCERO")]
    public partial class TERCERO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TERCERO()
        {
            CONTACTOS = new HashSet<CONTACTOS>();
            DEC_INDICADOR = new HashSet<DEC_INDICADOR>();
            DGA = new HashSet<DGA>();
            PARAMETRO_TERCERO = new HashSet<PARAMETRO_TERCERO>();
            PERIODO_BALANCE = new HashSet<PERIODO_BALANCE>();
            PERSONAL_DGA = new HashSet<PERSONAL_DGA>();
            REPRESENTANTE_LEGAL = new HashSet<REPRESENTANTE_LEGAL>();
            TERCERO_INSTALACION = new HashSet<TERCERO_INSTALACION>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_TERCERO { get; set; }

        public int? ID_TIPODOCUMENTO { get; set; }

        public int? ID_DIVIPOLA { get; set; }

        public int? ID_ACTIVIDADECONOMICA { get; set; }

        public int? ID_ESTADO { get; set; }

        public byte? N_TIPO { get; set; }

        public long N_DOCUMENTO { get; set; }

        public long? N_TELEFONO { get; set; }

        public int? N_FAX { get; set; }

        public int? N_FORMPAGO { get; set; }

        [StringLength(100)]
        public string S_CORREO { get; set; }

        [StringLength(100)]
        public string S_WEB { get; set; }

        [StringLength(50)]
        public string S_AAEREO { get; set; }

        [StringLength(2000)]
        public string S_OBSERVACION { get; set; }

        public int? N_USUARIO { get; set; }

        public DateTime? D_REGISTRO { get; set; }

        [StringLength(200)]
        public string S_RSOCIAL { get; set; }

        public byte? N_DIGITOVER { get; set; }

        public long? N_DOCUMENTON { get; set; }

        public long? N_CELULAR { get; set; }

        [StringLength(1)]
        public string S_AUTORIZANOTIFIELECTRO { get; set; }

        [StringLength(20)]
        public string S_RADICADOAUTORIZACION { get; set; }

        [StringLength(1)]
        public string S_PRIORITARIO { get; set; }

        [ForeignKey("ID_ACTIVIDADECONOMICA")]
        public virtual ACTIVIDAD_ECONOMICA ACTIVIDAD_ECONOMICA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONTACTOS> CONTACTOS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DEC_INDICADOR> DEC_INDICADOR { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DGA> DGA { get; set; }

        [ForeignKey("ID_DIVIPOLA")]
        public virtual DIVIPOLA DIVIPOLA { get; set; }

        [ForeignKey("ID_ESTADO")]
        public virtual ESTADO ESTADO { get; set; }

        public virtual JURIDICA JURIDICA { get; set; }

        public virtual NATURAL NATURAL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PARAMETRO_TERCERO> PARAMETRO_TERCERO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PERIODO_BALANCE> PERIODO_BALANCE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PERSONAL_DGA> PERSONAL_DGA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REPRESENTANTE_LEGAL> REPRESENTANTE_LEGAL { get; set; }

        [ForeignKey("ID_TIPODOCUMENTO")]
        public virtual TIPO_DOCUMENTO TIPO_DOCUMENTO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TERCERO_INSTALACION> TERCERO_INSTALACION { get; set; }
    }
}
