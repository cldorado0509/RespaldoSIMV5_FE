namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.INSTALACION")]
    public partial class INSTALACION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public INSTALACION()
        {
            DEC_INDICADOR = new HashSet<DEC_INDICADOR>();
            PERIODO_BALANCE = new HashSet<PERIODO_BALANCE>();
            PARAMETRO_INSTALACION = new HashSet<PARAMETRO_INSTALACION>();
            TERCERO_INSTALACION = new HashSet<TERCERO_INSTALACION>();
            LOTE_INSTALACION = new HashSet<LOTE_INSTALACION>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_INSTALACION { get; set; }

        [StringLength(100)]
        public string S_NOMBRE { get; set; }

        [StringLength(50)]
        public string S_CODIGO { get; set; }

        public int? ID_TIPOVIAPPAL { get; set; }

        public int? ID_TIPOVIASEC { get; set; }

        public int? ID_LETRAVIAPPAL { get; set; }

        public int? ID_LETRAVIASEC { get; set; }

        public int? ID_DIVIPOLA { get; set; }

        public int? ID_ESTADO { get; set; }

        public short? N_NUMEROVIAPPAL { get; set; }

        public short? N_NUMEROVIASEC { get; set; }

        [StringLength(1)]
        public string S_SENTIDOVIAPPAL { get; set; }

        [StringLength(1)]
        public string S_SENTIDOVIASEC { get; set; }

        public short? N_PLACA { get; set; }

        public short? N_INTERIOR { get; set; }

        [StringLength(100)]
        public string S_ESPECIAL { get; set; }

        public decimal? N_COORDX { get; set; }

        public decimal? N_COORDY { get; set; }

        public decimal? N_COORDZ { get; set; }

        [StringLength(20)]
        public string S_CEDULACATASTRAL { get; set; }

        [StringLength(20)]
        public string S_MATRICULAINMOBILIARIA { get; set; }

        [StringLength(2000)]
        public string S_OBSERVACION { get; set; }

        public int? N_USUARIO { get; set; }

        public DateTime? D_REGISTRO { get; set; }

        [StringLength(50)]
        public string S_TELEFONO { get; set; }

        /*public decimal? ID_TAMANIO_INSTALACION { get; set; }

        [StringLength(100)]
        public string S_TIPO_INSTALACION { get; set; }

        [StringLength(100)]
        public string S_DESC_ALREDEDORES { get; set; }*/

        [StringLength(30)]
        public string S_COORDX { get; set; }

        [StringLength(30)]
        public string S_COORDY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DEC_INDICADOR> DEC_INDICADOR { get; set; }

        public virtual DIVIPOLA DIVIPOLA { get; set; }

        public virtual ESTADO ESTADO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PERIODO_BALANCE> PERIODO_BALANCE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PARAMETRO_INSTALACION> PARAMETRO_INSTALACION { get; set; }

        public virtual LETRA_VIA LETRA_VIA { get; set; }

        public virtual LETRA_VIA LETRA_VIA1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TERCERO_INSTALACION> TERCERO_INSTALACION { get; set; }

        public virtual TIPO_VIA TIPO_VIA { get; set; }

        public virtual TIPO_VIA TIPO_VIA1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOTE_INSTALACION> LOTE_INSTALACION { get; set; }
    }
}
