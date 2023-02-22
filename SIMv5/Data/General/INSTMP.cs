namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.INSTMP")]
    public partial class INSTMP
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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

        public byte? N_PLACA { get; set; }

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
    }
}
