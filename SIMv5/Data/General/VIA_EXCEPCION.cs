namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.VIA_EXCEPCION")]
    public partial class VIA_EXCEPCION
    {
        [Key]
        public decimal ID_VIAEXCEPCION { get; set; }

        public int? ID_TIPOVIAPPAL { get; set; }

        public int? ID_TIPOVIASEC { get; set; }

        public int? ID_TIPOVIACAMBIA { get; set; }

        public int? ID_LETRAVIAPPAL { get; set; }

        public int? ID_LETRAVIASEC { get; set; }

        public int? ID_LETRAVIACAMBIA { get; set; }

        public byte? N_NUMEROVIAPPAL { get; set; }

        [StringLength(1)]
        public string S_SENTIDOVIAPPAL { get; set; }

        public byte? N_NUMEROVIASEC { get; set; }

        [StringLength(1)]
        public string S_SENTIDOVIASEC { get; set; }

        public byte? N_NUMEROVIACAMBIA { get; set; }

        [StringLength(1)]
        public string S_SENTIDOVIACAMBIA { get; set; }

        [ForeignKey("ID_LETRAVIAPPAL")]
        public virtual LETRA_VIA LETRA_VIA { get; set; }

        [ForeignKey("ID_LETRAVIASEC")]
        public virtual LETRA_VIA LETRA_VIA1 { get; set; }

        [ForeignKey("ID_LETRAVIACAMBIA")]
        public virtual LETRA_VIA LETRA_VIA2 { get; set; }

        [ForeignKey("ID_TIPOVIAPPAL")]
        public virtual TIPO_VIA TIPO_VIA { get; set; }

        [ForeignKey("ID_TIPOVIASEC")]
        public virtual TIPO_VIA TIPO_VIA1 { get; set; }

        [ForeignKey("ID_TIPOVIACAMBIA")]
        public virtual TIPO_VIA TIPO_VIA2 { get; set; }
    }
}
